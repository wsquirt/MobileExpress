using System;
using System.Collections.Generic;

namespace MobileExpress
{
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
        public string ModeDePaiement { get; set; }
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
        public bool? GarantieOption;
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
            decimal? price, int? garantie, bool? garantieOption, decimal? remise, decimal? accompte, decimal? resteDu,
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
            GarantieOption = garantieOption;
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
        public bool? Option;
        public Garantie() { }
        public Garantie(string tabName, int id, string productName, int? months, bool? option)
        {
            TabName = tabName;
            Id = id;
            ProductName = productName;
            Months = months;
            Option = option;
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
        public bool GarantieOption;
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
