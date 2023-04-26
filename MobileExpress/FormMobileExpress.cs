using IronPdf.Pdfium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using TextBox = System.Windows.Forms.TextBox;

namespace MobileExpress
{
    public partial class FormMobileExpress : Form
    {
        #region Attributs
        private List<Marque> MarquesDS { get; set; } = new List<Marque>();
        private List<Modele> ModelesDS { get; set; } = new List<Modele>();
        private List<Modele> VisibleModelesDS { get; set; } = new List<Modele>();
        private List<RepairType> RepairTypesDS { get; set; } = new List<RepairType>();
        private List<UnlockType> UnlockTypesDS { get; set; } = new List<UnlockType>();
        private List<PaymentMode> PaymentModesDS { get; set; } = new List<PaymentMode>();
        private List<Invoice> InvoicesDS { get; set; } = new List<Invoice>();
        private List<Article> ArticlesDS { get; set; } = new List<Article>();
        private List<Customer> CustomersDS { get; set; } = new List<Customer>();
        private List<Choice> ChoicesDS { get; set; } = new List<Choice>()
        {
            new Choice() { Id = 0, Name = "Réparation" },
            new Choice() { Id = 1, Name = "Déblockage" },
            new Choice() { Id = 2, Name = "Achat" }
        };

        private Customer CurrentCustomerRelation { get; set; }
        private int InvoiceNumber { get; set; } = 0;

        private Func<string, List<string>> GetDataFromFile = (path) =>
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
        };
        private Func<List<string>, string, bool, bool> RewriteDataToFile = (data, path, toAppend) =>
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
        };
        private Func<string, string, bool, bool> WriteLineTofile = (line, path, toAppend) =>
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(Paths.InvoicesDSPath, append: toAppend, Encoding.UTF8))
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
        };
        private Func<string, bool> ClearFile = (path) =>
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
        };
        #endregion

        public FormMobileExpress()
        {
            InitializeComponent();

            tabControlAll.SelectedIndexChanged += new EventHandler(TabControllAll_SelectedIndexChanged);
            dataGridViewCustomerRelationAll.CellClick += new DataGridViewCellEventHandler(DataGridViewCustomerRelationAll_CellClick);
            dataGridViewCustomerRelationOne.CellClick += new DataGridViewCellEventHandler(DataGridViewCustomerRelationOne_CellClick);
            textBoxCustomerSearchAll.KeyUp += new KeyEventHandler(TextBoxCustomerSearchAll_KeyUp);
            textBoxStockSearch.KeyUp += new KeyEventHandler(TextBoxStockSearch_KeyUp);
            dataGridViewStock.CellContentClick += new DataGridViewCellEventHandler(DataGridViewStock_CellContentClick);

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
            // initialiser historiques
            InitializeInvoices();
            // initialiser clients
            InitializeCustomers();
            // initialiser numéro facture
            InitializeInvoiceNumber();

            InitializeAll();
        }

        private void InitializeAll()
        {
            // initialiser listes déroulantes
            // Prise en charge
            textBoxTakeOverLastName_Load();
            textBoxTakeOverFirstName_Load();
            textBoxTakeOverPhoneNumber_Load();
            textBoxTakeOverEmailAddress_Load();
            invoiceNumberTakeOver_Load();
            dataGridViewTakeOverReceipt_Load(null, null);
            // Relation cliente
            dataGridViewAllCustomerRelation_Load();
            // Stock
            dataGridViewStock_Load();
        }

        #region Initialisation des listes
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
                        if (lineNum > 0)
                        {
                            string[] temp = line.Split(';');
                            MarquesDS.Add(new Marque(Int32.Parse(temp[0]), temp[1]));
                        }
                        else
                        {
                            MarquesDS.Add(new Marque(0, "Select a brand"));
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
                        if (linenum > 0)
                        {
                            string[] temp = line.Split(';');
                            ModelesDS.Add(new Modele(Int32.Parse(temp[0]), Int32.Parse(temp[1]), temp[2]));
                        }
                        else
                        {
                            ModelesDS.Add(new Modele(0, 0, "Sélectionner une modèle"));
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
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (linenum > 0)
                        {
                            string[] temp = line.Split(';');
                            RepairTypesDS.Add(new RepairType(Int32.Parse(temp[0]), temp[1], decimal.Parse(temp[2])));
                        }
                        else
                        {
                            RepairTypesDS.Add(new RepairType(0, "Sélectionner un type de réparation", 0));
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
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (linenum > 0)
                        {
                            string[] temp = line.Split(';');
                            UnlockTypesDS.Add(new UnlockType(Int32.Parse(temp[0]), temp[1], decimal.Parse(temp[2])));
                        }
                        else
                        {
                            UnlockTypesDS.Add(new UnlockType(0, "Sélectionner un type de déblocage", 0));
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
                        if (linenum > 0)
                        {
                            string[] temp = line.Split(';');
                            PaymentModesDS.Add(new PaymentMode(Int32.Parse(temp[0]), temp[1]));
                        }
                        else
                        {
                            PaymentModesDS.Add(new PaymentMode(0, "Sélectionner un mode de paiement"));
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
        private void InitializeInvoices()
        {
            string path = Paths.InvoicesDSPath;

            try
            {
                if (!File.Exists(path))
                    throw new Exception($"Le fichier {path} n'existe pas.");

                if (InvoicesDS == null)
                    InvoicesDS = new List<Invoice>();

                if (InvoicesDS?.Any() ?? false)
                    InvoicesDS.Clear();

                using (StreamReader reader = new StreamReader(path))
                {
                    if (reader == null)
                        throw new Exception($"La lecture du fichier {path} a échoué.");
                    int linenum = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (linenum > 0)
                        {
                            string[] temp = line.Split(';');
                            InvoicesDS.Add(new Invoice(
                                Int32.Parse(temp[0]),
                                DateTime.Parse(temp[1]),
                                Int32.Parse(temp[2]),
                                Tools.ToNullableInt(temp[3]),
                                Tools.ToNullableInt(temp[4]),
                                temp[5],
                                Tools.ToNullableInt(temp[6]),
                                Tools.ToNullableInt(temp[7]),
                                Tools.ToNullableInt(temp[8]),
                                Tools.ToNullableInt(temp[9]),
                                decimal.Parse(temp[10]),
                                Tools.ToNullableDecimal(temp[11]),
                                Tools.ToNullableDecimal(temp[12]),
                                Tools.ToNullableDecimal(temp[13]),
                                Int32.Parse(temp[14])
                            ));
                        }
                        linenum++;
                    }
                    InvoicesDS = InvoicesDS.OrderByDescending(x => x.Date).ToList();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Factures : " + e.Message);
            }
        }
        private void InitializeInvoiceNumber()
        {
            try
            {
                InvoiceNumber = (InvoicesDS?.Any() ?? false) ? (InvoicesDS.OrderByDescending(x => x.Id).First().Id + 1) : 1;
                invoiceNumberTakeOver.Text = InvoiceNumber.ToString();
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

                if (ArticlesDS == null)
                    ArticlesDS = new List<Article>();

                if (ArticlesDS?.Any() ?? false)
                    ArticlesDS.Clear();

                using (StreamReader reader = new StreamReader(path))
                {
                    if (reader == null)
                        throw new Exception($"La lecture du fichier {path} a échoué.");
                    int linenum = 0;
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (linenum > 0)
                        {
                            string[] temp = line.Split(';');
                            Article article = new Article(
                                Int32.Parse(temp[0]),
                                temp[1],
                                decimal.Parse(temp[2]),
                                Int32.Parse(temp[3]),
                                temp[4]);
                            ArticlesDS.Add(article);
                        }
                        linenum++;
                    }
                    ArticlesDS = ArticlesDS.OrderBy(a => a.Name).ToList();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Stock : " + e.Message);
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
                        if (linenum > 0)
                        {
                            //Id;Nom;Prénom;Numéro de téléphone;Adresse mail
                            string[] temp = line.Split(';');
                            Customer customer = new Customer(
                                Int32.Parse(temp[0]),
                                temp[1],
                                temp[2],
                                temp[3],
                                temp[4]);
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
            InitializeInvoices();
            InitializeStock();
        }
        #endregion

        #region Initialisation des onglets
        private void TakeOver_Load()
        {
            textBoxTakeOverLastName_Load();
            textBoxTakeOverFirstName_Load();
            textBoxTakeOverPhoneNumber_Load();
            textBoxTakeOverEmailAddress_Load();
            invoiceNumberTakeOver_Load();
        }
        private void textBoxTakeOverLastName_Load()
        {
            AutoCompleteStringCollection allowedTypes = new AutoCompleteStringCollection();
            allowedTypes.AddRange(CustomersDS.Select(x => x.LastName).ToArray());
            textBoxTakeOverLastName.AutoCompleteCustomSource = allowedTypes;
            textBoxTakeOverLastName.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBoxTakeOverLastName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxTakeOverLastName.KeyUp += new KeyEventHandler(TextBoxTakeOverLastName_KeyUp);
        }
        private void textBoxTakeOverFirstName_Load()
        {
            AutoCompleteStringCollection allowedTypes = new AutoCompleteStringCollection();
            allowedTypes.AddRange(CustomersDS.Select(x => x.FirstName).ToArray());
            textBoxTakeOverFirstName.AutoCompleteCustomSource = allowedTypes;
            textBoxTakeOverFirstName.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBoxTakeOverFirstName.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxTakeOverFirstName.KeyUp += new KeyEventHandler(TextBoxTakeOverFirstName_KeyUp);
        }
        private void textBoxTakeOverPhoneNumber_Load()
        {
            AutoCompleteStringCollection allowedTypes = new AutoCompleteStringCollection();
            allowedTypes.AddRange(CustomersDS.Select(x => x.PhoneNumber).ToArray());
            textBoxTakeOverPhoneNumber.AutoCompleteCustomSource = allowedTypes;
            textBoxTakeOverPhoneNumber.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBoxTakeOverPhoneNumber.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxTakeOverPhoneNumber.KeyUp += new KeyEventHandler(TextBoxTakeOverPhoneNumber_KeyUp);
        }
        private void textBoxTakeOverEmailAddress_Load()
        {
            AutoCompleteStringCollection allowedTypes = new AutoCompleteStringCollection();
            allowedTypes.AddRange(CustomersDS.Select(x => x.EmailAddress).ToArray());
            textBoxTakeOverEmailAddress.AutoCompleteCustomSource = allowedTypes;
            textBoxTakeOverEmailAddress.AutoCompleteMode = AutoCompleteMode.Suggest;
            textBoxTakeOverEmailAddress.AutoCompleteSource = AutoCompleteSource.CustomSource;
            textBoxTakeOverEmailAddress.KeyUp += new KeyEventHandler(TextBoxTakeOverEmailAddress_KeyUp);
        }
        private void invoiceNumberTakeOver_Load()
        {
            invoiceNumberTakeOver.KeyUp += new KeyEventHandler(InvoiceNumberTakeOver_KeyUp);
        }
        /*
             * Peur ? / survie
             * Honte ? / volonté
             * Culpabilité ? / plaisir
             * chagrin ? / amour
             * Mensonge ? / vérité
             * illusion ? / intériorité
             */
        private void dataGridViewTakeOverReceipt_Load(List<Invoice> invoices, int? choiceId)
        {
            dataGridViewTakeOverReceipt.DataSource = null;
            dataGridViewTakeOverReceipt.Rows.Clear();
            dataGridViewTakeOverReceipt.Columns.Clear();

            if (choiceId.HasValue)
            {
                if (choiceId.Value == 0)
                {
                    DataGridViewComboBoxColumn comboBoxRepairColumn = new DataGridViewComboBoxColumn()
                    {
                        HeaderText = "Réparation",
                        Name = "comboBoxTakeOverRepair",
                        MaxDropDownItems = 10,
                        AutoComplete = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        DataSource = RepairTypesDS.OrderBy(x => x.Name).ToList(),
                        DisplayMember = "Name",
                        ValueMember = "Id",
                    };
                    dataGridViewTakeOverReceipt.Columns.Add(comboBoxRepairColumn);

                    DataGridViewComboBoxColumn comboBoxMarqueColumn = new DataGridViewComboBoxColumn()
                    {
                        HeaderText = "Marque",
                        Name = "comboBoxTakeOverMarque",
                        MaxDropDownItems = 10,
                        AutoComplete = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        DataSource = MarquesDS.OrderBy(x => x.Name).ToList(),
                        DisplayMember = "Name",
                        ValueMember = "Id",
                    };
                    dataGridViewTakeOverReceipt.Columns.Add(comboBoxMarqueColumn);

                    DataGridViewComboBoxColumn comboBoxModeleColumn = new DataGridViewComboBoxColumn()
                    {
                        HeaderText = "Modèle",
                        Name = "comboBoxTakeOverModele",
                        MaxDropDownItems = 10,
                        AutoComplete = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        DataSource = ModelesDS.OrderBy(x => x.Name).ToList(),
                        DisplayMember = "Name",
                        ValueMember = "Id",
                    };
                    dataGridViewTakeOverReceipt.Columns.Add(comboBoxModeleColumn);

                    DataGridViewTextBoxColumn textBoxPrice = new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "Prix",
                        Name = "textBoxTakeOverPrice",
                        DataPropertyName = "Price",
                        ValueType = typeof(decimal),
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        ReadOnly = true,
                    };
                    textBoxPrice.DefaultCellStyle.ForeColor = Color.Black;
                    dataGridViewTakeOverReceipt.Columns.Add(textBoxPrice);

                    DataGridViewButtonColumn buttonDeleteRow = new DataGridViewButtonColumn()
                    {
                        HeaderText = string.Empty,
                        Name = "textBoxTakeOverDeleteRow",
                        Text = "Supprimer",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        UseColumnTextForButtonValue = true,
                        Tag = (Action<Invoice>)DataGridViewTakeOverDeleteRow_CellContentClick,
                    };
                    dataGridViewTakeOverReceipt.Columns.Add(buttonDeleteRow);
                }
                if (choiceId.Value == 1)
                {
                    DataGridViewComboBoxColumn comboBoxUnlockColumn = new DataGridViewComboBoxColumn()
                    {
                        HeaderText = "Déblocage",
                        Name = "comboBoxTakeOverUnlock",
                        MaxDropDownItems = 10,
                        AutoComplete = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        DataSource = UnlockTypesDS.OrderBy(x => x.Name).ToList(),
                        DisplayMember = "Name",
                        ValueMember = "Id",
                    };
                    dataGridViewTakeOverReceipt.Columns.Add(comboBoxUnlockColumn);

                    DataGridViewComboBoxColumn comboBoxMarqueColumn = new DataGridViewComboBoxColumn()
                    {
                        HeaderText = "Marque",
                        Name = "comboBoxTakeOverMarque",
                        MaxDropDownItems = 10,
                        AutoComplete = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        DataSource = MarquesDS.OrderBy(x => x.Name).ToList(),
                        DisplayMember = "Name",
                        ValueMember = "Id",
                    };
                    dataGridViewTakeOverReceipt.Columns.Add(comboBoxMarqueColumn);

                    DataGridViewComboBoxColumn comboBoxModeleColumn = new DataGridViewComboBoxColumn()
                    {
                        HeaderText = "Modèle",
                        Name = "comboBoxTakeOverModele",
                        MaxDropDownItems = 10,
                        AutoComplete = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        DataSource = ModelesDS.OrderBy(x => x.Name).ToList(),
                        DisplayMember = "Name",
                        ValueMember = "Id",
                    };
                    dataGridViewTakeOverReceipt.Columns.Add(comboBoxModeleColumn);

                    DataGridViewTextBoxColumn textBoxPrice = new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "Prix",
                        Name = "textBoxTakeOverPrice",
                        DataPropertyName = "Price",
                        ValueType = typeof(decimal),
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        ReadOnly = true,
                    };
                    textBoxPrice.DefaultCellStyle.ForeColor = Color.Black;
                    dataGridViewTakeOverReceipt.Columns.Add(textBoxPrice);

                    DataGridViewButtonColumn buttonDeleteRow = new DataGridViewButtonColumn()
                    {
                        HeaderText = string.Empty,
                        Name = "textBoxTakeOverDeleteRow",
                        Text = "Supprimer",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        UseColumnTextForButtonValue = true,
                        Tag = (Action<Invoice>)DataGridViewTakeOverDeleteRow_CellContentClick,
                    };
                    dataGridViewTakeOverReceipt.Columns.Add(buttonDeleteRow);
                }
                else if (choiceId.Value == 2)
                {
                    DataGridViewComboBoxColumn comboBoxAchatColumn = new DataGridViewComboBoxColumn()
                    {
                        HeaderText = "Article",
                        Name = "comboBoxTakeOverAchat",
                        MaxDropDownItems = 10,
                        AutoComplete = true,
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        DataSource = ArticlesDS.OrderBy(x => x.Name).ToList(),
                        DisplayMember = "Name",
                        ValueMember = "Id",
                    };
                    dataGridViewTakeOverReceipt.Columns.Add(comboBoxAchatColumn);

                    NumericUpDownColumn numericUpDownQuantity = new NumericUpDownColumn()
                    {
                        HeaderText = "Quantité",
                        Name = "textBoxTakeOverQuantity",
                        DataPropertyName = "Quantity",
                        ValueType = typeof(int),
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    };
                    dataGridViewStock.Columns.Add(numericUpDownQuantity);

                    DataGridViewTextBoxColumn textBoxPrice = new DataGridViewTextBoxColumn()
                    {
                        HeaderText = "Prix",
                        Name = "textBoxTakeOverPrice",
                        DataPropertyName = "Price",
                        ValueType = typeof(decimal),
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        ReadOnly = true,
                    };
                    textBoxPrice.DefaultCellStyle.ForeColor = Color.Black;
                    dataGridViewTakeOverReceipt.Columns.Add(textBoxPrice);

                    DataGridViewButtonColumn buttonDeleteRow = new DataGridViewButtonColumn()
                    {
                        HeaderText = string.Empty,
                        Name = "textBoxTakeOverDeleteRow",
                        Text = "Supprimer",
                        AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                        UseColumnTextForButtonValue = true,
                        Tag = (Action<Invoice>)DataGridViewTakeOverDeleteRow_CellContentClick,
                    };
                    dataGridViewTakeOverReceipt.Columns.Add(buttonDeleteRow);
                }
            }
            else
            {
                DataGridViewComboBoxColumn comboBoxChoiceColumn = new DataGridViewComboBoxColumn()
                {
                    HeaderText = "Choix",
                    Name = "comboBoxTakeOverChoice",
                    MaxDropDownItems = 10,
                    AutoComplete = true,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    DataSource = ChoicesDS,
                    DisplayMember = "Name",
                    ValueMember = "Id",
                };
                dataGridViewTakeOverReceipt.Columns.Add(comboBoxChoiceColumn);
            }

            dataGridViewTakeOverReceipt.AutoGenerateColumns = false; // add this line to disable auto-generation of columns
            
            if (invoices?.Any() ?? false)
            {
                dataGridViewTakeOverReceipt.DataSource = new BindingList<Invoice>(invoices);
            }
        }

        private void dataGridViewAllCustomerRelation_Load()
        {
            dataGridViewCustomerRelationAll.DataSource = null;
            dataGridViewCustomerRelationAll.Rows.Clear();
            dataGridViewCustomerRelationAll.Columns.Clear();

            DataGridViewTextBoxColumn textBoxColumnLastName = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Nom",
                Name = "textBoxCustomerRelationLastName",
                DataPropertyName = "LastName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewCustomerRelationAll.Columns.Add(textBoxColumnLastName);
            DataGridViewTextBoxColumn textBoxColumnFirstName = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Prénom",
                Name = "textBoxCustomerRelationFirstName",
                DataPropertyName = "FirstName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewCustomerRelationAll.Columns.Add(textBoxColumnFirstName);
            DataGridViewTextBoxColumn textBoxColumnInvoiceCount = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Nombre de factures",
                Name = "textBoxCustomerRelationInvoiceCount",
                DataPropertyName = "InvoiceCount",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewCustomerRelationAll.Columns.Add(textBoxColumnInvoiceCount);
            DataGridViewTextBoxColumn textBoxColumnInvoiceLastDate = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Dernière facture",
                Name = "textBoxCustomerRelationInvoiceLastDate",
                DataPropertyName = "LastInvoiceDate",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewCustomerRelationAll.Columns.Add(textBoxColumnInvoiceLastDate);

            List<HistoriqueAllView> historiqueAllViews = new List<HistoriqueAllView>();
            foreach (var customer in CustomersDS)
            {
                var tmp = new HistoriqueAllView()
                {
                    LastName = customer.LastName,
                    FirstName = customer.FirstName,
                    InvoiceCount = 0,
                };
                var invoices = InvoicesDS.Where(x => x.CustomerId == customer.Id);
                if (invoices?.Any() ?? false)
                {
                    tmp.LastInvoiceDate = invoices.OrderByDescending(x => x.Date).First().Date;
                    tmp.InvoiceCount = invoices.Count();
                }
                historiqueAllViews.Add(tmp);
            }

            dataGridViewCustomerRelationAll.DataSource = new BindingList<HistoriqueAllView>(
                historiqueAllViews
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .ToList());
        }
        private void dataGridViewOneCustomerRelation_Load(Customer customer)
        {
            List<HistoriqueOneView> historiqueOneViews = InvoicesDS
                .Where(x => x.CustomerId == customer.Id)
                .Select(x =>
                {
                    string invoiceType =
                        x.ArticleId != null ? "Achat" :
                        x.RepairTypeId != null ? "Réparation" :
                        x.UnlockTypeId != null ? "Déblocage" :
                        string.Empty;
                    bool paid = x.Accompte != null ? x.Price == x.Accompte + x.Paid : true;

                    return new HistoriqueOneView()
                    {
                        InvoiceNumber = x.Id,
                        InvoiceType = invoiceType,
                        Date = x.Date.ToString("dd/MM/yyyy"),
                        Price = x.Price,
                        Paid = paid,
                        PaymentMode = PaymentModesDS.FirstOrDefault(p => p.Id == x.PaymentModeId).Name,
                    };
                }).OrderByDescending(x => x.InvoiceNumber)
                .ToList();

            dataGridViewCustomerRelationOne.DataSource = null;
            dataGridViewCustomerRelationOne.Rows.Clear();
            dataGridViewCustomerRelationOne.Columns.Clear();

            DataGridViewTextBoxColumn textBoxColumnInvoiceNumber = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Numéro de facture",
                Name = "textBoxOneInvoiceNumber",
                DataPropertyName = "InvoiceNumber",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewCustomerRelationOne.Columns.Add(textBoxColumnInvoiceNumber);
            DataGridViewTextBoxColumn textBoxColumnDate = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Date",
                Name = "textBoxOneDate",
                DataPropertyName = "Date",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewCustomerRelationOne.Columns.Add(textBoxColumnDate);
            DataGridViewTextBoxColumn textBoxColumnInvoiceType = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Type de facture",
                Name = "textBoxOneInvoiceType",
                DataPropertyName = "InvoiceType",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewCustomerRelationOne.Columns.Add(textBoxColumnInvoiceType);
            DataGridViewTextBoxColumn textBoxColumnPrice = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Prix",
                Name = "textBoxOnePrice",
                DataPropertyName = "Price",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewCustomerRelationOne.Columns.Add(textBoxColumnPrice);
            DataGridViewTextBoxColumn textBoxColumnIsPaid = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Payé",
                Name = "textBoxOneIsPaid",
                DataPropertyName = "Paid",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewCustomerRelationOne.Columns.Add(textBoxColumnIsPaid);
            DataGridViewTextBoxColumn textBoxColumnPaymentMode = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Mode de paiement",
                Name = "textBoxOnePaymentMode",
                DataPropertyName = "PaymentMode",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewCustomerRelationOne.Columns.Add(textBoxColumnPaymentMode);

            dataGridViewCustomerRelationOne.DataSource = new BindingList<HistoriqueOneView>(historiqueOneViews);
        }
        private void dataGridViewOneCustomerRelationInvoice_Load(Invoice invoice)
        {
            if (invoice == null)
            {
                MessageBox.Show("Impossible d'afficher les informations de la facture. Contacter votre éditeur.");
            }

            string invoiceType = invoice.Quantity != null && invoice.Accompte != null ?
                "Achat" :
                RepairTypesDS.FirstOrDefault(x => x.Id == (invoice.RepairTypeId ?? 0))?.Name ??
                    UnlockTypesDS.FirstOrDefault(x => x.Id == (invoice.UnlockTypeId ?? 0))?.Name;

            labelCustomerOneInvoiceNumber.Text = invoice.Id.ToString();
            labelCustomerOneInvoicePrice.Text = invoice.Price.ToString();
            labelCustomerOneInvoiceType.Text = invoiceType;
            labelCustomerOneInvoiceDate.Text = invoice.Date.ToString("dd/MM/yyyy");
        }

        private void dataGridViewStock_Load()
        {
            dataGridViewStock.DataSource = null;
            dataGridViewStock.Rows.Clear();
            dataGridViewStock.Columns.Clear();

            // Id; Nom; Prix; Quantité; Description
            DataGridViewTextBoxColumn textBoxId = new DataGridViewTextBoxColumn()
            {
                Name = "textBoxStockId",
                DataPropertyName = "Id",
                Visible = false,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewStock.Columns.Add(textBoxId);

            DataGridViewTextBoxColumn textBoxName = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Nom",
                Name = "textBoxStockName",
                DataPropertyName = "Name",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewStock.Columns.Add(textBoxName);

            NumericUpDownColumn numericUpDownPrice = new NumericUpDownColumn()
            {
                HeaderText = "Prix",
                Name = "textBoxStockPrice",
                DataPropertyName = "Price",
                ValueType = typeof(decimal),
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewStock.Columns.Add(numericUpDownPrice);

            NumericUpDownColumn numericUpDownQuantity = new NumericUpDownColumn()
            {
                HeaderText = "Quantité",
                Name = "textBoxStockQuantity",
                DataPropertyName = "Quantity",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            };
            dataGridViewStock.Columns.Add(numericUpDownQuantity);

            DataGridViewTextBoxColumn textBoxDescription = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Description",
                Name = "textBoxStockDescription",
                DataPropertyName = "Description",
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

            dataGridViewStock.DataSource = new BindingList<Article>(ArticlesDS);
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
        private void TextBoxTakeOverLastName_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Customer customer = CustomersDS.FirstOrDefault(x => string.Compare(x.LastName, textBox.Text) == 0);
            if (customer != null)
            {
                textBoxTakeOverLastName.Text = customer.LastName;
                textBoxTakeOverFirstName.Text = customer.FirstName;
                textBoxTakeOverPhoneNumber.Text = customer.PhoneNumber;
                textBoxTakeOverEmailAddress.Text = customer.EmailAddress;
            }
        }
        private void TextBoxTakeOverFirstName_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Customer customer = CustomersDS.FirstOrDefault(x => string.Compare(x.FirstName, textBox.Text) == 0);
            if (customer != null)
            {
                textBoxTakeOverLastName.Text = customer.LastName;
                textBoxTakeOverFirstName.Text = customer.FirstName;
                textBoxTakeOverPhoneNumber.Text = customer.PhoneNumber;
                textBoxTakeOverEmailAddress.Text = customer.EmailAddress;
            }
        }
        private void TextBoxTakeOverPhoneNumber_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Customer customer = CustomersDS.FirstOrDefault(x => string.Compare(x.PhoneNumber, textBox.Text) == 0);
            if (customer != null)
            {
                textBoxTakeOverLastName.Text = customer.LastName;
                textBoxTakeOverFirstName.Text = customer.FirstName;
                textBoxTakeOverPhoneNumber.Text = customer.PhoneNumber;
                textBoxTakeOverEmailAddress.Text = customer.EmailAddress;
            }
        }
        private void TextBoxTakeOverEmailAddress_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            Customer customer = CustomersDS.FirstOrDefault(x => string.Compare(x.EmailAddress, textBox.Text) == 0);
            if (customer != null)
            {
                textBoxTakeOverLastName.Text = customer.LastName;
                textBoxTakeOverFirstName.Text = customer.FirstName;
                textBoxTakeOverPhoneNumber.Text = customer.PhoneNumber;
                textBoxTakeOverEmailAddress.Text = customer.EmailAddress;
            }
        }
        private void TextBoxTakeOverAccompte_TextChanged(object sender, EventArgs e)
        {
            decimal price = 0;
            foreach (var r in dataGridViewTakeOverReceipt.Rows.OfType<DataGridViewRow>())
            {
                if (r?.Cells[4]?.Value != null)
                {
                    price += (r.Cells[4].Value as decimal?).Value;
                }
            }
            decimal accompte = decimal.Parse(string.IsNullOrWhiteSpace(textBoxTakeOverAccompte.Text) ? "0" : textBoxTakeOverAccompte.Text);
            decimal paid = decimal.Parse(string.IsNullOrWhiteSpace(textBoxTakeOverPaid.Text) ? "0" : textBoxTakeOverPaid.Text);

            textBoxTakeOverResteDu.Text = (price - paid - accompte).ToString();
        }
        private void TextBoxTakeOverPaid_TextChanged(object sender, EventArgs e)
        {
            decimal price = 0;
            foreach (var r in dataGridViewTakeOverReceipt.Rows.OfType<DataGridViewRow>())
            {
                if (r?.Cells[4]?.Value != null)
                {
                    price += (r.Cells[4].Value as decimal?).Value;
                }
            }
            decimal accompte = decimal.Parse(string.IsNullOrWhiteSpace(textBoxTakeOverAccompte.Text) ? "0" : textBoxTakeOverAccompte.Text);
            decimal paid = decimal.Parse(string.IsNullOrWhiteSpace(textBoxTakeOverPaid.Text) ? "0" : textBoxTakeOverPaid.Text);

            textBoxTakeOverResteDu.Text = (price - paid - accompte).ToString();
        }
        private void InvoiceNumberTakeOver_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            List<Invoice> invoices = InvoicesDS
                .Where(x => string.Compare(x.Id.ToString(), textBox.Text) == 0)
                .ToList();
            if (invoices?.Any() ?? false)
            {
                dataGridViewTakeOverReceipt_Load(invoices, null);
            }
        }
        private void DataGridViewTakeOverReceipt_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridViewTakeOverReceipt.IsCurrentCellDirty)
            {
                dataGridViewTakeOverReceipt.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }
        private void DataGridViewTakeOverReceipt_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && (e.ColumnIndex == dataGridViewTakeOverReceipt.Columns["comboBoxTakeOverRepair"].Index ||
                                    e.ColumnIndex == dataGridViewTakeOverReceipt.Columns["comboBoxTakeOverUnlock"].Index))
            {
                DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)dataGridViewTakeOverReceipt.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (comboBoxCell.Value != null)
                {
                    int? repairTypeId = dataGridViewTakeOverReceipt["comboBoxTakeOverRepair", e.RowIndex].Value as int?;
                    int? unlockTypeId = dataGridViewTakeOverReceipt["comboBoxTakeOverUnlock", e.RowIndex].Value as int?;
                    RepairType repairType = RepairTypesDS.FirstOrDefault(x => x.Id == repairTypeId);
                    UnlockType unlockType = UnlockTypesDS.FirstOrDefault(x => x.Id == unlockTypeId);

                    if (e.ColumnIndex == dataGridViewTakeOverReceipt.Columns["comboBoxTakeOverRepair"].Index && repairType != null)
                    {
                        dataGridViewTakeOverReceipt["comboBoxTakeOverUnlock", e.RowIndex].Value = null;
                        dataGridViewTakeOverReceipt["textBoxTakeOverPrice", e.RowIndex].Value = repairType.Price;
                    }
                    else if (e.ColumnIndex == dataGridViewTakeOverReceipt.Columns["comboBoxTakeOverUnlock"].Index && unlockType != null)
                    {
                        dataGridViewTakeOverReceipt["comboBoxTakeOverRepair", e.RowIndex].Value = null;
                        dataGridViewTakeOverReceipt["textBoxTakeOverPrice", e.RowIndex].Value = unlockType.Price;
                    }

                    UpdateTotalPrice();
                }
            }
            else if (e.ColumnIndex == dataGridViewTakeOverReceipt.Columns["comboBoxTakeOverMarque"].Index)
            {
                int? marqueId = dataGridViewTakeOverReceipt.Rows[e.RowIndex].Cells["comboBoxTakeOverMarque"].Value as int?;
                Marque selectedMarque = marqueId == null ? dataGridViewTakeOverReceipt.Rows[e.RowIndex].Cells["comboBoxTakeOverMarque"].Value as Marque : MarquesDS.First(x => x.Id == marqueId.Value);

                if (selectedMarque != null && selectedMarque.Id != 0)
                {
                    VisibleModelesDS.Clear();
                    VisibleModelesDS = null;
                    VisibleModelesDS = new List<Modele>();
                    VisibleModelesDS.Add(new Modele(0, 0, string.Empty));
                    VisibleModelesDS.AddRange(ModelesDS.Where(x => x.MarqueId == selectedMarque.Id));
                    (dataGridViewTakeOverReceipt.Rows[e.RowIndex].Cells["comboBoxTakeOverModele"] as DataGridViewComboBoxCell).DataSource = VisibleModelesDS;
                }
            }
            else if (e.ColumnIndex == dataGridViewTakeOverReceipt.Columns["comboBoxTakeOverChoice"].Index)
            {
                int? choiceId = dataGridViewTakeOverReceipt["comboBoxTakeOverChoice", e.RowIndex].Value as int?;

                if (choiceId.HasValue)
                {
                    if (choiceId.Value == 0)
                    {

                    }
                    else if (choiceId.Value == 1)
                    {

                    }
                    else if (choiceId.Value == 2)
                    {

                    }
                }
            }
        }
        private void UpdateTotalPrice()
        {
            decimal totalPrice = 0;
            foreach (DataGridViewRow row in dataGridViewTakeOverReceipt.Rows)
            {
                decimal? price = row.Cells["textBoxTakeOverPrice"].Value as decimal?;
                if (price != null)
                {
                    totalPrice += price.Value;
                }
            }

            textBoxTakeOverTotalPrice.Text = totalPrice.ToString();
            textBoxTakeOverResteDu.Text = textBoxTakeOverTotalPrice.Text;
            TextBoxTakeOverAccompte_TextChanged(null, new EventArgs());
            TextBoxTakeOverPaid_TextChanged(null, new EventArgs());
        }
        private void dataGridViewTakeOverReceipt_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            e.Cancel = true;
        }
        private void DataGridViewTakeOverDeleteRow_CellContentClick(Invoice invoice)
        {
            if (invoice == null)
            {
                MessageBox.Show("Une erreur est survenue ! Veuillez contacter votre développeur.");
                return;
            }

            if (this.dataGridViewTakeOverReceipt.SelectedRows.Count > 0)
            {
                this.dataGridViewTakeOverReceipt.Rows.RemoveAt(this.dataGridViewTakeOverReceipt.SelectedRows[0].Index);
            }
        }

        //private void comboBoxUnlockMarque_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Marque selectedMarque = comboBoxUnlockMarque.SelectedItem as Marque;
        //    if (selectedMarque != null)
        //    {
        //        VisibleModelesDS = ModelesDS.Where(x => x.MarqueId == selectedMarque.Id).ToList();
        //        comboBoxUnlockModele.DataSource = VisibleModelesDS;
        //    }
        //}
        //private void comboBoxUnlockModele_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    Modele selectedModele = comboBoxUnlockModele.SelectedItem as Modele;
        //    if (selectedModele != null)
        //    {
        //        comboBoxUnlockMarque.SelectedItem = MarquesDS.First(x => x.Id == selectedModele.MarqueId);
        //    }
        //}
        //private void comboBoxUnlockType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    UnlockType selectedType = comboBoxUnlockType.SelectedItem as UnlockType;
        //    if (selectedType != null)
        //    {
        //        textBoxUnlockPrice.Text = UnlockTypesDS.First(x => x.Id == selectedType.Id).Price.ToString();
        //        textBoxUnlockResteDu.Text = textBoxUnlockPrice.Text;
        //    }
        //}
        //private void textBoxUnlockAccompte_TextChanged(object sender, EventArgs e)
        //{
        //    decimal price = Int32.Parse(string.IsNullOrWhiteSpace(textBoxUnlockPrice.Text) ? "0" : textBoxUnlockPrice.Text);
        //    decimal accompte = Int32.Parse(string.IsNullOrWhiteSpace(textBoxUnlockAccompte.Text) ? "0" : textBoxUnlockAccompte.Text);
        //    decimal paid = Int32.Parse(string.IsNullOrWhiteSpace(textBoxUnlockPaid.Text) ? "0" : textBoxUnlockPaid.Text);

        //    textBoxUnlockResteDu.Text = (price - paid - accompte).ToString();
        //}
        //private void textBoxUnlockPaid_TextChanged(object sender, EventArgs e)
        //{
        //    decimal price = Int32.Parse(string.IsNullOrWhiteSpace(textBoxUnlockPrice.Text) ? "0" : textBoxUnlockPrice.Text);
        //    decimal accompte = Int32.Parse(string.IsNullOrWhiteSpace(textBoxUnlockAccompte.Text) ? "0" : textBoxUnlockAccompte.Text);
        //    decimal paid = Int32.Parse(string.IsNullOrWhiteSpace(textBoxUnlockPaid.Text) ? "0" : textBoxUnlockPaid.Text);

        //    textBoxUnlockResteDu.Text = (price - paid - accompte).ToString();
        //}
        //private void TextBoxUnlockLastName_KeyUp(object sender, KeyEventArgs e)
        //{
        //    TextBox textBox = sender as TextBox;
        //    Customer customer = CustomersDS.FirstOrDefault(x => string.Compare(x.LastName, textBox.Text) == 0);
        //    if (customer != null)
        //    {
        //        textBoxUnlockLastName.Text = customer.LastName;
        //        textBoxUnlockFirstName.Text = customer.FirstName;
        //        textBoxUnlockPhoneNumber.Text = customer.PhoneNumber;
        //        textBoxUnlockEmailAddress.Text = customer.EmailAddress;
        //    }
        //}
        //private void TextBoxUnlockFirstName_KeyUp(object sender, KeyEventArgs e)
        //{
        //    TextBox textBox = sender as TextBox;
        //    Customer customer = CustomersDS.FirstOrDefault(x => string.Compare($"{x.FirstName} {x.LastName}", textBox.Text) == 0);
        //    if (customer != null)
        //    {
        //        textBoxUnlockLastName.Text = customer.LastName;
        //        textBoxUnlockFirstName.Text = customer.FirstName;
        //        textBoxUnlockPhoneNumber.Text = customer.PhoneNumber;
        //        textBoxUnlockEmailAddress.Text = customer.EmailAddress;
        //    }
        //}
        //private void TextBoxUnlockPhoneNumber_KeyUp(object sender, KeyEventArgs e)
        //{
        //    TextBox textBox = sender as TextBox;
        //    Customer customer = CustomersDS.FirstOrDefault(x => string.Compare(x.PhoneNumber, textBox.Text) == 0);
        //    if (customer != null)
        //    {
        //        textBoxUnlockLastName.Text = customer.LastName;
        //        textBoxUnlockFirstName.Text = customer.FirstName;
        //        textBoxUnlockPhoneNumber.Text = customer.PhoneNumber;
        //        textBoxUnlockEmailAddress.Text = customer.EmailAddress;
        //    }
        //}
        //private void TextBoxUnlockEmailAddress_KeyUp(object sender, KeyEventArgs e)
        //{
        //    TextBox textBox = sender as TextBox;
        //    Customer customer = CustomersDS.FirstOrDefault(x => string.Compare(x.EmailAddress, textBox.Text) == 0);
        //    if (customer != null)
        //    {
        //        textBoxUnlockLastName.Text = customer.LastName;
        //        textBoxUnlockFirstName.Text = customer.FirstName;
        //        textBoxUnlockPhoneNumber.Text = customer.PhoneNumber;
        //        textBoxUnlockEmailAddress.Text = customer.EmailAddress;
        //    }
        //}
        //private void unlockPrintButton_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Customer customer = CustomersDS.FirstOrDefault(x =>
        //            string.Compare(x.LastName, textBoxUnlockLastName.Text) == 0 &&
        //            string.Compare(x.FirstName, textBoxUnlockFirstName.Text) == 0);
        //        if (customer == null)
        //        {
        //            customer = new Customer(
        //                CustomersDS
        //                    .OrderByDescending(x => x.Id)
        //                    .FirstOrDefault()?.Id + 1 ?? 1,
        //                textBoxUnlockLastName.Text,
        //                textBoxUnlockFirstName.Text,
        //                textBoxUnlockPhoneNumber.Text,
        //                textBoxUnlockEmailAddress.Text);
        //            CustomersDS.Add(customer);
        //            var temp = GetDataFromFile(Paths.CustomersDSPath);
        //            temp.Add($"{customer.Id};{customer.LastName};{customer.FirstName};{customer.PhoneNumber};{customer.EmailAddress}");
        //            ClearFile(Paths.CustomersDSPath);
        //            RewriteDataToFile(temp, Paths.CustomersDSPath, false);
        //            MessageBox.Show($"Le nouveau client {customer.FirstName} {customer.LastName} a été sauvegardée.");
        //        }

        //        if (string.IsNullOrWhiteSpace((this.comboBoxUnlockMarque.SelectedItem as Marque)?.Name) ||
        //            string.IsNullOrWhiteSpace((this.comboBoxUnlockModele.SelectedItem as Modele)?.Name) ||
        //            string.IsNullOrWhiteSpace((this.comboBoxUnlockType.SelectedItem as RepairType)?.Name) ||
        //            string.IsNullOrWhiteSpace((this.comboBoxUnlockPaymentMode.SelectedItem as PaymentMode)?.Name))
        //        {
        //            MessageBox.Show("Veuillez renseigner toutes les informations...");
        //            return;
        //        }

        //        Marque marque = MarquesDS
        //            .Where(x => x.Id != 0)
        //            .First(x => x.Name == (this.comboBoxUnlockMarque.SelectedItem as Marque)?.Name);

        //        Modele modele = ModelesDS
        //            .Where(x => x.Id != 0)
        //            .First(x => x.Name == (this.comboBoxUnlockModele.SelectedItem as Modele)?.Name);

        //        UnlockType unlockType = UnlockTypesDS
        //            .Where(x => x.Id != 0)
        //            .First(x => x.Name == (this.comboBoxUnlockType.SelectedItem as UnlockType)?.Name);

        //        PaymentMode paymentMode = PaymentModesDS
        //            .Where(x => x.Id != 0)
        //            .First(x => x.Name == (this.comboBoxUnlockPaymentMode.SelectedItem as PaymentMode)?.Name);

        //        Invoice newLine = new Invoice(
        //            Int32.Parse(invoiceNumberUnlock.Text),
        //            DateTime.Parse(dateTimePickerUnlock.Text),
        //            customer.Id,
        //            marque.Id,
        //            modele.Id,
        //            textBoxUnlockIMEI.Text,
        //            null, unlockType.Id,
        //            null,
        //            null,
        //            decimal.Parse(textBoxUnlockPrice.Text == string.Empty ? "0" : textBoxUnlockPrice.Text),
        //            decimal.Parse(textBoxUnlockAccompte.Text == string.Empty ? "0" : textBoxUnlockAccompte.Text),
        //            decimal.Parse(textBoxUnlockResteDu.Text == string.Empty ? "0" : textBoxUnlockResteDu.Text),
        //            decimal.Parse(textBoxUnlockPaid.Text == string.Empty ? "0" : textBoxUnlockPaid.Text),
        //            paymentMode.Id);
        //        InvoicesDS.Add(newLine);
        //        WriteLineTofile($"{newLine.Id};" +
        //                $"{newLine.Date};" +
        //                $"{newLine.CustomerId};" +
        //                $"{newLine.MarqueId};" +
        //                $"{newLine.ModeleId};" +
        //                $"{newLine.IMEI};" +
        //                $";{newLine.UnlockTypeId};" +
        //                $";;" +
        //                $"{newLine.Price};" +
        //                $"{newLine.Accompte};" +
        //                $"{newLine.ResteDu};" +
        //                $"{newLine.Paid};" +
        //                $"{newLine.PaymentModeId};", Paths.InvoicesDSPath, true);
        //        MessageBox.Show($"La facture n°{invoiceNumberUnlock.Text} a été sauvegardée.");

        //        #region Nettoyage du formulaire
        //        textBoxUnlockLastName.Text = string.Empty;
        //        textBoxUnlockFirstName.Text = string.Empty;
        //        textBoxUnlockPhoneNumber.Text = string.Empty;
        //        textBoxUnlockEmailAddress.Text = string.Empty;
        //        comboBoxUnlockMarque.SelectedItem = null;
        //        comboBoxUnlockModele.SelectedItem = null;
        //        textBoxUnlockIMEI.Text = string.Empty;
        //        comboBoxUnlockType.SelectedItem = null;
        //        textBoxUnlockPrice.Text = string.Empty;
        //        textBoxUnlockAccompte.Text = string.Empty;
        //        textBoxUnlockResteDu.Text = string.Empty;
        //        textBoxUnlockPaid.Text = string.Empty;
        //        comboBoxUnlockPaymentMode.SelectedItem = null;
        //        InvoiceNumber++;
        //        invoiceNumberRepair.Text = InvoiceNumber.ToString();
        //        invoiceNumberUnlock.Text = InvoiceNumber.ToString();
        //        invoiceNumberInvoice.Text = InvoiceNumber.ToString();
        //        #endregion

        //        #region Génération PDF
        //        Tools.GeneratePdfFromHtml(
        //            Paths.LogoPath,
        //            customer.FirstName + " " + customer.LastName,
        //            newLine.Date.ToString("dd/MM/yyyy"),
        //            newLine.Id.ToString(),
        //            newLine.Price.ToString("F0"),
        //            "0123456789",
        //            "12 Rue de Saint-Cyr, Route de Montabo, 97300 Cayenne",
        //            "+594 (0)694 09 19 94",
        //            String.Empty,
        //            String.Empty,
        //            new List<Article>()
        //            {
        //                new Article(-1, unlockType.Name, unlockType.Price, 1, string.Empty),
        //            },
        //            Paths.PdfInvoicesDSPath + $"{newLine.Date.ToString("ddMMyyyy")}_Facture n°{newLine.Id}.pdf");
        //        #endregion
        //    }
        //    catch (Exception exception)
        //    {
        //        MessageBox.Show(exception.Message);
        //    }
        //}

        //private void DataGridViewInvoice_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        //{
        //    if (dataGridViewInvoice.IsCurrentCellDirty)
        //    {
        //        dataGridViewInvoice.CommitEdit(DataGridViewDataErrorContexts.Commit);
        //    }
        //}
        //private void DataGridViewInvoice_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == dataGridViewInvoice.Columns["comboBoxInvoiceArticle"].Index)
        //    {
        //        DataGridViewComboBoxCell comboBoxCell = (DataGridViewComboBoxCell)dataGridViewInvoice.Rows[e.RowIndex].Cells[0];
        //        if (comboBoxCell.Value != null)
        //        {
        //            Article article = ArticlesDS
        //                .First(x => x.Id == (dataGridViewInvoice["comboBoxInvoiceArticle", dataGridViewInvoice.CurrentCell.RowIndex].Value as int?));

        //            dataGridViewInvoice["numericUpDownInvoicePrice", dataGridViewInvoice.CurrentCell.RowIndex].Value = article.Price;
        //            dataGridViewInvoice["numericUpDownInvoiceQuantity", dataGridViewInvoice.CurrentCell.RowIndex].Value = 1;
        //            NumericUpDownCell numericUpDown = (NumericUpDownCell)dataGridViewInvoice.Rows[e.RowIndex].Cells[1];
        //            DataGridViewNumericUpDownInvoice_ValueChanged(this, new EventArgs());
        //        }
        //    }
        //    else if (e.ColumnIndex == dataGridViewInvoice.Columns["numericUpDownInvoiceQuantity"].Index)
        //    {
        //        NumericUpDownCell numericUpDown = (NumericUpDownCell)dataGridViewInvoice.Rows[e.RowIndex].Cells[1];
        //        if (numericUpDown != null)
        //        {
        //            DataGridViewNumericUpDownInvoice_ValueChanged(this, new EventArgs());
        //        }
        //    }
        //}
        //private void DataGridViewNumericUpDownInvoice_ValueChanged(object sender, EventArgs e)
        //{
        //    if (sender != null && dataGridViewInvoice["comboBoxInvoiceArticle", dataGridViewInvoice.CurrentCell.RowIndex].Value != null)
        //    {
        //        Article article = ArticlesDS
        //            .First(x => x.Id == (dataGridViewInvoice["comboBoxInvoiceArticle", dataGridViewInvoice.CurrentCell.RowIndex].Value as int?));

        //        int currentQuantity = Convert.ToInt32(dataGridViewInvoice.Rows[dataGridViewInvoice.CurrentCell.RowIndex].Cells[1].Value);
        //        int balance = article.Quantity - currentQuantity;
        //        if (balance < 0)
        //        {
        //            NumericUpDownCell numericUpDown = (NumericUpDownCell)dataGridViewInvoice.Rows[dataGridViewInvoice.CurrentCell.RowIndex].Cells[1];
        //            numericUpDown.Value = article.Quantity;
        //            currentQuantity = article.Quantity;
        //            MessageBox.Show($"Il ne reste que {article.Quantity} exemplaire(s) de cet article");
        //            return;
        //        }
        //        else
        //        {
        //            dataGridViewInvoice["numericUpDownInvoicePrice", dataGridViewInvoice.CurrentCell.RowIndex].Value = currentQuantity * article.Price;

        //            decimal totalPrice = 0;
        //            foreach (DataGridViewRow row in dataGridViewInvoice.Rows)
        //            {
        //                totalPrice += (row.Cells["numericUpDownInvoicePrice"].Value as decimal?) ?? 0;
        //            }
        //            textBoxInvoiceTotalPrice.Text = totalPrice.ToString();
        //        }
        //    }
        //}
        //private void TextBoxInvoiceLastName_KeyUp(object sender, KeyEventArgs e)
        //{
        //    TextBox textBox = sender as TextBox;
        //    Customer customer = CustomersDS.FirstOrDefault(x => string.Compare(x.LastName, textBox.Text) == 0);
        //    if (customer != null)
        //    {
        //        textBoxInvoiceLastName.Text = customer.LastName;
        //        textBoxInvoiceFirstName.Text = customer.FirstName;
        //        textBoxInvoicePhoneNumber.Text = customer.PhoneNumber;
        //        textBoxInvoiceEmailAddress.Text = customer.EmailAddress;
        //    }
        //}
        //private void TextBoxInvoiceFirstName_KeyUp(object sender, KeyEventArgs e)
        //{
        //    TextBox textBox = sender as TextBox;
        //    Customer customer = CustomersDS.FirstOrDefault(x => string.Compare(x.FirstName, textBox.Text) == 0);
        //    if (customer != null)
        //    {
        //        textBoxInvoiceLastName.Text = customer.LastName;
        //        textBoxInvoiceFirstName.Text = customer.FirstName;
        //        textBoxInvoicePhoneNumber.Text = customer.PhoneNumber;
        //        textBoxInvoiceEmailAddress.Text = customer.EmailAddress;
        //    }
        //}
        //private void TextBoxInvoicePhoneNumber_KeyUp(object sender, KeyEventArgs e)
        //{
        //    TextBox textBox = sender as TextBox;
        //    Customer customer = CustomersDS.FirstOrDefault(x => string.Compare(x.PhoneNumber, textBox.Text) == 0);
        //    if (customer != null)
        //    {
        //        textBoxInvoiceLastName.Text = customer.LastName;
        //        textBoxInvoiceFirstName.Text = customer.FirstName;
        //        textBoxInvoicePhoneNumber.Text = customer.PhoneNumber;
        //        textBoxInvoiceEmailAddress.Text = customer.EmailAddress;
        //    }
        //}
        //private void TextBoxInvoiceEmailAddress_KeyUp(object sender, KeyEventArgs e)
        //{
        //    TextBox textBox = sender as TextBox;
        //    Customer customer = CustomersDS.FirstOrDefault(x => string.Compare(x.EmailAddress, textBox.Text) == 0);
        //    if (customer != null)
        //    {
        //        textBoxInvoiceLastName.Text = customer.LastName;
        //        textBoxInvoiceFirstName.Text = customer.FirstName;
        //        textBoxInvoicePhoneNumber.Text = customer.PhoneNumber;
        //        textBoxInvoiceEmailAddress.Text = customer.EmailAddress;
        //    }
        //}
        //private void invoicePrintButton_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Customer customer = CustomersDS.FirstOrDefault(x =>
        //            string.Compare(x.LastName, textBoxInvoiceLastName.Text) == 0 &&
        //            string.Compare(x.FirstName, textBoxInvoiceFirstName.Text) == 0);
        //        if (customer == null)
        //        {
        //            customer = new Customer(
        //                CustomersDS
        //                    .OrderByDescending(x => x.Id)
        //                    .FirstOrDefault()?.Id + 1 ?? 1,
        //                textBoxInvoiceLastName.Text,
        //                textBoxInvoiceFirstName.Text,
        //                textBoxInvoicePhoneNumber.Text,
        //                textBoxInvoiceEmailAddress.Text);
        //            CustomersDS.Add(customer);
        //            var temp = GetDataFromFile(Paths.CustomersDSPath);
        //            temp.Add($"{customer.Id};{customer.LastName};{customer.FirstName};{customer.PhoneNumber};{customer.EmailAddress}");
        //            ClearFile(Paths.CustomersDSPath);
        //            RewriteDataToFile(temp, Paths.CustomersDSPath, false);
        //            MessageBox.Show($"Le nouveau client {customer.FirstName} {customer.LastName} a été sauvegardée.");
        //        }

        //        if (string.IsNullOrWhiteSpace((this.comboBoxInvoicePaymentMode.SelectedItem as PaymentMode)?.Name))
        //        {
        //            MessageBox.Show("Veuillez renseigner toutes les informations...");
        //            return;
        //        }

        //        PaymentMode paymentMode = PaymentModesDS
        //            .Where(x => x.Id != 0)
        //            .First(x => x.Name == (this.comboBoxInvoicePaymentMode.SelectedItem as PaymentMode)?.Name);

        //        List<Tuple<Article, int, decimal>> panier = new List<Tuple<Article, int, decimal>>();
        //        List<Tuple<Invoice, Article>> tmp = new List<Tuple<Invoice, Article>>();
        //        foreach (DataGridViewRow row in dataGridViewInvoice.Rows)
        //        {
        //            if (row?.Cells["comboBoxInvoiceArticle"]?.Value != null)
        //            {
        //                int quantity = Convert.ToInt32(row.Cells["numericUpDownInvoiceQuantity"].Value);
        //                Article article = ArticlesDS.First(x => x.Id == (row.Cells["comboBoxInvoiceArticle"].Value as int? ?? 0));
        //                decimal total = row.Cells["numericUpDownInvoicePrice"].Value as decimal? ?? 0;
        //                panier.Add(new Tuple<Article, int, decimal>(article, quantity, article.Price * quantity));

        //                Invoice newLine = new Invoice(
        //                    Int32.Parse(invoiceNumberInvoice.Text),
        //                    DateTime.Parse(dateTimePickerInvoice.Text),
        //                    customer.Id,
        //                    null, null, string.Empty, null, null,
        //                    quantity,
        //                    row.Cells["comboBoxInvoiceArticle"].Value as int? ?? 0,
        //                    total,
        //                    null, null, null,
        //                    paymentMode.Id);
        //                InvoicesDS.Add(newLine);
        //                WriteLineTofile($"{newLine.Id};" +
        //                        $"{newLine.Date};" +
        //                        $"{customer.Id};" +
        //                        $";;;;;" +
        //                        $"{newLine.Quantity};" +
        //                        $"{article.Id};" +
        //                        $"{article.Price * quantity};" +
        //                        $";;;" +
        //                        $"{paymentMode.Id}", Paths.InvoicesDSPath, true);
        //                tmp.Add(new Tuple<Invoice, Article>(newLine, article));
        //            }
        //        }

        //        // mise à jour du stock
        //        string path = Paths.StockDSPath;
        //        List<string> data = GetDataFromFile(path);
        //        ClearFile(path);
        //        panier.ForEach(x =>
        //        {
        //            x.Item1.Quantity -= x.Item2;
        //            for (int i = 0; i < data.Count; i++)
        //            {
        //                string[] fields = data[i].Split(';');
        //                if (i > 0 && Int32.Parse(fields[0]) == x.Item1.Id)
        //                {
        //                    data[i] = $"{x.Item1.Id};" +
        //                            $"{x.Item1.Name};" +
        //                            $"{x.Item1.Price};" +
        //                            $"{x.Item1.Quantity};" +
        //                            $"{x.Item1.Description}";
        //                    ArticlesDS[i - 1] = x.Item1 as Article;
        //                }
        //            }
        //        });
        //        RewriteDataToFile(data, path, false);
        //        MessageBox.Show($"La facture n°{invoiceNumberInvoice.Text} a été sauvegardée.");

        //        #region Nettoyage du formulaire
        //        textBoxInvoiceLastName.Text = string.Empty;
        //        textBoxInvoiceFirstName.Text = string.Empty;
        //        textBoxInvoicePhoneNumber.Text = string.Empty;
        //        textBoxInvoiceEmailAddress.Text = string.Empty;
        //        textBoxInvoiceTotalPrice.Text = string.Empty;
        //        comboBoxInvoicePaymentMode.SelectedItem = null;
        //        dataGridViewInvoice.DataSource = null;
        //        InvoiceNumber++;
        //        invoiceNumberRepair.Text = InvoiceNumber.ToString();
        //        invoiceNumberUnlock.Text = InvoiceNumber.ToString();
        //        invoiceNumberInvoice.Text = InvoiceNumber.ToString();
        //        #endregion

        //        InitializeLists();
        //        TakeOver_Load();
        //        dataGridViewAllCustomerRelation_Load();
        //        dataGridViewStock_Load();

        //        #region Génération PDF
        //        var invoice = tmp.First().Item1;
        //        Tools.GeneratePdfFromHtml(
        //            Paths.LogoPath,
        //            customer.FirstName + " " + customer.LastName,
        //            invoice.Date.ToString("dd/MM/yyyy"),
        //            invoice.Id.ToString(),
        //            tmp.Sum(x => x.Item1.Price).ToString("F0"),
        //            "0123456789",
        //            "12 Rue de Saint-Cyr, Route de Montabo, 97300 Cayenne",
        //            "+594 (0)694 09 19 94",
        //            String.Empty,
        //            String.Empty,
        //            tmp.Select(x => x.Item2).ToList(),
        //            Paths.PdfInvoicesDSPath + $"{invoice.Date.ToString("ddMMyyyy")}_Facture n°{invoice.Id}.pdf");
        //        #endregion
        //    }
        //    catch (Exception exception)
        //    {
        //        MessageBox.Show(exception.Message);
        //    }
        //}

        private void TextBoxCustomerSearchAll_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                dataGridViewCustomerRelationAll.DataSource = null;
                dataGridViewCustomerRelationAll.Rows.Clear();
                dataGridViewCustomerRelationAll.Columns.Clear();

                List<HistoriqueAllView> historiqueAllViews = new List<HistoriqueAllView>();
                foreach (var tmp in InvoicesDS)
                {
                    Customer customer = CustomersDS.First(c => c.Id == tmp.CustomerId);
                    var existingH = historiqueAllViews.FirstOrDefault(x =>
                            x.LastName == customer.LastName &&
                            x.FirstName == customer.FirstName);
                    if (existingH != null)
                    {
                        existingH.InvoiceCount += 1;
                        existingH.LastInvoiceDate = existingH.LastInvoiceDate < tmp.Date ? tmp.Date : existingH.LastInvoiceDate;
                    }
                    else
                    {
                        historiqueAllViews.Add(new HistoriqueAllView()
                        {
                            LastName = customer.LastName,
                            FirstName = customer.FirstName,
                            InvoiceCount = 1,
                            LastInvoiceDate = tmp.Date,
                        });
                    }
                }

                dataGridViewCustomerRelationAll.DataSource = new BindingList<HistoriqueAllView>(
                    historiqueAllViews
                    .OrderBy(x => x.LastName)
                    .ThenBy(x => x.FirstName)
                    .ToList());
            }
            else
            {
                List<Customer> customers = CustomersDS.Where(x => x.LastName.Contains(textBox.Text))?.ToList() ?? new List<Customer>();
                customers.AddRange(CustomersDS.Where(x => x.FirstName.Contains(textBox.Text))?.ToList() ?? new List<Customer>());
                customers.AddRange(CustomersDS.Where(x => x.PhoneNumber.Contains(textBox.Text))?.ToList() ?? new List<Customer>());
                if (customers?.Any() ?? false)
                {
                    dataGridViewCustomerRelationAll.DataSource = null;
                    dataGridViewCustomerRelationAll.Rows.Clear();
                    dataGridViewCustomerRelationAll.Columns.Clear();

                    List<HistoriqueAllView> historiqueAllViews = new List<HistoriqueAllView>();
                    foreach (var tmp in InvoicesDS)
                    {
                        Customer customer = customers.First(c => c.Id == tmp.CustomerId);
                        var existingH = historiqueAllViews.FirstOrDefault(x =>
                                x.LastName == customer.LastName &&
                                x.FirstName == customer.FirstName);
                        if (existingH != null)
                        {
                            existingH.InvoiceCount += 1;
                            existingH.LastInvoiceDate = existingH.LastInvoiceDate < tmp.Date ? tmp.Date : existingH.LastInvoiceDate;
                        }
                        else
                        {
                            historiqueAllViews.Add(new HistoriqueAllView()
                            {
                                LastName = customer.LastName,
                                FirstName = customer.FirstName,
                                InvoiceCount = 1,
                                LastInvoiceDate = tmp.Date,
                            });
                        }
                    }

                    dataGridViewCustomerRelationAll.DataSource = new BindingList<HistoriqueAllView>(
                        historiqueAllViews
                        .OrderBy(x => x.LastName)
                        .ThenBy(x => x.FirstName)
                        .ToList());
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
                    Id = CurrentCustomerRelation.Id,
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
            var customer = CustomersDS.First(x =>
                string.Compare(x.LastName, (sender as DataGridView).Rows[e.RowIndex].Cells[0].Value as string) == 0 &&
                string.Compare(x.FirstName, (sender as DataGridView).Rows[e.RowIndex].Cells[1].Value as string) == 0);

            CurrentCustomerRelation = customer;

            textBoxCustomerRelationLastName.Text = customer.LastName;
            textBoxCustomerRelationFirstName.Text = customer.FirstName;
            textBoxCustomerRelationPhone.Text = customer.PhoneNumber;
            textBoxCustomerRelationEmailAddress.Text = customer.EmailAddress;
            labelCustomerRelationFullName.Text = customer.FirstName + " " + customer.LastName;

            dataGridViewOneCustomerRelation_Load(customer);
        }
        private void DataGridViewCustomerRelationOne_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var invoice = InvoicesDS.First(x => x.Id == ((sender as DataGridView).Rows[e.RowIndex].Cells[0].Value as int? ?? 0));
                var customer = CustomersDS.First(x => x.Id == invoice.CustomerId);

                dataGridViewOneCustomerRelationInvoice_Load(invoice);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenu ! " + ex.Message);
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
                    ClearFile(path);
                    RewriteDataToFile(new List<string>() { "Id;Nom;Prénom;Numéro de téléphone;Adresse mail" }, path, false);
                    RewriteDataToFile(CustomersDS.Select(x => $"{x.Id};{x.LastName};{x.FirstName};{x.PhoneNumber};{x.EmailAddress}").ToList(), path, true);

                    InitializeLists();
                    TakeOver_Load();
                    dataGridViewAllCustomerRelation_Load();
                    dataGridViewStock_Load();

                    MessageBox.Show($"Les données de {customer.FirstName} {customer.LastName} ont été mises à jour.");
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur : " + e.Message);
            }
        }
        private void buttonCustomerRelationGetInvoicePath_Click(object sender, EventArgs e)
        {
            Invoice invoice = InvoicesDS.First(x => x.Id == Int32.Parse(labelCustomerOneInvoiceNumber.Text));
            MessageBox.Show(Paths.PdfInvoicesDSPath + $"{invoice.Date.ToString("ddMMyyyy")}_Facture n°{invoice.Id}.pdf");
        }
        private void buttonCustomerRelationPrintInvoice_Click(object sender, EventArgs e)
        {
            printPreviewDialogCustomerOneInvoice.Document = printDocumentCustomerOneInvoice;
            ToolStripButton b = new ToolStripButton();
            b.DisplayStyle = ToolStripItemDisplayStyle.Image;
            b.Click += buttonCustomerRelationPrintInvoice_PrintClick;
            ((ToolStrip)(printPreviewDialogCustomerOneInvoice.Controls[1])).Items.RemoveAt(0);
            ((ToolStrip)(printPreviewDialogCustomerOneInvoice.Controls[1])).Items.Insert(0, b);
            printPreviewDialogCustomerOneInvoice.ShowDialog();
        }
        private void buttonCustomerRelationPrintInvoice_PrintClick(object sender, EventArgs e)
        {
            try
            {
                Invoice invoice = InvoicesDS.First(x => x.Id == Int32.Parse(labelCustomerOneInvoiceNumber.Text));

                var path = Paths.PdfInvoicesDSPath;
                var fileName = $"{invoice.Date.ToString("ddMMyyyy")}_Facture n°{invoice.Id}.pdf";
                var fullPath = path + fileName;
                using (var document = PdfDocument.Load(fullPath))
                {
                    using (var printDocument = document.CreatePrintDocument())
                    {
                        printDocument.PrinterSettings.PrintFileName = fileName;
                        printDocument.PrinterSettings.PrinterName = @"printerName";
                        printDocument.DocumentName = fileName;
                        printDocument.PrinterSettings.PrintFileName = fileName;
                        printDocument.PrintController = new StandardPrintController();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Une erreur est survenue ! " + ex.Message);
            }
        }

        private void TextBoxStockSearch_KeyUp(object sender, KeyEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {
                dataGridViewCustomerRelationAll.DataSource = new BindingList<Article>(ArticlesDS)
                    .OrderBy(x => x.Name)
                    .ToList();
            }
            else
            {
                List<Article> articles = ArticlesDS.Where(x => x.Name.Contains(textBox.Text))?.ToList() ?? new List<Article>();
                if (articles?.Any() ?? false)
                {
                    dataGridViewStock.DataSource = null;
                    dataGridViewStock.Rows.Clear();
                    dataGridViewStock.Columns.Clear();

                    dataGridViewCustomerRelationAll.DataSource = new BindingList<Article>(articles)
                        .OrderBy(x => x.Name)
                        .ToList();
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
                ArticlesDS.Count,
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
                    WriteLineTofile($"{article.Id};" +
                            $"{article.Name};" +
                            $"{article.Price};" +
                            $"{article.Quantity};" +
                            $"{article.Description}", path, true);
                    MessageBox.Show($"L'article {article.Name} a été sauvegardée.");
                }
                // Suppression
                else if (index == -2)
                {
                    data = GetDataFromFile(path);
                    ClearFile(path);
                    int lineIndex = 0;
                    foreach (string line in data)
                    {
                        string[] fields = line.Split(';');
                        if (Int32.Parse(fields[0]) == article.Id)
                        {
                            data.RemoveAt(lineIndex);
                            ArticlesDS.Remove(article);
                            MessageBox.Show($"L'article \"{article.Name}\" a été supprimé.");
                        }
                        lineIndex++;
                    }
                    RewriteDataToFile(data, path, false);
                }
                // Modification
                else if (index >= 0)
                {
                    data = GetDataFromFile(path);
                    ClearFile(path);
                    for (int i = 0; i < data.Count; i++)
                    {
                        string[] fields = data[i].Split(';');
                        if (i > 0 && Int32.Parse(fields[0]) == article.Id)
                        {
                            data[i] = $"{article.Id};" +
                                    $"{article.Name};" +
                                    $"{article.Price};" +
                                    $"{article.Quantity};" +
                                    $"{article.Description}";
                            ArticlesDS[i] = article;
                            MessageBox.Show($"Les modifications de l'article {article.Name} ont été sauvegardées.");
                        }
                    }
                    RewriteDataToFile(data, path, false);
                }

                InitializeLists();
                TakeOver_Load();
                dataGridViewAllCustomerRelation_Load();
                dataGridViewStock_Load();
            }
            catch (Exception e)
            {
                MessageBox.Show("Erreur : " + e.Message);
            }
        }
        #endregion
    }
}
