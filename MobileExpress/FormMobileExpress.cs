using Microsoft.Office.Interop.Word;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.UI.WebControls;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Xml.Linq;
using Xceed.Document.NET;
using static Common.Logging.Configuration.ArgUtils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Color = System.Drawing.Color;
using ComboBox = System.Windows.Forms.ComboBox;
using Control = System.Windows.Forms.Control;
using DataTable = System.Data.DataTable;
using TextBox = System.Windows.Forms.TextBox;

namespace MobileExpress
{
    public partial class FormMobileExpress : Form
    {
        #region Attributs
        private List<Marque> MarquesDS { get; set; } = new List<Marque>();
        private List<Modele> ModelesDS { get; set; } = new List<Modele>();
        private List<PaymentMode> PaymentModesDS { get; set; } = new List<PaymentMode>();
        private List<MEData> MEDatasDS { get; set; } = new List<MEData>();
        private List<Customer> CustomersDS { get; set; } = new List<Customer>();
        private List<TakeOverType> TakeOverTypesDS { get; set; } = new List<TakeOverType>();
        private List<Garantie> GarantiesTemp { get; set; } = new List<Garantie>();
        private List<Remise> RemisesTemp { get; set; } = new List<Remise>();

        // Propriété publique pour accéder au TextBox depuis une autre classe
        public TextBox ScannerBarcode { get; set; } = new TextBox();

        #region Scanner
        public delegate Article updateTextBoxScannerBarcodeDelegate(string barcode);
        #endregion

        private Customer CurrentCustomer { get; set; }
        #endregion

        public FormMobileExpress()
        {
            InitializeComponent();

            // Configuration du TextBox
            ScannerBarcode.Visible = false; // Rend le TextBox invisible
            // Ajout du TextBox au formulaire
            this.Controls.Add(ScannerBarcode);

            tabControlAll.SelectedIndexChanged += new EventHandler(TabControllAll_SelectedIndexChanged);
            dataGridViewCustomerRelationAll.CellClick += new DataGridViewCellEventHandler(DataGridViewCustomerRelationAll_CellClick);
            textBoxCustomerSearchAll.KeyUp += new KeyEventHandler(TextBoxCustomerSearchAll_KeyUp);
            textBoxStockSearch.KeyUp += new KeyEventHandler(TextBoxStockSearch_KeyUp);
            dataGridViewStock.CellContentClick += new DataGridViewCellEventHandler(DataGridViewStock_CellContentClick);
            dataGridViewStock.CellMouseClick += new DataGridViewCellMouseEventHandler(DataGridViewStock_CellMouseClick);

            InitializeAll();
        }

        private void InitializeAll()
        {
            // initialiser les configurations
            // créer ou éditer Marques
            InitializeMarques();
            // créer ou éditer Modèles
            InitializeModeles();
            // créer ou éditer Type d'intervention
            InitializeRepairTypes();
            // créer ou éditer Type d'intervention
            InitializeUnlockTypes();
            // initialiser stock
            InitializeStock();
            // initialiser tickets de caisse
            InitializeReceipts();
            // initialiser clients
            InitializeCustomers();
            // initialiser numéro facture
            InitializeTakeOverNumber();
            // Prise en charge
            GarantiesTemp = new List<Garantie>();
            RemisesTemp = new List<Remise>();
            dateTimePickerTakeOverDate.Format = DateTimePickerFormat.Custom;
            dateTimePickerTakeOverDate.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            takeOverRepair_Load(null);
            takeOverUnlock_Load(null);
            takeOverAchat_Load(null);
            textBoxTakeOverTotalPrice_Load();
            textBoxTakeOverAccompte.Text = 0.ToString();
            textBoxTakeOverPaid.Text = 0.ToString();
            textBoxTakeOverResteDu.Text = 0.ToString();
            checkBoxCb.Checked = false;
            checkBoxEspece.Checked = false;
            checkBoxVirement.Checked = false;

            comboBoxTakeOverCustomer.Text = string.Empty;
            textBoxTakeOverLastName_Load(null);
            // Relation cliente
            dataGridViewAllCustomerRelation_Load(null);
            // Stock
            dataGridViewStock_Load(null);

            InitializeScanner();
        }

        #region Initialisation des listes
        private void InitializeRepairTypes()
        {
            string path = Paths.RepairTypesDSPath;

            try
            {
                if (!File.Exists(path))
                    throw new Exception($"Le fichier {path} n'existe pas.");
                using (StreamReader reader = new StreamReader(path))
                {
                    if (reader == null)
                        throw new Exception($"La lecture du fichier {path} a échoué.");
                    int linenum = 0;
                    string line;
                    List<RepairType> items = new List<RepairType>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (linenum > 0 && !string.IsNullOrWhiteSpace(line))
                        {
                            string[] temp = line.Split(';');
                            items.Add(new RepairType(Int32.Parse(temp[0]), temp[1], decimal.Parse(temp[2])));
                        }
                        linenum++;
                    }
                    TakeOverTypesDS.Add(new TakeOverType(0, "Réparation", items, null, null));
                    TakeOverTypesDS = TakeOverTypesDS.OrderBy(x => x.Name).ToList();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur : " + e.Message);
            }
        }
        private void InitializeUnlockTypes()
        {
            string path = Paths.UnlockTypesDSPath;

            try
            {
                if (!File.Exists(path))
                    throw new Exception($"Le fichier {path} n'existe pas.");
                using (StreamReader reader = new StreamReader(path))
                {
                    if (reader == null)
                        throw new Exception($"La lecture du fichier {path} a échoué.");
                    int linenum = 0;
                    string line;
                    List<UnlockType> items = new List<UnlockType>();
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (linenum > 0 && !string.IsNullOrWhiteSpace(line))
                        {
                            string[] temp = line.Split(';');
                            items.Add(new UnlockType(Int32.Parse(temp[0]), temp[1], decimal.Parse(temp[2])));
                        }
                        linenum++;
                    }
                    TakeOverTypesDS.Add(new TakeOverType(1, "Déblocage", null, items, null));
                    TakeOverTypesDS = TakeOverTypesDS.OrderBy(x => x.Name).ToList();

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur : " + e.Message);
            }
        }
        private void InitializeStock()
        {
            string path = Paths.StockDSPath;

            try
            {
                if (!File.Exists(path))
                    throw new Exception($"Le fichier {path} n'existe pas.");

                List<Article> items = TakeOverTypesDS.FirstOrDefault(x => x.Id == 2)?.Articles;

                if (items == null)
                {
                    TakeOverTypesDS.Add(new TakeOverType(2, "Achat", null, null, new List<Article>()));
                    items = new List<Article>();
                }
                else if (items.Any())
                    TakeOverTypesDS.ForEach(x => { if (x.Id == 2) { x.Articles.Clear(); } });

                using (StreamReader reader = new StreamReader(path))
                {
                    if (reader == null)
                        throw new Exception($"La lecture du fichier {path} a échoué.");
                    int rowIndex = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (rowIndex > 0 && !string.IsNullOrWhiteSpace(line))
                        {
                            string[] temp = line.Split(';');
                            int Id = Int32.Parse(temp[0]);
                            int? MarqueId = Tools.ToNullableInt(temp[1]);
                            int? ModeleId = Tools.ToNullableInt(temp[2]);
                            string Nom = temp[3];
                            decimal Prix = decimal.Parse(temp[4].Replace('.', ','));
                            int Quantite = Int32.Parse(temp[5]);
                            string CodeReference = temp[6];
                            string DisplayText = $"{Nom}" +
                                (MarqueId == null ? string.Empty : $" - {MarquesDS.FirstOrDefault(x => x.Id == MarqueId).Name}") +
                                (ModeleId == null ? string.Empty : $" {ModelesDS.FirstOrDefault(x => x.Id == ModeleId && x.MarqueId == MarqueId)?.Name ?? string.Empty}") +
                                (string.IsNullOrWhiteSpace(CodeReference) ? string.Empty : $" - {CodeReference}");
                            Article article = new Article(Id, MarqueId, ModeleId, Nom, Prix, Quantite, CodeReference)
                            {
                                DisplayText = DisplayText
                            };
                            items.Add(article);
                        }
                        rowIndex++;
                    }
                    items = items.OrderBy(a => a.Produit).ToList();
                    TakeOverTypesDS.ForEach(x => { if (x.Id == 2) { x.Articles = items; } });
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Stock : " + e.Message);
            }
        }
        private void InitializeMarques()
        {
            string path = Paths.MarquesDSPath;

            try
            {
                if (!File.Exists(path))
                {
                    throw new Exception($"The file {path} does not exist.");
                }

                using (StreamReader reader = new StreamReader(path))
                {
                    if (reader == null)
                    {
                        throw new Exception($"Failed to read the file {path}.");
                    }

                    int lineNum = 0;
                    string line;

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (lineNum > 0 && !string.IsNullOrWhiteSpace(line))
                        {
                            string[] temp = line.Split(';');
                            MarquesDS.Add(new Marque(Int32.Parse(temp[0]), temp[1]));
                        }
                        lineNum++;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error: " + e.Message);
            }
        }
        private void InitializeModeles()
        {
            string path = Paths.ModelesDSPath;

            try
            {
                if (!File.Exists(path))
                    throw new Exception($"Le fichier {path} n'existe pas.");
                using (StreamReader reader = new StreamReader(path))
                {
                    if (reader == null)
                        throw new Exception($"La lecture du fichier {path} a échoué.");
                    int linenum = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (linenum > 0 && !string.IsNullOrWhiteSpace(line))
                        {
                            string[] temp = line.Split(';');
                            ModelesDS.Add(new Modele(Int32.Parse(temp[0]), Int32.Parse(temp[1]), temp[2]));
                        }
                        linenum++;
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur : " + e.Message);
            }
        }
        private void InitializeReceipts()
        {
            string path = Paths.ReceiptDSPath;

            try
            {
                if (!File.Exists(path))
                    throw new Exception($"Le fichier {path} n'existe pas.");

                if (MEDatasDS == null)
                    MEDatasDS = new List<MEData>();

                if (MEDatasDS?.Any() ?? false)
                    MEDatasDS.Clear();

                using (StreamReader reader = new StreamReader(path))
                {
                    if (reader == null)
                        throw new Exception($"La lecture du fichier {path} a échoué.");
                    int linenum = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (linenum > 0 && !string.IsNullOrWhiteSpace(line))
                        {
                            string[] temp = line.Split(';');
                            if (!string.IsNullOrWhiteSpace(temp[0]))
                            {
                                int takeOverId = Int32.Parse(temp[0]);
                                DateTime date = DateTime.Parse(temp[1]);
                                int? invoiceId = Tools.ToNullableInt(temp[2]);
                                int customerId = Int32.Parse(temp[3]);
                                int? marqueId = Tools.ToNullableInt(temp[4]);
                                int? modeleId = Tools.ToNullableInt(temp[5]);
                                string imei = temp[6];
                                int? repairTypeId = Tools.ToNullableInt(temp[7]);
                                int? unlockTypeId = Tools.ToNullableInt(temp[8]);
                                int? articleId = Tools.ToNullableInt(temp[9]);
                                string displayText = null;
                                int quantity = Int32.Parse(temp[10]);
                                decimal price = decimal.Parse(temp[11]);
                                int? monthsGarantie = Tools.ToNullableInt(temp[12]);
                                decimal? remise = Tools.ToNullableDecimal(temp[13]);
                                decimal? accompte = Tools.ToNullableDecimal(temp[14]);
                                decimal? resteDu = Tools.ToNullableDecimal(temp[15]);
                                decimal? paid = Tools.ToNullableDecimal(temp[16]);
                                decimal total = decimal.Parse(temp[17]);
                                PaymentMode paymentMode = Tools.GetEnumFromDescription<PaymentMode>(temp[18]);
                                TakeOverState state = Tools.GetEnumFromDescription<TakeOverState>(temp[19]);
                                int id = Int32.Parse(temp[20]);
                                bool verification = string.Compare("Oui", temp[21]) == 0;
                                bool optionGarantie = string.Compare("Oui", temp[22]) == 0;
                                Article article = TakeOverTypesDS.First(x => x.Id == 2).Articles.FirstOrDefault(x => x.Id == articleId);
                                if (article != null)
                                {
                                    displayText = $"{article.Produit}" +
                                        (marqueId == null ? string.Empty : $" - {MarquesDS.FirstOrDefault(x => x.Id == marqueId).Name}") +
                                        (modeleId == null ? string.Empty : $" {ModelesDS.FirstOrDefault(x => x.Id == modeleId && x.MarqueId == marqueId)?.Name ?? string.Empty}") +
                                        (string.IsNullOrWhiteSpace(article.CodeReference) ? string.Empty : $" - {article.CodeReference}");
                                }
                                MEDatasDS.Add(new MEData(
                                    takeOverId, date, invoiceId, customerId, marqueId, modeleId, imei, repairTypeId, unlockTypeId, articleId, displayText,
                                    quantity, price, monthsGarantie, remise, accompte, resteDu, paid, total, paymentMode, state, id, verification, optionGarantie));
                            }
                        }
                        linenum++;
                    }
                    MEDatasDS = MEDatasDS.OrderByDescending(x => x.Date).ThenByDescending(x => x.TakeOverId).ThenBy(x => x.Id).ToList();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Factures : " + e.Message);
            }
        }
        private void InitializeTakeOverNumber()
        {
            try
            {
                takeOverNumber.Text = ((MEDatasDS?.Any() ?? false) ? (MEDatasDS.OrderByDescending(x => x.Date).First().TakeOverId + 1) : 1).ToString();
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur : " + e.Message);
            }
        }
        private void InitializeCustomers()
        {
            string path = Paths.CustomersDSPath;

            try
            {
                if (!File.Exists(path))
                    throw new Exception($"Le fichier {path} n'existe pas.");

                if (CustomersDS == null)
                    CustomersDS = new List<Customer>();

                if (CustomersDS?.Any() ?? false)
                    CustomersDS.Clear();

                using (StreamReader reader = new StreamReader(path))
                {
                    if (reader == null)
                        throw new Exception($"La lecture du fichier {path} a échoué.");
                    int linenum = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (linenum > 0 && !string.IsNullOrWhiteSpace(line))
                        {
                            //Id;Nom;Prénom;Numéro de téléphone;Adresse mail;Sexe
                            string[] temp = line.Split(';');
                            Customer customer = new Customer(
                                Int32.Parse(temp[0]),
                                temp[1],
                                temp[2],
                                temp[3],
                                temp[4],
                                Tools.GetEnumFromDescription<Sexe>(temp[5]));
                            CustomersDS.Add(customer);
                        }
                        linenum++;
                    }
                    CustomersDS = CustomersDS.OrderBy(x => x.LastName).OrderBy(x => x.FirstName).ToList();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Clients : " + e.Message);
            }
        }
        private void InitializeLists()
        {
            InitializeCustomers();
            InitializeReceipts();
            InitializeStock();
        }

        private void InitializeScanner()
        {
            Services.InitializeScanner(this, new updateTextBoxScannerBarcodeDelegate(GetArticleFromBarcode));
        }
        private Article GetArticleFromBarcode(string txtFromScanner)
        {
            Article article = null;
            if (tabControlAll.SelectedTab == tabPageTakeOver)
            {
                article = GetArticleFromBarcodeAchat(txtFromScanner);
            }
            else if (tabControlAll.SelectedTab == stockTabPage)
            {
                article = GetArticleFromBarcodeStock(txtFromScanner);
            }
            return article;
        }
        private Article GetArticleFromBarcodeAchat(string txtFromScanner)
        {
            Article article = null;
            try
            {
                if (string.IsNullOrWhiteSpace(txtFromScanner))
                    return null;

                if (tabControlTakeOver.SelectedTab != tabControlTakeOver.TabPages[2])
                    tabControlTakeOver.SelectedTab = tabControlTakeOver.TabPages[2];

                bool isArticleInStock = Services.CheckArticleInStock(txtFromScanner, TakeOverTypesDS.First(x => x.Id == 2).Articles);
                bool isArticleInDataGridView = Services.CheckArticleInDataGridView(txtFromScanner, dataGridViewTakeOverAchat, 1);

                if (!isArticleInStock)
                {
                    // si l'article n'est pas connu
                    // - récupérer depuis barcode url
                    if (Services.IsScannerOK)
                        article = Services.GetBarcodeDataFromUrlAsync(txtFromScanner, MarquesDS, ModelesDS, TakeOverTypesDS).Result;
                    // - form
                    var articleForm = new ArticleForm(
                            StockAction.Ajout
                            , TakeOverTypesDS.First(x => x.Id == 2).Articles
                            , MarquesDS
                            , ModelesDS
                            , txtFromScanner
                            , article);
                    articleForm.ShowDialog();
                    (StockAction action, Article articleTmp, List<Article> articles, List<Marque> marques, List<Modele> modeles) =
                        articleForm.GetResult();
                    article = articleTmp;
                    MarquesDS = marques;
                    ModelesDS = modeles;
                    TakeOverTypesDS.First(x => x.Id == 2).Articles = articles;
                    ;

                    isArticleInStock = true;
                }
                if (isArticleInStock)
                {
                    if (article == null)
                    {
                        article = TakeOverTypesDS.First(x => x.Id == 2).Articles.First(x => string.Compare(x.CodeReference.ToLowerInvariant(), txtFromScanner.ToLowerInvariant()) == 0);
                    }

                    if (!isArticleInDataGridView && article != null)
                    {
                        // si l'article est connu
                        // mettre à jour la combobox colonne pour prendre en compte le nouvelle article
                        // ajouter dans data grid view
                        ((DataGridViewComboBoxColumn)dataGridViewTakeOverAchat.Columns["article"]).Items.Add(article.DisplayText);

                        int index = dataGridViewTakeOverAchat.Rows.Add();

                        SetCellValue(dataGridViewTakeOverAchat, "id", index, article.Id);
                        SetCellValue(dataGridViewTakeOverAchat, "article", index, article.DisplayText);
                        SetCellValue(dataGridViewTakeOverAchat, "quantity", index, 1);
                        SetCellValue(dataGridViewTakeOverAchat, "price", index, article.Price);
                        SetCellValue(dataGridViewTakeOverAchat, "garantie", index, false);
                        SetCellValue(dataGridViewTakeOverAchat, "remise", index, false);

                        ScannerBarcode.Clear();

                        UpdateTotalPrice();
                    }

                    else if (isArticleInDataGridView && article != null)
                    {
                        // si l'article est connu + dans data grid view
                        // incrémenter dans data grid view

                        foreach (DataGridViewRow row in dataGridViewTakeOverAchat.Rows)
                        {
                            if (!row.IsNewRow) // incrément quantité
                            {
                                string articleName = row.Cells["article"].Value as string;
                                int? quantityTmp = (row.Cells["quantity"].Value as int?);
                                int quantity = quantityTmp.HasValue ? quantityTmp.Value + 1 : 1;
                                if (!string.IsNullOrWhiteSpace(articleName) && articleName.Trim().ToLower().Contains(article.Produit.Trim().ToLower()))
                                {
                                    SetCellValue(dataGridViewTakeOverAchat, "quantity", row.Index, quantity);

                                    ScannerBarcode.Clear();

                                    UpdateTotalPrice();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }

            return article;
        }
        private Article GetArticleFromBarcodeStock(string txtFromScanner)
        {
            
            // sinon, si l'article est en stock
            // - on ouvre l'article dans le formulaire
            // - si c'est une sauvegarde
            // -- on sauvegarde l'article dans le stock
            // -- on recharge la liste
            // - sinon, si c'est une supression
            // -- on suprime l'article du stock
            // -- on recharge la liste

            Article article = null;
            try
            {
                if (string.IsNullOrWhiteSpace(txtFromScanner))
                    return null;

                if (txtFromScanner.Length == 0)
                    return null;

                bool isArticleInStock = Services.CheckArticleInStock(txtFromScanner, TakeOverTypesDS.First(x => x.Id == 2).Articles);
                bool isArticleInDataGridView = Services.CheckArticleInDataGridView(txtFromScanner, dataGridViewStock, 1);

                if (!isArticleInStock)
                {
                    // si l'article n'est pas en stock
                    
                    // - on recherche l'article par url
                    if (Services.IsScannerOK)
                        article = Services.GetBarcodeDataFromUrlAsync(txtFromScanner, MarquesDS, ModelesDS, TakeOverTypesDS).Result;

                    // - on ouvre l'article dans le formulaire
                    var articleForm = new ArticleForm(
                            StockAction.Ajout
                            , TakeOverTypesDS.First(x => x.Id == 2).Articles
                            , MarquesDS
                            , ModelesDS
                            , txtFromScanner
                            , article);
                    articleForm.ShowDialog();
                    (StockAction action, Article articleTmp, List<Article> articles, List<Marque> marques, List<Modele> modeles) =
                        articleForm.GetResult();
                    article = articleTmp;
                    MarquesDS = marques;
                    ModelesDS = modeles;
                    TakeOverTypesDS.First(x => x.Id == 2).Articles = articles;

                    // - on recharge la liste
                    dataGridViewStock_Load(TakeOverTypesDS.First(x => x.Id == 2).Articles);

                    ScannerBarcode.Clear();
                }
                else if (!isArticleInDataGridView)
                {
                    // - recharge de la tab
                    dataGridViewStock_Load(TakeOverTypesDS.First(x => x.Id == 2).Articles);
                    ScannerBarcode.Clear();
                }
                else if (isArticleInStock && isArticleInDataGridView)
                {
                    if (article == null)
                    {
                        article = TakeOverTypesDS.First(x => x.Id == 2).Articles.First(x => string.Compare(x.CodeReference.ToLowerInvariant(), txtFromScanner.ToLowerInvariant()) == 0);
                    }

                    // si l'article est connu + dans datagridview
                    // - maj
                    var articleForm = new ArticleForm(
                        StockAction.MiseAJour
                        , TakeOverTypesDS.First(x => x.Id == 2).Articles
                        , MarquesDS
                        , ModelesDS
                        , txtFromScanner
                        , article);
                    articleForm.ShowDialog();
                    (StockAction action, Article articleTmp, List<Article> articles, List<Marque> marques, List<Modele> modeles) =
                        articleForm.GetResult();
                    article = articleTmp;
                    MarquesDS = marques;
                    ModelesDS = modeles;
                    TakeOverTypesDS.First(x => x.Id == 2).Articles = articles;
                    // - recharge de la tab
                    StockUpdateDatabase(article, action, true);

                    ScannerBarcode.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
            return article;
        }

        #endregion

        #region Initialisation des onglets
        private void TakeOver_Load()
        {
            textBoxTakeOverLastName_Load(null);
            textBoxTakeOverTotalPrice_Load();
            takeOverRepair_Load(null);
            takeOverUnlock_Load(null);
            takeOverAchat_Load(null);
        }
        private int GetPreferedWidthForComboBox(string text, ComboBox comboBox)
        {
            int toReturn = TextRenderer.MeasureText(text, comboBox.Font).Width;
            return toReturn;
        }
        private DataTable GetDataTableFromCustomers(List<Customer> customers)
        {
            DataTable dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[]
            {
                new DataColumn("id", typeof(int)),
                new DataColumn("name", typeof(string)),
            });

            dt.Rows.Add(0, string.Empty);

            foreach (Customer customer in customers)
            {
                dt.Rows.Add(customer.Id, Tools.GetStringFromCustomer(customer));
            }

            return dt;
        }
        private List<string> GetStringsFromCustomers(List<Customer> customers)
        {
            List<string> items = new List<string>() { string.Empty };
            foreach (Customer customer in customers)
            {
                string customerName = Tools.GetStringFromCustomer(customer);

                items.Add(customerName);
            }
            return items;
        }
        private DataTable dataTableCustomers;
        private void textBoxTakeOverLastName_Load(Customer customer)
        {
            try
            {
                if (comboBoxTakeOverCustomer.IsHandleCreated)
                {
                    comboBoxTakeOverCustomer.KeyUp -= ComboBoxTakeOverLastname_KeyUp;
                }

                List<string> strings = GetStringsFromCustomers(CustomersDS);
                dataTableCustomers = GetDataTableFromCustomers(CustomersDS);
                dataTableCustomers.CaseSensitive = false; //turn off case sensitivity for searching

                comboBoxTakeOverCustomer.DataSource = dataTableCustomers;
                comboBoxTakeOverCustomer.DisplayMember = "name";
                comboBoxTakeOverCustomer.ValueMember = "id";
                comboBoxTakeOverCustomer.KeyUp += ComboBoxTakeOverLastname_KeyUp;
                comboBoxTakeOverCustomer.DropDownWidth = GetPreferedWidthForComboBox(strings.OrderByDescending(x => x.Length).First(), comboBoxTakeOverCustomer);
                if (customer != null)
                {
                    comboBoxTakeOverCustomer.SelectedIndex = comboBoxTakeOverCustomer.FindStringExact(Tools.GetStringFromCustomer(customer));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void textBoxTakeOverTotalPrice_Load()
        {
            textBoxTakeOverTotalPrice.Text = decimal.Zero.ToString();
        }
        /*
             * Peur ? / survie
             * Honte ? / volonté
             * Culpabilité ? / plaisir
             * chagrin ? / amour
             * Mensonge ? / vérité
             * illusion ? / intériorité
             */
        private void CleanDataGridViewRepair()
        {
            this.dataGridViewTakeOverRepair.CellClick -= null;
            this.dataGridViewTakeOverRepair.CellBeginEdit -= null;
            this.dataGridViewTakeOverRepair.CellEndEdit -= null;
            this.dataGridViewTakeOverRepair.CurrentCellDirtyStateChanged -= null;
            this.dataGridViewTakeOverRepair.EditingControlShowing -= null;

            dataGridViewTakeOverRepair.Rows.Clear();
            dataGridViewTakeOverRepair.Columns.Clear();

            dataGridViewTakeOverRepair.DataSource = null;
            dataGridViewTakeOverRepair.AutoGenerateColumns = false; // add this line to disable auto-generation of columns

            dataGridViewTakeOverRepair.Columns.Add(Services.TakeOverRepairIdColumn);
            dataGridViewTakeOverRepair.Columns.Add(Services.TakeOverRepairMarqueColumn);
            dataGridViewTakeOverRepair.Columns.Add(Services.TakeOverRepairModeleColumn);
            dataGridViewTakeOverRepair.Columns.Add(Services.TakeOverRepairTypeColumn);
            dataGridViewTakeOverRepair.Columns.Add(Services.TakeOverRepairIMEIColumn);
            dataGridViewTakeOverRepair.Columns.Add(Services.TakeOverRepairPriceColumn);
            dataGridViewTakeOverRepair.Columns.Add(Services.TakeOverRepairGarantieColumn);
            dataGridViewTakeOverRepair.Columns.Add(Services.TakeOverRepairDeleteColumn);

            this.dataGridViewTakeOverRepair.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewTakeOverRepair_CellClick);
            this.dataGridViewTakeOverRepair.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.DataGridViewTakeOverRepair_CellBeginEdit);
            this.dataGridViewTakeOverRepair.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewTakeOverRepair_CellEndEdit);
            this.dataGridViewTakeOverRepair.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridViewTakeOverRepair_CurrentCellDirtyStateChanged);
            this.dataGridViewTakeOverRepair.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.DataGridViewTakeOverRepair_EditingControlShowing);
        }
        private void CleanDataGridViewUnlock()
        {
            //if (Controls.Contains(dataGridViewTakeOverUnlock))
            //{
            //    Controls.Remove(dataGridViewTakeOverUnlock);
            //}

            this.dataGridViewTakeOverUnlock.CellClick -= null;
            this.dataGridViewTakeOverUnlock.CellBeginEdit -= null;
            this.dataGridViewTakeOverUnlock.CellEndEdit -= null;
            this.dataGridViewTakeOverUnlock.CurrentCellDirtyStateChanged -= null;
            this.dataGridViewTakeOverUnlock.EditingControlShowing -= null;

            dataGridViewTakeOverUnlock.Rows.Clear();
            dataGridViewTakeOverUnlock.Columns.Clear();

            dataGridViewTakeOverUnlock.DataSource = null;
            //dataGridViewTakeOverUnlock.Dock = DockStyle.Fill;
            dataGridViewTakeOverUnlock.AutoGenerateColumns = false; // add this line to disable auto-generation of columns

            dataGridViewTakeOverUnlock.Columns.Add(Services.TakeOverUnlockIdColumn);
            dataGridViewTakeOverUnlock.Columns.Add(Services.TakeOverUnlockMarqueColumn);
            dataGridViewTakeOverUnlock.Columns.Add(Services.TakeOverUnlockModeleColumn);
            dataGridViewTakeOverUnlock.Columns.Add(Services.TakeOverUnlockTypeColumn);
            dataGridViewTakeOverUnlock.Columns.Add(Services.TakeOverUnlockIMEIColumn);
            dataGridViewTakeOverUnlock.Columns.Add(Services.TakeOverUnlockPriceColumn);
            dataGridViewTakeOverUnlock.Columns.Add(Services.TakeOverUnlockGarantieColumn);
            dataGridViewTakeOverUnlock.Columns.Add(Services.TakeOverUnlockDeleteColumn);

            this.dataGridViewTakeOverUnlock.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewTakeOverUnlock_CellClick);
            this.dataGridViewTakeOverUnlock.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.DataGridViewTakeOverUnlock_CellBeginEdit);
            this.dataGridViewTakeOverUnlock.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewTakeOverUnlock_CellEndEdit);
            this.dataGridViewTakeOverUnlock.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridViewTakeOverUnlock_CurrentCellDirtyStateChanged);
            this.dataGridViewTakeOverUnlock.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.DataGridViewTakeOverUnlock_EditingControlShowing);
            //Controls.Add(dataGridViewTakeOverUnlock);
        }
        private void CleanDataGridViewAchat()
        {
            try
            {
                this.dataGridViewTakeOverAchat.CellClick -= null;
                this.dataGridViewTakeOverAchat.CellBeginEdit -= null;
                this.dataGridViewTakeOverAchat.CurrentCellDirtyStateChanged -= null;
                this.dataGridViewTakeOverAchat.EditingControlShowing -= null;
                this.dataGridViewTakeOverAchat.CellValueChanged -= null;

                dataGridViewTakeOverAchat.Rows.Clear();
                dataGridViewTakeOverAchat.Columns.Clear();

                dataGridViewTakeOverAchat.DataSource = null;
                dataGridViewTakeOverAchat.AutoGenerateColumns = false;

                dataGridViewTakeOverAchat.Columns.Add(Services.TakeOverAchatIdColumn);
                DataGridViewComboBoxColumn articleComboboxColumn = Services.TakeOverAchatArticleCColumn;
                dataGridViewTakeOverAchat.Columns.Add(articleComboboxColumn);
                articleComboboxColumn.Items.Clear();
                foreach (string item in TakeOverTypesDS.First(x => x.Id == 2).Articles.Select(x => x.DisplayText))
                {
                    articleComboboxColumn.Items.Add(item);
                }
                dataGridViewTakeOverAchat.Columns.Add(Services.TakeOverAchatQuantityColumn);
                dataGridViewTakeOverAchat.Columns.Add(Services.TakeOverAchatPriceColumn);
                dataGridViewTakeOverAchat.Columns.Add(Services.TakeOverAchatGarantieColumn);
                dataGridViewTakeOverAchat.Columns.Add(Services.TakeOverAchatRemiseColumn);
                dataGridViewTakeOverAchat.Columns.Add(Services.TakeOverAchatDeleteColumn);

                this.dataGridViewTakeOverAchat.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridViewTakeOverAchat_CurrentCellDirtyStateChanged);
                this.dataGridViewTakeOverAchat.EditingControlShowing += new DataGridViewEditingControlShowingEventHandler(this.DataGridViewTakeOverAchat_EditingControlShowing);
                this.dataGridViewTakeOverAchat.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewTakeOverAchat_CellClick);
                this.dataGridViewTakeOverAchat.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.DataGridViewTakeOverAchat_CellBeginEdit);
                this.dataGridViewTakeOverAchat.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewTakeOverAchat_CellValueChanged);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void takeOverRepair_Load(List<MEData> mEDatas)
        {
            try
            {
                if (mEDatas?.Any() ?? false)
                {
                    CleanDataGridViewRepair();

                    Customer customer = null;
                    int index = 0;
                    foreach (MEData mEData in mEDatas)
                    {
                        dataGridViewTakeOverRepair.Rows.Add();

                        if (customer == null)
                            customer = CustomersDS.Find(x => x.Id == mEData.CustomerId);
                        if (string.IsNullOrWhiteSpace(comboBoxTakeOverCustomer.Text) ||
                            string.IsNullOrWhiteSpace(textBoxTakeOverAccompte.Text) ||
                            string.IsNullOrWhiteSpace(textBoxTakeOverResteDu.Text))
                        {
                            dateTimePickerTakeOverDate.Value = mEData.Date;
                            textBoxTakeOverLastName_Load(customer);
                        }

                        ((DataGridViewTextBoxCell)dataGridViewTakeOverRepair["id", index]).Value = mEData.Id;
                        if (dataGridViewTakeOverRepair.EditingControl is DataGridViewTextBoxEditingControl idTextBoxControl)
                        {
                            idTextBoxControl.Text = mEData.Id.ToString();
                        }

                        Marque marque = null;
                        if (mEData.MarqueId.HasValue)
                        {
                            marque = MarquesDS.FirstOrDefault(x => x.Id == mEData.MarqueId.Value);
                        }
                        ((DataGridViewTextBoxCell)dataGridViewTakeOverRepair["marque", index]).Value = marque?.Name ?? string.Empty;
                        if (dataGridViewTakeOverRepair.EditingControl is DataGridViewTextBoxEditingControl marqueTextBoxControl)
                        {
                            marqueTextBoxControl.Text = marque?.Name ?? string.Empty;
                        }

                        Modele modele = null;
                        if (mEData.ModeleId.HasValue)
                        {
                            modele = ModelesDS.FirstOrDefault(x => x.Id == mEData.ModeleId.Value);
                        }
                        ((DataGridViewTextBoxCell)dataGridViewTakeOverRepair["modele", index]).Value = modele?.Name ?? string.Empty;
                        if (dataGridViewTakeOverRepair.EditingControl is DataGridViewTextBoxEditingControl modeleTextBoxControl)
                        {
                            modeleTextBoxControl.Text = modele?.Name ?? string.Empty;
                        }

                        RepairType repairType = null;
                        if (mEData.RepairTypeId.HasValue)
                        {
                            repairType = TakeOverTypesDS.First(x => x.Id == 0).RepairTypes.FirstOrDefault(x => x.Id == mEData.RepairTypeId.Value);
                        }
                        ((DataGridViewTextBoxCell)dataGridViewTakeOverRepair["type", index]).Value = repairType?.Name ?? string.Empty;
                        if (dataGridViewTakeOverRepair.EditingControl is DataGridViewTextBoxEditingControl typeTextBoxControl)
                        {
                            typeTextBoxControl.Text = repairType?.Name ?? string.Empty;
                        }

                        ((DataGridViewTextBoxCell)dataGridViewTakeOverRepair["imei", index]).Value = mEData.IMEI;
                        if (dataGridViewTakeOverRepair.EditingControl is DataGridViewTextBoxEditingControl imeiTextBoxControl)
                        {
                            imeiTextBoxControl.Text = mEData.IMEI;
                        }

                        ((DataGridViewTextBoxCell)dataGridViewTakeOverRepair["price", index]).Value = mEData.Price;
                        if (dataGridViewTakeOverRepair.EditingControl is DataGridViewTextBoxEditingControl priceTextBoxControl)
                        {
                            priceTextBoxControl.Text = mEData.Price.ToString();
                        }

                        ((DataGridViewCheckBoxCell)dataGridViewTakeOverRepair["garantie", index]).Value = mEData.Garantie.HasValue && mEData.Garantie.Value > 0;

                        textBoxTakeOverAccompte.Text = mEData.Accompte.ToString();
                        textBoxTakeOverResteDu.Text = mEData.ResteDu.ToString();
                        textBoxTakeOverPaid.Text = mEData.Paid.ToString();
                        textBoxTakeOverTotalPrice.Text = mEData.Total.ToString();

                        index++;
                    }
                }
                else
                {
                    CleanDataGridViewRepair();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void takeOverUnlock_Load(List<MEData> mEDatas)
        {
            try
            {
                if (mEDatas?.Any() ?? false)
                {
                    CleanDataGridViewUnlock();

                    Customer customer = null;
                    int index = 0;
                    foreach (MEData mEData in mEDatas)
                    {
                        if (customer == null)
                            customer = CustomersDS.Find(x => x.Id == mEData.CustomerId);
                        if (string.IsNullOrWhiteSpace(comboBoxTakeOverCustomer.Text) ||
                            string.IsNullOrWhiteSpace(textBoxTakeOverAccompte.Text) ||
                            string.IsNullOrWhiteSpace(textBoxTakeOverResteDu.Text))
                        {
                            dateTimePickerTakeOverDate.Value = mEData.Date;
                            textBoxTakeOverLastName_Load(customer);
                        }

                        dataGridViewTakeOverUnlock.Rows.Add();

                        ((DataGridViewTextBoxCell)dataGridViewTakeOverUnlock["id", index]).Value = mEData.Id;
                        if (dataGridViewTakeOverUnlock.EditingControl is DataGridViewTextBoxEditingControl idTextBoxControl)
                        {
                            idTextBoxControl.Text = mEData.Id.ToString();
                        }

                        Marque marque = null;
                        if (mEData.MarqueId.HasValue)
                        {
                            marque = MarquesDS.FirstOrDefault(x => x.Id == mEData.MarqueId.Value);
                        }
                        ((DataGridViewTextBoxCell)dataGridViewTakeOverUnlock["marque", index]).Value = marque?.Name ?? string.Empty;
                        if (dataGridViewTakeOverUnlock.EditingControl is DataGridViewTextBoxEditingControl marqueTextBoxControl)
                        {
                            marqueTextBoxControl.Text = marque?.Name ?? string.Empty;
                        }

                        Modele modele = null;
                        if (mEData.ModeleId.HasValue)
                        {
                            modele = ModelesDS.FirstOrDefault(x => x.Id == mEData.ModeleId.Value);
                        }
                        ((DataGridViewTextBoxCell)dataGridViewTakeOverUnlock["modele", index]).Value = modele?.Name ?? string.Empty;
                        if (dataGridViewTakeOverUnlock.EditingControl is DataGridViewTextBoxEditingControl modeleTextBoxControl)
                        {
                            modeleTextBoxControl.Text = modele?.Name ?? string.Empty;
                        }

                        UnlockType unlockType = null;
                        if (mEData.UnlockTypeId.HasValue)
                        {
                            unlockType = TakeOverTypesDS.First(x => x.Id == 1).UnlockTypes.FirstOrDefault(x => x.Id == mEData.UnlockTypeId.Value);
                        }
                        ((DataGridViewTextBoxCell)dataGridViewTakeOverUnlock["type", index]).Value = unlockType?.Name ?? string.Empty;
                        if (dataGridViewTakeOverUnlock.EditingControl is DataGridViewTextBoxEditingControl typeTextBoxControl)
                        {
                            typeTextBoxControl.Text = unlockType?.Name ?? string.Empty;
                        }

                        ((DataGridViewTextBoxCell)dataGridViewTakeOverUnlock["imei", index]).Value = mEData.IMEI;
                        if (dataGridViewTakeOverUnlock.EditingControl is DataGridViewTextBoxEditingControl imeiTextBoxControl)
                        {
                            imeiTextBoxControl.Text = mEData.IMEI;
                        }

                        ((DataGridViewTextBoxCell)dataGridViewTakeOverUnlock["price", index]).Value = mEData.Price;
                        if (dataGridViewTakeOverUnlock.EditingControl is DataGridViewTextBoxEditingControl priceTextBoxControl)
                        {
                            priceTextBoxControl.Text = mEData.Price.ToString();
                        }

                        ((DataGridViewCheckBoxCell)dataGridViewTakeOverUnlock["garantie", index]).Value = mEData.Garantie.HasValue && mEData.Garantie.Value > 0;

                        textBoxTakeOverAccompte.Text = mEData.Accompte.ToString();
                        textBoxTakeOverResteDu.Text = mEData.ResteDu.ToString();
                        textBoxTakeOverPaid.Text = mEData.Paid.ToString();
                        textBoxTakeOverTotalPrice.Text = mEData.Total.ToString();

                        index++;
                    }
                }
                else
                {
                    CleanDataGridViewUnlock();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void takeOverAchat_Load(List<MEData> mEDatas)
        {
            try
            {
                if (mEDatas?.Any() ?? false)
                {
                    CleanDataGridViewAchat();

                    Customer customer = null;
                    int index = 0;
                    foreach (MEData mEData in mEDatas)
                    {
                        if (customer == null)
                            customer = CustomersDS.Find(x => x.Id == mEData.CustomerId);
                        if (string.IsNullOrWhiteSpace(comboBoxTakeOverCustomer.Text))
                        {
                            dateTimePickerTakeOverDate.Value = mEData.Date;
                            textBoxTakeOverLastName_Load(customer);
                        }

                        dataGridViewTakeOverAchat.Rows.Add();

                        SetCellValue(dataGridViewTakeOverAchat, "id", index, mEData.Id);
                        SetCellValue(dataGridViewTakeOverAchat, "article", index, mEData.Displaytext);
                        SetCellValue(dataGridViewTakeOverAchat, "quantity", index, mEData.Quantity);
                        SetCellValue(dataGridViewTakeOverAchat, "price", index, mEData.Price);
                        SetCellValue(dataGridViewTakeOverAchat, "garantie", index, mEData.Garantie.HasValue && mEData.Garantie.Value > 0);
                        SetCellValue(dataGridViewTakeOverAchat, "remise", index, mEData.Remise.HasValue && mEData.Remise.Value > 0);

                        textBoxTakeOverAccompte.Text = mEData.Accompte.ToString();
                        textBoxTakeOverResteDu.Text = mEData.ResteDu.ToString();
                        textBoxTakeOverPaid.Text = mEData.Paid.ToString();
                        textBoxTakeOverTotalPrice.Text = mEData.Total.ToString();

                        index++;
                    }
                }
                else
                {
                    CleanDataGridViewAchat();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        // Méthode générique pour définir la valeur d'une cellule dans le DataGridView
        private void SetCellValue<T>(DataGridView dataGridView, string columnName, int rowIndex, T value)
        {
            try
            {
                DataGridViewCell cell = dataGridView[columnName, rowIndex];
                if (cell != null && value != null)
                {
                    cell.Value = value;
                    if (dataGridView.EditingControl is Control editingControl)
                    {
                        editingControl.Text = value.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Une erreur est survenue. Erreur à remonter : {ex.StackTrace}", "Erreur", MessageBoxButtons.OK);
            }
        }

        private void dataGridViewAllCustomerRelation_Load(List<CustomerView> customerViews)
        {
            try
            {
                dataGridViewCustomerRelationAll.DataSource = null;
                dataGridViewCustomerRelationAll.Rows.Clear();
                dataGridViewCustomerRelationAll.Columns.Clear();

                DataGridViewTextBoxColumn textBoxColumnLastName = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Nom",
                    Name = "textBoxCustomerRelationLastName",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewCustomerRelationAll.Columns.Add(textBoxColumnLastName);
                DataGridViewTextBoxColumn textBoxColumnFirstName = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Prénom",
                    Name = "textBoxCustomerRelationFirstName",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewCustomerRelationAll.Columns.Add(textBoxColumnFirstName);
                DataGridViewTextBoxColumn textBoxColumnPhone = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Téléphone",
                    Name = "textBoxCustomerRelationPhone",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewCustomerRelationAll.Columns.Add(textBoxColumnPhone);
                DataGridViewTextBoxColumn textBoxColumnEmail = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Adresse mail",
                    Name = "textBoxCustomerRelationEmail",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewCustomerRelationAll.Columns.Add(textBoxColumnEmail);
                DataGridViewTextBoxColumn textBoxColumnCount = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Nombre de prises en charge",
                    Name = "textBoxCustomerRelationCount",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewCustomerRelationAll.Columns.Add(textBoxColumnCount);

                List<CustomerView> items = null;
                if (customerViews?.Any() ?? false)
                {
                    items = customerViews.Select(x =>
                    {
                        if (x.LastName == null)
                            x.LastName = string.Empty;
                        if (x.FirstName == null)
                            x.FirstName = string.Empty;
                        if (x.Phone == null)
                            x.Phone = string.Empty;
                        if (x.Email == null)
                            x.Email = string.Empty;
                        if (x.Count == null)
                            x.Count = string.Empty;
                        return x;
                    }).OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
                }
                else
                {
                    customerViews = new List<CustomerView>();
                    foreach (var customer in CustomersDS)
                    {
                        var tmp = new CustomerView()
                        {
                            LastName = customer.LastName ?? string.Empty,
                            FirstName = customer.FirstName ?? string.Empty,
                            Phone = customer.PhoneNumber ?? string.Empty,
                            Email = customer.EmailAddress ?? string.Empty,
                            Count = MEDatasDS.Where(x => x.CustomerId == customer.Id).GroupBy(x => x.TakeOverId).Count().ToString(),
                        };
                        customerViews.Add(tmp);
                    }
                    items = customerViews.OrderBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
                }

                int index = 0;
                foreach (CustomerView item in items)
                {
                    dataGridViewCustomerRelationAll.Rows.Add();

                    DataGridViewTextBoxCell lastnameTextBoxCell = (DataGridViewTextBoxCell)dataGridViewCustomerRelationAll.Rows[index].Cells["textBoxCustomerRelationLastName"];
                    lastnameTextBoxCell.Value = item.LastName;
                    if (dataGridViewCustomerRelationAll.EditingControl is DataGridViewTextBoxEditingControl lastnameTextBoxControl)
                    {
                        lastnameTextBoxControl.Text = item.LastName;
                    }

                    DataGridViewTextBoxCell firstnameTextBoxCell = (DataGridViewTextBoxCell)dataGridViewCustomerRelationAll.Rows[index].Cells["textBoxCustomerRelationFirstName"];
                    firstnameTextBoxCell.Value = item.FirstName;
                    if (dataGridViewCustomerRelationAll.EditingControl is DataGridViewTextBoxEditingControl firstnameTextBoxControl)
                    {
                        firstnameTextBoxControl.Text = item.FirstName;
                    }

                    DataGridViewTextBoxCell phoneTextBoxCell = (DataGridViewTextBoxCell)dataGridViewCustomerRelationAll.Rows[index].Cells["textBoxCustomerRelationPhone"];
                    phoneTextBoxCell.Value = item.Phone;
                    if (dataGridViewCustomerRelationAll.EditingControl is DataGridViewTextBoxEditingControl phoneTextBoxControl)
                    {
                        phoneTextBoxControl.Text = item.Phone.ToString();
                    }

                    DataGridViewTextBoxCell emailTextBoxCell = (DataGridViewTextBoxCell)dataGridViewCustomerRelationAll.Rows[index].Cells["textBoxCustomerRelationEmail"];
                    emailTextBoxCell.Value = item.Email;
                    if (dataGridViewCustomerRelationAll.EditingControl is DataGridViewTextBoxEditingControl emailTextBoxControl)
                    {
                        emailTextBoxControl.Text = item.Email;
                    }

                    DataGridViewTextBoxCell countTextBoxCell = (DataGridViewTextBoxCell)dataGridViewCustomerRelationAll.Rows[index].Cells["textBoxCustomerRelationCount"];
                    countTextBoxCell.Value = item.Count;
                    if (dataGridViewCustomerRelationAll.EditingControl is DataGridViewTextBoxEditingControl countTextBoxControl)
                    {
                        countTextBoxControl.Text = item.Count;
                    }

                    index++;
                }

                if (comboBoxCustomerRelationSexe.Items.Count > 0)
                    comboBoxCustomerRelationSexe.Items.Clear();
                // Replace MyEnum with the name of your enum
                foreach (Sexe value in Enum.GetValues(typeof(Sexe)))
                {
                    comboBoxCustomerRelationSexe.Items.Add(Tools.GetEnumDescriptionFromEnum<Sexe>(value));
                }
                comboBoxCustomerRelationSexe.DrawMode = DrawMode.Normal;
                comboBoxCustomerRelationSexe.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void dataGridViewOneCustomerRelation_Load(Customer customer)
        {
            try
            {
                List<HistoriqueOneView> historiqueOneViews = MEDatasDS
                    .Where(x => x.CustomerId == customer.Id)
                    .Select(x =>
                    {
                        string typeItem =
                            x.ArticleId.HasValue ? "Achat" :
                            x.RepairTypeId.HasValue ? "Réparation" :
                            "Déblocage";

                        string item = x.RepairTypeId.HasValue ? TakeOverTypesDS.First(t => t.Id == 0).RepairTypes.First(t => t.Id == x.RepairTypeId).Name :
                            x.UnlockTypeId.HasValue ? TakeOverTypesDS.First(t => t.Id == 1).UnlockTypes.First(t => t.Id == x.UnlockTypeId).Name :
                            TakeOverTypesDS.First(t => t.Id == 2).Articles.First(t => t.Id == x.ArticleId).Produit;

                        string restedu = x.ResteDu == 0 && x.Total > 0 ? "Payé" : x.ResteDu.ToString();

                        string paymentMode = Tools.GetEnumDescriptionFromEnum<PaymentMode>(x.PaymentMode);

                        string marque = MarquesDS.FirstOrDefault(m => m.Id == x.MarqueId)?.Name ?? string.Empty;
                        string modele = ModelesDS.FirstOrDefault(m => m.Id == x.ModeleId)?.Name ?? string.Empty;

                        return new HistoriqueOneView()
                        {
                            Id = x.Id,
                            PriseEnCharge = x.TakeOverId,
                            Date = x.Date.ToString("dd/MM/yyyy HH:mm:ss"),
                            Facture = x.InvoiceId.HasValue && x.InvoiceId > 0 && x.State == TakeOverState.PickedUp ? "Facturé" : "Non facturé",
                            FactureNumero = x.InvoiceId.HasValue ? x.InvoiceId.Value : 0,
                            TypeItem = typeItem,
                            Item = item,
                            Marque = marque,
                            Modele = modele,
                            IMEI = x.IMEI,
                            Quantity = x.Quantity,
                            Price = x.Price.Value.ToString(),
                            Garantie = x.Garantie.HasValue ? x.Garantie.Value.ToString() + " mois" : "Non",
                            Remise = x.Remise.HasValue ? x.Remise.Value.ToString() : "0",
                            Accompte = x.Accompte.Value.ToString(),
                            ResteDu = restedu,
                            Paid = x.Paid.Value.ToString(),
                            Total = x.Total.ToString(),
                            PaymentMode = paymentMode,
                            State = x.State,
                            Verification = x.Verification,
                        };
                    }).OrderByDescending(x => x.Date)
                    .ToList();
                int? invoiceId = (MEDatasDS.OrderByDescending(x => x.InvoiceId ?? 0)?.FirstOrDefault()?.InvoiceId ?? 0) + 1;
                if (historiqueOneViews?.Any() ?? false)
                {
                    using (HistoriqueOneViewForm dialog = new HistoriqueOneViewForm(
                        historiqueOneViews,
                        MEDatasDS,
                        customer,
                        MarquesDS,
                        ModelesDS,
                        TakeOverTypesDS))
                    {
                        // Afficher la boîte de dialogue modale
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            var factureAMettreAJour = dialog.GetResult().ToList();

                            // propager les invoiceId
                            for (int i = 0; i < factureAMettreAJour.Count; i++)
                            {
                                if (MEDatasDS.Any(x => x.Id == factureAMettreAJour[i].Id))
                                {
                                    for (int j = 0; j < MEDatasDS.Count; j++)
                                    {
                                        if (MEDatasDS[j].Id == factureAMettreAJour[i].Id)
                                        {
                                            MEDatasDS[j].InvoiceId = invoiceId;
                                            MEDatasDS[j].State = factureAMettreAJour[i].State;
                                            MEDatasDS[j].Verification = factureAMettreAJour[i].Verification;
                                            MEDatasDS[j].Price = Tools.ToNullableDecimal(factureAMettreAJour[i].Price);
                                            MEDatasDS[j].Paid = Tools.ToNullableDecimal(factureAMettreAJour[i].Paid);
                                            MEDatasDS[j].ResteDu = Tools.ToNullableDecimal(factureAMettreAJour[i].ResteDu);
                                            MEDatasDS[j].Total = decimal.Parse(factureAMettreAJour[i].Total);
                                        }
                                    }
                                }
                            }

                            string path = Paths.ReceiptDSPath;

                            List<string> items = new List<string>() {
                                "Numéro de prise en charge;Date;Numéro de facture;Numéro du client;Numéro de la marque;" +
                                "Numéro du modèle;IMEI;Numéro du type de réparation;Numéro du type de déblocage;Numéro de l'article;" +
                                "Quantité;Prix;Garantie;Remise;Accompte;" +
                                "Reste dû;Payé;Total;Numéro de mode de paiement;Etat;" +
                                "Id;Vérification;Option de garantie" };
                            items.AddRange(MEDatasDS.Select(x =>
                                $"{x.TakeOverId};{x.Date};{x.InvoiceId};{x.CustomerId};{x.MarqueId};" +
                                $"{x.ModeleId};{x.IMEI};{x.RepairTypeId};{x.UnlockTypeId};{x.ArticleId};" +
                                $"{x.Quantity};{x.Price};{x.Garantie};{x.Remise};{x.Accompte};" +
                                $"{x.ResteDu};{x.Paid};{x.Total};{Tools.GetEnumDescriptionFromEnum<PaymentMode>(x.PaymentMode)};{Tools.GetEnumDescriptionFromEnum<TakeOverState>(x.State)};" +
                                $"{x.Id};{(x.Verification ? "Oui" : "Non")};{(x.OptionGarantie ? "Oui" : "Non")}"));
                            Tools.RewriteDataToFile(items, path, false);

                            InitializeAll();
                            dataGridViewOneCustomerRelation_Load(customer);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Il n'y a pas d'éléments à afficher pour ce client.", "Alerte", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }

        private void dataGridViewStock_Load(List<Article> articles)
        {
            try
            {
                dataGridViewStock.DataSource = null;
                dataGridViewStock.Rows.Clear();
                dataGridViewStock.Columns.Clear();
                dataGridViewStock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dataGridViewStock.ClearSelection();

                DataGridViewTextBoxColumn textBoxId = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Id",
                    Name = "textBoxStockId",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    ReadOnly = true,
                };
                dataGridViewStock.Columns.Add(textBoxId);
                DataGridViewTextBoxColumn textBoxCodeRef = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Code de référence",
                    Name = "textBoxStockCodeRef",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    ReadOnly = true,
                };
                dataGridViewStock.Columns.Add(textBoxCodeRef);
                DataGridViewTextBoxColumn textBoxMarque = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Marque",
                    Name = "textBoxStockMarque",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewStock.Columns.Add(textBoxMarque);
                DataGridViewTextBoxColumn textBoxModele = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Modèle",
                    Name = "textBoxStockModele",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewStock.Columns.Add(textBoxModele);
                DataGridViewTextBoxColumn textBoxProduit = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Produit",
                    Name = "textBoxStockProduit",
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewStock.Columns.Add(textBoxProduit);
                DataGridViewTextBoxColumn textBoxPrice = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Prix",
                    Name = "textBoxStockPrice",
                    ValueType = typeof(decimal),
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewStock.Columns.Add(textBoxPrice);
                DataGridViewTextBoxColumn textBoxQuantity = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Quantité",
                    Name = "textBoxStockQuantity",
                    ValueType = typeof(int),
                    ReadOnly = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewStock.Columns.Add(textBoxQuantity);

                dataGridViewStock.AllowUserToAddRows = false;

                List<Article> items = (articles?.Any() ?? false) ? articles : TakeOverTypesDS.Find(x => x.Id == 2).Articles;

                int index = 0;
                foreach (Article article in items)
                {
                    dataGridViewStock.Rows.Add();

                    DataGridViewTextBoxCell idTextBoxCell = (DataGridViewTextBoxCell)dataGridViewStock.Rows[index].Cells["textBoxStockId"];
                    idTextBoxCell.Value = article.Id;
                    if (dataGridViewStock.EditingControl is DataGridViewTextBoxEditingControl idTextBoxControl)
                    {
                        idTextBoxControl.Text = article.Id.ToString();
                    }
                    DataGridViewTextBoxCell codeRefTextBoxCell = (DataGridViewTextBoxCell)dataGridViewStock.Rows[index].Cells["textBoxStockCodeRef"];
                    codeRefTextBoxCell.Value = article.CodeReference;
                    if (dataGridViewStock.EditingControl is DataGridViewTextBoxEditingControl codeRefTextBoxControl)
                    {
                        codeRefTextBoxControl.Text = article.CodeReference;
                    }

                    Marque marque = null;
                    if (article.MarqueId.HasValue)
                    {
                        marque = MarquesDS.Find(x => x.Id == (article.MarqueId.Value as int?).Value);
                        DataGridViewTextBoxCell marqueComboBoxCell = (DataGridViewTextBoxCell)dataGridViewStock.Rows[index].Cells["textBoxStockMarque"];
                        marqueComboBoxCell.Value = marque.Name;
                        if (dataGridViewStock.EditingControl is DataGridViewTextBoxEditingControl marqueTextBoxControl)
                        {
                            marqueTextBoxControl.Text = marque.Name;
                        }
                    }

                    Modele modele = null;
                    if (article.ModeleId.HasValue)
                    {
                        modele = ModelesDS.Find(x => x.Id == (article.ModeleId.Value as int?).Value);
                        DataGridViewTextBoxCell modeleComboBoxCell = (DataGridViewTextBoxCell)dataGridViewStock.Rows[index].Cells["textBoxStockModele"];
                        modeleComboBoxCell.Value = modele.Name;
                        if (dataGridViewStock.EditingControl is DataGridViewTextBoxEditingControl modeleTextBoxControl)
                        {
                            modeleTextBoxControl.Text = modele.Name;
                        }
                    }

                    DataGridViewTextBoxCell nameTextBoxCell = (DataGridViewTextBoxCell)dataGridViewStock.Rows[index].Cells["textBoxStockProduit"];
                    nameTextBoxCell.Value = article.Produit;
                    if (dataGridViewStock.EditingControl is DataGridViewTextBoxEditingControl nameTextBoxControl)
                    {
                        nameTextBoxControl.Text = article.Produit;
                    }

                    DataGridViewTextBoxCell priceTextBoxCell = (DataGridViewTextBoxCell)dataGridViewStock.Rows[index].Cells["textBoxStockPrice"];
                    priceTextBoxCell.Value = article.Price;
                    if (dataGridViewStock.EditingControl is DataGridViewTextBoxEditingControl priceTextBoxControl)
                    {
                        priceTextBoxControl.Text = article.Price.ToString();
                    }

                    DataGridViewTextBoxCell quantityTextBoxCell = (DataGridViewTextBoxCell)dataGridViewStock.Rows[index].Cells["textBoxStockQuantity"];
                    quantityTextBoxCell.Value = article.Quantity;
                    if (dataGridViewStock.EditingControl is DataGridViewTextBoxEditingControl quantityTextBoxControl)
                    {
                        quantityTextBoxControl.Text = article.Quantity.ToString();
                    }

                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        #endregion

        #region Evénements composants
        private void TabControllAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitializeAll();
        }
        // TakeOver
        private void buttonAddCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxTakeOverLastName_Load(null);
                using (CustomerForm dialog = new CustomerForm(null, CustomersDS))
                {
                    // Afficher la boîte de dialogue modale
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        // Récupérer les éléments cochés
                        (bool isUpdate, Customer customer) = dialog.GetResult();
                        if (isUpdate)
                        {
                            CustomersDS.Remove(CustomersDS.First(x => x.Id == customer.Id));
                        }
                        CustomersDS.Add(customer);

                        string path = Paths.CustomersDSPath;
                        List<string> items = new List<string>() { "Id;Nom;Prénom;Numéro de téléphone;Adresse mail;Sexe" };
                        items.AddRange(CustomersDS.Select(x => $"{x.Id};{x.LastName};{x.FirstName};{x.PhoneNumber};{x.EmailAddress};{Tools.GetEnumDescriptionFromEnum<Sexe>(x.Sexe)}"));
                        Tools.RewriteDataToFile(items, path, false);

                        textBoxTakeOverLastName_Load(customer);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void buttonUpdateCustomer_Click(object sender, EventArgs e)
        {
            try
            {
                textBoxTakeOverLastName_Load(null);
                Customer customerToUpdate = CustomersDS.FirstOrDefault(x => x.Id == ((int?)comboBoxTakeOverCustomer.SelectedValue ?? 0));
                if (customerToUpdate == null)
                {
                    MessageBox.Show("Le client n'est pas connu du système. Veuillez appuyer sur le bouton \"Ajouter un client\"", "Alerte", MessageBoxButtons.OK);
                    return;
                }

                using (CustomerForm dialog = new CustomerForm(customerToUpdate, CustomersDS))
                {
                    // Afficher la boîte de dialogue modale
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        // Récupérer les éléments cochés
                        (bool isUpdate, Customer customer) = dialog.GetResult();
                        if (isUpdate)
                        {
                            CustomersDS.Remove(CustomersDS.First(x => x.Id == customer.Id));
                        }
                        CustomersDS.Add(customer);

                        string path = Paths.CustomersDSPath;
                        List<string> items = new List<string>() { "Id;Nom;Prénom;Numéro de téléphone;Adresse mail;Sexe" };
                        items.AddRange(CustomersDS.Select(x => $"{x.Id};{x.LastName};{x.FirstName};{x.PhoneNumber};{x.EmailAddress};{Tools.GetEnumDescriptionFromEnum<Sexe>(x.Sexe)}"));
                        Tools.RewriteDataToFile(items, path, false);

                        textBoxTakeOverLastName_Load(customer);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void ComboBoxTakeOverLastname_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                //use keyUp event, as text changed traps too many other evengts.
                ComboBox oBox = (ComboBox)sender;
                string sBoxText = oBox.Text;

                DataRow[] oFilteredRows = dataTableCustomers.Select("name" + " Like '%" + sBoxText + "%'");
                DataTable oFilteredDT = oFilteredRows.Length > 0 ? oFilteredRows.CopyToDataTable() : dataTableCustomers;

                //NOW THAT WE HAVE OUR FILTERED LIST, WE NEED TO RE-BIND IT WIHOUT CHANGING THE TEXT IN THE ComboBox.

                //1).UNREGISTER THE SELECTED EVENT BEFORE RE-BINDING, b/c IT TRIGGERS ON BIND.
                oBox.DataSource = oFilteredDT; //2).rebind to filtered list.
                //3).show the user the new filtered list.
                oBox.DroppedDown = true; //this will overwrite the text in the ComboBox, so 4&5 put it back.
                //4).binding data source erases text, so now we need to put the user's text back,
                oBox.Text = sBoxText;
                //5). need to put the user's cursor back where it was.
                oBox.SelectionStart = sBoxText.Length;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private string previousAccompte = "0";
        private void TextBoxTakeOverAccompte_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;

                decimal prevAccompte = decimal.Parse(string.IsNullOrWhiteSpace(previousAccompte) ? "0" : previousAccompte);
                decimal accompte = decimal.Parse(string.IsNullOrWhiteSpace(textBox.Text) ? "0" : textBox.Text);
                decimal resteDu = decimal.Parse(string.IsNullOrWhiteSpace(textBoxTakeOverResteDu.Text) ? "0" : textBoxTakeOverResteDu.Text);
                // Mettez à jour l'ancienne valeur avec la nouvelle valeur
                previousAccompte = textBox.Text;

                textBoxTakeOverResteDu.Text = (resteDu + prevAccompte - accompte).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private string previousPaid = "0";
        private void TextBoxTakeOverPaid_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox textBox = (TextBox)sender;

                decimal prevPaid = decimal.Parse(string.IsNullOrWhiteSpace(previousPaid) ? "0" : previousPaid);
                decimal paid = decimal.Parse(string.IsNullOrWhiteSpace(textBox.Text) ? "0" : textBox.Text);
                decimal resteDu = decimal.Parse(string.IsNullOrWhiteSpace(textBoxTakeOverResteDu.Text) ? "0" : textBoxTakeOverResteDu.Text);
                // Mettez à jour l'ancienne valeur avec la nouvelle valeur
                previousPaid = textBox.Text;

                textBoxTakeOverResteDu.Text = (resteDu + prevPaid - paid).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void buttonTakeOverSearch_Click(object sender, EventArgs e)
        {
            try
            {
                List<MEData> receipts = MEDatasDS
                    .Where(x => string.Compare(x.TakeOverId.ToString(), takeOverNumber.Text) == 0)
                    .OrderBy(x => x.Id)
                    .ToList();
                if (receipts?.Any() ?? false)
                {
                    // initialiser les garanties
                    GarantiesTemp = new List<Garantie>();
                    // initialiser les remises
                    RemisesTemp = new List<Remise>();
                    for (int i = 0; i < receipts.Count; i++)
                    {
                        MEData mEData = receipts[i];
                        string tabName = mEData.RepairTypeId.HasValue ? "Réparation" : mEData.UnlockTypeId.HasValue ? "Déblocage" : "Achat";
                        RepairType produitRepair = mEData.RepairTypeId.HasValue ? TakeOverTypesDS.First(x => x.Id == 0).RepairTypes.FirstOrDefault(x => x.Id == (mEData.RepairTypeId.Value as int?).Value) : null;
                        UnlockType produitUnlock = mEData.UnlockTypeId.HasValue ? TakeOverTypesDS.First(x => x.Id == 1).UnlockTypes.FirstOrDefault(x => x.Id == (mEData.UnlockTypeId.Value as int?).Value) : null;
                        Article produitArticle = mEData.ArticleId.HasValue ? TakeOverTypesDS.First(x => x.Id == 2).Articles.FirstOrDefault(x => x.Id == (mEData.ArticleId.Value as int?).Value) : null;

                        int marqueId = receipts[i].MarqueId.HasValue ? receipts[i].MarqueId.Value : 0;
                        Marque marque = MarquesDS.FirstOrDefault(x => x.Id == marqueId);

                        int modeleId = receipts[i].ModeleId.HasValue ? receipts[i].ModeleId.Value : 0;
                        Modele modele = ModelesDS.FirstOrDefault(x => x.Id == modeleId);

                        string marqueText = marque?.Name;
                        string modeleText = modele?.Name;
                        string productName =
                            !string.IsNullOrWhiteSpace(marqueText) && !string.IsNullOrWhiteSpace(modeleText) ? marqueText + " " + modeleText + " - " + (produitRepair?.Name ?? produitUnlock?.Name ?? produitArticle.Produit) :
                            !string.IsNullOrWhiteSpace(marqueText) && !string.IsNullOrWhiteSpace(modeleText) ? marqueText + " - " + (produitRepair?.Name ?? produitUnlock?.Name ?? produitArticle.Produit) :
                            (produitRepair?.Name ?? produitUnlock?.Name ?? produitArticle.Produit);

                        if (mEData.Garantie.HasValue)
                        {
                            // ajouter la ligne de garantie
                            GarantiesTemp.Add(new Garantie(
                                id: mEData.Id,
                                tabName: tabName,
                                productName: productName,
                                months: mEData.Garantie.Value,
                                option: mEData.OptionGarantie));
                        }
                        if (mEData.Remise.HasValue)
                        {
                            // ajouter la ligne de remise
                            RemisesTemp.Add(new Remise(
                                id: mEData.Id,
                                tabName: tabName,
                                productName: productName,
                                prix: mEData.Remise.Value));
                        }
                    }

                    (bool cb, bool espece, bool virement) = Tools.GetBoolFromPaymentMode(receipts.First().PaymentMode);
                    checkBoxCb.Checked = cb;
                    checkBoxEspece.Checked = espece;
                    checkBoxVirement.Checked = virement;

                    // initialiser les onglets
                    takeOverRepair_Load(receipts.Where(x => x.RepairTypeId.HasValue).ToList());
                    takeOverUnlock_Load(receipts.Where(x => x.UnlockTypeId.HasValue).ToList());
                    takeOverAchat_Load(receipts.Where(x => x.ArticleId.HasValue).ToList());

                    UpdateTotalPrice();
                }
                else
                {
                    MessageBox.Show(text: $"La prise en charge n°{takeOverNumber.Text} n'a pas été trouvée", caption: "Alerte", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void buttonTakeOverReset_Click(object sender, EventArgs e)
        {
            InitializeAll();
        }
        private void buttonTakeOverSave_Click(object sender, EventArgs e)
        {
            try
            {
                int takeOverId = string.IsNullOrWhiteSpace(takeOverNumber.Text) ? MEDatasDS.OrderByDescending(x => x.TakeOverId).First().TakeOverId + 1 : int.Parse(takeOverNumber.Text);
                DateTime date = dateTimePickerTakeOverDate.Value;
                Customer takeOverCustomer = CustomersDS.FirstOrDefault(x => x.Id == ((int?)comboBoxTakeOverCustomer.SelectedValue ?? 0));
                if (takeOverCustomer == null)
                {
                    MessageBox.Show("Vous devez définir un client.", "Alerte", MessageBoxButtons.OK);
                    return;
                }

                decimal accompte = Tools.ToNullableDecimal(textBoxTakeOverAccompte.Text) ?? 0;
                decimal resteDu = Tools.ToNullableDecimal(textBoxTakeOverResteDu.Text) ?? 0;
                decimal paid = Tools.ToNullableDecimal(textBoxTakeOverPaid.Text) ?? 0;
                decimal total = Tools.ToNullableDecimal(textBoxTakeOverTotalPrice.Text) ?? 0;
                PaymentMode paymentMode = Tools.GetPaymentModeFromBool(checkBoxCb.Checked, checkBoxEspece.Checked, checkBoxVirement.Checked);

                List<MEData> mEDatas = new List<MEData>();
                Dictionary<int, string> repairTypeNames = new Dictionary<int, string>();
                Dictionary<int, string> unlockTypeNames = new Dictionary<int, string>();
                Dictionary<int, string> articles = new Dictionary<int, string>();
                Dictionary<int, string> marques = new Dictionary<int, string>();
                Dictionary<int, string> modeles = new Dictionary<int, string>();
                foreach (DataGridViewRow row in dataGridViewTakeOverRepair.Rows)
                {
                    ProcessDataGridViewRow(0, row, ref mEDatas, ref repairTypeNames, ref unlockTypeNames, ref articles, ref marques, ref modeles, takeOverId, date, takeOverCustomer, accompte, resteDu, paid, total, paymentMode);
                }
                foreach (DataGridViewRow row in dataGridViewTakeOverUnlock.Rows)
                {
                    ProcessDataGridViewRow(1, row, ref mEDatas, ref repairTypeNames, ref unlockTypeNames, ref articles, ref marques, ref modeles, takeOverId, date, takeOverCustomer, accompte, resteDu, paid, total, paymentMode);
                }
                foreach (DataGridViewRow row in dataGridViewTakeOverAchat.Rows)
                {
                    ProcessDataGridViewRow(2, row, ref mEDatas, ref repairTypeNames, ref unlockTypeNames, ref articles, ref marques, ref modeles, takeOverId, date, takeOverCustomer, accompte, resteDu, paid, total, paymentMode);
                }

                if (mEDatas?.Any() ?? false)
                {
                    foreach (var mData in mEDatas)
                    {
                        var existingData = MEDatasDS.FirstOrDefault(x => x.Id == mData.Id);
                        if (existingData != null)
                        {
                            int? invoiceIdMEData = existingData.InvoiceId;
                            int? invoiceIdNewMEData = mData.InvoiceId;
                            mData.Garantie = mData.Garantie.HasValue ? mData.Garantie : existingData.Garantie;
                            mData.Remise = mData.Remise.HasValue ? mData.Remise : existingData.Remise;

                            if (invoiceIdMEData != invoiceIdNewMEData)
                            {
                                mData.InvoiceId = invoiceIdMEData.HasValue && invoiceIdNewMEData.HasValue ? invoiceIdNewMEData : invoiceIdMEData;
                            }

                            MEDatasDS[MEDatasDS.IndexOf(existingData)] = mData;
                        }
                        else
                        {
                            MEDatasDS.Add(mData);
                        }
                    }

                    // Code pour générer et sauvegarder les données dans un fichier
                    string path = Paths.ReceiptDSPath;
                    List<string> items = new List<string>() {
                        "Numéro de prise en charge;Date;Numéro de facture;Numéro du client;Numéro de la marque;" +
                        "Numéro du modèle;IMEI;Numéro du type de réparation;Numéro du type de déblocage;Numéro de l'article;" +
                        "Quantité;Prix;Garantie;Remise;Accompte;" +
                        "Reste dû;Payé;Total;Numéro de mode de paiement;Etat;" +
                        "Id;Vérification;Option de garantie" };
                    items.AddRange(MEDatasDS.Select(x =>
                        $"{x.TakeOverId};{x.Date};{x.InvoiceId};{x.CustomerId};{x.MarqueId};" +
                        $"{x.ModeleId};{x.IMEI};{x.RepairTypeId};{x.UnlockTypeId};{x.ArticleId};" +
                        $"{x.Quantity};{x.Price};{x.Garantie};{x.Remise};{x.Accompte};" +
                        $"{x.ResteDu};{x.Paid};{x.Total};{Tools.GetEnumDescriptionFromEnum<PaymentMode>(x.PaymentMode)};{Tools.GetEnumDescriptionFromEnum<TakeOverState>(x.State)};" +
                        $"{x.Id};{(x.Verification ? "Oui" : "Non")};{(x.OptionGarantie ? "Oui" : "Non")}"));
                    Tools.RewriteDataToFile(items, path, false);

                    string type = GetItemTypeDescription(mEDatas);
                    if (type == "Les achats")
                    {
                        GenerateFacture(type, mEDatas, takeOverCustomer, takeOverId, date, repairTypeNames, unlockTypeNames, articles, marques, modeles);
                    }
                    else
                    {
                        GeneratePriseEnCharge(type, mEDatas, takeOverCustomer, takeOverId, date, repairTypeNames, unlockTypeNames, articles, marques, modeles, accompte, paid);
                    }

                    InitializeAll();
                }
                else
                {
                    MessageBox.Show("Il n'y a pas d'éléments dans la prise en charge. Veuillez remplir les onglets pour sauvegarder le reçu.", "Alerte", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void GenerateFacture(string type, List<MEData> mEDatas, Customer takeOverCustomer, int takeOverId, DateTime date, Dictionary<int, string> repairTypeNames, Dictionary<int, string> unlockTypeNames, Dictionary<int, string> articles, Dictionary<int, string> marques, Dictionary<int, string> modeles)
        {
            try
            {
                List<MEData> aFacturers = MEDatasDS.Where(x => mEDatas.FirstOrDefault(y => y.TakeOverId == x.TakeOverId) != null).ToList();

                int invoiceId = (MEDatasDS.OrderByDescending(x => x.InvoiceId ?? 0)?.FirstOrDefault()?.InvoiceId ?? 0) + 1;
                using (GenerateInvoiceForm dialog = new GenerateInvoiceForm(mEDatas, takeOverCustomer, MarquesDS, ModelesDS, TakeOverTypesDS, invoiceId))
                {
                    // Afficher la boîte de dialogue modale
                    if (dialog.ShowDialog() == DialogResult.Yes)
                    {
                        // Récupérer les éléments cochés
                        var factureACreer = dialog.GetResult().ToList();

                        // propager les invoiceId
                        for (int i = 0; i < factureACreer.Count; i++)
                        {
                            if (mEDatas.Any(x => x.Id == factureACreer[i].Id))
                            {
                                for (int j = 0; j < mEDatas.Count; j++)
                                {
                                    if (mEDatas[j].Id == factureACreer[i].Id)
                                    {
                                        mEDatas[j].State = factureACreer[i].State;
                                        mEDatas[j].Price = factureACreer[i].Price;
                                        mEDatas[j].Accompte = factureACreer[i].Accompte;
                                        mEDatas[j].ResteDu = factureACreer[i].ResteDu;
                                        mEDatas[j].Paid = factureACreer[i].Paid;
                                        mEDatas[j].Total = factureACreer[i].Total;
                                        mEDatas[j].InvoiceId = invoiceId;
                                    }
                                }
                            }
                        }

                        for (int i = 0; i < mEDatas.Count; i++)
                        {
                            if (MEDatasDS.Any(x => x.Id == mEDatas[i].Id))
                            {
                                for (int j = 0; j < MEDatasDS.Count; j++)
                                {
                                    if (MEDatasDS[j].Id == mEDatas[i].Id)
                                    {
                                        MEDatasDS[j] = mEDatas[i];
                                    }
                                }
                            }
                        }

                        string path = Paths.ReceiptDSPath;

                        List<string> items = new List<string>() {
                            "Numéro de prise en charge;Date;Numéro de facture;Numéro du client;Numéro de la marque;" +
                            "Numéro du modèle;IMEI;Numéro du type de réparation;Numéro du type de déblocage;Numéro de l'article;" +
                            "Quantité;Prix;Garantie;Remise;Accompte;" +
                            "Reste dû;Payé;Total;Numéro de mode de paiement;Etat;" +
                            "Id;Vérification;Option de garantie" };
                        items.AddRange(MEDatasDS.Select(x =>
                        $"{x.TakeOverId};{x.Date};{x.InvoiceId};{x.CustomerId};{x.MarqueId};" +
                        $"{x.ModeleId};{x.IMEI};{x.RepairTypeId};{x.UnlockTypeId};{x.ArticleId};" +
                        $"{x.Quantity};{x.Price};{x.Garantie};{x.Remise};{x.Accompte};" +
                        $"{x.ResteDu};{x.Paid};{x.Total};{Tools.GetEnumDescriptionFromEnum<PaymentMode>(x.PaymentMode)};{Tools.GetEnumDescriptionFromEnum<TakeOverState>(x.State)};" +
                        $"{x.Id};{(x.Verification ? "Oui" : "Non")};{(x.OptionGarantie ? "Oui" : "Non")}"));
                        Tools.RewriteDataToFile(items, path, false);

                        decimal payeAFacturer = factureACreer.First().Paid.Value;
                        decimal accompteAFacturer = factureACreer.First().Accompte.Value;
                        decimal totalAFacturer = 0;
                        foreach (MEData afacturer in factureACreer)
                        {
                            if (afacturer.Remise.HasValue)
                            {
                                totalAFacturer += (afacturer.Quantity * afacturer.Price.Value) - (afacturer.Quantity * afacturer.Remise.Value);
                            }
                            else
                            {
                                totalAFacturer += afacturer.Quantity * afacturer.Price ?? 0;
                            }

                            Marque marque = MarquesDS.FirstOrDefault(x => afacturer.MarqueId == x.Id);
                            Modele modele = ModelesDS.FirstOrDefault(x => afacturer.ModeleId == x.Id);

                            RepairType repairType = TakeOverTypesDS.FirstOrDefault(x => x.Id == 0).RepairTypes.FirstOrDefault(x => x.Id == afacturer.RepairTypeId);
                            if (repairType != null)
                            {
                                if (repairType != null && !repairTypeNames.Any(x => x.Key == repairType.Id))
                                {
                                    repairTypeNames.Add(repairType.Id, repairType.Name);
                                }
                                if (marque != null && !marques.Any(x => x.Key == marque.Id))
                                {
                                    marques.Add(marque.Id, marque.Name);
                                }
                                if (modele != null && !modeles.Any(x => x.Key == modele.Id))
                                {
                                    modeles.Add(modele.Id, modele.Name);
                                }
                            }

                            UnlockType unlockType = TakeOverTypesDS.FirstOrDefault(x => x.Id == 1).UnlockTypes.FirstOrDefault(x => x.Id == afacturer.UnlockTypeId);
                            if (unlockType != null)
                            {
                                if (unlockType != null && !unlockTypeNames.Any(x => x.Key == unlockType.Id))
                                {
                                    unlockTypeNames.Add(unlockType.Id, unlockType.Name);
                                }
                                if (marque != null && !marques.Any(x => x.Key == marque.Id))
                                {
                                    marques.Add(marque.Id, marque.Name);
                                }
                                if (modele != null && !modeles.Any(x => x.Key == modele.Id))
                                {
                                    modeles.Add(modele.Id, modele.Name);
                                }
                            }

                            Article article = TakeOverTypesDS.FirstOrDefault(x => x.Id == 2).Articles.FirstOrDefault(x => x.Id == afacturer.ArticleId);
                            if (article != null)
                            {
                                if (article != null && !articles.Any(x => x.Key == article.Id))
                                {
                                    articles.Add(article.Id, article.Produit);
                                }
                                if (marque != null && !marques.Any(x => x.Key == marque.Id))
                                {
                                    marques.Add(marque.Id, marque.Name);
                                }
                                if (modele != null && !modeles.Any(x => x.Key == modele.Id))
                                {
                                    modeles.Add(modele.Id, modele.Name);
                                }
                            }
                        }
                        if (accompteAFacturer > 0)
                        {
                            DialogResult acccompteResult = MessageBox.Show($"Voulez-vous prendre en compte l'accompte de {accompteAFacturer.ToString()} versé ?", "Information", MessageBoxButtons.YesNo);
                            if (acccompteResult == DialogResult.Yes)
                            {
                                payeAFacturer += accompteAFacturer;
                            }
                        }
                        string modeDePaiement = Tools.GetEnumDescriptionFromEnum<PaymentMode>(factureACreer.First().PaymentMode);
                        // générer pdf
                        string customerName = (
                            !string.IsNullOrWhiteSpace(takeOverCustomer.LastName) && !string.IsNullOrWhiteSpace(takeOverCustomer.FirstName) ? takeOverCustomer.LastName + " " + takeOverCustomer.FirstName :
                            !string.IsNullOrWhiteSpace(takeOverCustomer.LastName) && string.IsNullOrWhiteSpace(takeOverCustomer.FirstName) ? takeOverCustomer.LastName :
                            takeOverCustomer.FirstName);
                        string title = $@"Facture_{(invoiceId < 10 ? $"0{invoiceId}" : invoiceId.ToString())}_PriseEnCharge_{(factureACreer.First().TakeOverId < 10 ? $"0{factureACreer.First().TakeOverId}" : factureACreer.First().TakeOverId.ToString())}_{factureACreer.First().Date.ToString("ddMMyyyyHHmmss")}";
                        Services.GenerateDocx(
                            type: 1,
                            logo: Paths.LogoPath,
                            customerName: (takeOverCustomer.Sexe == Sexe.Femme ? $"Madame {customerName}" : takeOverCustomer.Sexe == Sexe.Homme ? $"Monsieur {customerName}" : customerName),
                            customerPhone: takeOverCustomer.PhoneNumber,
                            customerEmail: takeOverCustomer.EmailAddress,
                            takeOverDate: factureACreer.First().Date.ToString("dddd dd MMMM yyyy"),
                            takeOverNumber: (invoiceId < 10 ? $"0{invoiceId}" : invoiceId.ToString()),
                            accompte: null,
                            paid: payeAFacturer,
                            modeDePaiement: modeDePaiement,
                            mEDatas: factureACreer,
                            path: $@"{Paths.FactureDirectory}\{title}.docx",
                            title: title,
                            repairTypeNames: repairTypeNames,
                            unlockTypeNames: unlockTypeNames,
                            articles: articles,
                            marques: marques,
                            modeles: modeles);
                        // Vérifier si le fichier existe
                        if (System.IO.File.Exists($@"{Paths.FactureDirectory}\{title}.docx"))
                        {
                            // Ouvrir le fichier PDF dans une fenêtre externe
                            Process.Start($@"{Paths.FactureDirectory}\{title}.docx");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void GeneratePriseEnCharge(string type, List<MEData> mEDatas, Customer takeOverCustomer, int takeOverId, DateTime date, Dictionary<int, string> repairTypeNames, Dictionary<int, string> unlockTypeNames, Dictionary<int, string> articles, Dictionary<int, string> marques, Dictionary<int, string> modeles, decimal accompte, decimal paid)
        {
            try
            {
                // Code pour générer un reçu PDF
                DialogResult dialogResult = MessageBox.Show($"{type} ont été sauvegardés. Voulez-vous générer un reçu ?", "Information", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    string customerName =
                        takeOverCustomer.Sexe == Sexe.Unknown ? takeOverCustomer.LastName + " " + takeOverCustomer.FirstName :
                        takeOverCustomer.Sexe == Sexe.Femme ? "Mme. " + takeOverCustomer.LastName + " " + takeOverCustomer.FirstName :
                        "M. " + takeOverCustomer.LastName + " " + takeOverCustomer.FirstName;
                    PaymentMode paymentMode = Tools.GetPaymentModeFromBool(checkBoxCb.Checked, checkBoxEspece.Checked, checkBoxVirement.Checked);
                    // générer pdf
                    string title = $@"PriseEnCharge_{(takeOverId < 10 ? $"0{takeOverId}" : takeOverId.ToString())}_{date.ToString("ddMMyyyyHHmmss")}";
                    Services.GenerateDocx(
                        type: 0,
                        logo: Paths.LogoPath,
                        customerName: customerName,
                        customerPhone: takeOverCustomer.PhoneNumber,
                        customerEmail: takeOverCustomer.EmailAddress,
                        takeOverDate: date.ToString("dd/MM/yyyy"),
                        takeOverNumber: takeOverId.ToString(),
                        accompte: accompte,
                        paid: paid,
                        mEDatas: mEDatas,
                        modeDePaiement: Tools.GetEnumDescriptionFromEnum<PaymentMode>(paymentMode),
                        path: $@"{Paths.PriseEnChargeDirectory}\{title}.docx",
                        title: title,
                        repairTypeNames: repairTypeNames,
                        unlockTypeNames: unlockTypeNames,
                        articles: articles,
                        marques: marques,
                        modeles: modeles);
                    // Vérifier si le fichier existe
                    if (System.IO.File.Exists($@"{Paths.PriseEnChargeDirectory}\{title}.docx"))
                    {
                        // Ouvrir le fichier PDF dans une fenêtre externe
                        Process.Start($@"{Paths.PriseEnChargeDirectory}\{title}.docx");

                        DialogResult dialogResultFacture = MessageBox.Show("Voulez-vous générer une facture ?", "Information", MessageBoxButtons.YesNo);
                        if (dialogResultFacture == DialogResult.Yes)
                        {
                            GenerateFacture(type, mEDatas, takeOverCustomer, takeOverId, date, repairTypeNames, unlockTypeNames, articles, marques, modeles);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Le fichier PDF n'existe pas.", "Alerte", MessageBoxButtons.OK);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private string GetItemTypeDescription(IEnumerable<MEData> dataList)
        {
            bool hasRepairs = dataList.Any(x => x.RepairTypeId.HasValue);
            bool hasUnlocks = dataList.Any(x => x.UnlockTypeId.HasValue);
            bool hasArticles = dataList.Any(x => x.ArticleId.HasValue);

            if (hasRepairs && hasUnlocks && hasArticles) return "Les réparations, les déblocages, et les achats";
            if (hasRepairs && hasUnlocks) return "Les réparations et les déblocages";
            if (hasRepairs && hasArticles) return "Les réparations et les achats";
            if (hasRepairs) return "Les réparations";
            if (hasUnlocks && hasArticles) return "Les déblocages et les achats";
            if (hasUnlocks) return "Les déblocages";
            return "Les achats";
        }
        private void ProcessDataGridViewRow(int type, DataGridViewRow row, ref List<MEData> mEDatas, ref Dictionary<int, string> repairTypeNames, ref Dictionary<int, string> unlockTypeNames, ref Dictionary<int, string> articles, ref Dictionary<int, string> marques, ref Dictionary<int, string> modeles, int takeOverId, DateTime date, Customer takeOverCustomer, decimal accompte, decimal resteDu, decimal paid, decimal total, PaymentMode paymentMode)
        {
            try
            {
                if (row.IsNewRow)
                    return;

                int id = (int)row.Cells["id"].Value;
                Marque marque = null;
                Modele modele = null;
                Article article = null;
                RepairType repairType = null;
                UnlockType unlockType = null;
                string typeText = null;
                bool? isGarantie = (bool?)row.Cells["garantie"].Value;
                decimal? price = row.Cells["price"].Value as decimal?;
                int quantity = 1;

                if (type == 2)
                {
                    string articleName = row.Cells["article"].Value as string;
                    article = TakeOverTypesDS
                        .First(x => x.Id == 2).Articles
                        .FirstOrDefault(a => string.Compare(articleName,
                            $"{Services.GetPropertyValue(a, "Produit")}" +
                            (a.MarqueId == null ? string.Empty : $" - {MarquesDS.FirstOrDefault(x => x.Id == a.MarqueId).Name}") +
                            (a.ModeleId == null ? string.Empty : $" {ModelesDS.FirstOrDefault(x => x.Id == a.ModeleId && x.MarqueId == a.MarqueId)?.Name ?? string.Empty}") +
                            (string.IsNullOrWhiteSpace(a.CodeReference) ? string.Empty : $" - {Services.GetPropertyValue(a, "CodeReference")}")) == 0);
                    marque = MarquesDS.FirstOrDefault(x => article.MarqueId == x.Id);
                    modele = ModelesDS.FirstOrDefault(x => article.MarqueId == x.MarqueId && article.ModeleId == x.Id);
                    quantity = (row.Cells["quantity"].Value as int?).Value;
                }
                else
                {
                    typeText = row.Cells["type"].Value as string;
                    string marqueText = row.Cells["marque"].Value as string;
                    string modeleText = row.Cells["modele"].Value as string;
                    marque = MarquesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), marqueText?.ToLower()) == 0);
                    modele = ModelesDS.FirstOrDefault(x => x.MarqueId == (marque?.Id ?? 0) && string.Compare(x.Name.ToLower(), modeleText?.ToLower()) == 0);
                    repairType = TakeOverTypesDS.First(x => x.Id == 0).RepairTypes
                        .FirstOrDefault(x => string.Compare(x.Name.ToLower(), typeText?.ToLower()) == 0);
                    unlockType = TakeOverTypesDS.First(x => x.Id == 1).UnlockTypes
                        .FirstOrDefault(x => string.Compare(x.Name.ToLower(), typeText?.ToLower()) == 0);
                }

                ProcessRowData(row, ref mEDatas, ref repairTypeNames, ref unlockTypeNames, ref articles, ref marques, ref modeles, takeOverId, date, takeOverCustomer, accompte, resteDu, paid, total, paymentMode, id, marque, modele, repairType, unlockType, article, price, quantity, isGarantie);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void ProcessRowData(DataGridViewRow row, ref List<MEData> mEDatas, ref Dictionary<int, string> repairTypeNames, ref Dictionary<int, string> unlockTypeNames, ref Dictionary<int, string> articles, ref Dictionary<int, string> marques, ref Dictionary<int, string> modeles, int takeOverId, DateTime date, Customer takeOverCustomer, decimal accompte, decimal resteDu, decimal paid, decimal total, PaymentMode paymentMode, int id, Marque marque, Modele modele, RepairType repairType, UnlockType unlockType, Article article, decimal? price, int quantity, bool? isGarantie)
        {
            try
            {
                if (repairType != null && !repairTypeNames.ContainsKey(repairType.Id))
                {
                    repairTypeNames.Add(repairType.Id, repairType.Name);
                }
                if (unlockType != null && !unlockTypeNames.ContainsKey(unlockType.Id))
                {
                    unlockTypeNames.Add(unlockType.Id, unlockType.Name);
                }
                if (article != null && !articles.ContainsKey(article.Id))
                {
                    articles.Add(article.Id, article.Produit);
                }

                if (marque != null && !marques.ContainsKey(marque.Id))
                {
                    marques.Add(marque.Id, marque.Name);
                }

                if (modele != null && !modeles.ContainsKey(modele.Id))
                {
                    modeles.Add(modele.Id, modele.Name);
                }

                Garantie garantie = isGarantie.HasValue && isGarantie.Value ? GarantiesTemp.FirstOrDefault(x => x.Id == id) : null;

                MEData newTakeOver = new MEData(
                    takeOverId, date, null, takeOverCustomer.Id, marque?.Id,
                    modele?.Id, null, repairType?.Id, unlockType?.Id, article?.Id,
                    article?.DisplayText, quantity, price, garantie?.Months, null,
                    accompte, resteDu, paid, total, paymentMode,
                    TakeOverState.InProgress, id, false, garantie?.Option ?? false);

                mEDatas.Add(newTakeOver);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void buttonTakeOverManageGarantie_Click(object sender, EventArgs e)
        {
            try
            {
                using (GarantieForm dialog = new GarantieForm(GarantiesTemp))
                {
                    // Afficher la boîte de dialogue modale
                    if (!dialog.IsDisposed && dialog.ShowDialog() == DialogResult.OK)
                    {
                        // Récupérer les éléments cochés
                        GarantiesTemp = dialog.GetResult().ToList();

                        foreach (Garantie garantie in GarantiesTemp)
                        {
                            for (int i = 0; i < MEDatasDS.Count; i++)
                            {
                                if (garantie.Id == MEDatasDS[i].Id)
                                {
                                    MEDatasDS[i].Garantie = garantie.Months;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void buttonTakeOverManageRemise_Click(object sender, EventArgs e)
        {
            try
            {
                using (RemiseForm dialog = new RemiseForm(RemisesTemp))
                {
                    // Afficher la boîte de dialogue modale
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        // Récupérer les éléments cochés
                        RemisesTemp = dialog.GetResult().ToList();

                        foreach (Remise remise in RemisesTemp)
                        {
                            if (MEDatasDS.Any(x => x.Id == remise.Id))
                            {
                                for (int i = 0; i < MEDatasDS.Count; i++)
                                {
                                    if (remise.Id == MEDatasDS[i].Id)
                                    {
                                        MEDatasDS[i].Remise = remise.Prix;
                                    }
                                }
                            }
                        }
                        UpdateTotalPrice();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        #region Reparation
        private void DataGridViewTakeOverRepair_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewTakeOverRepair.CurrentCell is DataGridViewComboBoxCell)
            {
                dataGridViewTakeOverRepair.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void DataGridViewTakeOverRepair_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (0 <= e.RowIndex && e.RowIndex < dataGridViewTakeOverRepair.Rows.Count &&
                    0 <= e.ColumnIndex && e.ColumnIndex < dataGridViewTakeOverRepair.Columns.Count &&
                    !dataGridViewTakeOverRepair.Rows[e.RowIndex].IsNewRow &&
                    (dataGridViewTakeOverRepair["id", e.RowIndex].Value as int?).HasValue)
                {
                    int id = (dataGridViewTakeOverRepair["id", e.RowIndex].Value as int?).Value;

                    if (e.ColumnIndex == dataGridViewTakeOverRepair["delete", e.RowIndex].ColumnIndex)
                    {
                        dataGridViewTakeOverRepair.Rows.RemoveAt(e.RowIndex);
                    }
                    if (e.ColumnIndex == dataGridViewTakeOverRepair["garantie", e.RowIndex].ColumnIndex)
                    {
                        bool isGarantie = (dataGridViewTakeOverRepair["garantie", e.RowIndex].Value as bool?).HasValue ?
                            (dataGridViewTakeOverRepair["garantie", e.RowIndex].Value as bool?).Value : false;

                        // si le click == déjà coché : retirer la garantie
                        if (isGarantie)
                        {
                            dataGridViewTakeOverRepair["garantie", e.RowIndex].Value = false;

                            MEData oldMEData = MEDatasDS.FirstOrDefault(x => x.Id == id);
                            // si oldreceipt != null : c'est un ancien receipt
                            // retirer la garantie de receipt
                            if (oldMEData != null)
                            {
                                for (int i = 0; i < MEDatasDS.Count; i++)
                                {
                                    if (MEDatasDS[i].Id == oldMEData.Id)
                                    {
                                        MEDatasDS[i].Garantie = null;
                                        break;
                                    }
                                }
                            }
                            // supprimer la ligne de garantie
                            for (int i = 0; i < GarantiesTemp.Count; i++)
                            {
                                if (id == GarantiesTemp[i].Id)
                                {
                                    GarantiesTemp.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                        // si le click == décoché : ajouter la garantie
                        else if (!isGarantie)
                        {
                            dataGridViewTakeOverRepair["garantie", e.RowIndex].Value = true;

                            // ajouter la ligne de garantie
                            string marqueText = dataGridViewTakeOverRepair["marque", e.RowIndex].Value as string;
                            string modeleText = dataGridViewTakeOverRepair["modele", e.RowIndex].Value as string;
                            string typeText =
                                !string.IsNullOrWhiteSpace(marqueText) && !string.IsNullOrWhiteSpace(modeleText) ? marqueText + " " + modeleText + " - " + (dataGridViewTakeOverRepair["type", e.RowIndex].Value as string) :
                                !string.IsNullOrWhiteSpace(marqueText) && string.IsNullOrWhiteSpace(modeleText) ? marqueText + " - " + (dataGridViewTakeOverRepair["type", e.RowIndex].Value as string) :
                                (dataGridViewTakeOverRepair["type", e.RowIndex].Value as string);
                            GarantiesTemp.Add(new Garantie(tabName: "Réparation", id: id, productName: typeText, months: null, option: false));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void DataGridViewTakeOverRepair_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int currentColumnIndex = dataGridViewTakeOverRepair.CurrentCell.ColumnIndex;
                int currentRowIndex = dataGridViewTakeOverRepair.CurrentCell.RowIndex;

                if (0 <= currentRowIndex && currentRowIndex < dataGridViewTakeOverRepair.Rows.Count &&
                    0 <= currentColumnIndex && currentColumnIndex < dataGridViewTakeOverRepair.Columns.Count)
                {
                    int id = (dataGridViewTakeOverRepair["id", currentRowIndex].Value as int?).Value;
                    DataGridViewTextBoxEditingControl textBox = (DataGridViewTextBoxEditingControl)e.Control;
                    DataGridViewTextBoxEditingControl textBoxEditingControl = (sender as DataGridView).EditingControl as DataGridViewTextBoxEditingControl;

                    if (textBox != null)
                    {
                        if (currentColumnIndex == dataGridViewTakeOverRepair["marque", currentRowIndex].ColumnIndex)
                        {
                            // Créez et initialisez votre source de données pour l'auto-complétion
                            AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                            autoCompleteData.AddRange(MarquesDS.Select(x => x.Name).ToArray());

                            // Définissez la source de données de l'auto-complétion pour le TextBox
                            textBox.Clear();
                            textBox.AutoCompleteCustomSource = autoCompleteData;
                            textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                        if (currentColumnIndex == dataGridViewTakeOverRepair["modele", currentRowIndex].ColumnIndex)
                        {
                            // Créez et initialisez votre source de données pour l'auto-complétion
                            AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();

                            string marqueName = dataGridViewTakeOverRepair["marque", currentRowIndex].Value as string;
                            Marque marque = null;
                            if (!string.IsNullOrWhiteSpace(marqueName))
                            {
                                marque = MarquesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), marqueName.ToLower()) == 0);
                                autoCompleteData.AddRange(ModelesDS.Where(x => x.MarqueId == marque.Id).Select(x => x.Name).ToArray());
                            }
                            else
                            {
                                MessageBox.Show("Aucune marque n'a été saisie. Veuillez en saisir une.", "Alerte", MessageBoxButtons.OK);
                                return;
                            }

                            if (autoCompleteData.Count == 0)
                            {
                                autoCompleteData.AddRange(ModelesDS.Select(x => x.Name).ToArray());
                            }

                            // Définissez la source de données de l'auto-complétion pour le TextBox
                            textBox.Clear();
                            textBox.AutoCompleteCustomSource = autoCompleteData;
                            textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                        if (currentColumnIndex == dataGridViewTakeOverRepair["type", currentRowIndex].ColumnIndex)
                        {
                            // Créez et initialisez votre source de données pour l'auto-complétion
                            AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                            List<RepairType> repairTypes = TakeOverTypesDS.Find(x => x.Id == 0).RepairTypes;

                            autoCompleteData.AddRange(repairTypes.Select(x => x.Name).ToArray());

                            // Définissez la source de données de l'auto-complétion pour le TextBox
                            textBox.Clear();
                            textBox.AutoCompleteCustomSource = autoCompleteData;
                            textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void DataGridViewTakeOverRepair_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (0 <= e.RowIndex && e.RowIndex < dataGridViewTakeOverRepair.Rows.Count &&
                    0 <= e.ColumnIndex && e.ColumnIndex < dataGridViewTakeOverRepair.Columns.Count)
                {
                    if (!(dataGridViewTakeOverRepair["id", e.RowIndex].Value as int?).HasValue)
                    {
                        dataGridViewTakeOverRepair["id", e.RowIndex].Value = GetItemId();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void DataGridViewTakeOverRepair_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (0 <= e.RowIndex && e.RowIndex < dataGridViewTakeOverRepair.Rows.Count &&
                    0 <= e.ColumnIndex && e.ColumnIndex < dataGridViewTakeOverRepair.Columns.Count &&
                    !dataGridViewTakeOverRepair.Rows[e.RowIndex].IsNewRow &&
                    (dataGridViewTakeOverRepair["id", e.RowIndex].Value as int?).HasValue)
                {
                    int id = (dataGridViewTakeOverRepair["id", e.RowIndex].Value as int?).Value;

                    if (e.ColumnIndex == dataGridViewTakeOverRepair["price", e.RowIndex].ColumnIndex)
                    {
                        UpdateTotalPrice();
                    }
                    if (e.ColumnIndex == dataGridViewTakeOverRepair["marque", e.RowIndex].ColumnIndex)
                    {
                        string marqueName = dataGridViewTakeOverRepair["marque", e.RowIndex].Value as string;
                        if (!string.IsNullOrWhiteSpace(marqueName))
                        {
                            Marque marque = MarquesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), marqueName.ToLower()) == 0);
                            if (marque == null)
                            {
                                marque = new Marque(MarquesDS.OrderByDescending(x => x.Id).First().Id + 1, char.ToUpper(marqueName[0]) + marqueName.Substring(1));
                                DialogResult result = MessageBox.Show($"La marque \"{marqueName}\" n'est pas connue du système. Voulez-vous l'ajouter ?", "Information", MessageBoxButtons.YesNo);
                                if (result == DialogResult.Yes)
                                {
                                    MarquesDS.Add(marque);
                                    Tools.WriteLineTofile($"{marque.Id};{marque.Name}", Paths.MarquesDSPath, true);
                                }
                            }
                        }
                    }
                    if (e.ColumnIndex == dataGridViewTakeOverRepair["modele", e.RowIndex].ColumnIndex)
                    {
                        string modeleName = dataGridViewTakeOverRepair["modele", e.RowIndex].Value as string;
                        if (!string.IsNullOrWhiteSpace(modeleName))
                        {
                            Modele modele = ModelesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), modeleName.ToLower()) == 0);
                            if (modele == null)
                            {
                                string marqueName = dataGridViewTakeOverRepair["marque", e.RowIndex].Value as string;
                                Marque marque = null;
                                if (!string.IsNullOrWhiteSpace(marqueName))
                                {
                                    marque = MarquesDS.First(x => string.Compare(x.Name.ToLower(), marqueName.ToLower()) == 0);
                                }
                                else
                                {
                                    MessageBox.Show("Pour ajouter un nouveau modèle, assurez-vous d'avoir sélectionné une marque.", "Alerte");
                                    return;
                                }

                                modele = new Modele(ModelesDS.OrderByDescending(x => x.Id).First().Id + 1, marque.Id, char.ToUpper(modeleName[0]) + modeleName.Substring(1));
                                DialogResult result = MessageBox.Show($"Le modele \"{modeleName}\" n'est pas connue du système. Voulez-vous l'ajouter ?", "Information", MessageBoxButtons.YesNo);
                                if (result == DialogResult.Yes)
                                {
                                    ModelesDS.Add(modele);
                                    Tools.WriteLineTofile($"{modele.Id};{modele.MarqueId};{modele.Name}", Paths.ModelesDSPath, true);
                                }
                            }
                        }
                    }
                    if (e.ColumnIndex == dataGridViewTakeOverRepair["type", e.RowIndex].ColumnIndex)
                    {
                        string typeName = dataGridViewTakeOverRepair["type", e.RowIndex].Value as string;
                        if (!string.IsNullOrWhiteSpace(typeName))
                        {
                            RepairType repairType = TakeOverTypesDS.Find(x => x.Id == 0).RepairTypes.FirstOrDefault(x => string.Compare(x.Name.ToLower(), typeName.ToLower()) == 0);
                            if (repairType == null)
                            {
                                repairType = new RepairType(TakeOverTypesDS.Find(x => x.Id == 0).RepairTypes.OrderByDescending(x => x.Id).First().Id + 1, char.ToUpper(typeName[0]) + typeName.Substring(1), 0);
                                DialogResult result = MessageBox.Show($"La réparation \"{typeName}\" n'est pas connue du système. Voulez-vous l'ajouter ?", "Information", MessageBoxButtons.YesNo);
                                if (result == DialogResult.Yes)
                                {
                                    TakeOverTypesDS.Find(x => x.Id == 0).RepairTypes.Add(repairType);
                                    Tools.WriteLineTofile($"{repairType.Id};{repairType.Name};{repairType.Price}", Paths.RepairTypesDSPath, true);
                                }
                            }
                            else
                            {
                                dataGridViewTakeOverRepair["price", e.RowIndex].Value = repairType.Price;
                                UpdateTotalPrice();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        #endregion
        #region Deblocage
        private void DataGridViewTakeOverUnlock_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewTakeOverUnlock.CurrentCell is DataGridViewComboBoxCell)
            {
                dataGridViewTakeOverUnlock.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void DataGridViewTakeOverUnlock_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (0 <= e.RowIndex && e.RowIndex < dataGridViewTakeOverUnlock.Rows.Count &&
                    0 <= e.ColumnIndex && e.ColumnIndex < dataGridViewTakeOverUnlock.Columns.Count &&
                    !dataGridViewTakeOverUnlock.Rows[e.RowIndex].IsNewRow &&
                    (dataGridViewTakeOverUnlock["id", e.RowIndex].Value as int?).HasValue)
                {
                    int id = (dataGridViewTakeOverUnlock["id", e.RowIndex].Value as int?).Value;

                    if (e.ColumnIndex == dataGridViewTakeOverUnlock["delete", e.RowIndex].ColumnIndex)
                    {
                        dataGridViewTakeOverUnlock.Rows.RemoveAt(e.RowIndex);
                    }
                    if (e.ColumnIndex == dataGridViewTakeOverUnlock["garantie", e.RowIndex].ColumnIndex)
                    {
                        bool isGarantie = (dataGridViewTakeOverUnlock["garantie", e.RowIndex].Value as bool?).HasValue ?
                            (dataGridViewTakeOverUnlock["garantie", e.RowIndex].Value as bool?).Value : false;

                        // si le click == déjà coché : retirer la garantie
                        if (isGarantie)
                        {
                            dataGridViewTakeOverUnlock["garantie", e.RowIndex].Value = false;

                            MEData oldMEData = MEDatasDS.FirstOrDefault(x => x.Id == id);
                            // si oldreceipt != null : c'est un ancien receipt
                            // retirer la garantie de receipt
                            if (oldMEData != null)
                            {
                                for (int i = 0; i < MEDatasDS.Count; i++)
                                {
                                    if (MEDatasDS[i].Id == oldMEData.Id)
                                    {
                                        MEDatasDS[i].Garantie = null;
                                        break;
                                    }
                                }
                            }
                            // supprimer la ligne de garantie
                            for (int i = 0; i < GarantiesTemp.Count; i++)
                            {
                                if (id == GarantiesTemp[i].Id)
                                {
                                    GarantiesTemp.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                        // si le click == décoché : ajouter la garantie
                        else if (!isGarantie)
                        {
                            dataGridViewTakeOverUnlock["garantie", e.RowIndex].Value = true;

                            // ajouter la ligne de garantie
                            string marqueText = dataGridViewTakeOverUnlock["marque", e.RowIndex].Value as string;
                            string modeleText = dataGridViewTakeOverUnlock["modele", e.RowIndex].Value as string;
                            string typeText = dataGridViewTakeOverUnlock["type", e.RowIndex].Value as string;

                            string productName =
                                !string.IsNullOrWhiteSpace(marqueText) && !string.IsNullOrWhiteSpace(modeleText) ? marqueText + " " + modeleText + " - " + typeText :
                                !string.IsNullOrWhiteSpace(marqueText) && string.IsNullOrWhiteSpace(modeleText) ? marqueText + " - " + typeText :
                                typeText;
                            GarantiesTemp.Add(new Garantie(tabName: "Déblocage", id: id, productName: productName, months: null, option: false));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void DataGridViewTakeOverUnlock_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int currentColumnIndex = dataGridViewTakeOverUnlock.CurrentCell.ColumnIndex;
                int currentRowIndex = dataGridViewTakeOverUnlock.CurrentCell.RowIndex;

                if (0 <= currentRowIndex && currentRowIndex < dataGridViewTakeOverUnlock.Rows.Count &&
                    0 <= currentColumnIndex && currentColumnIndex < dataGridViewTakeOverUnlock.Columns.Count)
                {
                    int id = (dataGridViewTakeOverUnlock["id", currentRowIndex].Value as int?).Value;
                    DataGridViewTextBoxEditingControl textBox = (DataGridViewTextBoxEditingControl)e.Control;

                    if (textBox != null)
                    {
                        if (currentColumnIndex == dataGridViewTakeOverUnlock["marque", currentRowIndex].ColumnIndex)
                        {
                            // Créez et initialisez votre source de données pour l'auto-complétion
                            AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                            autoCompleteData.AddRange(MarquesDS.Select(x => x.Name).ToArray());

                            // Définissez la source de données de l'auto-complétion pour le TextBox
                            textBox.AutoCompleteCustomSource = autoCompleteData;
                            textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                        if (currentColumnIndex == dataGridViewTakeOverUnlock["modele", currentRowIndex].ColumnIndex)
                        {
                            // Créez et initialisez votre source de données pour l'auto-complétion
                            AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();

                            string marqueName = dataGridViewTakeOverUnlock["marque", currentRowIndex].Value as string;
                            Marque marque = null;
                            if (!string.IsNullOrWhiteSpace(marqueName))
                            {
                                marque = MarquesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), marqueName.ToLower()) == 0);
                                autoCompleteData.AddRange(ModelesDS.Where(x => x.MarqueId == marque.Id).Select(x => x.Name).ToArray());
                            }
                            else
                            {
                                MessageBox.Show("Aucune marque n'a été saisie. Veuillez en saisir une.", "Alerte", MessageBoxButtons.OK);
                                return;
                            }

                            if (autoCompleteData.Count == 0)
                            {
                                autoCompleteData.AddRange(ModelesDS.Select(x => x.Name).ToArray());
                            }

                            // Définissez la source de données de l'auto-complétion pour le TextBox
                            textBox.AutoCompleteCustomSource = autoCompleteData;
                            textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                        if (currentColumnIndex == dataGridViewTakeOverUnlock["type", currentRowIndex].ColumnIndex)
                        {
                            // Créez et initialisez votre source de données pour l'auto-complétion
                            AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                            List<UnlockType> unlockTypes = TakeOverTypesDS.Find(x => x.Id == 1).UnlockTypes;

                            autoCompleteData.AddRange(unlockTypes.Select(x => x.Name).ToArray());

                            // Définissez la source de données de l'auto-complétion pour le TextBox
                            textBox.AutoCompleteCustomSource = autoCompleteData;
                            textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void DataGridViewTakeOverUnlock_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (0 <= e.RowIndex && e.RowIndex < dataGridViewTakeOverUnlock.Rows.Count &&
                    0 <= e.ColumnIndex && e.ColumnIndex < dataGridViewTakeOverUnlock.Columns.Count)
                {
                    if (!(dataGridViewTakeOverUnlock["id", e.RowIndex].Value as int?).HasValue)
                    {
                        dataGridViewTakeOverUnlock["id", e.RowIndex].Value = GetItemId();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void DataGridViewTakeOverUnlock_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (0 <= e.RowIndex && e.RowIndex < dataGridViewTakeOverUnlock.Rows.Count &&
                    0 <= e.ColumnIndex && e.ColumnIndex < dataGridViewTakeOverUnlock.Columns.Count &&
                    !dataGridViewTakeOverUnlock.Rows[e.RowIndex].IsNewRow &&
                    (dataGridViewTakeOverUnlock["id", e.RowIndex].Value as int?).HasValue)
                {
                    int id = (dataGridViewTakeOverUnlock["id", e.RowIndex].Value as int?).Value;

                    if (e.ColumnIndex == dataGridViewTakeOverUnlock["price", e.RowIndex].ColumnIndex)
                    {
                        UpdateTotalPrice();
                    }
                    if (e.ColumnIndex == dataGridViewTakeOverUnlock["marque", e.RowIndex].ColumnIndex)
                    {
                        string marqueName = dataGridViewTakeOverUnlock["marque", e.RowIndex].Value as string;
                        if (!string.IsNullOrWhiteSpace(marqueName))
                        {
                            Marque marque = MarquesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), marqueName.ToLower()) == 0);
                            if (marque == null)
                            {
                                marque = new Marque(MarquesDS.OrderByDescending(x => x.Id).First().Id + 1, char.ToUpper(marqueName[0]) + marqueName.Substring(1));
                                DialogResult result = MessageBox.Show($"La marque \"{marqueName}\" n'est pas connue du système. Voulez-vous l'ajouter ?", "Information", MessageBoxButtons.YesNo);
                                if (result == DialogResult.Yes)
                                {
                                    MarquesDS.Add(marque);
                                    Tools.WriteLineTofile($"{marque.Id};{marque.Name}", Paths.MarquesDSPath, true);
                                }
                            }
                        }
                    }
                    if (e.ColumnIndex == dataGridViewTakeOverUnlock["modele", e.RowIndex].ColumnIndex)
                    {
                        string modeleName = dataGridViewTakeOverUnlock["modele", e.RowIndex].Value as string;
                        if (!string.IsNullOrWhiteSpace(modeleName))
                        {
                            Modele modele = ModelesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), modeleName.ToLower()) == 0);
                            if (modele == null)
                            {
                                string marqueName = dataGridViewTakeOverUnlock["marque", e.RowIndex].Value as string;
                                Marque marque = null;
                                if (!string.IsNullOrWhiteSpace(marqueName))
                                {
                                    marque = MarquesDS.First(x => string.Compare(x.Name.ToLower(), marqueName.ToLower()) == 0);
                                }
                                else
                                {
                                    MessageBox.Show("Pour ajouter un nouveau modèle, assurez-vous d'avoir sélectionné une marque.", "Alerte");
                                    return;
                                }

                                modele = new Modele(ModelesDS.OrderByDescending(x => x.Id).First().Id + 1, marque.Id, char.ToUpper(modeleName[0]) + modeleName.Substring(1));
                                DialogResult result = MessageBox.Show($"Le modele \"{modeleName}\" n'est pas connue du système. Voulez-vous l'ajouter ?", "Information", MessageBoxButtons.YesNo);
                                if (result == DialogResult.Yes)
                                {
                                    ModelesDS.Add(modele);
                                    Tools.WriteLineTofile($"{modele.Id};{modele.MarqueId};{modele.Name}", Paths.ModelesDSPath, true);
                                }
                            }
                        }
                    }
                    if (e.ColumnIndex == dataGridViewTakeOverUnlock["type", e.RowIndex].ColumnIndex)
                    {
                        string typeName = dataGridViewTakeOverUnlock["type", e.RowIndex].Value as string;
                        if (!string.IsNullOrWhiteSpace(typeName))
                        {
                            UnlockType unlockType = TakeOverTypesDS.Find(x => x.Id == 1).UnlockTypes.FirstOrDefault(x => string.Compare(x.Name.ToLower(), typeName.ToLower()) == 0);
                            if (unlockType == null)
                            {
                                unlockType = new UnlockType(TakeOverTypesDS.Find(x => x.Id == 1).UnlockTypes.OrderByDescending(x => x.Id).First().Id + 1, char.ToUpper(typeName[0]) + typeName.Substring(1), 0);
                                DialogResult result = MessageBox.Show($"Le déblocage \"{typeName}\" n'est pas connue du système. Voulez-vous l'ajouter ?", "Information", MessageBoxButtons.YesNo);
                                if (result == DialogResult.Yes)
                                {
                                    TakeOverTypesDS.Find(x => x.Id == 1).UnlockTypes.Add(unlockType);
                                    Tools.WriteLineTofile($"{unlockType.Id};{unlockType.Name};{unlockType.Price}", Paths.UnlockTypesDSPath, true);
                                }
                            }
                            else
                            {
                                dataGridViewTakeOverUnlock["price", e.RowIndex].Value = unlockType.Price;
                                UpdateTotalPrice();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        #endregion
        #region Achat
        private void DataGridViewTakeOverAchat_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (dataGridViewTakeOverAchat.IsCurrentCellDirty)
                {
                    dataGridViewTakeOverAchat.CommitEdit(DataGridViewDataErrorContexts.Commit);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void DataGridViewTakeOverAchat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (0 <= e.RowIndex && e.RowIndex < dataGridViewTakeOverAchat.Rows.Count &&
                    0 <= e.ColumnIndex && e.ColumnIndex < dataGridViewTakeOverAchat.Columns.Count &&
                    !dataGridViewTakeOverAchat.Rows[e.RowIndex].IsNewRow &&
                    (dataGridViewTakeOverAchat["id", e.RowIndex].Value as int?).HasValue)
                {
                    int id = (dataGridViewTakeOverAchat["id", e.RowIndex].Value as int?).Value;

                    if (e.ColumnIndex == dataGridViewTakeOverAchat["delete", e.RowIndex].ColumnIndex)
                    {
                        dataGridViewTakeOverAchat.Rows.RemoveAt(e.RowIndex);
                    }
                    if (e.ColumnIndex == dataGridViewTakeOverAchat["garantie", e.RowIndex].ColumnIndex)
                    {
                        bool isGarantie = (dataGridViewTakeOverAchat["garantie", e.RowIndex].Value as bool?).HasValue ?
                            (dataGridViewTakeOverAchat["garantie", e.RowIndex].Value as bool?).Value : false;

                        // si le click == déjà coché : retirer la garantie
                        if (isGarantie)
                        {
                            dataGridViewTakeOverAchat["garantie", e.RowIndex].Value = false;

                            MEData oldMEData = MEDatasDS.FirstOrDefault(x => x.Id == id);
                            // si oldreceipt != null : c'est un ancien receipt
                            // retirer la garantie de receipt
                            if (oldMEData != null)
                            {
                                for (int i = 0; i < MEDatasDS.Count; i++)
                                {
                                    if (MEDatasDS[i].Id == oldMEData.Id)
                                    {
                                        MEDatasDS[i].Garantie = null;
                                        break;
                                    }
                                }
                            }
                            // supprimer la ligne de garantie
                            for (int i = 0; i < GarantiesTemp.Count; i++)
                            {
                                if (id == GarantiesTemp[i].Id)
                                {
                                    GarantiesTemp.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                        // si le click == décoché : ajouter la garantie
                        else if (!isGarantie)
                        {
                            dataGridViewTakeOverAchat["garantie", e.RowIndex].Value = true;

                            // ajouter la ligne de garantie
                            string marqueText = dataGridViewTakeOverAchat["marque", e.RowIndex].Value as string;
                            string modeleText = dataGridViewTakeOverAchat["modele", e.RowIndex].Value as string;
                            string productName =
                                !string.IsNullOrWhiteSpace(marqueText) && !string.IsNullOrWhiteSpace(modeleText) ? marqueText + " " + modeleText + " - " + (dataGridViewTakeOverAchat["article", e.RowIndex].Value as string) :
                                !string.IsNullOrWhiteSpace(marqueText) && string.IsNullOrWhiteSpace(modeleText) ? marqueText + " - " + (dataGridViewTakeOverAchat["article", e.RowIndex].Value as string) :
                                (dataGridViewTakeOverAchat["article", e.RowIndex].Value as string);
                            GarantiesTemp.Add(new Garantie(tabName: "Déblocage", id: id, productName: productName, months: null, option: false));
                        }
                    }
                    if (e.ColumnIndex == dataGridViewTakeOverAchat["remise", e.RowIndex].ColumnIndex)
                    {
                        bool isRemise = (dataGridViewTakeOverAchat["remise", e.RowIndex].Value as bool?).HasValue ?
                            (dataGridViewTakeOverAchat["remise", e.RowIndex].Value as bool?).Value : false;

                        // si le click == cocher : retirer la remise
                        if (isRemise)
                        {
                            dataGridViewTakeOverAchat["remise", e.RowIndex].Value = false;
                            MEData oldMEData = MEDatasDS.FirstOrDefault(x => x.Id == id);
                            // si oldreceipt != null : c'est un ancien receipt
                            // retirer la remise de receipt
                            if (oldMEData != null)
                            {
                                for (int i = 0; i < MEDatasDS.Count; i++)
                                {
                                    if (MEDatasDS[i].Id == oldMEData.Id)
                                    {
                                        MEDatasDS[i].Remise = null;
                                        break;
                                    }
                                }
                            }
                            // supprimer la ligne de remise
                            for (int i = 0; i < RemisesTemp.Count; i++)
                            {
                                if (id == RemisesTemp[i].Id)
                                {
                                    RemisesTemp.RemoveAt(i);
                                    break;
                                }
                            }
                        }
                        // si le click == décocher : ajouter la remise
                        else if (!isRemise)
                        {
                            dataGridViewTakeOverAchat["remise", e.RowIndex].Value = true;
                            // ajouter la ligne de remise
                            string marqueText = dataGridViewTakeOverAchat["marque", e.RowIndex].Value as string;
                            string modeleText = dataGridViewTakeOverAchat["modele", e.RowIndex].Value as string;
                            string productName =
                                !string.IsNullOrWhiteSpace(marqueText) && !string.IsNullOrWhiteSpace(modeleText) ? marqueText + " " + modeleText + " - " + (dataGridViewTakeOverAchat["article", e.RowIndex].Value as string) :
                                !string.IsNullOrWhiteSpace(marqueText) && string.IsNullOrWhiteSpace(modeleText) ? marqueText + " - " + (dataGridViewTakeOverAchat["article", e.RowIndex].Value as string) :
                                (dataGridViewTakeOverAchat["article", e.RowIndex].Value as string);
                            RemisesTemp.Add(new Remise(tabName: "Achat", id: id, productName: productName, prix: null));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void DataGridViewTakeOverAchat_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int currentColumnIndex = dataGridViewTakeOverAchat.CurrentCell.ColumnIndex;
                int currentRowIndex = dataGridViewTakeOverAchat.CurrentCell.RowIndex;

                if (0 <= currentRowIndex && currentRowIndex < dataGridViewTakeOverAchat.Rows.Count &&
                    0 <= currentColumnIndex && currentColumnIndex < dataGridViewTakeOverAchat.Columns.Count)
                {
                    // Vérifiez si la colonne en cours d'édition est celle de l'article
                    if (dataGridViewTakeOverAchat.Columns[currentColumnIndex].Name == "article")
                    {
                        ComboBox comboBox = e.Control as ComboBox;
                        comboBox.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        comboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
                        comboBox.DropDownStyle = ComboBoxStyle.DropDown;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void DataGridViewTakeOverAchat_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (0 <= e.RowIndex && e.RowIndex < dataGridViewTakeOverAchat.Rows.Count &&
                    0 <= e.ColumnIndex && e.ColumnIndex < dataGridViewTakeOverAchat.Columns.Count)
                {
                    if (!(dataGridViewTakeOverAchat["id", e.RowIndex].Value as int?).HasValue)
                    {
                        dataGridViewTakeOverAchat["id", e.RowIndex].Value = GetItemId();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void DataGridViewTakeOverAchat_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (0 <= e.RowIndex && e.RowIndex < dataGridViewTakeOverAchat.Rows.Count &&
                0 <= e.ColumnIndex && e.ColumnIndex < dataGridViewTakeOverAchat.Columns.Count)
            {
                if (e.ColumnIndex == dataGridViewTakeOverAchat.Columns["article"].Index && e.RowIndex >= 0)
                {
                    string selectedArticleName = (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["article"].Value as string);
                    if (!string.IsNullOrWhiteSpace(selectedArticleName))
                    {
                        Article selectedArticle = TakeOverTypesDS.First(x => x.Id == 2).Articles.FirstOrDefault(a => a.DisplayText == selectedArticleName);

                        if (selectedArticle != null)
                        {
                            dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["id"].Value = selectedArticle.Id;
                            dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["quantity"].Value = 1;
                            dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["price"].Value = selectedArticle.Price;

                            UpdateTotalPrice();

                            dataGridViewTakeOverAchat.EndEdit();
                        }
                    }
                }
                else if (e.ColumnIndex == dataGridViewTakeOverAchat.Columns["quantity"].Index && e.RowIndex >= 0)
                {
                    int quantity = (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["quantity"].Value as int?) ?? 0;
                    if (quantity == 0)
                    {
                        dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["id"].Value = null;
                        dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["article"].Value = null;
                        dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["quantity"].Value = null;
                        dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["price"].Value = null;
                        return;
                    }
                    string selectedArticleName = (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["article"].Value as string);
                    if (!string.IsNullOrWhiteSpace(selectedArticleName))
                    {
                        Article selectedArticle = TakeOverTypesDS.First(x => x.Id == 2).Articles.FirstOrDefault(a => a.DisplayText == selectedArticleName);
                        if (selectedArticle != null)
                        {
                            dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["price"].Value = selectedArticle.Price;
                            UpdateTotalPrice();
                            dataGridViewTakeOverAchat.EndEdit();
                        }
                    }
                }
                else if (e.ColumnIndex == dataGridViewTakeOverAchat.Columns["price"].Index && e.RowIndex >= 0)
                {
                    UpdateTotalPrice();
                }
            }
        }
        #endregion
        private void UpdateTotalPrice()
        {
            try
            {
                decimal totalPrice = 0;
                decimal remise = 0;
                foreach (DataGridViewRow row in dataGridViewTakeOverRepair.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        decimal? price = row.Cells["price"].Value as decimal?;
                        if (price.HasValue)
                        {
                            totalPrice += price.Value;
                        }
                    }
                }
                foreach (DataGridViewRow row in dataGridViewTakeOverUnlock.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        decimal? price = row.Cells["price"].Value as decimal?;
                        if (price.HasValue)
                        {
                            totalPrice += price.Value;
                        }
                    }
                }
                foreach (DataGridViewRow row in dataGridViewTakeOverAchat.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        decimal? price = row.Cells["price"].Value as decimal?;
                        int? quantity = row.Cells["quantity"].Value as int?;
                        if (price.HasValue && quantity.HasValue)
                        {
                            bool? isRemise = row.Cells["remise"].Value as bool?;
                            int? id = row.Cells["id"].Value as int?;
                            if (isRemise.HasValue && isRemise.Value && id.HasValue && RemisesTemp.Any(x => x.Id == id.Value))
                            {
                                remise += RemisesTemp.First(x => x.Id == id.Value).Prix.Value;
                            }
                            totalPrice += price.Value * quantity.Value;
                        }
                    }
                }

                textBoxTakeOverTotalPrice.Text = (totalPrice - remise).ToString();

                decimal accompte = decimal.Parse(string.IsNullOrWhiteSpace(textBoxTakeOverAccompte.Text) ? "0" : textBoxTakeOverAccompte.Text);
                decimal paid = decimal.Parse(string.IsNullOrWhiteSpace(textBoxTakeOverPaid.Text) ? "0" : textBoxTakeOverPaid.Text);
                textBoxTakeOverResteDu.Text = (totalPrice - remise - accompte - paid).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private int GetItemId()
        {
            int newId = 0;

            if (MEDatasDS?.Any() ?? false)
            {
                newId = MEDatasDS.OrderByDescending(x => x.Id).First().Id;
            }

            foreach (DataGridViewRow row in dataGridViewTakeOverRepair.Rows)
            {
                if ((row.Cells["id"].Value as int?).HasValue)
                {
                    int id = (row.Cells["id"].Value as int?).Value;
                    newId = id > newId ? id : newId;
                }
            }
            foreach (DataGridViewRow row in dataGridViewTakeOverUnlock.Rows)
            {
                if ((row.Cells["id"].Value as int?).HasValue)
                {
                    int id = (row.Cells["id"].Value as int?).Value;
                    newId = id > newId ? id : newId;
                }
            }
            foreach (DataGridViewRow row in dataGridViewTakeOverAchat.Rows)
            {
                if ((row.Cells["id"].Value as int?).HasValue)
                {
                    int id = (row.Cells["id"].Value as int?).Value;
                    newId = id > newId ? id : newId;
                }
            }

            newId = newId + 1;
            return newId;
        }

        private List<CustomerView> CalculateCustomerViews(List<MEData> mEDatas, List<Customer> customers)
        {
            List<CustomerView> customerViews = new List<CustomerView>();
            foreach (Customer customer in customers)
            {
                customerViews.Add(new CustomerView()
                {
                    Id = customer.Id,
                    LastName = customer.LastName ?? string.Empty,
                    FirstName = customer.FirstName ?? string.Empty,
                    Phone = customer.PhoneNumber ?? string.Empty,
                    Email = customer.EmailAddress ?? string.Empty,
                    Count = mEDatas.Where(x => x.CustomerId == customer.Id).GroupBy(x => x.TakeOverId).Count().ToString(),
                });
            }
            return customerViews
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToList();
        }
        private List<Customer> SearchCustomers(string searchText)
        {
            searchText = searchText.ToUpper();

            List<Customer> customers = CustomersDS
                .Where(x =>
                    x.LastName.ToUpper().Contains(searchText) ||
                    x.FirstName.ToUpper().Contains(searchText) ||
                    x.PhoneNumber.ToUpper().Contains(searchText) ||
                    x.EmailAddress.ToUpper().Contains(searchText))
                .Distinct()
                .ToList();

            return customers;
        }
        private void ClearDataGridViewCustomerRelationAll()
        {
            dataGridViewCustomerRelationAll.DataSource = null;
            dataGridViewCustomerRelationAll.Rows.Clear();
            dataGridViewCustomerRelationAll.Columns.Clear();
        }
        private void TextBoxCustomerSearchAll_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                ClearDataGridViewCustomerRelationAll();
                List<CustomerView> customerViews = CalculateCustomerViews(MEDatasDS, CustomersDS);
                dataGridViewAllCustomerRelation_Load(customerViews);
            }
            else
            {
                List<Customer> customers = SearchCustomers(textBox.Text);
                if (customers?.Any() ?? false)
                {
                    ClearDataGridViewCustomerRelationAll();
                    List<CustomerView> customerViews = CalculateCustomerViews(MEDatasDS, customers);
                    dataGridViewAllCustomerRelation_Load(customerViews);
                }
            }
        }
        private void DataGridViewCustomerRelationAll_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                var customer = CustomersDS.First(x =>
                    string.Compare(x.LastName, (sender as DataGridView).Rows[e.RowIndex].Cells[0].Value as string) == 0 &&
                    string.Compare(x.FirstName, (sender as DataGridView).Rows[e.RowIndex].Cells[1].Value as string) == 0);

                comboBoxCustomerRelationSexe.SelectedItem = Tools.GetEnumDescriptionFromEnum<Sexe>(customer.Sexe);
                comboBoxCustomerRelationSexe.Text = Tools.GetEnumDescriptionFromEnum<Sexe>(customer.Sexe);
                textBoxCustomerRelationLastName.Text = customer.LastName;
                textBoxCustomerRelationFirstName.Text = customer.FirstName;
                textBoxCustomerRelationPhone.Text = customer.PhoneNumber;
                textBoxCustomerRelationEmailAddress.Text = customer.EmailAddress;
                CurrentCustomer = customer;

                dataGridViewOneCustomerRelation_Load(customer);
            }
        }
        private void buttonCustomerRelationSaveCustomerData_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxCustomerRelationLastName.Text) &&
                    string.IsNullOrWhiteSpace(textBoxCustomerRelationFirstName.Text) &&
                    string.IsNullOrWhiteSpace(textBoxCustomerRelationPhone.Text) &&
                    string.IsNullOrWhiteSpace(textBoxCustomerRelationEmailAddress.Text))
                {
                    MessageBox.Show("Veuillez saisir au moins le Numéro de téléphone, le Nom, le Prénom, ou l'Adresse mail.");
                    return;
                }

                CustomerRelationUpdateDatabase(new Customer()
                {
                    Id = CurrentCustomer.Id,
                    LastName = textBoxCustomerRelationLastName.Text,
                    FirstName = textBoxCustomerRelationFirstName.Text,
                    PhoneNumber = textBoxCustomerRelationPhone.Text,
                    EmailAddress = textBoxCustomerRelationEmailAddress.Text,
                    Sexe = Tools.GetEnumFromDescription<Sexe>(comboBoxCustomerRelationSexe.Text),
                }, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }
        private void CustomerRelationUpdateDatabase(Customer customer, int index)
        {
            string path = Paths.CustomersDSPath;

            try
            {
                if (!File.Exists(path))
                    throw new Exception($"Le fichier {path} n'existe pas.");

                // Mise à jour
                if (index == 0)
                {
                    CustomersDS.ForEach(c =>
                    {
                        if (c.Id == customer.Id)
                        {
                            c.LastName = customer.LastName;
                            c.FirstName = customer.FirstName;
                            c.PhoneNumber = customer.PhoneNumber;
                            c.EmailAddress = customer.EmailAddress;
                            c.Sexe = customer.Sexe;
                        }
                    });

                    List<string> items = new List<string>() { "Id;Nom;Prénom;Numéro de téléphone;Adresse mail;Sexe" };
                    items.AddRange(CustomersDS.Select(x => $"{x.Id};{x.LastName};{x.FirstName};{x.PhoneNumber};{x.EmailAddress};{Tools.GetEnumDescriptionFromEnum<Sexe>(x.Sexe)}"));
                    Tools.RewriteDataToFile(items, path, false);

                    InitializeLists();
                    TakeOver_Load();
                    dataGridViewAllCustomerRelation_Load(null);
                    dataGridViewStock_Load(null);

                    MessageBox.Show($"Les données de {customer.FirstName} {customer.LastName} ont été mises à jour.");
                    CurrentCustomer = null;
                }
                else if (index == 1)
                {

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur : " + e.Message);
            }
        }

        private List<Article> SearchArticles(string searchText)
        {
            searchText = searchText.ToUpper();

            List<Article> articles = TakeOverTypesDS.Find(x => x.Id == 2).Articles
                .Where(x =>
                {
                    bool likeProductName = x.Produit.ToLowerInvariant().Contains(searchText.ToLowerInvariant());
                    if (likeProductName)
                        return true;

                    bool likeCodeReference = x.CodeReference.ToLowerInvariant().Contains(searchText.ToLowerInvariant());
                    if (likeCodeReference)
                        return true;

                    string marqueName = MarquesDS.FirstOrDefault(m => m.Id == x.MarqueId)?.Name ?? string.Empty;
                    bool likeMarque = marqueName.ToLowerInvariant().Contains(searchText.ToLowerInvariant());
                    if (likeMarque)
                        return true;
                    
                    string modeleName = ModelesDS.FirstOrDefault(m => m.Id == x.ModeleId)?.Name ?? string.Empty;
                    bool likeModele = modeleName.ToLowerInvariant().Contains(searchText.ToLowerInvariant());
                    if (likeModele)
                        return true;
                    return false;
                }).Distinct().ToList();

            return articles;
        }
        private void ClearDataGridViewStock()
        {
            dataGridViewCustomerRelationAll.DataSource = null;
            dataGridViewCustomerRelationAll.Rows.Clear();
            dataGridViewCustomerRelationAll.Columns.Clear();
        }
        private void TextBoxStockSearch_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                ClearDataGridViewStock();
                dataGridViewStock_Load(null);
            }
            else
            {
                List<Article> articles = SearchArticles(textBox.Text);
                if (articles?.Any() ?? false)
                {
                    ClearDataGridViewStock();
                    dataGridViewStock_Load(articles);
                }
            }
        }
        private void DataGridViewStock_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;
                if (e.RowIndex == 5 && (sender as DataGridView)[e.ColumnIndex, e.RowIndex] is DataGridViewButtonCell)
                {
                    Action<Article> clickHandler = (Action<Article>)(sender as DataGridView).Columns[e.ColumnIndex].Tag;
                    var article = (Article)(sender as DataGridView).Rows[e.RowIndex].DataBoundItem;

                    clickHandler(article);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void DataGridViewStock_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0 || e.ColumnIndex < 0)
                    return;
                int? id = dataGridViewStock.Rows[e.RowIndex].Cells["textBoxStockId"].Value as int?;
                if (e.RowIndex >= 0 &&
                    !dataGridViewStock.Rows[e.RowIndex].IsNewRow &&
                    id.HasValue)
                {
                    Article articleToUpdate = TakeOverTypesDS.First(x => x.Id == 2).Articles.First(x => x.Id == id.Value);

                    using (ArticleForm dialog = new ArticleForm(
                        StockAction.MiseAJour
                        , TakeOverTypesDS.First(x => x.Id == 2).Articles
                        , MarquesDS
                        , ModelesDS
                        , null
                        , articleToUpdate))
                    {
                        // Afficher la boîte de dialogue modale
                        if (dialog != null && dialog.ShowDialog() == DialogResult.OK)
                        {
                            (StockAction action, Article article, List<Article> articles, List<Marque> marques, List<Modele> modeles) = dialog.GetResult();
                            TakeOverTypesDS.First(x => x.Id == 2).Articles.Clear();
                            TakeOverTypesDS.First(x => x.Id == 2).Articles.AddRange(articles);
                            MarquesDS.Clear();
                            MarquesDS.AddRange(marques);
                            ModelesDS.Clear();
                            ModelesDS.AddRange(modeles);
                            StockUpdateDatabase(article, action);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void buttonStockAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (ArticleForm dialog = new ArticleForm(
                    StockAction.Ajout
                    , TakeOverTypesDS.First(x => x.Id == 2).Articles
                    , MarquesDS
                    , ModelesDS))
                {
                    // Afficher la boîte de dialogue modale
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        (StockAction action, Article articleToAdd, List<Article> articles, List<Marque> marques, List<Modele> modeles) = dialog.GetResult();
                        TakeOverTypesDS.First(x => x.Id == 2).Articles.Clear();
                        TakeOverTypesDS.First(x => x.Id == 2).Articles.AddRange(articles);
                        MarquesDS.Clear();
                        MarquesDS.AddRange(marques);
                        ModelesDS.Clear();
                        ModelesDS.AddRange(modeles);
                        StockUpdateDatabase(articleToAdd, StockAction.Ajout);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void StockUpdateDatabase(Article article, StockAction action, bool haveToRefresh = true)
        {
            string path = Paths.StockDSPath;

            try
            {
                if (!File.Exists(path))
                    throw new Exception($"Le fichier {path} n'existe pas.");

                List<string> data = new List<string>();

                // Ajout
                if (action == StockAction.Ajout)
                {
                    Tools.WriteLineTofile(
                        $"{article.Id};" +
                        $"{(article.MarqueId.HasValue ? article.MarqueId.Value.ToString() : string.Empty)};" +
                        $"{(article.ModeleId.HasValue ? article.ModeleId.Value.ToString() : string.Empty)};" +
                        $"{article.Produit};" +
                        $"{article.Price};" +
                        $"{article.Quantity};" +
                        $"{article.CodeReference}", path, true);
                    MessageBox.Show($"L'article {article.Produit} a été sauvegardée.");
                }
                // Suppression
                else if (action == StockAction.Suppression)
                {
                    data = Tools.GetDataFromFile(path);
                    int lineIndex = 0;
                    foreach (string line in data)
                    {
                        string[] fields = line.Split(';');
                        if (Int32.Parse(fields[0]) == article.Id)
                        {
                            data.RemoveAt(lineIndex);
                            TakeOverTypesDS.Find(x => x.Id == 2).Articles.Remove(article);
                            MessageBox.Show($"L'article \"{article.Produit}\" a été supprimé.");
                        }
                        lineIndex++;
                    }
                    Tools.RewriteDataToFile(data, path, false);
                }
                // Modification
                else if (action == StockAction.MiseAJour)
                {
                    data = Tools.GetDataFromFile(path);
                    for (int i = 0; i < data.Count; i++)
                    {
                        string[] fields = data[i].Split(';');
                        if (i > 0 && Int32.Parse(fields[0]) == article.Id)
                        {
                            data[i] =
                                $"{article.Id};" +
                                $"{(article.MarqueId.HasValue ? article.MarqueId.Value.ToString() : string.Empty)};" +
                                $"{(article.ModeleId.HasValue ? article.ModeleId.Value.ToString() : string.Empty)};" +
                                $"{article.Produit};" +
                                $"{article.Price};" +
                                $"{article.Quantity};" +
                                $"{article.CodeReference}";
                            TakeOverTypesDS.Find(x => x.Id == 2).Articles[i - 1] = article;
                            MessageBox.Show($"Les modifications de l'article {article.Produit} ont été sauvegardées.");
                        }
                    }
                    Tools.RewriteDataToFile(data, path, false);
                }

                if (haveToRefresh)
                {
                    InitializeLists();
                    TakeOver_Load();
                    dataGridViewAllCustomerRelation_Load(null);
                    dataGridViewStock_Load(null);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur : " + e.Message);
            }
        }
        #endregion
    }
}
