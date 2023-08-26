using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using WebSupergoo.ABCpdf12;
using System.Diagnostics;
using WebSupergoo.ABCpdf11.Elements;
using System.Text;

namespace MobileExpress
{
    public static class Tools
    {
        public static string TemplateFacturePath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Template facture.html";
        public static string TemplateRecuPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Template reçu.html";
        public static string ArticleHtml = @"<tr><td valign='top' style='font-size:12px;'>{{articleName}}</td><td valign='top' style='font-size:12px;'>{{articleQuantity}}</td><td valign='top' style='font-size:12px;'>{{articlePrice}}</td><td valign='top' style='font-size:12px;'>{{articleTotal}}</td></tr>";
        public static string GeneratePdfFromHtml(
            int type,
            string logo,
            string customerName,
            string customerPhone,
            string customerEmail,
            string takeOverDate,
            string takeOverNumber,
            decimal? accompte,
            decimal paid,
            bool carteBleu,
            bool espece,
            bool virement,
            List<MEData> mEDatas,
            string path,
            Dictionary<int, string> repairTypeNames,
            Dictionary<int, string> unlockTypeNames,
            Dictionary<int, string> articles,
            Dictionary<int, string> marques,
            Dictionary<int, string> modeles)
        {
            Func<string, string, string, string, string> func = (tmpnom, tmpmarque, tmpmodele, tmpimei) =>
            {
                return
                    !string.IsNullOrWhiteSpace(tmpmarque) && !string.IsNullOrWhiteSpace(tmpimei) ?
                        $"{tmpmarque} {tmpmodele} (IMEI:{tmpimei}) - {tmpnom}" :
                    string.IsNullOrWhiteSpace(tmpmarque) && !string.IsNullOrWhiteSpace(tmpimei) ?
                        $"(IMEI:{tmpimei}) {tmpnom}" :
                    !string.IsNullOrWhiteSpace(tmpmarque) && string.IsNullOrWhiteSpace(tmpimei) ?
                        $"{tmpmarque} {tmpmodele} - {tmpnom}" : $"{tmpnom}";
            };

            decimal total = 0;

            string articlesHtml = string.Empty;
            foreach (MEData mEData in mEDatas)
            {
                if (type == 0 && mEData.ArticleId.HasValue)
                {
                    continue;
                }

                string articlename = type == 1 ?
                    (mEData.RepairTypeId.HasValue ? repairTypeNames[mEData.RepairTypeId.Value] :
                    mEData.UnlockTypeId.HasValue ? unlockTypeNames[mEData.UnlockTypeId.Value] :
                    articles[mEData.ArticleId.Value]) :
                    (mEData.RepairTypeId.HasValue ? repairTypeNames[mEData.RepairTypeId.Value] :
                    unlockTypeNames[mEData.UnlockTypeId.Value]);
                string marquename = mEData.MarqueId.HasValue ? marques[mEData.MarqueId.Value] : string.Empty;
                string modelename = mEData.ModeleId.HasValue ? modeles[mEData.ModeleId.Value] : string.Empty;
                string imei = mEData.IMEI;

                if (mEData.Verification)
                {
                    articlesHtml += ArticleHtml
                        .Replace("{{articleName}}", func(articlename, marquename, modelename, imei))
                        .Replace("{{articleQuantity}}", mEData.Quantity.ToString())
                        .Replace("{{articlePrice}}", string.Empty)
                        .Replace("{{articleTotal}}", string.Empty);
                    articlesHtml += ArticleHtml
                        .Replace("{{articleName}}", "Vérification - " + func(articlename, marquename, modelename, imei))
                        .Replace("{{articleQuantity}}", mEData.Quantity.ToString())
                        .Replace("{{articlePrice}}", mEData.Price.Value.ToString())
                        .Replace("{{articleTotal}}", (mEData.Quantity * mEData.Price.Value).ToString());
                    total += (mEData.Quantity * mEData.Price.Value);
                }
                else
                {
                    articlesHtml += ArticleHtml
                        .Replace("{{articleName}}", func(articlename, marquename, modelename, imei))
                        .Replace("{{articleQuantity}}", mEData.Quantity.ToString())
                        .Replace("{{articlePrice}}", mEData.Price.ToString())
                        .Replace("{{articleTotal}}", (mEData.Quantity * mEData.Price.Value).ToString());
                    total += (mEData.Quantity * mEData.Price.Value);
                }
                if (type == 1 && mEData.Remise.HasValue)
                {
                    articlesHtml += ArticleHtml
                        .Replace("{{articleName}}", "Remise - " + func(articlename, marquename, modelename, imei))
                        .Replace("{{articleQuantity}}", mEData.Quantity.ToString())
                        .Replace("{{articlePrice}}", (mEData.Remise.Value * -1).ToString())
                        .Replace("{{articleTotal}}", (mEData.Quantity * mEData.Remise.Value * -1).ToString());
                    total += (mEData.Quantity * mEData.Remise.Value * -1);
                }
                if (type == 1 && mEData.Garantie.HasValue)
                {
                    articlesHtml += ArticleHtml
                        .Replace("{{articleName}}", $"Garantie {$"{mEData.Garantie.Value.ToString()} mois"} - " + func(articlename, marquename, modelename, imei))
                        .Replace("{{articleQuantity}}", mEData.Quantity.ToString())
                        .Replace("{{articlePrice}}", string.Empty)
                        .Replace("{{articleTotal}}", string.Empty);
                }
            }

            string html = File.ReadAllText(type == 0 ? TemplateRecuPath : TemplateFacturePath);

            string htmlBody = html
                .Replace("{{mobileExpressLogo}}", logo)
                .Replace("{{customerName}}", customerName)
                .Replace("{{customerPhone}}", customerPhone)
                .Replace("{{customerEmail}}", customerEmail)
                .Replace("{{invoiceDate}}", takeOverDate)
                .Replace("{{invoiceNumber}}", takeOverNumber)
                .Replace("{{total}}", (total == 0 ? string.Empty : total.ToString()))
                .Replace("{{paid}}", paid.ToString())
                .Replace("{{articles}}", articlesHtml)
                .Replace("{{carteBleu}}", carteBleu ? "X" : " ")
                .Replace("{{espece}}", espece ? "X" : " ")
                .Replace("{{virement}}", virement ? "X" : " ");
            if (type == 0)
            {
                htmlBody = htmlBody
                    .Replace("{{accompte}}", accompte.Value.ToString())
                    .Replace("{{resteDu}}", (total - paid).ToString());
            }

            //generate pdf
            using (Doc pdfDocument = new Doc())
            {
                // Add html to Doc
                int theID = pdfDocument.AddImageHtml(htmlBody);

                // Loop through document to create multi-page PDF
                while (true)
                {
                    if (!pdfDocument.Chainable(theID))
                        break;
                    pdfDocument.Page = pdfDocument.AddPage();
                    theID = pdfDocument.AddImageToChain(theID);
                }

                // Flatten the PDF
                for (int i = 1; i <= pdfDocument.PageCount; i++)
                {
                    pdfDocument.PageNumber = i;
                    pdfDocument.Flatten();
                }

                // Vérifier si le fichier existe
                if (System.IO.File.Exists(path))
                {
                    // supprimer le fichier avant de créer le nouveau
                    File.Delete(path);
                }

                //save the pdf, pdfFullPath: path to save the pdf
                pdfDocument.Save(path);
            }

            return path;
        }
        
        public static int? ToNullableInt(string s)
        {
            int i;
            if (int.TryParse(s, out i)) return i;
            return null;
        }
        public static decimal? ToNullableDecimal(string s)
        {
            decimal i;
            if (decimal.TryParse(s, out i)) return i;
            return null;
        }
        public static string GetEnumDescription<T>(T enumValue) where T : Enum
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : enumValue.ToString();
        }
        public static T GetEnumFromDescription<T>(string description) where T : Enum
        {
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                if (GetEnumDescription(enumValue) == description)
                {
                    return enumValue;
                }
            }

            throw new ArgumentException("Enum value not found for the given description.", nameof(description));
        }
        public static string GetStringFromCustomer(Customer customer)
        {
            string customerName;

            if (!string.IsNullOrWhiteSpace(customer.LastName))
            {
                if (!string.IsNullOrWhiteSpace(customer.FirstName))
                {
                    if (!string.IsNullOrWhiteSpace(customer.PhoneNumber))
                    {
                        if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                        {
                            customerName = $"{customer.FirstName} {customer.LastName} - {customer.PhoneNumber} - {customer.EmailAddress}";
                        }
                        else
                        {
                            customerName = $"{customer.FirstName} {customer.LastName} - {customer.PhoneNumber}";
                        }
                    }
                    else if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                    {
                        customerName = $"{customer.FirstName} {customer.LastName} - {customer.EmailAddress}";
                    }
                    else
                    {
                        customerName = $"{customer.FirstName} {customer.LastName}";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(customer.PhoneNumber))
                {
                    if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                    {
                        customerName = $"{customer.LastName} - {customer.PhoneNumber} - {customer.EmailAddress}";
                    }
                    else
                    {
                        customerName = $"{customer.LastName} - {customer.PhoneNumber}";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                {
                    customerName = $"{customer.LastName} - {customer.EmailAddress}";
                }
                else
                {
                    customerName = $"{customer.LastName}";
                }
            }
            else if (!string.IsNullOrWhiteSpace(customer.FirstName))
            {
                if (!string.IsNullOrWhiteSpace(customer.PhoneNumber))
                {
                    if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                    {
                        customerName = $"{customer.FirstName} - {customer.PhoneNumber} - {customer.EmailAddress}";
                    }
                    else
                    {
                        customerName = $"{customer.FirstName} - {customer.PhoneNumber}";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                {
                    customerName = $"{customer.FirstName} - {customer.EmailAddress}";
                }
                else
                {
                    customerName = $"{customer.FirstName}";
                }
            }
            else if (!string.IsNullOrWhiteSpace(customer.PhoneNumber))
            {
                if (!string.IsNullOrWhiteSpace(customer.EmailAddress))
                {
                    customerName = $"{customer.PhoneNumber} - {customer.EmailAddress}";
                }
                else
                {
                    customerName = $"{customer.PhoneNumber}";
                }
            }
            else
            {
                customerName = $"{customer.EmailAddress}";
            }

            return customerName;
        }

        public static List<string> GetDataFromFile(string path)
        {
            try
            {
                List<string> data = new List<string>();
                using (StreamReader reader = new StreamReader(path, Encoding.UTF8))
                {
                    if (reader == null)
                        throw new Exception($"La lecture du fichier {path} a échoué.");
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        data.Add(line);
                    }
                }
                return data;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return null;
            }
        }
        public static bool RewriteDataToFile(List<string> data, string path, bool toAppend)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path, append: toAppend, Encoding.UTF8))
                {
                    if (writer == null)
                        MessageBox.Show($"L'écrire dans le fichier {path} a échoué.");

                    foreach (string line in data)
                    {
                        writer.WriteLine(line);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return false;
            }
        }
        public static bool WriteLineTofile(string line, string path, bool toAppend)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path: path, append: toAppend, Encoding.UTF8))
                {
                    writer.WriteLine(line);
                }

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Une erreur est survenue ! " + e.Message);
                return false;
            }
        }
        public static bool ClearFile(string path)
        {
            try
            {
                File.WriteAllText(path, string.Empty);
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Une erreur est survenue ! " + e.Message);
                return false;
            }
        }

        public static (bool, bool, bool) GetPaymentModeFromInt(int paymentMode)
        {
            return (
                paymentMode == 1 ? (true, false, false) :
                paymentMode == 2 ? (true, false, false) :
                paymentMode == 3 ? (true, true, false) :
                paymentMode == 4 ? (true, false, true) :
                paymentMode == 5 ? (true, false, true) :
                paymentMode == 6 ? (false, true, true) :
                paymentMode == 7 ? (true, true, true) :
                (false, false, false)
            );
        }
        public static int GetIntFromPaymentMode(bool cb, bool espece, bool virement)
        {
            int toReturn = 0;
            toReturn += cb ? 1 : 0;
            toReturn += espece ? 2 : 0;
            toReturn += virement ? 4 : 0;
            return toReturn;
        }
    }
    public static class Paths
    {
        public static string LogoPath = @"C:\Users\merto\OneDrive\Documents\Nicepage\Site_26231059\images\Logoeducdombleu.png";

        public static string PrinterName = @"";

        public static string MarquesDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Marques.csv";
        public static string ModelesDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Modèles.csv";
        public static string UnlockTypesDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Types de déblocage.csv";
        public static string RepairTypesDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Types de réparation.csv";
        public static string PaymentModesDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Modes de paiement.csv";
        
        public static string ReceiptDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Ticket de caisse.csv";
        public static string CustomersDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Client.csv";
        public static string StockDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Article.csv";

        public static string TakeOverPdfsPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Prises en charges\";
        public static string InvoicePdfsPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Factures\";
    }

    public class Marque
    {
        public int Id;
        public string Name;
        public Marque(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    public class Modele
    {
        public int Id;
        public int MarqueId;
        public string Name;
        public Modele(int id, int marqueId, string name)
        {
            Id = id;
            MarqueId = marqueId;
            Name = name;
        }
    }
    public class UnlockType
    {
        public int Id;
        public string Name;
        public decimal Price;
        public UnlockType(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
    public class RepairType
    {
        public int Id;
        public string Name;
        public decimal Price;
        public RepairType(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
    public class PaymentMode
    {
        public int Id;
        public string Name;
        public PaymentMode(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class TakeOverType
    {
        public int Id;
        public string Name;
        public List<RepairType> RepairTypes;
        public List<UnlockType> UnlockTypes;
        public List<Article> Articles;
        public TakeOverType(int id, string name, List<RepairType> repairTypes = null, List<UnlockType> unlockTypes = null, List<Article> articles = null)
        {
            Id = id;
            Name = name;
            RepairTypes = repairTypes;
            UnlockTypes = unlockTypes;
            Articles = articles;
        }
        public TakeOverType() { }
    }
    public class Customer
    {
        public int Id;
        public string LastName;
        public string FirstName;
        public string PhoneNumber;
        public string EmailAddress;
        public Sexe Sexe;
        public Customer(int id, string lastName, string firstName, string phoneNumber, string emailAddress, Sexe sexe)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
            Sexe = sexe;
        }

        public Customer()
        {
        }
    }
    public enum Sexe
    {
        [Description("Femme")]
        Femme = 0,
        [Description("Homme")]
        Homme = 1,
        [Description("Non renseigné")]
        Unknown = 2,
    }
    public class MEData
    {
        public int Id;
        public int TakeOverId;
        public DateTime Date;
        public int? InvoiceId;
        public int CustomerId;
        public int? MarqueId;
        public int? ModeleId;
        public string IMEI;
        public int? RepairTypeId;
        public int? UnlockTypeId;
        public int? ArticleId;
        public int Quantity;
        public decimal? Price;
        public int? Garantie;
        public decimal? Remise;
        public decimal? Accompte;
        public decimal? ResteDu;
        public decimal? Paid;
        public decimal Total;
        public int PaymentMode;
        public TakeOverState State;
        public bool Verification;
        public MEData(
            int takeOverId, DateTime date, int? invoiceId, int customerId, int? marqueId, int? modeleId,
            string imei, int? repairTypeId, int? unlockTypeId, int? articleId, int quantity,
            decimal? price, int? garantie, decimal? remise, decimal? accompte, decimal? resteDu,
            decimal? paid, decimal total, int paymentMode, TakeOverState state, int id, bool verification)
        {
            TakeOverId = takeOverId;
            Date = date;
            InvoiceId = invoiceId;
            CustomerId = customerId;
            MarqueId = marqueId;
            ModeleId = modeleId;
            IMEI = imei;
            RepairTypeId = repairTypeId;
            UnlockTypeId = unlockTypeId;
            ArticleId = articleId;
            Quantity = quantity;
            Garantie = garantie;
            Remise = remise;
            Price = price;
            Accompte = accompte;
            ResteDu = resteDu;
            Paid = paid;
            Total = total;
            PaymentMode = paymentMode;
            State = state;
            Id = id;
            Verification = verification;
        }
        public MEData()
        {
        }
    }
    public enum TakeOverState
    {
        [Description("En cours")]
        InProgress = 0,
        [Description("Terminé")]
        Done = 1,
        [Description("Récupéré")]
        PickedUp = 2,
        [Description("Annulé")]
        Canceled = 3,
        [Description("Récupéré non réparable")]
        PickedUpIrreparable = 4,
    }
    public class Article
    {
        public int Id;
        public int? MarqueId;
        public int? ModeleId;
        public string Name;
        public decimal Price;
        public int Quantity;
        public string Description;
        public Article(int id, int? marqueId, int? modeleId, string name, decimal price, int quantity, string description)
        {
            Id = id;
            MarqueId = marqueId;
            ModeleId = modeleId;
            Name = name;
            Price = price;
            Quantity = quantity;
            Description = description;
        }
        public Article()
        {

        }
    }
    public class Garantie
    {
        public string TabName;
        public int Id;
        public string ProductName;
        public int? Months;
        public Garantie() { }
        public Garantie(string tabName, int id, string productName, int? months)
        {
            TabName = tabName;
            Id = id;
            ProductName = productName;
            Months = months;
        }
    }
    public class Remise
    {
        public string TabName;
        public int Id;
        public string ProductName;
        public decimal? Prix;
        public Remise() { }
        public Remise(string tabName, int id, string productName, decimal? prix)
        {
            TabName = tabName;
            Id = id;
            ProductName = productName;
            Prix = prix;
        }
    }

    public class CustomerView
    {
        public int Id;
        public string LastName;
        public string FirstName;
        public string Phone;
        public string Email;
        public string Count;
    }
    public class HistoriqueOneView
    {
        public int Id;
        public int PriseEnCharge;
        public string Date;
        public int FactureNumero;
        public string Facture;
        public string TypeItem;
        public string Item;
        public string Marque;
        public string Modele;
        public string IMEI;
        public int Quantity;
        public string Price;
        public string Garantie;
        public string Remise;
        public string Accompte;
        public string ResteDu;
        public string Paid;
        public string Total;
        public string PaymentMode;
        public TakeOverState State;
        public bool Verification;
    }

    public class NumericUpDownColumn : DataGridViewColumn
    {
        public NumericUpDownColumn()
            : base(new NumericUpDownCell())
        {
        }

        public override DataGridViewCell CellTemplate
        {
            get { return base.CellTemplate; }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(NumericUpDownCell)))
                {
                    throw new InvalidCastException("Must be a NumericUpDownCell");
                }
                base.CellTemplate = value;
            }
        }
    }
    public class NumericUpDownCell : DataGridViewTextBoxCell
    {
        private readonly decimal min = 0;
        private readonly decimal max = 999;

        public NumericUpDownCell()
            : base()
        {
            Style.Format = "F2";
        }
        public NumericUpDownCell(decimal min, decimal max)
            : base()
        {
            this.min = min;
            this.max = max;
            Style.Format = "F2";
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            NumericUpDownEditingControl ctl = DataGridView.EditingControl as NumericUpDownEditingControl;
            ctl.Minimum = this.min;
            ctl.Maximum = this.max;
            ctl.Value = Convert.ToDecimal(this.Value);
        }

        public override Type EditType
        {
            get { return typeof(NumericUpDownEditingControl); }
        }

        public override Type ValueType
        {
            get { return typeof(Decimal); }
        }

        public override object DefaultNewRowValue
        {
            get { return null; } //未編集の新規行に余計な初期値が出ないようにする
        }
    }
    public class NumericUpDownEditingControl : NumericUpDown, IDataGridViewEditingControl
    {
        private DataGridView dataGridViewControl;
        private bool valueIsChanged = false;
        private int rowIndexNum;

        public NumericUpDownEditingControl()
            : base()
        {
            this.DecimalPlaces = 2;
        }

        public DataGridView EditingControlDataGridView
        {
            get { return dataGridViewControl; }
            set { dataGridViewControl = value; }
        }

        public object EditingControlFormattedValue
        {
            get { return this.Value.ToString("F2"); }
            set { this.Value = Decimal.Parse(value.ToString()); }
        }
        public int EditingControlRowIndex
        {
            get { return rowIndexNum; }
            set { rowIndexNum = value; }
        }
        public bool EditingControlValueChanged
        {
            get { return valueIsChanged; }
            set { valueIsChanged = value; }
        }

        public Cursor EditingPanelCursor
        {
            get { return base.Cursor; }
        }

        public bool RepositionEditingControlOnValueChange
        {
            get { return false; }
        }

        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.ForeColor = dataGridViewCellStyle.ForeColor;
            this.BackColor = dataGridViewCellStyle.BackColor;
        }

        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            return (keyData == Keys.Left || keyData == Keys.Right ||
                keyData == Keys.Up || keyData == Keys.Down ||
                keyData == Keys.Home || keyData == Keys.End ||
                keyData == Keys.PageDown || keyData == Keys.PageUp);
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return this.Value.ToString("F2");
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
        }

        protected override void OnValueChanged(EventArgs e)
        {
            valueIsChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(e);
        }
    }

    public class AutoCompleteComboBox : ComboBox
    {
        public event CancelEventHandler NotInList;

        private bool _limitToList = true;
        private bool _inEditMode = false;

        public AutoCompleteComboBox() : base()
        {
        }

        [Category("Behavior")]
        public bool LimitToList
        {
            get { return _limitToList; }
            set { _limitToList = value; }
        }
        protected virtual void OnNotInList(System.ComponentModel.CancelEventArgs e)
        {
            if (NotInList != null)
            {
                NotInList(this, e);
            }
        }
        protected override void OnTextChanged(System.EventArgs e)
        {
            if (_inEditMode)
            {
                string input = Text;
                int index = FindString(input);

                if (index >= 0)
                {
                    _inEditMode = false;
                    SelectedIndex = index;
                    _inEditMode = true;
                    Select(input.Length, Text.Length);
                }
            }

            base.OnTextChanged(e);
        }
        protected override void OnValidating(System.ComponentModel.CancelEventArgs e)
        {
            if (this.LimitToList)
            {
                int pos = this.FindStringExact(this.Text);

                if (pos == -1)
                {
                    OnNotInList(e);
                }
                else
                {
                    this.SelectedIndex = pos;
                }
            }

            base.OnValidating(e);
        }
        protected override void OnKeyDown(System.Windows.Forms.KeyEventArgs e)
        {
            _inEditMode =
                (e.KeyCode != Keys.Back && e.KeyCode != Keys.Delete);
            base.OnKeyDown(e);
        }
    }
}
