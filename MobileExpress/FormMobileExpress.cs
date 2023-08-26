using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Windows.Forms;
using ComboBox = System.Windows.Forms.ComboBox;
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
        #endregion

        public FormMobileExpress()
        {
            InitializeComponent();

            tabControlAll.SelectedIndexChanged += new EventHandler(TabControllAll_SelectedIndexChanged);
            dataGridViewCustomerRelationAll.CellClick += new DataGridViewCellEventHandler(DataGridViewCustomerRelationAll_CellClick);
            textBoxCustomerSearchAll.KeyUp += new KeyEventHandler(TextBoxCustomerSearchAll_KeyUp);
            textBoxStockSearch.KeyUp += new KeyEventHandler(TextBoxStockSearch_KeyUp);
            dataGridViewStock.CellContentClick += new DataGridViewCellEventHandler(DataGridViewStock_CellContentClick);

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
            // créer ou éditer Mode de paiement
            InitializePaymentModes();
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
            takeOverRepair_Load(null, null);
            takeOverUnlock_Load(null, null);
            takeOverAchat_Load(null, null);
            textBoxTakeOverTotalPrice_Load();
            textBoxTakeOverAccompte.Text = 0.ToString();
            textBoxTakeOverPaid.Text = 0.ToString();
            textBoxTakeOverResteDu.Text = 0.ToString();

            comboBoxTakeOverLastname.Text = string.Empty;
            textBoxTakeOverLastName_Load(null);
            // Relation cliente
            dataGridViewAllCustomerRelation_Load(null);
            // Stock
            dataGridViewStock_Load(null);
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
                            Article article = new Article(
                                Int32.Parse(temp[0]),
                                Tools.ToNullableInt(temp[1]),
                                Tools.ToNullableInt(temp[2]),
                                temp[3],
                                decimal.Parse(temp[4].Replace('.',',')),
                                Int32.Parse(temp[5]),
                                temp[6]);
                            items.Add(article);
                        }
                        rowIndex++;
                    }
                    items = items.OrderBy(a => a.Name).ToList();
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
        private void InitializePaymentModes()
        {
            string path = Paths.PaymentModesDSPath;

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
                            PaymentModesDS.Add(new PaymentMode(Int32.Parse(temp[0]), temp[1]));
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
                                int quantity = Int32.Parse(temp[10]);
                                decimal price = decimal.Parse(temp[11]);
                                int? monthsGarantie = Tools.ToNullableInt(temp[12]);
                                decimal? remise = Tools.ToNullableDecimal(temp[13]);
                                decimal? accompte = Tools.ToNullableDecimal(temp[14]);
                                decimal? resteDu = Tools.ToNullableDecimal(temp[15]);
                                decimal? paid = Tools.ToNullableDecimal(temp[16]);
                                decimal total = decimal.Parse(temp[17]);
                                int paymentModeId = Int32.Parse(temp[18]);
                                TakeOverState state = (TakeOverState)Int32.Parse(temp[19]);
                                int id = Int32.Parse(temp[20]);
                                bool verification = string.Compare("Oui", temp[21]) == 0;
                                MEDatasDS.Add(new MEData(
                                    takeOverId, date, invoiceId, customerId, marqueId, modeleId, imei, repairTypeId, unlockTypeId, articleId,
                                    quantity, price, monthsGarantie, remise, accompte, resteDu, paid, total, paymentModeId, state, id, verification));
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
        #endregion

        #region Initialisation des onglets
        private void TakeOver_Load()
        {
            textBoxTakeOverLastName_Load(null);
            textBoxTakeOverTotalPrice_Load();
            takeOverRepair_Load(null, null);
            takeOverUnlock_Load(null, null);
            takeOverAchat_Load(null, null);
        }
        private int GetPreferedWidthForComboBox(string text, ComboBox comboBox)
        {
            int toReturn = TextRenderer.MeasureText(text, comboBox.Font).Width;
            return toReturn;
        }
        private Dictionary<int, string> GetDictionnaryFromCustomers(List<Customer> customers)
        {
            Dictionary<int, string> items = new Dictionary<int, string>();
            items.Add(0, string.Empty);
            foreach (Customer customer in CustomersDS)
            {
                string customerName = Tools.GetStringFromCustomer(customer);

                items.Add(customer.Id, customerName);
            }
            return items;
        }
        private List<string> GetStringsFromCustomers(List<Customer> customers)
        {
            List<string> items = new List<string>() { string.Empty };
            foreach (Customer customer in CustomersDS)
            {
                string customerName = Tools.GetStringFromCustomer(customer);

                items.Add(customerName);
            }
            return items;
        }
        private int GetIdFromCustomerString(string customerString)
        {
            int id = 0;
            Dictionary<int, string> customers = GetDictionnaryFromCustomers(CustomersDS);
            foreach (var item in customers)
            {
                if (string.Compare(item.Value, customerString) == 0)
                {
                    id = item.Key;
                    return id;
                }
            }
            
            return id;
        }
        private void textBoxTakeOverLastName_Load(Customer customer)
        {
            try
            {
                List<string> strings = GetStringsFromCustomers(CustomersDS);

                comboBoxTakeOverLastname.Items.Clear();
                comboBoxTakeOverLastname.Items.AddRange(strings.ToArray());

                comboBoxTakeOverLastname.DropDownStyle = ComboBoxStyle.DropDown;
                comboBoxTakeOverLastname.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBoxTakeOverLastname.AutoCompleteSource = AutoCompleteSource.CustomSource;

                comboBoxTakeOverLastname.KeyUp += ComboBoxTakeOverLastname_KeyUp;

                comboBoxTakeOverLastname.DropDownWidth = GetPreferedWidthForComboBox(strings.OrderByDescending(x => x.Length).First(), comboBoxTakeOverLastname);

                if (customer != null)
                {
                    comboBoxTakeOverLastname.ResetText();
                    comboBoxTakeOverLastname.SelectedText = Tools.GetStringFromCustomer(customer);
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
            dataGridViewTakeOverRepair.DataSource = null;
            dataGridViewTakeOverRepair.Rows.Clear();
            dataGridViewTakeOverRepair.Columns.Clear();

            dataGridViewTakeOverRepair.AutoGenerateColumns = false; // add this line to disable auto-generation of columns

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
            {
                Name = "textBoxTakeOverRepairId",
                ValueType = typeof(int),
                Visible = false,
                ReadOnly = true,
            };
            DataGridViewTextBoxColumn repairMarqueColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Marque",
                Name = "textBoxTakeOverRepairMarque",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = false,
            };
            DataGridViewTextBoxColumn repairModeleColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Modèle",
                Name = "textBoxTakeOverRepairModele",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = false,
            };
            DataGridViewTextBoxColumn repairTypeColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Panne",
                Name = "textBoxTakeOverRepairType",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = false,
            };
            DataGridViewTextBoxColumn repairImeiColumn = new DataGridViewTextBoxColumn()
            {
                HeaderText = "IMEI",
                Name = "textBoxTakeOverRepairIMEI",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = false,
            };
            DataGridViewTextBoxColumn repairPriceColumn = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Prix",
                Name = "textBoxTakeOverRepairPrice",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(decimal),
                Visible = true,
                ReadOnly = false,
            };
            DataGridViewCheckBoxColumn garantieColumn = new DataGridViewCheckBoxColumn()
            {
                HeaderText = "Garantie",
                Name = "checkBoxTakeOverRepairGarantie",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Visible = true,
                ReadOnly = false,
            };
            DataGridViewButtonColumn buttonDeleteRow = new DataGridViewButtonColumn()
            {
                HeaderText = "Supprimer",
                Name = "buttonTakeOverRepairDeleteRow",
                Text = "Supprimer",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                Visible = true,
                ReadOnly = false,
            };

            dataGridViewTakeOverRepair.Columns.Add(idColumn);
            dataGridViewTakeOverRepair.Columns.Add(repairMarqueColumn);
            dataGridViewTakeOverRepair.Columns.Add(repairModeleColumn);
            dataGridViewTakeOverRepair.Columns.Add(repairTypeColumn);
            dataGridViewTakeOverRepair.Columns.Add(repairImeiColumn);
            dataGridViewTakeOverRepair.Columns.Add(repairPriceColumn);
            dataGridViewTakeOverRepair.Columns.Add(garantieColumn);
            dataGridViewTakeOverRepair.Columns.Add(buttonDeleteRow);
        }
        private void CleanDataGridViewUnlock()
        {
            dataGridViewTakeOverUnlock.DataSource = null;
            dataGridViewTakeOverUnlock.Rows.Clear();
            dataGridViewTakeOverUnlock.Columns.Clear();

            dataGridViewTakeOverUnlock.AutoGenerateColumns = false; // add this line to disable auto-generation of columns

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
            {
                Name = "textBoxTakeOverUnlockId",
                ValueType = typeof(int),
                Visible = false,
                ReadOnly = true,
            };
            DataGridViewTextBoxColumn unlockMarqueColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Marque",
                Name = "textBoxTakeOverUnlockMarque",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = false,
            };
            DataGridViewTextBoxColumn unlockModeleColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Modèle",
                Name = "textBoxTakeOverUnlockModele",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = false,
            };
            DataGridViewTextBoxColumn unlockTypeColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Déblocage",
                Name = "textBoxTakeOverUnlockType",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = false,
            };
            DataGridViewTextBoxColumn unlockImeiColumn = new DataGridViewTextBoxColumn()
            {
                HeaderText = "IMEI",
                Name = "textBoxTakeOverUnlockIMEI",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = false,
            };
            DataGridViewTextBoxColumn unlockPriceColumn = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Prix",
                Name = "textBoxTakeOverUnlockPrice",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(decimal),
                Visible = true,
                ReadOnly = false,
            };
            DataGridViewCheckBoxColumn garantieColumn = new DataGridViewCheckBoxColumn()
            {
                HeaderText = "Garantie",
                Name = "checkBoxTakeOverUnlockGarantie",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Visible = true,
                ReadOnly = false,
            };
            DataGridViewButtonColumn buttonDeleteRow = new DataGridViewButtonColumn()
            {
                HeaderText = "Supprimer",
                Name = "buttonTakeOverUnlockDeleteRow",
                Text = "Supprimer",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                Visible = true,
                ReadOnly = false,
            };

            dataGridViewTakeOverUnlock.Columns.Add(idColumn);
            dataGridViewTakeOverUnlock.Columns.Add(unlockMarqueColumn);
            dataGridViewTakeOverUnlock.Columns.Add(unlockModeleColumn);
            dataGridViewTakeOverUnlock.Columns.Add(unlockTypeColumn);
            dataGridViewTakeOverUnlock.Columns.Add(unlockImeiColumn);
            dataGridViewTakeOverUnlock.Columns.Add(unlockPriceColumn);
            dataGridViewTakeOverUnlock.Columns.Add(garantieColumn);
            dataGridViewTakeOverUnlock.Columns.Add(buttonDeleteRow);
        }
        private void CleanDataGridViewAchat()
        {
            dataGridViewTakeOverAchat.DataSource = null;
            dataGridViewTakeOverAchat.Rows.Clear();
            dataGridViewTakeOverAchat.Columns.Clear();

            dataGridViewTakeOverAchat.AutoGenerateColumns = false; // add this line to disable auto-generation of columns

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn
            {
                Name = "textBoxTakeOverAchatId",
                ValueType = typeof(int),
                Visible = false,
                ReadOnly = true,
            };
            dataGridViewTakeOverAchat.Columns.Add(idColumn);
            DataGridViewTextBoxColumn achatMarqueColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Marque",
                Name = "textBoxTakeOverAchatMarque",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = false,
            };
            dataGridViewTakeOverAchat.Columns.Add(achatMarqueColumn);
            DataGridViewTextBoxColumn achatModeleColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Modèle",
                Name = "textBoxTakeOverAchatModele",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = false,
            };
            dataGridViewTakeOverAchat.Columns.Add(achatModeleColumn);
            DataGridViewTextBoxColumn achatTypeColumn = new DataGridViewTextBoxColumn
            {
                HeaderText = "Article",
                Name = "textBoxTakeOverAchatType",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = false,
            };
            dataGridViewTakeOverAchat.Columns.Add(achatTypeColumn);
            DataGridViewTextBoxColumn achatQuantityColumn = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Quantité",
                Name = "textBoxTakeOverAchatQuantity",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(int),
                Visible = true,
                ReadOnly = false,
            };
            dataGridViewTakeOverAchat.Columns.Add(achatQuantityColumn);
            DataGridViewTextBoxColumn achatPriceColumn = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Prix",
                Name = "textBoxTakeOverAchatPrice",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(decimal),
                Visible = true,
                ReadOnly = false,
            };
            dataGridViewTakeOverAchat.Columns.Add(achatPriceColumn);
            DataGridViewCheckBoxColumn garantieColumn = new DataGridViewCheckBoxColumn()
            {
                HeaderText = "Garantie",
                Name = "checkBoxTakeOverAchatGarantie",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Visible = true,
                ReadOnly = false,
            };
            dataGridViewTakeOverAchat.Columns.Add(garantieColumn);
            DataGridViewCheckBoxColumn remiseColumn = new DataGridViewCheckBoxColumn()
            {
                HeaderText = "Remise",
                Name = "checkBoxTakeOverAchatRemise",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Visible = true,
                ReadOnly = false,
            };
            dataGridViewTakeOverAchat.Columns.Add(remiseColumn);
            DataGridViewButtonColumn buttonDeleteRow = new DataGridViewButtonColumn()
            {
                HeaderText = "Supprimer",
                Name = "buttonTakeOverAchatDeleteRow",
                Text = "Supprimer",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                Visible = true,
                ReadOnly = false,
            };
            dataGridViewTakeOverAchat.Columns.Add(buttonDeleteRow);
        }
        private void takeOverRepair_Load(List<MEData> mEDatas, int? rowIndex)
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
                        if (string.IsNullOrWhiteSpace(comboBoxTakeOverLastname.Text) ||
                            string.IsNullOrWhiteSpace(textBoxTakeOverAccompte.Text) ||
                            string.IsNullOrWhiteSpace(textBoxTakeOverResteDu.Text))
                        {
                            comboBoxTakeOverLastname.SelectedText = CustomersDS
                                .Where(x => x.Id == mEData.CustomerId)
                                .Select(x => Tools.GetStringFromCustomer(x))
                                .First();
                        }

                        DataGridViewTextBoxCell idComboBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverRepair.Rows[index].Cells["textBoxTakeOverRepairId"];
                        idComboBoxCell.Value = mEData.Id;
                        if (dataGridViewTakeOverRepair.EditingControl is DataGridViewTextBoxEditingControl idTextBoxControl)
                        {
                            idTextBoxControl.Text = mEData.Id.ToString();
                        }

                        int marqueId = (mEData.MarqueId.Value as int?).Value;
                        Marque marque = MarquesDS.Find(x => x.Id == marqueId);
                        DataGridViewTextBoxCell marqueComboBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverRepair.Rows[index].Cells["textBoxTakeOverRepairMarque"];
                        marqueComboBoxCell.Value = marque.Name;
                        if (dataGridViewTakeOverRepair.EditingControl is DataGridViewTextBoxEditingControl marqueTextBoxControl)
                        {
                            marqueTextBoxControl.Text = marque.Name;
                        }

                        int modeleId = (mEData.ModeleId.Value as int?).Value;
                        Modele modele = ModelesDS.Find(x => x.Id == modeleId);
                        DataGridViewTextBoxCell modeleComboBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverRepair.Rows[index].Cells["textBoxTakeOverRepairModele"];
                        modeleComboBoxCell.Value = modele.Name;
                        if (dataGridViewTakeOverRepair.EditingControl is DataGridViewTextBoxEditingControl modeleTextBoxControl)
                        {
                            modeleTextBoxControl.Text = modele.Name;
                        }

                        int type = (mEData.RepairTypeId.Value as int?).Value;
                        RepairType repairType = TakeOverTypesDS.Find(x => x.Id == 0).RepairTypes.Find(x => x.Id == type);
                        DataGridViewTextBoxCell typeTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverRepair.Rows[index].Cells["textBoxTakeOverRepairType"];
                        typeTextBoxCell.Value = repairType.Name;
                        if (dataGridViewTakeOverRepair.EditingControl is DataGridViewTextBoxEditingControl typeTextBoxControl)
                        {
                            typeTextBoxControl.Text = repairType.Name;
                        }

                        DataGridViewTextBoxCell imeiTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverRepair.Rows[index].Cells["textBoxTakeOverRepairIMEI"];
                        imeiTextBoxCell.Value = mEData.IMEI;
                        if (dataGridViewTakeOverRepair.EditingControl is DataGridViewTextBoxEditingControl imeiTextBoxControl)
                        {
                            imeiTextBoxControl.Text = mEData.IMEI;
                        }

                        DataGridViewTextBoxCell priceTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverRepair.Rows[index].Cells["textBoxTakeOverRepairPrice"];
                        priceTextBoxCell.Value = mEData.Price;
                        if (dataGridViewTakeOverRepair.EditingControl is DataGridViewTextBoxEditingControl priceTextBoxControl)
                        {
                            priceTextBoxControl.Text = mEData.Price.ToString();
                        }

                        DataGridViewCheckBoxCell garantieCheckBoxCell = (DataGridViewCheckBoxCell)dataGridViewTakeOverRepair.Rows[index].Cells["checkBoxTakeOverRepairGarantie"];
                        garantieCheckBoxCell.Value = mEData.Garantie.HasValue;

                        textBoxTakeOverAccompte.Text = mEData.Accompte.ToString();
                        textBoxTakeOverResteDu.Text = mEData.ResteDu.ToString();
                        textBoxTakeOverPaid.Text = mEData.Paid.ToString();
                        textBoxTakeOverTotalPrice.Text = mEData.Total.ToString();

                        index++;
                    }
                }
                else if (rowIndex.HasValue)
                {

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
        private void takeOverUnlock_Load(List<MEData> mEDatas, int? rowIndex)
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
                        if (string.IsNullOrWhiteSpace(comboBoxTakeOverLastname.Text) ||
                            string.IsNullOrWhiteSpace(textBoxTakeOverAccompte.Text) ||
                            string.IsNullOrWhiteSpace(textBoxTakeOverResteDu.Text))
                        {
                            comboBoxTakeOverLastname.SelectedText = CustomersDS
                                .Where(x => x.Id == mEData.CustomerId)
                                .Select(x => $"{x.FirstName} {x.LastName} - {x.PhoneNumber} - {x.EmailAddress}")
                                .First();
                        }

                        dataGridViewTakeOverUnlock.Rows.Add();

                        DataGridViewTextBoxCell idComboBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverUnlock.Rows[index].Cells["textBoxTakeOverUnlockId"];
                        idComboBoxCell.Value = mEData.Id;
                        if (dataGridViewTakeOverUnlock.EditingControl is DataGridViewTextBoxEditingControl idTextBoxControl)
                        {
                            idTextBoxControl.Text = mEData.Id.ToString();
                        }

                        int marqueId = (mEData.MarqueId.Value as int?).Value;
                        Marque marque = MarquesDS.Find(x => x.Id == marqueId);
                        DataGridViewTextBoxCell marqueComboBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverUnlock.Rows[index].Cells["textBoxTakeOverUnlockMarque"];
                        marqueComboBoxCell.Value = marque.Name;
                        if (dataGridViewTakeOverUnlock.EditingControl is DataGridViewTextBoxEditingControl marqueTextBoxControl)
                        {
                            marqueTextBoxControl.Text = marque.Name;
                        }

                        int modeleId = (mEData.ModeleId.Value as int?).Value;
                        Modele modele = ModelesDS.Find(x => x.Id == modeleId);
                        DataGridViewTextBoxCell modeleComboBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverUnlock.Rows[index].Cells["textBoxTakeOverUnlockModele"];
                        modeleComboBoxCell.Value = modele.Name;
                        if (dataGridViewTakeOverUnlock.EditingControl is DataGridViewTextBoxEditingControl modeleTextBoxControl)
                        {
                            modeleTextBoxControl.Text = modele.Name;
                        }

                        int type = (mEData.UnlockTypeId.Value as int?).Value;
                        UnlockType unlockType = TakeOverTypesDS.Find(x => x.Id == 1).UnlockTypes.Find(x => x.Id == type);
                        DataGridViewTextBoxCell typeTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverUnlock.Rows[index].Cells["textBoxTakeOverUnlockType"];
                        typeTextBoxCell.Value = unlockType.Name;
                        if (dataGridViewTakeOverUnlock.EditingControl is DataGridViewTextBoxEditingControl typeTextBoxControl)
                        {
                            typeTextBoxControl.Text = unlockType.Name;
                        }

                        DataGridViewTextBoxCell imeiTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverUnlock.Rows[index].Cells["textBoxTakeOverUnlockIMEI"];
                        imeiTextBoxCell.Value = mEData.IMEI;
                        if (dataGridViewTakeOverUnlock.EditingControl is DataGridViewTextBoxEditingControl imeiTextBoxControl)
                        {
                            imeiTextBoxControl.Text = mEData.IMEI;
                        }

                        DataGridViewTextBoxCell priceTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverUnlock.Rows[index].Cells["textBoxTakeOverUnlockPrice"];
                        priceTextBoxCell.Value = mEData.Price;
                        if (dataGridViewTakeOverUnlock.EditingControl is DataGridViewTextBoxEditingControl priceTextBoxControl)
                        {
                            priceTextBoxControl.Text = mEData.Price.ToString();
                        }

                        DataGridViewCheckBoxCell garantieCheckBoxCell = (DataGridViewCheckBoxCell)dataGridViewTakeOverUnlock.Rows[index].Cells["checkBoxTakeOverUnlockGarantie"];
                        garantieCheckBoxCell.Value = mEData.Garantie.HasValue;

                        textBoxTakeOverAccompte.Text = mEData.Accompte.ToString();
                        textBoxTakeOverResteDu.Text = mEData.ResteDu.ToString();
                        textBoxTakeOverPaid.Text = mEData.Paid.ToString();
                        textBoxTakeOverTotalPrice.Text = mEData.Total.ToString();

                        index++;
                    }
                }
                else if (rowIndex.HasValue)
                {
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
        private void takeOverAchat_Load(List<MEData> mEDatas, int? rowIndex)
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
                        if (string.IsNullOrWhiteSpace(comboBoxTakeOverLastname.Text))
                        {
                            comboBoxTakeOverLastname.SelectedText = CustomersDS
                                .Where(x => x.Id == mEData.CustomerId)
                                .Select(x => $"{x.FirstName} {x.LastName} - {x.PhoneNumber} - {x.EmailAddress}")
                                .First();
                        }

                        dataGridViewTakeOverAchat.Rows.Add();

                        DataGridViewTextBoxCell idComboBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverAchat.Rows[index].Cells["textBoxTakeOverAchatId"];
                        idComboBoxCell.Value = mEData.Id;
                        if (dataGridViewTakeOverAchat.EditingControl is DataGridViewTextBoxEditingControl idTextBoxControl)
                        {
                            idTextBoxControl.Text = mEData.Id.ToString();
                        }

                        int marqueId = (mEData.MarqueId.Value as int?).Value;
                        Marque marque = MarquesDS.Find(x => x.Id == marqueId);
                        DataGridViewTextBoxCell marqueComboBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverAchat.Rows[index].Cells["textBoxTakeOverAchatMarque"];
                        marqueComboBoxCell.Value = marque.Name;
                        if (dataGridViewTakeOverAchat.EditingControl is DataGridViewTextBoxEditingControl marqueTextBoxControl)
                        {
                            marqueTextBoxControl.Text = marque.Name;
                        }

                        int modeleId = (mEData.ModeleId.Value as int?).Value;
                        Modele modele = ModelesDS.Find(x => x.Id == modeleId);
                        DataGridViewTextBoxCell modeleComboBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverAchat.Rows[index].Cells["textBoxTakeOverAchatModele"];
                        modeleComboBoxCell.Value = modele.Name;
                        if (dataGridViewTakeOverAchat.EditingControl is DataGridViewTextBoxEditingControl modeleTextBoxControl)
                        {
                            modeleTextBoxControl.Text = modele.Name;
                        }

                        int type = (mEData.ArticleId.Value as int?).Value;
                        Article article = TakeOverTypesDS.Find(x => x.Id == 2).Articles.Find(x => x.Id == type);
                        DataGridViewTextBoxCell typeTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverAchat.Rows[index].Cells["textBoxTakeOverAchatType"];
                        typeTextBoxCell.Value = article.Name;
                        if (dataGridViewTakeOverAchat.EditingControl is DataGridViewTextBoxEditingControl typeTextBoxControl)
                        {
                            typeTextBoxControl.Text = article.Name;
                        }

                        DataGridViewTextBoxCell quantityTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverAchat.Rows[index].Cells["textBoxTakeOverAchatQuantity"];
                        quantityTextBoxCell.Value = mEData.Quantity;
                        if (dataGridViewTakeOverAchat.EditingControl is DataGridViewTextBoxEditingControl quantityTextBoxControl)
                        {
                            quantityTextBoxControl.Text = mEData.Quantity.ToString();
                        }

                        DataGridViewTextBoxCell priceTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverAchat.Rows[index].Cells["textBoxTakeOverAchatPrice"];
                        priceTextBoxCell.Value = mEData.Price;
                        if (dataGridViewTakeOverAchat.EditingControl is DataGridViewTextBoxEditingControl priceTextBoxControl)
                        {
                            priceTextBoxControl.Text = mEData.Price.ToString();
                        }

                        DataGridViewCheckBoxCell garantieCheckBoxCell = (DataGridViewCheckBoxCell)dataGridViewTakeOverAchat.Rows[index].Cells["checkBoxTakeOverAchatGarantie"];
                        garantieCheckBoxCell.Value = mEData.Garantie.HasValue;

                        DataGridViewCheckBoxCell remiseCheckBoxCell = (DataGridViewCheckBoxCell)dataGridViewTakeOverAchat.Rows[index].Cells["checkBoxTakeOverAchatRemise"];
                        remiseCheckBoxCell.Value = mEData.Remise.HasValue;

                        textBoxTakeOverAccompte.Text = mEData.Accompte.ToString();
                        textBoxTakeOverResteDu.Text = mEData.ResteDu.ToString();
                        textBoxTakeOverPaid.Text = mEData.Paid.ToString();
                        textBoxTakeOverTotalPrice.Text = mEData.Total.ToString();

                        index++;
                    }
                }
                else if (rowIndex.HasValue)
                {

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
                    comboBoxCustomerRelationSexe.Items.Add(Tools.GetEnumDescription(value));
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
                            TakeOverTypesDS.First(t => t.Id == 2).Articles.First(t => t.Id == x.ArticleId).Name;

                        string restedu = x.ResteDu == 0 ? "Payé" : x.ResteDu.ToString();

                        (bool cb, bool espece, bool virement) = Tools.GetPaymentModeFromInt(x.PaymentMode);
                        string paymentMode =
                            cb && espece && virement ? "CB / ESP / VIR" :
                            cb && espece && !virement ? "CB / ESP" :
                            cb && !espece && virement ? "CB / VIR" :
                            cb && !espece && !virement ? "CB" :
                            !cb && espece && virement ? "ESP / VIR" :
                            !cb && espece && !virement ? "ESP" :
                            !cb && !espece && virement ? "VIR" :
                            string.Empty;

                        string marque = MarquesDS.FirstOrDefault(m => m.Id == x.MarqueId)?.Name ?? string.Empty;
                        string modele = ModelesDS.FirstOrDefault(m => m.Id == x.ModeleId)?.Name ?? string.Empty;

                        return new HistoriqueOneView()
                        {
                            Id = x.Id,
                            PriseEnCharge = x.TakeOverId,
                            Date = x.Date.ToString("dd/MM/yyyy HHmmss"),
                            Facture = x.InvoiceId.HasValue ? "Facturé" : "Non facturé",
                            FactureNumero = x.InvoiceId.HasValue ? x.InvoiceId.Value : 0,
                            TypeItem = typeItem,
                            Item = item,
                            Marque = marque,
                            Modele = modele,
                            IMEI = x.IMEI,
                            Quantity = x.Quantity,
                            Price = x.Price.Value.ToString(),
                            Garantie = x.Garantie.HasValue ? x.Garantie.Value.ToString() + " mois" : "Non",
                            Remise = x.Remise.HasValue ? x.Remise.Value.ToString() : string.Empty,
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
                int invoiceId = MEDatasDS.OrderByDescending(x => x.InvoiceId ?? 0)?.FirstOrDefault()?.InvoiceId ?? 0;
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

                            List<string> items = new List<string>() { "Numéro de prise en charge;Date;Numéro de facture;Numéro du client;Numéro de la marque;Numéro du modèle;IMEI;Numéro du type de réparation;Numéro du type de déblocage;Numéro de l'article;Quantité;Prix;Garantie;Remise;Accompte;Reste dû;Payé;Total;Numéro de mode de paiement;Etat;Id;Vérification" };
                            items.AddRange(MEDatasDS.Select(x => $"{x.TakeOverId};{x.Date};{x.InvoiceId};{x.CustomerId};{x.MarqueId};{x.ModeleId};{x.IMEI};{x.RepairTypeId};{x.UnlockTypeId};{x.ArticleId};{x.Quantity};{x.Price};{x.Garantie};{x.Remise};{x.Accompte};{x.ResteDu};{x.Paid};{x.Total};{x.PaymentMode};{(int)x.State};{x.Id};{(x.Verification ? "Oui" : "Non")}"));
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

                DataGridViewTextBoxColumn textBoxId = new DataGridViewTextBoxColumn()
                {
                    Name = "textBoxStockId",
                    Visible = false,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewStock.Columns.Add(textBoxId);
                DataGridViewTextBoxColumn textBoxMarque = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Marque",
                    Name = "textBoxStockMarque",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewStock.Columns.Add(textBoxMarque);
                DataGridViewTextBoxColumn textBoxModele = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Modèle",
                    Name = "textBoxStockModele",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewStock.Columns.Add(textBoxModele);
                DataGridViewTextBoxColumn textBoxName = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Nom",
                    Name = "textBoxStockName",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewStock.Columns.Add(textBoxName);
                DataGridViewTextBoxColumn textBoxPrice = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Prix",
                    Name = "textBoxStockPrice",
                    ValueType = typeof(decimal),
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewStock.Columns.Add(textBoxPrice);
                DataGridViewTextBoxColumn textBoxQuantity = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Quantité",
                    Name = "textBoxStockQuantity",
                    ValueType= typeof(int),
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewStock.Columns.Add(textBoxQuantity);
                DataGridViewTextBoxColumn textBoxDescription = new DataGridViewTextBoxColumn()
                {
                    HeaderText = "Description",
                    Name = "textBoxStockDescription",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                };
                dataGridViewStock.Columns.Add(textBoxDescription);
                DataGridViewButtonColumn buttonSaveRow = new DataGridViewButtonColumn()
                {
                    HeaderText = string.Empty,
                    Name = "textBoxStockSaveRow",
                    Text = "Sauvegarder",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    UseColumnTextForButtonValue = true,
                    Tag = (Action<Article>)DataGridViewStockSaveRow_CellContentClick,
                };
                dataGridViewStock.Columns.Add(buttonSaveRow);
                DataGridViewButtonColumn buttonDeleteRow = new DataGridViewButtonColumn()
                {
                    HeaderText = string.Empty,
                    Name = "textBoxStockDeleteRow",
                    Text = "Supprimer",
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    UseColumnTextForButtonValue = true,
                    Tag = (Action<Article>)DataGridViewStockDeleteRow_CellContentClick,
                };
                dataGridViewStock.Columns.Add(buttonDeleteRow);

                dataGridViewStock.AllowUserToAddRows = false;

                List<Article> items = (articles?.Any() ?? false) ? articles : TakeOverTypesDS.Find(x => x.Id == 2).Articles;

                int index = 0;
                foreach (Article article in items)
                {
                    dataGridViewStock.Rows.Add();

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

                    DataGridViewTextBoxCell nameTextBoxCell = (DataGridViewTextBoxCell)dataGridViewStock.Rows[index].Cells["textBoxStockName"];
                    nameTextBoxCell.Value = article.Name;
                    if (dataGridViewStock.EditingControl is DataGridViewTextBoxEditingControl nameTextBoxControl)
                    {
                        nameTextBoxControl.Text = article.Name;
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

                    DataGridViewTextBoxCell descriptionTextBoxCell = (DataGridViewTextBoxCell)dataGridViewStock.Rows[index].Cells["textBoxStockDescription"];
                    descriptionTextBoxCell.Value = article.Description;
                    if (dataGridViewStock.EditingControl is DataGridViewTextBoxEditingControl descriptionTextBoxControl)
                    {
                        descriptionTextBoxControl.Text = article.Description;
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
            if (tabControlAll.SelectedTab == tabPageTakeOver)
            {
                InitializeAll();
            }
            else if (tabControlAll.SelectedTab == customerRelationTabPage)
            {
                InitializeAll();
            }
            else if (tabControlAll.SelectedTab == stockTabPage)
            {
                InitializeAll();
            }
        }
        // TakeOver
        private void buttonAddCustomer_Click(object sender, EventArgs e)
        {
            try
            {
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
                        items.AddRange(CustomersDS.Select(x => $"{x.Id};{x.LastName};{x.FirstName};{x.PhoneNumber};{x.EmailAddress};{Tools.GetEnumDescription<Sexe>(x.Sexe)}"));
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
                if (!CustomersDS.Any(x => string.Compare(Tools.GetStringFromCustomer(x), comboBoxTakeOverLastname.Text) == 0))
                {
                    MessageBox.Show("Le client n'est pas connu du système. Veuillez appuyer sur le bouton \"Ajouter un client\"", "Alerte", MessageBoxButtons.OK);
                    return;
                }

                Customer customerToUpdate = CustomersDS.First(x => string.Compare(Tools.GetStringFromCustomer(x), comboBoxTakeOverLastname.Text) == 0);
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
                        items.AddRange(CustomersDS.Select(x => $"{x.Id};{x.LastName};{x.FirstName};{x.PhoneNumber};{x.EmailAddress};{Tools.GetEnumDescription<Sexe>(x.Sexe)}"));
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
                string text = comboBoxTakeOverLastname.Text;

                List<string> strings = GetStringsFromCustomers(CustomersDS).Where(x => x.ToLowerInvariant().Contains(text.ToLowerInvariant())).ToList();

                comboBoxTakeOverLastname.DropDownStyle = ComboBoxStyle.DropDown;
                comboBoxTakeOverLastname.Items.Clear();
                comboBoxTakeOverLastname.Items.AddRange(strings.ToArray());

                comboBoxTakeOverLastname.AutoCompleteMode = AutoCompleteMode.Suggest;
                comboBoxTakeOverLastname.AutoCompleteSource = AutoCompleteSource.CustomSource;

                comboBoxTakeOverLastname.SelectionLength = 0;
                comboBoxTakeOverLastname.SelectionStart = text.Length;

                comboBoxTakeOverLastname.DropDownWidth = GetPreferedWidthForComboBox(strings.OrderByDescending(x => x.Length).First(), comboBoxTakeOverLastname);
                comboBoxTakeOverLastname.DroppedDown = true;
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
        private void buttonTakeOverSearch_Click_1(object sender, EventArgs e)
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
                        RepairType produitRepair = mEData.RepairTypeId.HasValue ? TakeOverTypesDS.First(x => x.Id == 0).RepairTypes.First(x => x.Id == (mEData.RepairTypeId.Value as int?).Value) : null;
                        UnlockType produitUnlock = mEData.UnlockTypeId.HasValue ? TakeOverTypesDS.First(x => x.Id == 1).UnlockTypes.First(x => x.Id == (mEData.UnlockTypeId.Value as int?).Value) : null;
                        Article produitArticle = mEData.ArticleId.HasValue ? TakeOverTypesDS.First(x => x.Id == 2).Articles.First(x => x.Id == (mEData.ArticleId.Value as int?).Value) : null;

                        int marqueId = receipts[i].MarqueId.HasValue ? receipts[i].MarqueId.Value : 0;
                        Marque marque = MarquesDS.FirstOrDefault(x => x.Id == marqueId);

                        int modeleId = receipts[i].ModeleId.HasValue ? receipts[i].ModeleId.Value : 0;
                        Modele modele = ModelesDS.FirstOrDefault(x => x.Id == modeleId);

                        string marqueText = marque?.Name;
                        string modeleText = modele?.Name;
                        string productName = produitRepair?.Name ?? produitUnlock?.Name ?? produitArticle.Name;
                        if (!string.IsNullOrWhiteSpace(marqueText))
                        {
                            if (!string.IsNullOrWhiteSpace(modeleText))
                            {
                                productName = marqueText + " " + modeleText + " - " + productName;
                            }
                            else
                            {
                                productName = marqueText + " - " + productName;
                            }
                        }

                        if (mEData.Garantie.HasValue)
                        {
                            // ajouter la ligne de garantie
                            GarantiesTemp.Add(new Garantie(
                                id: mEData.Id,
                                tabName: tabName,
                                productName: productName,
                                months: mEData.Garantie.Value));
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

                    (bool cb, bool espece, bool virement) = Tools.GetPaymentModeFromInt(receipts.First().PaymentMode);
                    checkBoxCb.Checked = cb;
                    checkBoxEspece.Checked = espece;
                    checkBoxVirement.Checked = virement;

                    // initialiser les onglets
                    takeOverRepair_Load(receipts.Where(x => x.RepairTypeId.HasValue).ToList(), null);
                    takeOverUnlock_Load(receipts.Where(x => x.UnlockTypeId.HasValue).ToList(), null);
                    takeOverAchat_Load(receipts.Where(x => x.ArticleId.HasValue).ToList(), null);

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
                DateTime date = DateTime.Parse(dateTimePickerTakeOverDate.Text);
                Customer takeOverCustomer = CustomersDS.First(x => string.Compare(Tools.GetStringFromCustomer(x), comboBoxTakeOverLastname.Text) == 0);
                if (takeOverCustomer == null)
                {
                    MessageBox.Show("Vous devez définir un client.", "Alerte", MessageBoxButtons.OK);
                    return;
                }
                decimal accompte = Tools.ToNullableDecimal(textBoxTakeOverAccompte.Text) ?? 0;
                decimal resteDu = Tools.ToNullableDecimal(textBoxTakeOverResteDu.Text) ?? 0;
                decimal paid = Tools.ToNullableDecimal(textBoxTakeOverPaid.Text) ?? 0;
                decimal total = Tools.ToNullableDecimal(textBoxTakeOverTotalPrice.Text) ?? 0;
                int paymentMode = Tools.GetIntFromPaymentMode(checkBoxCb.Checked, checkBoxEspece.Checked, checkBoxVirement.Checked);

                List<MEData> mEDatas = new List<MEData>();
                Dictionary<int, string> repairTypeNames = new Dictionary<int, string>();
                Dictionary<int, string> unlockTypeNames = new Dictionary<int, string>();
                Dictionary<int, string> articles = new Dictionary<int, string>();
                Dictionary<int, string> marques = new Dictionary<int, string>();
                Dictionary<int, string> modeles = new Dictionary<int, string>();
                foreach (DataGridViewRow row in dataGridViewTakeOverRepair.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        int id = (row.Cells["textBoxTakeOverRepairId"].Value as int?).Value;
                        string marqueText = row.Cells["textBoxTakeOverRepairMarque"].Value as string;
                        string modeleText = row.Cells["textBoxTakeOverRepairModele"].Value as string;
                        string typeText = row.Cells["textBoxTakeOverRepairType"].Value as string;
                        string imeiText = row.Cells["textBoxTakeOverRepairIMEI"].Value as string;
                        decimal? price = row.Cells["textBoxTakeOverRepairPrice"].Value as decimal?;
                        bool? isGarantie = (row.Cells["checkBoxTakeOverRepairGarantie"].Value as bool?);

                        Marque marque = MarquesDS.First(x => string.Compare(x.Name, marqueText) == 0);
                        Modele modele = ModelesDS.First(x => string.Compare(x.Name, modeleText) == 0);
                        RepairType repairType = TakeOverTypesDS.First(x => x.Id == 0).RepairTypes.First(x => string.Compare(x.Name, typeText) == 0);
                        if (!repairTypeNames.Any(x => x.Key == repairType.Id))
                        {
                            repairTypeNames.Add(repairType.Id, repairType.Name);
                        }
                        if (!marques.Any(x => x.Key == marque.Id))
                        {
                            marques.Add(marque.Id, marque.Name);
                        }
                        if (!modeles.Any(x => x.Key == modele.Id))
                        {
                            modeles.Add(modele.Id, modele.Name);
                        }

                        Garantie garantie = isGarantie.HasValue && isGarantie.Value ? GarantiesTemp.FirstOrDefault(x => x.Id == id) : null;

                        MEData newTakeOver = new MEData(
                            takeOverId, date, null, takeOverCustomer.Id, marque.Id,
                            modele.Id, imeiText, repairType.Id, null, null,
                            1, price, garantie == null ? null : garantie.Months, null, accompte, resteDu,
                            paid, total, paymentMode, TakeOverState.InProgress, id, false);
                        mEDatas.Add(newTakeOver);
                    }
                }
                foreach (DataGridViewRow row in dataGridViewTakeOverUnlock.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        int id = (row.Cells["textBoxTakeOverUnlockId"].Value as int?).Value;
                        string marqueText = row.Cells["textBoxTakeOverUnlockMarque"].Value as string;
                        string modeleText = row.Cells["textBoxTakeOverUnlockModele"].Value as string;
                        string typeText = row.Cells["textBoxTakeOverUnlockType"].Value as string;
                        string imeiText = row.Cells["textBoxTakeOverUnlockIMEI"].Value as string;
                        decimal? price = row.Cells["textBoxTakeOverUnlockPrice"].Value as decimal?;
                        bool? isGarantie = (row.Cells["checkBoxTakeOverUnlockGarantie"].Value as bool?);

                        Marque marque = MarquesDS.First(x => string.Compare(x.Name, marqueText) == 0);
                        Modele modele = ModelesDS.First(x => string.Compare(x.Name, modeleText) == 0);
                        UnlockType unlockType = TakeOverTypesDS.First(x => x.Id == 1).UnlockTypes.First(x => string.Compare(x.Name, typeText) == 0);
                        if (!unlockTypeNames.Any(x => x.Key == unlockType.Id))
                        {
                            unlockTypeNames.Add(unlockType.Id, unlockType.Name);
                        }
                        if (!marques.Any(x => x.Key == marque.Id))
                        {
                            marques.Add(marque.Id, marque.Name);
                        }
                        if (!modeles.Any(x => x.Key == modele.Id))
                        {
                            modeles.Add(modele.Id, modele.Name);
                        }

                        Garantie garantie = isGarantie.HasValue && isGarantie.Value ? GarantiesTemp.FirstOrDefault(x => x.Id == id) : null;

                        MEData newTakeOver = new MEData(
                            takeOverId, date, null, takeOverCustomer.Id, marque.Id,
                            modele.Id, imeiText, null, unlockType.Id, null,
                            1, price, garantie == null ? null : garantie.Months, null, accompte, resteDu,
                            paid, total, paymentMode, TakeOverState.InProgress, id, false);
                        mEDatas.Add(newTakeOver);
                    }
                }
                foreach (DataGridViewRow row in dataGridViewTakeOverAchat.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        int id = (row.Cells["textBoxTakeOverAchatId"].Value as int?).Value;
                        string marqueText = row.Cells["textBoxTakeOverAchatMarque"].Value as string;
                        string modeleText = row.Cells["textBoxTakeOverAchatModele"].Value as string;
                        string typeText = row.Cells["textBoxTakeOverAchatType"].Value as string;
                        int quantity = (row.Cells["textBoxTakeOverAchatQuantity"].Value as int?).Value;
                        decimal? price = row.Cells["textBoxTakeOverAchatPrice"].Value as decimal?;
                        bool? isGarantie = (row.Cells["checkBoxTakeOverAchatGarantie"].Value as bool?);
                        bool? isRemise = (row.Cells["checkBoxTakeOverAchatRemise"].Value as bool?);

                        Marque marque = MarquesDS.First(x => string.Compare(x.Name, marqueText) == 0);
                        Modele modele = ModelesDS.First(x => string.Compare(x.Name, modeleText) == 0);
                        Article article = TakeOverTypesDS.First(x => x.Id == 2).Articles.First(x => string.Compare(x.Name, typeText) == 0);
                        if (!articles.Any(x => x.Key == article.Id))
                        {
                            articles.Add(article.Id, article.Name);
                        }
                        if (!marques.Any(x => x.Key == marque.Id))
                        {
                            marques.Add(marque.Id, marque.Name);
                        }
                        if (!modeles.Any(x => x.Key == modele.Id))
                        {
                            modeles.Add(modele.Id, modele.Name);
                        }

                        Garantie garantie = isGarantie.HasValue && isGarantie.Value ? GarantiesTemp.FirstOrDefault(x => x.Id == id) : null;
                        Remise remise = isRemise.HasValue && isRemise.Value ? RemisesTemp.FirstOrDefault(x => x.Id == id) : null;

                        MEData newTakeOver = new MEData(
                            takeOverId, date, null, takeOverCustomer.Id, marque.Id,
                            modele.Id, null, null, null, article.Id,
                            quantity, price, garantie?.Months, remise?.Prix, accompte, resteDu,
                            paid, total, paymentMode, TakeOverState.InProgress, id, false);
                        mEDatas.Add(newTakeOver);
                    }
                }

                if (mEDatas?.Any() ?? false)
                {
                    // propager les nouvelles données
                    for (int i = 0; i < mEDatas.Count; i++)
                    {
                        if (MEDatasDS.Any(x => x.Id == mEDatas[i].Id))
                        {
                            for (int j = 0; j < MEDatasDS.Count; j++)
                            {
                                if (MEDatasDS[j].Id == mEDatas[i].Id)
                                {
                                    int? invoiceIdMEData = MEDatasDS[j].InvoiceId;
                                    int? invoiceIdNewMEData = mEDatas[i].InvoiceId;
                                    MEDatasDS[j] = mEDatas[i];
                                    if (invoiceIdMEData != invoiceIdNewMEData)
                                    {
                                        MEDatasDS[j].InvoiceId = invoiceIdMEData.HasValue && invoiceIdNewMEData.HasValue ? invoiceIdNewMEData : invoiceIdMEData;
                                    }
                                }
                            }
                        }
                        else
                        {
                            MEDatasDS.Add(mEDatas[i]);
                        }
                    }

                    string path = Paths.ReceiptDSPath;

                    List<string> items = new List<string>() { "Numéro de prise en charge;Date;Numéro de facture;Numéro du client;Numéro de la marque;Numéro du modèle;IMEI;Numéro du type de réparation;Numéro du type de déblocage;Numéro de l'article;Quantité;Prix;Garantie;Remise;Accompte;Reste dû;Payé;Total;Numéro de mode de paiement;Etat;Id;Vérification" };
                    items.AddRange(MEDatasDS.Select(x => $"{x.TakeOverId};{x.Date};{x.InvoiceId};{x.CustomerId};{x.MarqueId};{x.ModeleId};{x.IMEI};{x.RepairTypeId};{x.UnlockTypeId};{x.ArticleId};{x.Quantity};{x.Price};{x.Garantie};{x.Remise};{x.Accompte};{x.ResteDu};{x.Paid};{x.Total};{paymentMode};{(int)x.State};{x.Id};{(x.Verification ? "Oui" : "Non")}"));
                    Tools.RewriteDataToFile(items, path, false);

                    InitializeReceipts();
                    string customerName =
                        takeOverCustomer.Sexe == Sexe.Unknown ? takeOverCustomer.LastName + " " + takeOverCustomer.FirstName :
                        takeOverCustomer.Sexe == Sexe.Femme ? "Mme. " + takeOverCustomer.LastName + " " + takeOverCustomer.FirstName :
                        "M. " + takeOverCustomer.LastName + " " + takeOverCustomer.FirstName;
                    // générer pdf
                    string takeoverfilePath = Tools.GeneratePdfFromHtml(
                        type: 0,
                        logo: Paths.LogoPath,
                        customerName: customerName,
                        customerPhone: takeOverCustomer.PhoneNumber,
                        customerEmail: takeOverCustomer.EmailAddress,
                        takeOverDate: date.ToString("dddd dd MMMM yyyy"),
                        takeOverNumber: takeOverId.ToString(),
                        accompte: accompte,
                        paid: paid,
                        mEDatas: mEDatas,
                        carteBleu: checkBoxCb.Checked,
                        espece: checkBoxEspece.Checked,
                        virement: checkBoxVirement.Checked,
                        path: $@"{Paths.TakeOverPdfsPath}\PriseEnCharge_{(takeOverId < 10 ? $"0{takeOverId.ToString()}" : takeOverId.ToString())}_{date.ToString("ddMMyyyy HHmmss")}.pdf",
                        repairTypeNames: repairTypeNames,
                        unlockTypeNames: unlockTypeNames,
                        articles: articles,
                        marques: marques,
                        modeles: modeles);
                    // Vérifier si le fichier existe
                    if (System.IO.File.Exists(takeoverfilePath))
                    {
                        MessageBox.Show("La prise en charge a été sauvegardée. Vous pouvez générer une facture.", "Information", MessageBoxButtons.OK);
                        // Ouvrir le fichier PDF dans une fenêtre externe
                        Process.Start(takeoverfilePath);
                    }
                    else
                    {
                        MessageBox.Show("Le fichier PDF n'existe pas.", "Alerte", MessageBoxButtons.OK);
                    }
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
        private void buttonTakeOverGenerateInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                int takeOverId = string.IsNullOrWhiteSpace(takeOverNumber.Text) ? MEDatasDS.OrderByDescending(x => x.TakeOverId).First().TakeOverId + 1 : int.Parse(takeOverNumber.Text);
                int invoiceId = (MEDatasDS.OrderByDescending(x => x.InvoiceId).FirstOrDefault()?.InvoiceId ?? 0) + 1;
                DateTime date = DateTime.Parse(dateTimePickerTakeOverDate.Text);
                Customer takeOverCustomer =
                    CustomersDS
                        .First(x => string.Compare(Tools.GetStringFromCustomer(x), comboBoxTakeOverLastname.Text) == 0);
                if (takeOverCustomer == null)
                {
                    MessageBox.Show("Vous devez définir un client.", "Alerte", MessageBoxButtons.OK);
                    return;
                }
                decimal? accompte = Tools.ToNullableDecimal(textBoxTakeOverAccompte.Text);
                decimal? resteDu = Tools.ToNullableDecimal(textBoxTakeOverResteDu.Text);
                decimal? paid = Tools.ToNullableDecimal(textBoxTakeOverPaid.Text);
                decimal? total = Tools.ToNullableDecimal(textBoxTakeOverTotalPrice.Text);
                int paymentMode = Tools.GetIntFromPaymentMode(checkBoxCb.Checked, checkBoxEspece.Checked, checkBoxVirement.Checked);

                List<MEData> mEDatas = new List<MEData>();
                Dictionary<int, string> repairTypeNames = new Dictionary<int, string>();
                Dictionary<int, string> unlockTypeNames = new Dictionary<int, string>();
                Dictionary<int, string> articles = new Dictionary<int, string>();
                Dictionary<int, string> marques = new Dictionary<int, string>();
                Dictionary<int, string> modeles = new Dictionary<int, string>();
                foreach (DataGridViewRow row in dataGridViewTakeOverRepair.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        int id = (row.Cells["textBoxTakeOverRepairId"].Value as int?).Value;
                        string marqueText = row.Cells["textBoxTakeOverRepairMarque"].Value as string;
                        string modeleText = row.Cells["textBoxTakeOverRepairModele"].Value as string;
                        string typeText = row.Cells["textBoxTakeOverRepairType"].Value as string;
                        string imeiText = row.Cells["textBoxTakeOverRepairIMEI"].Value as string;
                        decimal? price = row.Cells["textBoxTakeOverRepairPrice"].Value as decimal?;
                        bool? isGarantie = (row.Cells["checkBoxTakeOverRepairGarantie"].Value as bool?);

                        Marque marque = MarquesDS.First(x => string.Compare(x.Name, marqueText) == 0);
                        Modele modele = ModelesDS.First(x => string.Compare(x.Name, modeleText) == 0);
                        RepairType repairType = TakeOverTypesDS.First(x => x.Id == 0).RepairTypes.First(x => string.Compare(x.Name, typeText) == 0);
                        if (!repairTypeNames.Any(x => x.Key == repairType.Id))
                        {
                            repairTypeNames.Add(repairType.Id, repairType.Name);
                        }
                        if (!marques.Any(x => x.Key == marque.Id))
                        {
                            marques.Add(marque.Id, marque.Name);
                        }
                        if (!modeles.Any(x => x.Key == modele.Id))
                        {
                            modeles.Add(modele.Id, modele.Name);
                        }

                        Garantie garantie = isGarantie.HasValue && isGarantie.Value ? GarantiesTemp.FirstOrDefault(x => x.Id == id) : null;

                        MEData newTakeOver = new MEData(
                            takeOverId, date, null, takeOverCustomer.Id, marque.Id,
                            modele.Id, imeiText, repairType.Id, null, null,
                            1, price, garantie == null ? null : garantie.Months, null, accompte, resteDu,
                            paid, total.Value, paymentMode, TakeOverState.InProgress, id, false);
                        mEDatas.Add(newTakeOver);
                    }
                }
                foreach (DataGridViewRow row in dataGridViewTakeOverUnlock.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        int id = (row.Cells["textBoxTakeOverUnlockId"].Value as int?).Value;
                        string marqueText = row.Cells["textBoxTakeOverUnlockMarque"].Value as string;
                        string modeleText = row.Cells["textBoxTakeOverUnlockModele"].Value as string;
                        string typeText = row.Cells["textBoxTakeOverUnlockType"].Value as string;
                        string imeiText = row.Cells["textBoxTakeOverUnlockIMEI"].Value as string;
                        decimal? price = row.Cells["textBoxTakeOverUnlockPrice"].Value as decimal?;
                        bool? isGarantie = (row.Cells["checkBoxTakeOverUnlockGarantie"].Value as bool?);

                        Marque marque = MarquesDS.First(x => string.Compare(x.Name, marqueText) == 0);
                        Modele modele = ModelesDS.First(x => string.Compare(x.Name, modeleText) == 0);
                        UnlockType unlockType = TakeOverTypesDS.First(x => x.Id == 1).UnlockTypes.First(x => string.Compare(x.Name, typeText) == 0);
                        if (!unlockTypeNames.Any(x => x.Key == unlockType.Id))
                        {
                            unlockTypeNames.Add(unlockType.Id, unlockType.Name);
                        }
                        if (!marques.Any(x => x.Key == marque.Id))
                        {
                            marques.Add(marque.Id, marque.Name);
                        }
                        if (!modeles.Any(x => x.Key == modele.Id))
                        {
                            modeles.Add(modele.Id, modele.Name);
                        }

                        Garantie garantie = isGarantie.HasValue && isGarantie.Value ? GarantiesTemp.FirstOrDefault(x => x.Id == id) : null;

                        MEData newTakeOver = new MEData(
                            takeOverId, date, null, takeOverCustomer.Id, marque.Id,
                            modele.Id, imeiText, null, unlockType.Id, null,
                            1, price, garantie == null ? null : garantie.Months, null, accompte, resteDu,
                            paid, total.Value, paymentMode, TakeOverState.InProgress, id, false);
                        mEDatas.Add(newTakeOver);
                    }
                }
                foreach (DataGridViewRow row in dataGridViewTakeOverAchat.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        int id = (row.Cells["textBoxTakeOverAchatId"].Value as int?).Value;
                        string marqueText = row.Cells["textBoxTakeOverAchatMarque"].Value as string;
                        string modeleText = row.Cells["textBoxTakeOverAchatModele"].Value as string;
                        string typeText = row.Cells["textBoxTakeOverAchatType"].Value as string;
                        int quantity = (row.Cells["textBoxTakeOverAchatQuantity"].Value as int?).Value;
                        decimal? price = row.Cells["textBoxTakeOverAchatPrice"].Value as decimal?;
                        bool? isGarantie = (row.Cells["checkBoxTakeOverAchatGarantie"].Value as bool?);
                        bool? isRemise = (row.Cells["checkBoxTakeOverAchatRemise"].Value as bool?);

                        Marque marque = MarquesDS.First(x => string.Compare(x.Name, marqueText) == 0);
                        Modele modele = ModelesDS.First(x => string.Compare(x.Name, modeleText) == 0);
                        Article article = TakeOverTypesDS.First(x => x.Id == 2).Articles.First(x => string.Compare(x.Name, typeText) == 0);
                        if (!articles.Any(x => x.Key == article.Id))
                        {
                            articles.Add(article.Id, article.Name);
                        }
                        if (!marques.Any(x => x.Key == marque.Id))
                        {
                            marques.Add(marque.Id, marque.Name);
                        }
                        if (!modeles.Any(x => x.Key == modele.Id))
                        {
                            modeles.Add(modele.Id, modele.Name);
                        }

                        Garantie garantie = isGarantie.HasValue && isGarantie.Value ? GarantiesTemp.FirstOrDefault(x => x.Id == id) : null;
                        Remise remise = isRemise.HasValue && isRemise.Value ? RemisesTemp.FirstOrDefault(x => x.Id == id) : null;

                        MEData newTakeOver = new MEData(
                            takeOverId, date, null, takeOverCustomer.Id, marque.Id,
                            modele.Id, null, null, null, article.Id,
                            quantity, price, garantie?.Months, remise?.Prix, accompte, resteDu,
                            paid, total.Value, paymentMode, TakeOverState.InProgress, id, false);
                        mEDatas.Add(newTakeOver);
                    }
                }

                if (mEDatas?.Any() ?? false)
                {
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
                                            mEDatas[j].InvoiceId = invoiceId;
                                            mEDatas[j].State = factureACreer[i].State;
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
                                else
                                {
                                    MEDatasDS.Add(mEDatas[i]);
                                }
                            }

                            string path = Paths.ReceiptDSPath;

                            List<string> items = new List<string>() { "Numéro de prise en charge;Date;Numéro de facture;Numéro du client;Numéro de la marque;Numéro du modèle;IMEI;Numéro du type de réparation;Numéro du type de déblocage;Numéro de l'article;Quantité;Prix;Garantie;Remise;Accompte;Reste dû;Payé;Total;Numéro de mode de paiement;Etat;Id;Vérification" };
                            items.AddRange(MEDatasDS.Select(x => $"{x.TakeOverId};{x.Date};{x.InvoiceId};{x.CustomerId};{x.MarqueId};{x.ModeleId};{x.IMEI};{x.RepairTypeId};{x.UnlockTypeId};{x.ArticleId};{x.Quantity};{x.Price};{x.Garantie};{x.Remise};{x.Accompte};{x.ResteDu};{x.Paid};{x.Total};{paymentMode};{(int)x.State};{x.Id};{(x.Verification ? "Oui" : "Non")}"));
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
                            }
                            DialogResult acccompteResult = MessageBox.Show($"Voulez-vous prendre en compte l'accompte de {accompteAFacturer.ToString()} versé ?", "Information", MessageBoxButtons.YesNo);
                            if (acccompteResult == DialogResult.Yes)
                            {
                                payeAFacturer += accompteAFacturer;
                            }
                            string customerName =
                                takeOverCustomer.Sexe == Sexe.Unknown ? takeOverCustomer.LastName + " " + takeOverCustomer.FirstName :
                                takeOverCustomer.Sexe == Sexe.Femme ? "Mme. " + takeOverCustomer.LastName + " " + takeOverCustomer.FirstName :
                                "M. " + takeOverCustomer.LastName + " " + takeOverCustomer.FirstName;
                            // générer pdf
                            string filePath = Tools.GeneratePdfFromHtml(
                                type: 1,
                                logo: Paths.LogoPath,
                                customerName: customerName,
                                customerPhone: takeOverCustomer.PhoneNumber,
                                customerEmail: takeOverCustomer.EmailAddress,
                                takeOverDate: date.ToString("dddd dd MMMM yyyy"),
                                takeOverNumber: takeOverId.ToString(),
                                accompte: null,
                                paid: payeAFacturer,
                                carteBleu: checkBoxCb.Checked,
                                espece: checkBoxEspece.Checked,
                                virement: checkBoxVirement.Checked,
                                mEDatas: factureACreer,
                                path: $@"{Paths.InvoicePdfsPath}\Facture_{(invoiceId < 10 ? $"0{invoiceId.ToString()}" : invoiceId.ToString())}_PriseEnCharge_{(takeOverId < 10 ? $"0{takeOverId.ToString()}" : takeOverId.ToString())}_{date.ToString("ddMMyyyy HHmmss")}.pdf",
                                repairTypeNames: repairTypeNames,
                                unlockTypeNames: unlockTypeNames,
                                articles: articles,
                                marques: marques,
                                modeles: modeles);;
                            // Vérifier si le fichier existe
                            if (System.IO.File.Exists(filePath))
                            {
                                // Ouvrir le fichier PDF dans une fenêtre externe
                                Process.Start(filePath);
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Il n'y a pas d'éléments dans la facture. Veuillez remplir les onglets pour générer une facture.", "Alerte", MessageBoxButtons.OK);
                }
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
                    if (dialog.ShowDialog() == DialogResult.OK)
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
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    if (e.ColumnIndex == dataGridViewTakeOverRepair.Columns["buttonTakeOverRepairDeleteRow"].Index)
                    {
                        // Suppression de la ligne
                        dataGridViewTakeOverRepair.Rows.RemoveAt(e.RowIndex);
                    }
                    else if (e.ColumnIndex == dataGridViewTakeOverRepair.Columns["checkBoxTakeOverRepairGarantie"].Index)
                    {
                        int? id = ((DataGridViewTextBoxCell)dataGridViewTakeOverRepair.Rows[e.RowIndex].Cells["textBoxTakeOverRepairId"]).Value as int?;
                        if (id.HasValue)
                        {
                            bool? isGarantie = (dataGridViewTakeOverRepair.Rows[e.RowIndex].Cells["checkBoxTakeOverRepairGarantie"]).Value as bool?;
                            // si le click == coché : retirer la garantie
                            if (isGarantie.HasValue && isGarantie.Value)
                            {
                                (dataGridViewTakeOverRepair.Rows[e.RowIndex].Cells["checkBoxTakeOverRepairGarantie"]).Value = false;

                                MEData oldMEData = MEDatasDS.FirstOrDefault(x => x.Id == id.Value);
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
                                    if (id.Value == GarantiesTemp[i].Id)
                                    {
                                        GarantiesTemp.RemoveAt(i);
                                        break;
                                    }
                                }
                            }
                            // si le click == décoché : ajouter la garantie
                            else if (!isGarantie.HasValue || !isGarantie.Value)
                            {
                                (dataGridViewTakeOverRepair.Rows[e.RowIndex].Cells["checkBoxTakeOverRepairGarantie"]).Value = true;

                                // ajouter la ligne de garantie
                                string marqueText = (dataGridViewTakeOverRepair.Rows[e.RowIndex].Cells["textBoxTakeOverRepairMarque"]).Value as string;
                                string modeleText = (dataGridViewTakeOverRepair.Rows[e.RowIndex].Cells["textBoxTakeOverRepairModele"]).Value as string;
                                string typeText = (dataGridViewTakeOverRepair.Rows[e.RowIndex].Cells["textBoxTakeOverRepairType"]).Value as string;

                                string productName = typeText;
                                if (!string.IsNullOrWhiteSpace(marqueText))
                                {
                                    if (!string.IsNullOrWhiteSpace(modeleText))
                                    {
                                        productName = marqueText + " " + modeleText + " - " + productName;
                                    }
                                    else
                                    {
                                        productName = marqueText + " - " + productName;
                                    }
                                }

                                GarantiesTemp.Add(new Garantie(tabName: "Réparation", id: id.Value, productName: productName, months: null));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Assurez-vous d'avoir renseigné un produit.", "Alerte", MessageBoxButtons.OK);
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

                if (currentColumnIndex >= 0 && currentRowIndex >= 0)
                {
                    if (currentColumnIndex == dataGridViewTakeOverRepair.Columns["textBoxTakeOverRepairPrice"].Index)
                    {
                        UpdateTotalPrice();
                    }
                    else if (e.Control is DataGridViewTextBoxEditingControl textBox && textBox != null)
                    {
                        if (currentColumnIndex == dataGridViewTakeOverRepair.Columns["textBoxTakeOverRepairMarque"].Index)
                        {
                            // Créez et initialisez votre source de données pour l'auto-complétion
                            AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                            autoCompleteData.AddRange(MarquesDS.Select(x => x.Name).ToArray());

                            // Définissez la source de données de l'auto-complétion pour le TextBox
                            textBox.AutoCompleteCustomSource = autoCompleteData;
                            textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                        else if (currentColumnIndex == dataGridViewTakeOverRepair.Columns["textBoxTakeOverRepairModele"].Index)
                        {
                            DataGridViewTextBoxCell marqueTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverRepair.Rows[currentRowIndex].Cells["textBoxTakeOverRepairMarque"];
                            Marque marque = MarquesDS.FirstOrDefault(x => string.Compare(x.Name, marqueTextBoxCell?.Value as string) == 0);

                            // Créez et initialisez votre source de données pour l'auto-complétion
                            AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                            autoCompleteData.AddRange((marque != null ? ModelesDS.Where(x => x.MarqueId == marque.Id) : ModelesDS).Select(x => x.Name).ToArray());

                            // Définissez la source de données de l'auto-complétion pour le TextBox
                            textBox.AutoCompleteCustomSource = autoCompleteData;
                            textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                        else if (currentColumnIndex == dataGridViewTakeOverRepair.Columns["textBoxTakeOverRepairType"].Index)
                        {
                            // Créez et initialisez votre source de données pour l'auto-complétion
                            AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                            var repairTypes = TakeOverTypesDS.Find(x => x.Id == 0).RepairTypes;

                            autoCompleteData.AddRange(repairTypes.Select(x => x.Name).ToArray());

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
        private void DataGridViewTakeOverRepair_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int columnIndex = dataGridViewTakeOverRepair.CurrentCell.ColumnIndex;
                int rowIndex = dataGridViewTakeOverRepair.CurrentCell.RowIndex;
                if (columnIndex >= 0 && rowIndex >= 0)
                {
                    DataGridViewTextBoxCell textBox = (sender as DataGridView).CurrentCell as DataGridViewTextBoxCell;
                    DataGridViewTextBoxEditingControl textBoxEditingControl = (sender as DataGridView).EditingControl as DataGridViewTextBoxEditingControl;
                    if (textBox != null && textBox.Value != null)
                    {
                        if (!string.IsNullOrWhiteSpace(textBox.Value as string) && textBoxEditingControl != null)
                        {
                            if (!(dataGridViewTakeOverRepair.Rows[e.RowIndex].Cells["textBoxTakeOverRepairId"].Value as int?).HasValue)
                            {
                                int newId = GetItemId();
                                dataGridViewTakeOverRepair.Rows[e.RowIndex].Cells["textBoxTakeOverRepairId"].Value = newId;
                            }
                            if (columnIndex == dataGridViewTakeOverRepair.Columns["textBoxTakeOverRepairMarque"].Index)
                            {
                                Marque marque = MarquesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), (textBox.Value as string).ToLower()) == 0);
                                if (marque == null)
                                {
                                    string marqueName = textBox.Value as string;
                                    marque = new Marque(MarquesDS.OrderByDescending(x => x.Id).First().Id + 1, char.ToUpper(marqueName[0]) + marqueName.Substring(1));
                                    DialogResult result = MessageBox.Show($"La marque \"{marqueName}\" n'est pas connue du système. Voulez-vous l'ajouter ?", "Information", MessageBoxButtons.YesNo);
                                    if (result == DialogResult.Yes)
                                    {
                                        MarquesDS.Add(marque);
                                        Tools.WriteLineTofile($"{marque.Id};{marque.Name}", Paths.MarquesDSPath, true);

                                        // initialisez votre source de données pour l'auto-complétion
                                        AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                                        autoCompleteData.AddRange(MarquesDS.Select(x => x.Name).ToArray());
                                        textBoxEditingControl.AutoCompleteCustomSource = autoCompleteData;
                                    }
                                }
                            }
                            else if (columnIndex == dataGridViewTakeOverRepair.Columns["textBoxTakeOverRepairModele"].Index)
                            {
                                Modele modele = ModelesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), (textBox.Value as string).ToLower()) == 0);
                                if (modele == null)
                                {
                                    DataGridViewTextBoxCell marqueTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverRepair.Rows[rowIndex].Cells["textBoxTakeOverRepairMarque"];
                                    Marque marque = null;
                                    if (marqueTextBoxCell != null && !string.IsNullOrWhiteSpace(marqueTextBoxCell.Value as string))
                                    {
                                        marque = MarquesDS.First(x => string.Compare(x.Name.ToLower(), (marqueTextBoxCell.Value as string).ToLower()) == 0);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Pour ajouter un nouveau modèle, assurez-vous d'avoir sélectionné une marque.", "Alerte");
                                        return;
                                    }

                                    string modeleName = textBox.Value as string;
                                    modele = new Modele(ModelesDS.OrderByDescending(x => x.Id).First().Id + 1, marque.Id, char.ToUpper(modeleName[0]) + modeleName.Substring(1));
                                    DialogResult result = MessageBox.Show($"Le modele \"{modeleName}\" n'est pas connue du système. Voulez-vous l'ajouter ?", "Information", MessageBoxButtons.YesNo);
                                    if (result == DialogResult.Yes)
                                    {
                                        ModelesDS.Add(modele);
                                        Tools.WriteLineTofile($"{modele.Id};{modele.MarqueId};{modele.Name}", Paths.ModelesDSPath, true);

                                        // initialisez votre source de données pour l'auto-complétion
                                        AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                                        autoCompleteData.AddRange(ModelesDS.Select(x => x.Name).ToArray());
                                        textBoxEditingControl.AutoCompleteCustomSource = autoCompleteData;
                                    }
                                }
                            }
                            else if (columnIndex == dataGridViewTakeOverRepair.Columns["textBoxTakeOverRepairType"].Index)
                            {
                                RepairType repairType = TakeOverTypesDS.Find(x => x.Id == 0).RepairTypes.FirstOrDefault(x => string.Compare(x.Name.ToLower(), (textBox.Value as string).ToLower()) == 0);
                                if (repairType == null)
                                {
                                    string typeName = textBox.Value as string;
                                    repairType = new RepairType(TakeOverTypesDS.Find(x => x.Id == 0).RepairTypes.OrderByDescending(x => x.Id).First().Id + 1, char.ToUpper(typeName[0]) + typeName.Substring(1), 0);

                                    DialogResult result = MessageBox.Show($"La réparation \"{typeName}\" n'est pas connue du système. Voulez-vous l'ajouter ?", "Information", MessageBoxButtons.YesNo);
                                    if (result == DialogResult.Yes)
                                    {
                                        TakeOverTypesDS.Find(x => x.Id == 0).RepairTypes.Add(repairType);
                                        Tools.WriteLineTofile($"{repairType.Id};{repairType.Name};{repairType.Price}", Paths.RepairTypesDSPath, true);

                                        // initialisez votre source de données pour l'auto-complétion
                                        AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                                        autoCompleteData.AddRange(TakeOverTypesDS.Find(x => x.Id == 0).RepairTypes.Select(x => x.Name).ToArray());
                                        textBoxEditingControl.AutoCompleteCustomSource = autoCompleteData;
                                    }
                                }
                                else
                                {
                                    dataGridViewTakeOverRepair.Rows[rowIndex].Cells["textBoxTakeOverRepairPrice"].Value = repairType.Price;
                                    UpdateTotalPrice();
                                }
                            }
                        }
                        else if ((textBox.Value as decimal?).HasValue)
                        {
                            if (columnIndex == dataGridViewTakeOverRepair.Rows[rowIndex].Cells["textBoxTakeOverRepairPrice"].ColumnIndex)
                            {
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
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    if (e.ColumnIndex == dataGridViewTakeOverUnlock.Columns["buttonTakeOverUnlockDeleteRow"].Index)
                    {
                        // Suppression de la ligne
                        dataGridViewTakeOverUnlock.Rows.RemoveAt(e.RowIndex);
                    }
                    else if (e.ColumnIndex == dataGridViewTakeOverUnlock.Columns["checkBoxTakeOverUnlockGarantie"].Index)
                    {
                        int? id = ((DataGridViewTextBoxCell)dataGridViewTakeOverUnlock.Rows[e.RowIndex].Cells["textBoxTakeOverUnlockId"]).Value as int?;
                        if (id.HasValue)
                        {
                            bool? isGarantie = (dataGridViewTakeOverUnlock.Rows[e.RowIndex].Cells["checkBoxTakeOverUnlockGarantie"]).Value as bool?;
                            // si le click == coché : retirer la garantie
                            if (isGarantie.HasValue && isGarantie.Value)
                            {
                                (dataGridViewTakeOverUnlock.Rows[e.RowIndex].Cells["checkBoxTakeOverUnlockGarantie"]).Value = false;

                                MEData oldMEData = MEDatasDS.FirstOrDefault(x => x.Id == id.Value);
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
                                    if (id.Value == GarantiesTemp[i].Id)
                                    {
                                        GarantiesTemp.RemoveAt(i);
                                        break;
                                    }
                                }
                            }
                            // si le click == décoché : ajouter la garantie
                            else if (!isGarantie.HasValue || !isGarantie.Value)
                            {
                                (dataGridViewTakeOverUnlock.Rows[e.RowIndex].Cells["checkBoxTakeOverUnlockGarantie"]).Value = true;
                                // ajouter la ligne de garantie
                                string marqueText = (dataGridViewTakeOverUnlock.Rows[e.RowIndex].Cells["textBoxTakeOverUnlockMarque"]).Value as string;
                                string modeleText = (dataGridViewTakeOverUnlock.Rows[e.RowIndex].Cells["textBoxTakeOverUnlockModele"]).Value as string;
                                string typeText = (dataGridViewTakeOverUnlock.Rows[e.RowIndex].Cells["textBoxTakeOverUnlockType"]).Value as string;

                                string productName = typeText;
                                if (!string.IsNullOrWhiteSpace(marqueText))
                                {
                                    if (!string.IsNullOrWhiteSpace(modeleText))
                                    {
                                        productName = marqueText + " " + modeleText + " - " + productName;
                                    }
                                    else
                                    {
                                        productName = marqueText + " - " + productName;
                                    }
                                }

                                GarantiesTemp.Add(new Garantie(tabName: "Déblocage", id: id.Value, productName: productName, months: null));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Assurez-vous d'avoir renseigné un produit.", "Alerte", MessageBoxButtons.OK);
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

                if (currentColumnIndex >= 0 && currentRowIndex >= 0)
                {
                    if (currentColumnIndex == dataGridViewTakeOverUnlock.Columns["textBoxTakeOverUnlockPrice"].Index)
                    {
                        UpdateTotalPrice();
                    }
                    else if (e.Control is DataGridViewTextBoxEditingControl textBox && textBox != null)
                    {
                        if (currentColumnIndex == dataGridViewTakeOverUnlock.Columns["textBoxTakeOverUnlockMarque"].Index)
                        {
                            // Créez et initialisez votre source de données pour l'auto-complétion
                            AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                            autoCompleteData.AddRange(MarquesDS.Select(x => x.Name).ToArray());

                            // Définissez la source de données de l'auto-complétion pour le TextBox
                            textBox.AutoCompleteCustomSource = autoCompleteData;
                            textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                        else if (currentColumnIndex == dataGridViewTakeOverUnlock.Columns["textBoxTakeOverUnlockModele"].Index)
                        {
                            DataGridViewTextBoxCell marqueTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverUnlock.Rows[currentRowIndex].Cells["textBoxTakeOverUnlockMarque"];
                            Marque marque = MarquesDS.FirstOrDefault(x => string.Compare(x.Name, marqueTextBoxCell?.Value as string) == 0);

                            // Créez et initialisez votre source de données pour l'auto-complétion
                            AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                            autoCompleteData.AddRange((marque != null ? ModelesDS.Where(x => x.MarqueId == marque.Id) : ModelesDS).Select(x => x.Name).ToArray());

                            // Définissez la source de données de l'auto-complétion pour le TextBox
                            textBox.AutoCompleteCustomSource = autoCompleteData;
                            textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                            textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                        else if (currentColumnIndex == dataGridViewTakeOverUnlock.Columns["textBoxTakeOverUnlockType"].Index)
                        {
                            // Créez et initialisez votre source de données pour l'auto-complétion
                            AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                            var UnlockTypes = TakeOverTypesDS.Find(x => x.Id == 1).UnlockTypes;

                            autoCompleteData.AddRange(UnlockTypes.Select(x => x.Name).ToArray());

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
        private void DataGridViewTakeOverUnlock_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int columnIndex = dataGridViewTakeOverUnlock.CurrentCell.ColumnIndex;
                int rowIndex = dataGridViewTakeOverUnlock.CurrentCell.RowIndex;
                if (columnIndex >= 0 && rowIndex >= 0)
                {
                    DataGridViewTextBoxCell textBox = (sender as DataGridView).CurrentCell as DataGridViewTextBoxCell;
                    DataGridViewTextBoxEditingControl textBoxEditingControl = (sender as DataGridView).EditingControl as DataGridViewTextBoxEditingControl;
                    if (textBox != null && textBox.Value != null)
                    {
                        if (!string.IsNullOrWhiteSpace(textBox.Value as string) && textBoxEditingControl != null)
                        {
                            if (!(dataGridViewTakeOverUnlock.Rows[e.RowIndex].Cells["textBoxTakeOverUnlockId"].Value as int?).HasValue)
                            {
                                int newId = GetItemId();
                                dataGridViewTakeOverUnlock.Rows[e.RowIndex].Cells["textBoxTakeOverUnlockId"].Value = newId;
                            }
                            if (columnIndex == dataGridViewTakeOverUnlock.Columns["textBoxTakeOverUnlockMarque"].Index)
                            {
                                string marqueName = textBox.Value as string;
                                Marque marque = MarquesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), marqueName.ToLower()) == 0);
                                if (marque == null)
                                {
                                    marque = new Marque(MarquesDS.OrderByDescending(x => x.Id).First().Id + 1, char.ToUpper(marqueName[0]) + marqueName.Substring(1));

                                    DialogResult result = MessageBox.Show($"La marque \"{marqueName}\" n'est pas connue du système. Voullez-vous l'ajouter ?", "Information", MessageBoxButtons.YesNo);
                                    if (result == DialogResult.Yes)
                                    {
                                        MarquesDS.Add(marque);
                                        Tools.WriteLineTofile($"{marque.Id};{marque.Name}", Paths.MarquesDSPath, true);

                                        // initialisez votre source de données pour l'auto-complétion
                                        AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                                        autoCompleteData.AddRange(MarquesDS.Select(x => x.Name).ToArray());
                                        textBoxEditingControl.AutoCompleteCustomSource = autoCompleteData;
                                    }
                                }
                            }
                            else if (columnIndex == dataGridViewTakeOverUnlock.Columns["textBoxTakeOverUnlockModele"].Index)
                            {
                                string modeleName = textBox.Value as string;
                                Modele modele = ModelesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), modeleName.ToLower()) == 0);
                                if (modele == null)
                                {
                                    DataGridViewTextBoxCell marqueTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverUnlock.Rows[rowIndex].Cells["textBoxTakeOverUnlockMarque"];
                                    Marque marque = null;
                                    if (marqueTextBoxCell != null && !string.IsNullOrWhiteSpace(marqueTextBoxCell.Value as string))
                                    {
                                        marque = MarquesDS.First(x => string.Compare(x.Name.ToLower(), (marqueTextBoxCell.Value as string).ToLower()) == 0);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Pour ajouter un nouveau modèle, assurez-vous d'avoir sélectionné une marque.", "Alerte");
                                        return;
                                    }

                                    modele = new Modele(ModelesDS.OrderByDescending(x => x.Id).First().Id + 1, marque.Id, char.ToUpper(modeleName[0]) + modeleName.Substring(1));
                                    DialogResult result = MessageBox.Show($"Le modele \"{modeleName}\" n'est pas connue du système. Voullez-vous l'ajouter ?", "Information", MessageBoxButtons.YesNo);
                                    if (result == DialogResult.Yes)
                                    {
                                        ModelesDS.Add(modele);
                                        Tools.WriteLineTofile($"{modele.Id};{modele.MarqueId};{modele.Name}", Paths.ModelesDSPath, true);

                                        // initialisez votre source de données pour l'auto-complétion
                                        AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                                        autoCompleteData.AddRange(ModelesDS.Select(x => x.Name).ToArray());
                                        textBoxEditingControl.AutoCompleteCustomSource = autoCompleteData;
                                    }
                                }
                            }
                            else if (columnIndex == dataGridViewTakeOverUnlock.Columns["textBoxTakeOverUnlockType"].Index)
                            {
                                string typeName = textBox.Value as string;
                                UnlockType UnlockType = TakeOverTypesDS.Find(x => x.Id == 1).UnlockTypes.FirstOrDefault(x => string.Compare(x.Name.ToLower(), typeName.ToLower()) == 0);
                                if (UnlockType == null)
                                {
                                    UnlockType = new UnlockType(TakeOverTypesDS.Find(x => x.Id == 1).UnlockTypes.OrderByDescending(x => x.Id).First().Id + 1, char.ToUpper(typeName[0]) + typeName.Substring(1), 0);
                                    DialogResult result = MessageBox.Show($"Le déblocage \"{typeName}\" n'est pas connue du système. Voullez-vous l'ajouter ?", "Information", MessageBoxButtons.YesNo);
                                    if (result == DialogResult.Yes)
                                    {
                                        TakeOverTypesDS.Find(x => x.Id == 1).UnlockTypes.Add(UnlockType);
                                        Tools.WriteLineTofile($"{UnlockType.Id};{UnlockType.Name};{UnlockType.Price}", Paths.MarquesDSPath, true);

                                        // initialisez votre source de données pour l'auto-complétion
                                        AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                                        autoCompleteData.AddRange(TakeOverTypesDS.Find(x => x.Id == 1).UnlockTypes.Select(x => x.Name).ToArray());
                                        textBoxEditingControl.AutoCompleteCustomSource = autoCompleteData;
                                    }
                                }
                                else
                                {
                                    dataGridViewTakeOverUnlock.Rows[rowIndex].Cells["textBoxTakeOverUnlockPrice"].Value = UnlockType.Price;
                                    UpdateTotalPrice();
                                }
                            }
                        }
                        else if ((textBox.Value as decimal?).HasValue)
                        {
                            if (dataGridViewTakeOverUnlock.Rows[rowIndex].Cells["textBoxTakeOverUnlockPrice"].ColumnIndex == columnIndex)
                            {
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
            if (dataGridViewTakeOverAchat.CurrentCell is DataGridViewComboBoxCell)
            {
                dataGridViewTakeOverAchat.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void DataGridViewTakeOverAchat_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    if (e.ColumnIndex == dataGridViewTakeOverAchat.Columns["buttonTakeOverAchatDeleteRow"].Index)
                    {
                        // Suppression de la ligne
                        dataGridViewTakeOverAchat.Rows.RemoveAt(e.RowIndex);
                    }
                    else if (e.ColumnIndex == dataGridViewTakeOverAchat.Columns["checkBoxTakeOverAchatGarantie"].Index)
                    {
                        int? id = ((DataGridViewTextBoxCell)dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["textBoxTakeOverAchatId"]).Value as int?;
                        if (id.HasValue)
                        {
                            bool? isGarantie = (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["checkBoxTakeOverAchatGarantie"]).Value as bool?;
                            // si le click == coché : retirer la garantie
                            if (isGarantie.HasValue && isGarantie.Value)
                            {
                                (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["checkBoxTakeOverAchatGarantie"]).Value = false;
                                MEData oldMEData = MEDatasDS.FirstOrDefault(x => x.Id == id.Value);
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
                                    if (id.Value == GarantiesTemp[i].Id)
                                    {
                                        GarantiesTemp.RemoveAt(i);
                                        break;
                                    }
                                }
                            }
                            // si le click == décoché : ajouter la garantie
                            else if (!isGarantie.HasValue || !isGarantie.Value)
                            {
                                (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["checkBoxTakeOverAchatGarantie"]).Value = true;
                                // ajouter la ligne de garantie
                                string marqueText = (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["textBoxTakeOverAchatMarque"]).Value as string;
                                string modeleText = (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["textBoxTakeOverAchatModele"]).Value as string;
                                string typeText = (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["textBoxTakeOverAchatType"]).Value as string;

                                string productName = typeText;
                                if (!string.IsNullOrWhiteSpace(marqueText))
                                {
                                    if (!string.IsNullOrWhiteSpace(modeleText))
                                    {
                                        productName = marqueText + " " + modeleText + " - " + productName;
                                    }
                                    else
                                    {
                                        productName = marqueText + " - " + productName;
                                    }
                                }

                                GarantiesTemp.Add(new Garantie(tabName: "Achat", id: id.Value, productName: productName, months: null));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Assurez-vous d'avoir renseigné un produit.", "Alerte", MessageBoxButtons.OK);
                        }
                    }
                    else if (e.ColumnIndex == dataGridViewTakeOverAchat.Columns["checkBoxTakeOverAchatRemise"].Index)
                    {
                        int? id = ((DataGridViewTextBoxCell)dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["textBoxTakeOverAchatId"]).Value as int?;
                        if (id.HasValue)
                        {
                            bool? isRemise = (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["checkBoxTakeOverAchatRemise"]).Value as bool?;
                            // si le click == cocher : retirer la remise
                            if (isRemise.HasValue && isRemise.Value)
                            {
                                (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["checkBoxTakeOverAchatRemise"]).Value = false;
                                MEData oldMEData = MEDatasDS.FirstOrDefault(x => x.Id == id.Value);
                                // si oldreceipt != null : c'est un ancien receipt
                                // retirer la garantie de receipt
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
                                    if (id.Value == RemisesTemp[i].Id)
                                    {
                                        RemisesTemp.RemoveAt(i);
                                        break;
                                    }
                                }
                            }
                            // si le click == décocher : ajouter la remise
                            else if (!isRemise.HasValue || !isRemise.Value)
                            {
                                (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["checkBoxTakeOverAchatRemise"]).Value = true;
                                // ajouter la ligne de remise
                                string marqueText = (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["textBoxTakeOverAchatMarque"]).Value as string;
                                string modeleText = (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["textBoxTakeOverAchatModele"]).Value as string;
                                string typeText = (dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["textBoxTakeOverAchatType"]).Value as string;

                                string productName = typeText;
                                if (!string.IsNullOrWhiteSpace(marqueText))
                                {
                                    if (!string.IsNullOrWhiteSpace(modeleText))
                                    {
                                        productName = marqueText + " " + modeleText + " - " + productName;
                                    }
                                    else
                                    {
                                        productName = marqueText + " - " + productName;
                                    }
                                }

                                RemisesTemp.Add(new Remise(tabName: "Achat", id: id.Value, productName: productName, prix: null));
                            }
                        }
                        else
                        {
                            MessageBox.Show("Assurez-vous d'avoir renseigné un produit.", "Alerte", MessageBoxButtons.OK);
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
                if (currentColumnIndex >= 0 && currentRowIndex >= 0 && e.Control is DataGridViewTextBoxEditingControl textBox && textBox != null)
                {
                    if (currentColumnIndex == dataGridViewTakeOverAchat.Columns["textBoxTakeOverAchatMarque"].Index)
                    {
                        // Créez et initialisez votre source de données pour l'auto-complétion
                        AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                        autoCompleteData.AddRange(MarquesDS.Select(x => x.Name).ToArray());

                        // Définissez la source de données de l'auto-complétion pour le TextBox
                        textBox.AutoCompleteCustomSource = autoCompleteData;
                        textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                        textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    }
                    else if (currentColumnIndex == dataGridViewTakeOverAchat.Columns["textBoxTakeOverAchatModele"].Index)
                    {
                        DataGridViewTextBoxCell marqueTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverAchat.Rows[currentRowIndex].Cells["textBoxTakeOverAchatMarque"];
                        Marque marque = MarquesDS.FirstOrDefault(x => string.Compare(x.Name, marqueTextBoxCell?.Value as string) == 0);

                        // Créez et initialisez votre source de données pour l'auto-complétion
                        AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                        autoCompleteData.AddRange((marque != null ? ModelesDS.Where(x => x.MarqueId == marque.Id) : ModelesDS).Select(x => x.Name).ToArray());

                        // Définissez la source de données de l'auto-complétion pour le TextBox
                        textBox.AutoCompleteCustomSource = autoCompleteData;
                        textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                        textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    }
                    else if (currentColumnIndex == dataGridViewTakeOverAchat.Columns["textBoxTakeOverAchatType"].Index)
                    {
                        DataGridViewTextBoxCell marqueTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverAchat.Rows[currentRowIndex].Cells["textBoxTakeOverAchatMarque"];
                        Marque marque = MarquesDS.FirstOrDefault(x => string.Compare(x.Name, marqueTextBoxCell?.Value as string) == 0);

                        DataGridViewTextBoxCell modeleTextBoxCell = (DataGridViewTextBoxCell)dataGridViewTakeOverAchat.Rows[currentRowIndex].Cells["textBoxTakeOverAchatModele"];
                        Modele modele = ModelesDS.FirstOrDefault(x => string.Compare(x.Name, modeleTextBoxCell?.Value as string) == 0);

                        // Créez et initialisez votre source de données pour l'auto-complétion
                        AutoCompleteStringCollection autoCompleteData = new AutoCompleteStringCollection();
                        var articles = TakeOverTypesDS.Find(x => x.Id == 2).Articles;

                        if (marque != null && modele != null)
                        {
                            autoCompleteData.AddRange(articles.Where(x => x.MarqueId == marque.Id && x.ModeleId == modele.Id).Select(x => x.Name).ToArray());
                        }
                        else if (marque != null)
                        {
                            autoCompleteData.AddRange(articles.Where(x => x.MarqueId == marque.Id).Select(x => x.Name).ToArray());
                        }
                        else if (modele != null)
                        {
                            autoCompleteData.AddRange(articles.Where(x => x.ModeleId == modele.Id).Select(x => x.Name).ToArray());
                        }
                        else
                        {
                            autoCompleteData.AddRange(articles.Select(x => x.Name).ToArray());
                        }

                        // Définissez la source de données de l'auto-complétion pour le TextBox
                        textBox.AutoCompleteCustomSource = autoCompleteData;
                        textBox.AutoCompleteMode = AutoCompleteMode.Suggest;
                        textBox.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    }
                    else if (currentColumnIndex == dataGridViewTakeOverAchat.Columns["textBoxTakeOverAchatPrice"].Index)
                    {
                        UpdateTotalPrice();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void DataGridViewTakeOverAchat_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int columnIndex = dataGridViewTakeOverAchat.CurrentCell.ColumnIndex;
                int rowIndex = dataGridViewTakeOverAchat.CurrentCell.RowIndex;
                if (columnIndex >= 0 && rowIndex >= 0)
                {
                    DataGridViewTextBoxCell textBox = (sender as DataGridView).CurrentCell as DataGridViewTextBoxCell;
                    if (textBox != null && textBox.Value != null)
                    {
                        if (!string.IsNullOrWhiteSpace(textBox.Value as string))
                        {
                            if (!(dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["textBoxTakeOverAchatId"].Value as int?).HasValue)
                            {
                                int newId = GetItemId();
                                dataGridViewTakeOverAchat.Rows[e.RowIndex].Cells["textBoxTakeOverAchatId"].Value = newId;
                            }
                            if (columnIndex == dataGridViewTakeOverAchat.Columns["textBoxTakeOverAchatType"].Index)
                            {
                                string articleName = textBox.Value as string;
                                Article article = TakeOverTypesDS.Find(x => x.Id == 2).Articles.FirstOrDefault(x => string.Compare(x.Name.ToLower(), articleName.ToLower()) == 0);
                                dataGridViewTakeOverAchat.Rows[rowIndex].Cells["textBoxTakeOverAchatQuantity"].Value = 1;
                                dataGridViewTakeOverAchat.Rows[rowIndex].Cells["textBoxTakeOverAchatPrice"].Value = article.Price;
                                UpdateTotalPrice();
                            }
                        }
                        else if ((textBox.Value as int?).HasValue)
                        {
                            if (columnIndex == dataGridViewTakeOverAchat.Columns["textBoxTakeOverAchatQuantity"].Index)
                            {
                                string marqueName = dataGridViewTakeOverAchat.Rows[rowIndex].Cells["textBoxTakeOverAchatMarque"].Value as string;
                                string modeleName = dataGridViewTakeOverAchat.Rows[rowIndex].Cells["textBoxTakeOverAchatModele"].Value as string;
                                string articleName = dataGridViewTakeOverAchat.Rows[rowIndex].Cells["textBoxTakeOverAchatType"].Value as string;
                                Marque marque = MarquesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), marqueName.ToLower()) == 0);
                                Modele modele = ModelesDS.FirstOrDefault(x => x.MarqueId == marque.Id && string.Compare(x.Name.ToLower(), modeleName.ToLower()) == 0);
                                Article article =
                                    TakeOverTypesDS.Find(x => x.Id == 2).Articles.FirstOrDefault(x => x.MarqueId == marque.Id && x.ModeleId == modele.Id && string.Compare(x.Name.ToLower(), articleName.ToLower()) == 0) ??
                                    TakeOverTypesDS.Find(x => x.Id == 2).Articles.FirstOrDefault(x => string.Compare(x.Name.ToLower(), articleName.ToLower()) == 0);
                                if (article == null)
                                {
                                    MessageBox.Show("L'article renseigné est introuvable. Veuillez créer un article dans l'onglet Stock.", "Alerte", MessageBoxButtons.OK);
                                    return;
                                }

                                int quantity = (textBox.Value as int?).Value;
                                if (article.Quantity < quantity)
                                {
                                    dataGridViewTakeOverAchat.Rows[rowIndex].Cells["textBoxTakeOverAchatQuantity"].Value = article.Quantity;
                                    quantity = article.Quantity;
                                }

                                dataGridViewTakeOverAchat.Rows[rowIndex].Cells["textBoxTakeOverAchatPrice"].Value = quantity * article.Price;
                                UpdateTotalPrice();
                            }
                        }
                        else if ((textBox.Value as decimal?).HasValue)
                        {
                            if (columnIndex == dataGridViewTakeOverAchat.Columns["textBoxTakeOverAchatPrice"].Index)
                            {
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
                        decimal? price = row.Cells["textBoxTakeOverRepairPrice"].Value as decimal?;
                        if (price != null)
                        {
                            totalPrice += price.Value;
                        }
                    }
                }
                foreach (DataGridViewRow row in dataGridViewTakeOverUnlock.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        decimal? price = row.Cells["textBoxTakeOverUnlockPrice"].Value as decimal?;
                        if (price != null)
                        {
                            totalPrice += price.Value;
                        }
                    }
                }
                foreach (DataGridViewRow row in dataGridViewTakeOverAchat.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        decimal? price = row.Cells["textBoxTakeOverAchatPrice"].Value as decimal?;
                        if (price != null)
                        {
                            bool? isRemise = row.Cells["checkBoxTakeOverAchatRemise"].Value as bool?;
                            int? id = row.Cells["textBoxTakeOverAchatId"].Value as int?;
                            if (isRemise.HasValue && isRemise.Value && RemisesTemp.Any(x => x.Id == id.Value))
                            {
                                remise += RemisesTemp.First(x => x.Id == id.Value).Prix.Value;
                            }
                            totalPrice += price.Value;
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
                if ((row.Cells["textBoxTakeOverRepairId"].Value as int?).HasValue)
                {
                    int id = (row.Cells["textBoxTakeOverRepairId"].Value as int?).Value;
                    newId = id > newId ? id : newId;
                }
            }
            foreach (DataGridViewRow row in dataGridViewTakeOverUnlock.Rows)
            {
                if ((row.Cells["textBoxTakeOverUnlockId"].Value as int?).HasValue)
                {
                    int id = (row.Cells["textBoxTakeOverUnlockId"].Value as int?).Value;
                    newId = id > newId ? id : newId;
                }
            }
            foreach (DataGridViewRow row in dataGridViewTakeOverAchat.Rows)
            {
                if ((row.Cells["textBoxTakeOverAchatId"].Value as int?).HasValue)
                {
                    int id = (row.Cells["textBoxTakeOverAchatId"].Value as int?).Value;
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
        private void buttonCustomerRelationSaveCustomerData_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxCustomerRelationLastName.Text))
                {
                    MessageBox.Show("Veuillez saisir le nom du client.");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(textBoxCustomerRelationFirstName.Text))
                {
                    MessageBox.Show("Veuillez saisir le prénom du client.");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(textBoxCustomerRelationPhone.Text))
                {
                    MessageBox.Show("Veuillez saisir le numéro de téléphone du client.");
                    return;
                }
                else if (string.IsNullOrWhiteSpace(textBoxCustomerRelationEmailAddress.Text))
                {
                    MessageBox.Show("Veuillez saisir l'adresse mail du client.");
                    return;
                }

                CustomerRelationUpdateDatabase(new Customer()
                {
                    Id = CustomersDS.First(x =>
                        string.Compare(textBoxCustomerRelationLastName.Text, x.LastName ?? string.Empty) == 0 &&
                        string.Compare(textBoxCustomerRelationFirstName.Text, x.FirstName ?? string.Empty) == 0 &&
                        string.Compare(textBoxCustomerRelationPhone.Text, x.PhoneNumber ?? string.Empty) == 0 &&
                        string.Compare(textBoxCustomerRelationEmailAddress.Text, x.EmailAddress ?? string.Empty) == 0).Id,
                    LastName = textBoxCustomerRelationLastName.Text,
                    FirstName = textBoxCustomerRelationFirstName.Text,
                    PhoneNumber = textBoxCustomerRelationPhone.Text,
                    EmailAddress = textBoxCustomerRelationEmailAddress.Text
                }, 0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur : " + ex.Message);
            }
        }
        private void DataGridViewCustomerRelationAll_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                var customer = CustomersDS.First(x =>
                    string.Compare(x.LastName, (sender as DataGridView).Rows[e.RowIndex].Cells[0].Value as string) == 0 &&
                    string.Compare(x.FirstName, (sender as DataGridView).Rows[e.RowIndex].Cells[1].Value as string) == 0);

                comboBoxCustomerRelationSexe.SelectedItem = Tools.GetEnumDescription<Sexe>(customer.Sexe);
                textBoxCustomerRelationLastName.Text = customer.LastName;
                textBoxCustomerRelationFirstName.Text = customer.FirstName;
                textBoxCustomerRelationPhone.Text = customer.PhoneNumber;
                textBoxCustomerRelationEmailAddress.Text = customer.EmailAddress;

                dataGridViewOneCustomerRelation_Load(customer);
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
                        }
                    });

                    List<string> items = new List<string>() { "Id;Nom;Prénom;Numéro de téléphone;Adresse mail;Sexe" };
                    items.AddRange(CustomersDS.Select(x => $"{x.Id};{x.LastName};{x.FirstName};{x.PhoneNumber};{x.EmailAddress};{x.Sexe}"));
                    Tools.RewriteDataToFile(items, path, false);

                    InitializeLists();
                    TakeOver_Load();
                    dataGridViewAllCustomerRelation_Load(null);
                    dataGridViewStock_Load(null);

                    MessageBox.Show($"Les données de {customer.FirstName} {customer.LastName} ont été mises à jour.");
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
                    x.Name.ToUpper().Contains(searchText))
                .Distinct()
                .ToList();

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
            if (e.RowIndex == 5 && (sender as DataGridView)[e.ColumnIndex, e.RowIndex] is DataGridViewButtonCell)
            {
                Action<Article> clickHandler = (Action<Article>)(sender as DataGridView).Columns[e.ColumnIndex].Tag;
                var article = (Article)(sender as DataGridView).Rows[e.RowIndex].DataBoundItem;

                clickHandler(article);
            }
        }
        private void DataGridViewStockSaveRow_CellContentClick(Article article)
        {
            if (article == null)
            {
                MessageBox.Show("Une erreur est survenue ! Veuillez contacter votre développeur.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(article.Name))
            {
                MessageBox.Show("Le nom de l'article n'est pas renseigné !");
                return;
            }
            else if (article.Price <= 0)
            {
                MessageBox.Show("Le prix de l'article est égale à 0 !");
                return;
            }
            else if (article.Quantity <= 0)
            {
                MessageBox.Show("Le quantité de l'article est égale à 0 !");
                return;
            }

            StockUpdateDatabase(article, dataGridViewStock.CurrentCell.RowIndex);
        }
        private void buttonStockAddArticle_Click(object sender, EventArgs e)
        {
            if (sender == null)
            {
                MessageBox.Show("Une erreur est survenue ! Veuillez contacter votre développeur.");
                return;
            }
            else if (string.IsNullOrWhiteSpace(textBoxStockNewName.Text))
            {
                MessageBox.Show("Le nom de l'article n'est pas renseigné !");
                return;
            }
            else if (numericUpDownStockNewPrice.Value <= 0)
            {
                MessageBox.Show("Le prix de l'article est égale à 0 !");
                return;
            }
            else if (numericUpDownStockNewQuantity.Value <= 0)
            {
                MessageBox.Show("La quantité de l'article est égale à 0 !");
                return;
            }

            Article article = new Article(
                TakeOverTypesDS.Find(x => x.Id == 2).Articles.OrderByDescending(x => x.Id).First().Id + 1,
                comboBoxStockMarque.SelectedValue as int?,
                comboBoxStockModele.SelectedValue as int?,
                textBoxStockNewName.Text,
                (decimal)numericUpDownStockNewPrice.Value,
                (int)numericUpDownStockNewQuantity.Value,
                textBoxStockNewDescription.Text);
            StockUpdateDatabase(article, -1);
        }
        private void DataGridViewStockDeleteRow_CellContentClick(Article article)
        {
            if (article == null)
            {
                MessageBox.Show("Une erreur est survenue ! Veuillez contacter votre développeur.");
                return;
            }

            StockUpdateDatabase(article, -2);
        }
        private void StockUpdateDatabase(Article article, int index)
        {
            string path = Paths.StockDSPath;

            try
            {
                if (!File.Exists(path))
                    throw new Exception($"Le fichier {path} n'existe pas.");

                List<string> data = new List<string>();

                // Ajout
                if (index == -1)
                {
                    Tools.WriteLineTofile($"{article.Id};" +
                            $"{(article.MarqueId.HasValue ? article.MarqueId.Value.ToString() : string.Empty)};" +
                            $"{(article.ModeleId.HasValue ? article.ModeleId.Value.ToString() : string.Empty)};" +
                            $"{article.Name};" +
                            $"{article.Price};" +
                            $"{article.Quantity};" +
                            $"{article.Description}", path, true);
                    MessageBox.Show($"L'article {article.Name} a été sauvegardée.");
                }
                // Suppression
                else if (index == -2)
                {
                    data = Tools.GetDataFromFile(path);
                    Tools.ClearFile(path);
                    int lineIndex = 0;
                    foreach (string line in data)
                    {
                        string[] fields = line.Split(';');
                        if (Int32.Parse(fields[0]) == article.Id)
                        {
                            data.RemoveAt(lineIndex);
                            TakeOverTypesDS.Find(x => x.Id == 2).Articles.Remove(article);
                            MessageBox.Show($"L'article \"{article.Name}\" a été supprimé.");
                        }
                        lineIndex++;
                    }
                    Tools.RewriteDataToFile(data, path, false);
                }
                // Modification
                else if (index >= 0)
                {
                    data = Tools.GetDataFromFile(path);
                    Tools.ClearFile(path);
                    for (int i = 0; i < data.Count; i++)
                    {
                        string[] fields = data[i].Split(';');
                        if (i > 0 && Int32.Parse(fields[0]) == article.Id)
                        {
                            data[i] = $"{article.Id};" +
                                    $"{(article.MarqueId.HasValue ? article.MarqueId.Value.ToString() : string.Empty)};" +
                                    $"{(article.ModeleId.HasValue ? article.ModeleId.Value.ToString() : string.Empty)};" +
                                    $"{article.Name};" +
                                    $"{article.Price};" +
                                    $"{article.Quantity};" +
                                    $"{article.Description}";
                            TakeOverTypesDS.Find(x => x.Id == 2).Articles[i] = article;
                            MessageBox.Show($"Les modifications de l'article {article.Name} ont été sauvegardées.");
                        }
                    }
                    Tools.RewriteDataToFile(data, path, false);
                }

                InitializeLists();
                TakeOver_Load();
                dataGridViewAllCustomerRelation_Load(null);
                dataGridViewStock_Load(null);
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur : " + e.Message);
            }
        }
        #endregion
    }
}
