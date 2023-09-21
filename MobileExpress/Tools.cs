using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Text;
using Microsoft.Office.Interop.Word;
using Xceed.Words.NET;
using ComboBox = System.Windows.Forms.ComboBox;
using System.Linq;
using Task = System.Threading.Tasks.Task;

namespace MobileExpress
{
    public static class Tools
    {
        public static string TemplateFacturePath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Template facture.docx";
        public static string TemplateRecuPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Template prise en charge.docx";
        public static void GenerateDocx(
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
            string title,
            Dictionary<int, string> repairTypeNames,
            Dictionary<int, string> unlockTypeNames,
            Dictionary<int, string> articles,
            Dictionary<int, string> marques,
            Dictionary<int, string> modeles)
        {
            try
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
                int priseEnChargeId = 0;
                int invoiceId = 0;
                string articlesHtml = string.Empty;
                List<DocxArticle> docxArticles = new List<DocxArticle>();
                foreach (MEData mEData in mEDatas)
                {
                    if (priseEnChargeId == 0)
                    {
                        priseEnChargeId = mEData.TakeOverId;
                    }
                    if (type == 1 && invoiceId == 0 && mEData.InvoiceId.HasValue)
                    {
                        invoiceId = mEData.InvoiceId.Value;
                    }
                    if (type == 0 && mEData.ArticleId.HasValue)
                    {
                        paid -= (mEData.Price.Value * mEData.Quantity + (mEData.Quantity * (mEData.Remise ?? 0) * -1));
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
                        docxArticles.Add(new DocxArticle()
                        {
                            Name = func(articlename, marquename, modelename, imei),
                            Quantity = mEData.Quantity.ToString(),
                            Price = string.Empty,
                            Total = string.Empty,
                        });
                        docxArticles.Add(new DocxArticle()
                        {
                            Name = "Vérification - " + func(articlename, marquename, modelename, imei),
                            Quantity = mEData.Quantity.ToString(),
                            Price = mEData.Price.Value.ToString(),
                            Total = (mEData.Quantity * mEData.Price.Value).ToString(),
                        });
                        total += (mEData.Quantity * mEData.Price.Value);
                    }
                    else
                    {
                        docxArticles.Add(new DocxArticle()
                        {
                            Name = func(articlename, marquename, modelename, imei),
                            Quantity = mEData.Quantity.ToString(),
                            Price = mEData.Price.ToString(),
                            Total = (mEData.Quantity * mEData.Price.Value).ToString(),
                        });
                        total += (mEData.Quantity * mEData.Price.Value);
                    }
                    if (type == 1 && mEData.Remise.HasValue)
                    {
                        docxArticles.Add(new DocxArticle()
                        {
                            Name = "Remise - " + func(articlename, marquename, modelename, imei),
                            Quantity = mEData.Quantity.ToString(),
                            Price = (mEData.Remise.Value * -1).ToString(),
                            Total = (mEData.Quantity * mEData.Remise.Value * -1).ToString(),
                        });
                        total += (mEData.Quantity * mEData.Remise.Value * -1);
                    }
                    if (type == 1 && mEData.Garantie.HasValue)
                    {
                        docxArticles.Add(new DocxArticle()
                        {
                            Name = $"Garantie {$"{mEData.Garantie.Value.ToString()} mois"} - " + func(articlename, marquename, modelename, imei),
                            Quantity = mEData.Quantity.ToString(),
                            Price = string.Empty,
                            Total = string.Empty,
                        });
                    }
                }

                string directoryPath = type == 0 ? Paths.PriseEnChargeDirectory : Paths.FactureDirectory; // Remplacez par le chemin de votre répertoire
                                                                                                          // Modèle de fichier recherché
                string priseEnChargeSearchPattern = $@"PriseEnCharge_{(priseEnChargeId < 10 ? $"0{priseEnChargeId}" : $"{priseEnChargeId}")}_*.docx";
                string factureSearchPattern = $@"Facture_{(invoiceId < 10 ? $"0{invoiceId}" : $"{invoiceId}")}_PriseEnCharge_{(priseEnChargeId < 10 ? $"0{priseEnChargeId}" : $"{priseEnChargeId}")}_*.docx";
                string searchPattern = type == 0 ? priseEnChargeSearchPattern : factureSearchPattern;
                // Vérifiez si des fichiers correspondent au modèle dans le répertoire
                string[] files = Directory.GetFiles(directoryPath, searchPattern);
                foreach (var filePath in files)
                {
                    // Supprimez chaque fichier trouvé
                    File.Delete(filePath);
                }

                Task task = new Task(delegate {
                    CreateDocx(new DocxData()
                    {
                        Path = path,
                        Type = type,
                        Logo = logo,
                        Date = takeOverDate,
                        Number = takeOverNumber,
                        CustomerName = customerName,
                        CustomerPhone = customerPhone,
                        CustomerEmail = customerEmail,
                        Total = (total == decimal.Zero ? string.Empty : $"{total}"),
                        Accompte = $"{(accompte ?? decimal.Zero)}",
                        Paid = paid.ToString(),
                        ResteDu = (total - paid - (accompte ?? decimal.Zero)).ToString(),
                        CB = carteBleu ? "X" : " ",
                        ESP = espece ? "X" : " ",
                        VIR = virement ? "X" : " ",
                        Articles = docxArticles,
                    });
                });
                task.Start();
                task.Wait();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private static void CreateDocx(DocxData docxData)
        {
            try
            {
                // Charger le modèle de facture .docx
                var doc = DocX.Load(docxData.Type == 0 ? TemplateRecuPath : TemplateFacturePath);

                // Remplacer les balises par les valeurs correspondantes
                doc.ReplaceText("{{date}}", docxData.Date);
                doc.ReplaceText("{{number}}", docxData.Number);
                doc.ReplaceText("{{customerName}}", docxData.CustomerName);
                doc.ReplaceText("{{customerPhone}}", docxData.CustomerPhone);
                doc.ReplaceText("{{customerEmail}}", docxData.CustomerEmail);
                doc.ReplaceText("{{total}}", docxData.Total);
                doc.ReplaceText("{{accompte}}", docxData.Accompte);
                doc.ReplaceText("{{paid}}", docxData.Paid);
                doc.ReplaceText("{{resteDu}}", docxData.ResteDu);
                doc.ReplaceText("{{cb}}", docxData.CB);
                doc.ReplaceText("{{esp}}", docxData.ESP);
                doc.ReplaceText("{{vir}}", docxData.VIR);

                // Récupérer la table à partir du modèle (en supposant qu'elle soit la première table dans le document)
                var table1 = doc.Tables[5];
                var table2 = doc.Tables[12];
                // Remplir la table avec les données des articles
                foreach (var article in docxData.Articles)
                {
                    var row1 = table1.InsertRow();
                    row1.Cells[0].Paragraphs.First().Append(article.Name);
                    row1.Cells[0].Paragraphs.First().FontSize(9);
                    row1.Cells[1].Paragraphs.First().Append(article.Quantity);
                    row1.Cells[1].Paragraphs.First().FontSize(9);
                    row1.Cells[2].Paragraphs.First().Append(article.Price);
                    row1.Cells[2].Paragraphs.First().FontSize(9);
                    row1.Cells[3].Paragraphs.First().Append(article.Total);
                    row1.Cells[3].Paragraphs.First().FontSize(9);
                    var row2 = table2.InsertRow();
                    row2.Cells[0].Paragraphs.First().Append(article.Name);
                    row2.Cells[0].Paragraphs.First().FontSize(9);
                    row2.Cells[1].Paragraphs.First().Append(article.Quantity);
                    row2.Cells[1].Paragraphs.First().FontSize(9);
                    row2.Cells[2].Paragraphs.First().Append(article.Price);
                    row2.Cells[2].Paragraphs.First().FontSize(9);
                    row2.Cells[3].Paragraphs.First().Append(article.Total);
                    row2.Cells[3].Paragraphs.First().FontSize(9);
                }

                // Enregistrez le document modifié
                doc.SaveAs(docxData.Path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
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
        public static string GetEnumDescriptionFromEnum<T>(T enumValue) where T : Enum
        {
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : enumValue.ToString();
        }
        public static T GetEnumFromDescription<T>(string description) where T : Enum
        {
            foreach (T enumValue in Enum.GetValues(typeof(T)))
            {
                if (GetEnumDescriptionFromEnum(enumValue) == description)
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

        public static (bool, bool, bool) GetBoolFromPaymentMode(PaymentMode paymentMode)
        {
            return (
                paymentMode == PaymentMode.CB ? (true, false, false) :
                paymentMode == PaymentMode.ESP ? (false, true, false) :
                paymentMode == PaymentMode.CBESP ? (true, true, false) :
                paymentMode == PaymentMode.VIR ? (false, false, true) :
                paymentMode == PaymentMode.CBVIR ? (true, false, true) :
                paymentMode == PaymentMode.ESPVIR ? (false, true, true) :
                paymentMode == PaymentMode.CBESPVIR ? (true, true, true) :
                (false, false, false)
            );
        }
        public static PaymentMode GetPaymentModeFromBool(bool cb, bool espece, bool virement)
        {
            return
                cb && espece && virement ? PaymentMode.CBESPVIR :
                cb && espece ? PaymentMode.CBESP :
                cb && virement ? PaymentMode.CBVIR :
                cb ? PaymentMode.CB :
                espece && virement ? PaymentMode.ESPVIR :
                espece ? PaymentMode.ESP :
                virement ? PaymentMode.VIR :
                PaymentMode.NONE;
        }
    }
    public static class Paths
    {
        public static string LogoPath = @"C:\Users\merto\OneDrive\Documents\Nicepage\Site_26231059\images\Logoeducdombleu.png";

        public static string MarquesDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Marques.csv";
        public static string ModelesDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Modèles.csv";
        public static string UnlockTypesDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Types de déblocage.csv";
        public static string RepairTypesDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Types de réparation.csv";
        public static string PaymentModesDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Configuration\Modes de paiement.csv";
        
        public static string ReceiptDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Ticket de caisse.csv";
        public static string CustomersDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Client.csv";
        public static string StockDSPath = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Article.csv";

        public static string PriseEnChargeDirectory = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Prises en charges\";
        public static string FactureDirectory = @"C:\Users\merto\OneDrive\Documents\MobileExpressApp\Factures\";
    }
    public class DocxData
    {
        public int Type { get; set; }
        public string Path { get; set; }
        public string Logo { get; set; }
        public string Date { get; set; }
        public string Number { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public string Total { get; set; }
        public string Accompte { get; set; }
        public string Paid { get; set; }
        public string ResteDu { get; set; }
        public string CB { get; set; }
        public string ESP { get; set; }
        public string VIR { get; set; }
        public List<DocxArticle> Articles { get; set; }
    }
    public class DocxArticle
    {
        public string Name { get; set; }
        public string Quantity { get; set; }
        public string Price { get; set; }
        public string Total { get; set; }
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
    public enum PaymentMode
    {
        [Description("Non payé")]
        NONE = 0,
        [Description("CB")]
        CB = 1,
        [Description("ESP")]
        ESP = 2,
        [Description("CB/ESP")]
        CBESP = 3,
        [Description("VIR")]
        VIR = 4,
        [Description("CB/VIR")]
        CBVIR = 5,
        [Description("ESP/VIR")]
        ESPVIR = 6,
        [Description("CB/ESP/VIR")]
        CBESPVIR = 7,
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
        public PaymentMode PaymentMode;
        public TakeOverState State;
        public bool Verification;
        public MEData(
            int takeOverId, DateTime date, int? invoiceId, int customerId, int? marqueId, int? modeleId,
            string imei, int? repairTypeId, int? unlockTypeId, int? articleId, int quantity,
            decimal? price, int? garantie, decimal? remise, decimal? accompte, decimal? resteDu,
            decimal? paid, decimal total, PaymentMode paymentMode, TakeOverState state, int id, bool verification)
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
    public enum StockAction
    {
        // 1: Achat, 2: Ajout, 3: Ajout + Achat, 4: Mise à jour, 5: Suppression
        Achat = 1,
        Ajout = 2,
        AchatAjout = 3,
        MiseAJour = 4,
        Suppression = 5,
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
        public string UPC;
        public string EAN;
        public string GTIN;
        public string ISBN;
        public int? MarqueId;
        public int? ModeleId;
        public string Name;
        public decimal Price;
        public int Quantity;
        public Article(int id, int? marqueId, int? modeleId, string name, decimal price, int quantity, string upc, string ean, string gtin, string isbn)
        {
            Id = id;
            MarqueId = marqueId;
            ModeleId = modeleId;
            Name = name;
            Price = price;
            Quantity = quantity;
            UPC = upc;
            EAN = ean;
            GTIN = gtin;
            ISBN = isbn;
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

    public class Item
    {
        public string ean { get; set; }
        public string title { get; set; }
        public string upc { get; set; }
        public string isbn { get; set; }
        public string gtin { get; set; }
        public string asin { get; set; }
        public string description { get; set; }
        public string brand { get; set; }
        public string model { get; set; }
        public string dimension { get; set; }
        public string weight { get; set; }
        public string category { get; set; }
        public string currency { get; set; }
        public double lowest_recorded_price { get; set; }
        public double highest_recorded_price { get; set; }
        public List<string> images { get; set; }
        public List<Offer> offers { get; set; }
    }
    public class Offer
    {
        public string merchant { get; set; }
        public string domain { get; set; }
        public string title { get; set; }
        public string currency { get; set; }
        public double? list_price { get; set; }
        public double price { get; set; }
        public string shipping { get; set; }
        public string condition { get; set; }
        public string availability { get; set; }
        public string link { get; set; }
        public int updated_t { get; set; }
    }
    public class Root
    {
        public string code { get; set; }
        public int total { get; set; }
        public int offset { get; set; }
        public List<Item> items { get; set; }
    }
}
