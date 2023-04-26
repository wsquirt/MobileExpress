using IronPdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;

namespace MobileExpress
{
    public static class Tools
    {
        private static string Html { get; set; } = @"<!DOCTYPE html PUBLIC '-//W3C//DTD XHTML 1.0 Transitional//EN' 'https://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd'><html xmlns='https://www.w3.org/1999/xhtml'><head><meta http-equiv='Content-Type' content='text/html; charset=utf-8' /><body style='font-family:Tahoma;font-size:12px;color: #333333;background-color:#FFFFFF;'><table align='center' border='0' cellpadding='0' cellspacing='0' style='height:842px; width:595px;font-size:12px;'><tr><td valign='top'><table width='100%' cellspacing='0' cellpadding='0'><tr><td valign='bottom' width='50%' height='50'><div align='left'><img src='{{mobileExpressLogo}}' /></div><br /></td><td width='50%'>&nbsp;</td></tr></table>Destinataire: <br/><br/><table width='100%' cellspacing='0' cellpadding='0'><tr><td valign='top' width='35%' style='font-size:12px;'> <strong >{{customerName}}</strong> <br /></td><td valign='top' width='35%'></td><td valign='top' width='30%' style='font-size:12px;'>Date de facturation: {{invoiceDate}}<br/></td></tr></table><table width='100%' height='100' cellspacing='0' cellpadding='0'><tr><td><div align='center' style='font-size: 14px;font-weight: bold;'>Facture №  {{invoiceNumber}} </div></td></tr></table><table width='100%' cellspacing='0' cellpadding='2' border='1' bordercolor='#CCCCCC'><tr><td width='35%' bordercolor='#ccc' bgcolor='#f2f2f2' style='font-size:12px;'><strong>Désignation </strong></td><td bordercolor='#ccc' bgcolor='#f2f2f2' style='font-size:12px;'><strong>Quantité</strong></td><td bordercolor='#ccc' bgcolor='#f2f2f2' style='font-size:12px;'><strong>Prix</strong></td><td bordercolor='#ccc' bgcolor='#f2f2f2' style='font-size:12px;'><strong>Total</strong></td></tr><tr style=\""display:none;\""><td colspan=\""*\"">{{articles}}<tr><td valign='top' style='font-size:12px;'>&nbsp;</td><td valign='top' style='font-size:12px;'>&nbsp;</td><td valign='top' style='font-size:12px;'>&nbsp;</td><td valign='top' style='font-size:12px;'>&nbsp;</td></tr></td></tr></table><table width='100%' cellspacing='0' cellpadding='2' border='0'><tr><td style='font-size:12px;width:50%;'><strong> </strong></td><td><table width='100%' cellspacing='0' cellpadding='2' border='0'><tr><td align='right' style='font-size:12px;' >Total</td><td  align='right' style='font-size:12px;'>{{total}} €<td></tr></table></td></tr></table><table width='100%' height='50'><tr><td style='font-size:12px;text-align:justify;'>TVA non applicable, article 293B du code général des impôts</td></tr></table><table  width='100%' cellspacing='0' cellpadding='2'><tr><td width='33%' style='border-top:double medium #CCCCCC;font-size:12px;' valign='top' ><b>Mobile Express</b><br/>SIRET:  {{siret}}<br/></td><td width='33%' style='border-top:double medium #CCCCCC; font-size:12px;' align='center' valign='top'>{{address}} <br/>Tél: {{phoneNumber}}<br/></td><td valign='top' width='34%' style='border-top:double medium #CCCCCC;font-size:12px;' align='right'>{{bankName}}<br/> {{bankAccount}}  <br/></td></tr></table></td></tr></table></body></html>";
        private static string ArticleHtml { get; set; } = @"<tr>\r\n\r\n<td valign='top' style='font-size:12px;'>{{articleName}}</td>\r\n<td valign='top' style='font-size:12px;'>{{articleQuantity}}</td>\r\n<td valign='top' style='font-size:12px;'>{{articlePrice}}</td>\r\n<td valign='top' style='font-size:12px;'>{{articleTotal}}</td>\r\n\r\n</tr>";
        public static void GeneratePdfFromHtml(
            string logo,
            string customerName,
            string invoiceDate,
            string invoiceNumber,
            string total,
            string siret,
            string address,
            string phoneNumber,
            string bankName,
            string bankAccount,
            List<Article> articles, string path)
        {
            // Instantiate Renderer
            var Renderer = new ChromePdfRenderer();

            /* Main Document */
            //As we have a Cover Page, we're going to start the page numbers at 2.
            Renderer.RenderingOptions.FirstPageNumber = 1;

            Renderer.RenderingOptions.HtmlFooter = new HtmlHeaderFooter()
            {
                MaxHeight = 15, //millimeters
                HtmlFragment = "<center><i>{page} of {total-pages}<i></center>",
                DrawDividerLine = true
            };

            string articlesHtml = string.Empty;
            foreach (var article in articles)
            {
                articlesHtml += ArticleHtml
                    .Replace("{{articleName}}", article.Name)
                    .Replace("{{articleQuantity}}", article.Quantity.ToString())
                    .Replace("{{articlePrice}}", article.Price.ToString())
                    .Replace("{{articleTotal}}", (article.Quantity * article.Price).ToString());
            }
            PdfDocument Pdf = Renderer.RenderHtmlAsPdf(
                Html
                    .Replace("{{mobileExpressLogo}}", logo)
                    .Replace("{{customerName}}", customerName)
                    .Replace("{{invoiceDate}}", invoiceDate)
                    .Replace("{{invoiceNumber}}", invoiceNumber)
                    .Replace("{{total}}", total)
                    .Replace("{{siret}}", siret)
                    .Replace("{{address}}", address)
                    .Replace("{{phoneNumber}}", phoneNumber)
                    .Replace("{{bankName}}", bankName)
                    .Replace("{{bankAccount}}", bankAccount)
                    .Replace("{{articles}}", articlesHtml));

            //PDF Settings
            Pdf.SecuritySettings.AllowUserCopyPasteContent = false;
            //Pdf.SecuritySettings.UserPassword = string.Empty;

            Pdf.SaveAs(path);
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
    }
    public static class Paths
    {
        public static string LogoPath { get; set; } = @"C:\Users\merto\OneDrive\Documents\Nicepage\Site_26231059\images\Logoeducdombleu.png";
        public static string MarquesDSPath { get; set; } = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Marques.csv";
        public static string ModelesDSPath { get; set; } = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Modèles.csv";
        public static string UnlockTypesDSPath { get; set; } = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Types de déblocage.csv";
        public static string RepairTypesDSPath { get; set; } = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Types de réparation.csv";
        public static string PaymentModesDSPath { get; set; } = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Modes de paiement.csv";
        public static string InvoicesDSPath { get; set; } = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Historique.csv";
        public static string StockDSPath { get; set; } = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Article.csv";
        public static string CustomersDSPath { get; set; } = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Client.csv";
        public static string PdfInvoicesDSPath { get; set; } = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Archive Factures\";
        public static string PrinterName { get; set; } = @"";
    }

    public class Marque
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Marque(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
    public class Modele
    {
        public int Id { get; set; }
        public int MarqueId { get; set; }
        public string Name { get; set; }
        public Modele(int id, int marqueId, string name)
        {
            Id = id;
            MarqueId = marqueId;
            Name = name;
        }
    }
    public class UnlockType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public UnlockType(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
    public class RepairType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public RepairType(int id, string name, decimal price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
    public class PaymentMode
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public PaymentMode(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class Choice
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class Customer
    {
        public int Id { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public Customer(int id, string lastName, string firstName, string phoneNumber, string emailAddress)
        {
            Id = id;
            LastName = lastName;
            FirstName = firstName;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddress;
        }

        public Customer()
        {
        }
    }
    public class Invoice
    {
        public int CustomerId { get; set; }
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int? MarqueId { get; set; }
        public int? ModeleId { get; set; }
        public string IMEI { get; set; }
        public int? RepairTypeId { get; set; }
        public int? UnlockTypeId { get; set; }
        public int? Quantity { get; set; }
        public int? ArticleId { get; set; }
        public decimal Price { get; set; }
        public decimal? Accompte { get; set; }
        public decimal? ResteDu { get; set; }
        public decimal? Paid { get; set; }
        public int PaymentModeId { get; set; }
        public Invoice(
            int id, DateTime date, int customerId, int? marqueId, int? modeleId,
            string imei, int? repairTypeId, int? unlockTypeId, int? quantity, int? articleId,
            decimal price, decimal? accompte, decimal? resteDu, decimal? paid,
            int paymentModeId)
        {
            Id = id;
            Date = date;
            CustomerId = customerId;
            MarqueId = marqueId;
            ModeleId = modeleId;
            IMEI = imei;
            RepairTypeId = repairTypeId;
            UnlockTypeId = unlockTypeId;
            Quantity = quantity;
            ArticleId = articleId;
            Price = price;
            Accompte = accompte;
            ResteDu = resteDu;
            Paid = paid;
            PaymentModeId = paymentModeId;
        }
        public Invoice()
        {
        }
    }
    public class Article
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public Article(int id, string name, decimal price, int quantity, string description)
        {
            Id = id;
            Name = name;
            Price = price;
            Quantity = quantity;
            Description = description;
        }
    }

    public class HistoriqueAllView
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public int InvoiceCount { get; set; } = 0;
        public DateTime LastInvoiceDate { get; set; }
    }
    public class HistoriqueOneView
    {
        public int InvoiceNumber { get; set; }
        public string InvoiceType { get; set; }
        public string Date { get; set; }
        public decimal Price { get; set; }
        public bool Paid { get; set; }
        public string PaymentMode { get; set; }
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
