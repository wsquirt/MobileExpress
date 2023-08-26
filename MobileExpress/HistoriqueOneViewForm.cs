using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.UI;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MobileExpress
{
    public partial class HistoriqueOneViewForm : Form
    {
        List<HistoriqueOneView> Historiques { get; set; }
        List<MEData> MEDatasDS { get; set; }
        Customer Customer { get; set; }
        List<Marque> MarquesDS { get; set; }
        List<Modele> ModelesDS { get; set; }
        List<TakeOverType> TakeOversDS { get; set; }
        public HistoriqueOneViewForm(
            List<HistoriqueOneView> historiques,
            List<MEData> receipts,
            Customer customer,
            List<Marque> marquesDS,
            List<Modele> modelesDS,
            List<TakeOverType> takeOversDS)
        {
            InitializeComponent();

            Historiques = historiques;
            MEDatasDS = receipts;
            Customer = customer;
            MarquesDS = marquesDS;
            ModelesDS = modelesDS;
            TakeOversDS = takeOversDS;

            dataGridViewHistorique.CellDoubleClick += new DataGridViewCellEventHandler(dataGridView1_CellDoubleClick);
            dataGridViewHistorique.CellValidated += new DataGridViewCellEventHandler(DataGridView_CellValidated);
            DataGridView_Load();
        }
        private void ClearDataGridView()
        {
            dataGridViewHistorique.DataSource = null;
            dataGridViewHistorique.Rows.Clear();
            dataGridViewHistorique.Columns.Clear();
            dataGridViewHistorique.AutoGenerateColumns = false;
            dataGridViewHistorique.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            DataGridViewTextBoxColumn id = new DataGridViewTextBoxColumn()
            {
                Name = "textBoxId",
                ValueType = typeof(int),
                Visible = false,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(id);
            DataGridViewTextBoxColumn priseEnCharge = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Prise en charge",
                Name = "textBoxPriseEnCharge",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(priseEnCharge);
            DataGridViewTextBoxColumn date = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Date",
                Name = "textBoxDate",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(date);
            DataGridViewTextBoxColumn facture = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Facture",
                Name = "textBoxFacture",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(facture);
            DataGridViewTextBoxColumn invoiceid = new DataGridViewTextBoxColumn()
            {
                Name = "textBoxInvoiceId",
                ValueType = typeof(int),
                Visible = false,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(invoiceid);
            DataGridViewTextBoxColumn typeItem = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Type",
                Name = "textBoxTypeItem",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(typeItem);
            DataGridViewTextBoxColumn item = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Référence",
                Name = "textBoxReference",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(item);
            DataGridViewTextBoxColumn marque = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Marque",
                Name = "textBoxMarque",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(marque);
            DataGridViewTextBoxColumn modele = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Modèle",
                Name = "textBoxModele",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(modele);
            DataGridViewTextBoxColumn iMEI = new DataGridViewTextBoxColumn()
            {
                HeaderText = "IMEI",
                Name = "textBoxIMEI",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(iMEI);
            DataGridViewTextBoxColumn quantity = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Quantité",
                Name = "textBoxQuantity",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(quantity);
            DataGridViewTextBoxColumn price = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Prix",
                Name = "textBoxPrice",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = false,
            };
            dataGridViewHistorique.Columns.Add(price);
            DataGridViewTextBoxColumn garantie = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Garantie",
                Name = "textBoxGarantie",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(garantie);
            DataGridViewTextBoxColumn remise = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Remise",
                Name = "textBoxRemise",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(remise);
            DataGridViewTextBoxColumn accompte = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Accompte",
                Name = "textBoxAccompte",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(accompte);
            DataGridViewTextBoxColumn resteDu = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Reste dû",
                Name = "textBoxResteDu",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(resteDu);
            DataGridViewTextBoxColumn paid = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Payé",
                Name = "textBoxPaid",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = false,
            };
            dataGridViewHistorique.Columns.Add(paid);
            DataGridViewTextBoxColumn total = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Total",
                Name = "textBoxTotal",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(total);
            DataGridViewTextBoxColumn paymentMode = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Mode de paiement",
                Name = "textBoxPaymentMode",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(paymentMode);
            DataGridViewComboBoxColumn state = new DataGridViewComboBoxColumn()
            {
                HeaderText = "Etat",
                Name = "comboBoxState",
                DataSource = Enum.GetValues(typeof(TakeOverState))
                                            .Cast<TakeOverState>()
                                            .Select(status => new KeyValuePair<string, TakeOverState>(Tools.GetEnumDescription(status), status))
                                            .ToList(),
                DisplayMember = "Key",
                ValueMember = "Value",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                Visible = true,
                ReadOnly = false,
            };
            dataGridViewHistorique.Columns.Add(state);
            DataGridViewCheckBoxColumn verification = new DataGridViewCheckBoxColumn()
            {
                HeaderText = "Vérification",
                Name = "checkBoxVerification",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Visible = true,
                ReadOnly = false,
            };
            dataGridViewHistorique.Columns.Add(verification);
        }
        private void DataGridView_Load()
        {
            try
            {
                ClearDataGridView();

                int index = 0;
                foreach (HistoriqueOneView historique in Historiques)
                {
                    dataGridViewHistorique.Rows.Add();

                    DataGridViewTextBoxCell id = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxId"];
                    id.Value = historique.Id;

                    DataGridViewTextBoxCell invoiceid = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxInvoiceId"];
                    invoiceid.Value = historique.FactureNumero;

                    DataGridViewTextBoxCell priseEnCharge = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxPriseEnCharge"];
                    priseEnCharge.Value = historique.PriseEnCharge;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl priseEnChargeControl)
                    {
                        priseEnChargeControl.Text = historique.PriseEnCharge.ToString();
                    }

                    DataGridViewTextBoxCell date = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxDate"];
                    date.Value = historique.Date;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl dateControl)
                    {
                        dateControl.Text = historique.Date;
                    }

                    DataGridViewTextBoxCell facture = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxFacture"];
                    facture.Value = historique.Facture;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl factureControl)
                    {
                        factureControl.Text = historique.Facture;
                    }

                    DataGridViewTextBoxCell typeItem = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxTypeItem"];
                    typeItem.Value = historique.TypeItem;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl typeItemControl)
                    {
                        typeItemControl.Text = historique.TypeItem;
                    }

                    DataGridViewTextBoxCell item = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxReference"];
                    item.Value = historique.Item;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl itemControl)
                    {
                        itemControl.Text = historique.Item;
                    }

                    DataGridViewTextBoxCell marque = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxMarque"];
                    marque.Value = historique.Marque;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl marqueControl)
                    {
                        marqueControl.Text = historique.Marque;
                    }

                    DataGridViewTextBoxCell modele = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxModele"];
                    modele.Value = historique.Modele;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl modeleControl)
                    {
                        modeleControl.Text = historique.Modele;
                    }

                    DataGridViewTextBoxCell imei = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxIMEI"];
                    imei.Value = historique.IMEI;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl imeiControl)
                    {
                        imeiControl.Text = historique.IMEI;
                    }

                    DataGridViewTextBoxCell quantity = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxQuantity"];
                    quantity.Value = historique.Quantity;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl quantityControl)
                    {
                        quantityControl.Text = historique.Quantity.ToString();
                    }

                    DataGridViewTextBoxCell price = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxPrice"];
                    price.Value = historique.Price;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl priceControl)
                    {
                        priceControl.Text = historique.Price;
                    }

                    DataGridViewTextBoxCell garantie = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxGarantie"];
                    garantie.Value = historique.Garantie;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl garantieControl)
                    {
                        garantieControl.Text = historique.Garantie;
                    }

                    DataGridViewTextBoxCell remise = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxRemise"];
                    remise.Value = historique.Remise;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl remiseControl)
                    {
                        remiseControl.Text = historique.Remise;
                    }

                    DataGridViewTextBoxCell accompte = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxAccompte"];
                    accompte.Value = historique.Accompte;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl accompteControl)
                    {
                        accompteControl.Text = historique.Accompte;
                    }

                    DataGridViewTextBoxCell resteDu = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxResteDu"];
                    resteDu.Value = historique.ResteDu;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl resteDuControl)
                    {
                        resteDuControl.Text = historique.ResteDu;
                    }

                    DataGridViewTextBoxCell paid = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxPaid"];
                    paid.Value = historique.Paid;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl paidControl)
                    {
                        paidControl.Text = historique.Paid;
                    }

                    DataGridViewTextBoxCell total = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxTotal"];
                    total.Value = historique.Total;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl totalControl)
                    {
                        totalControl.Text = historique.Total;
                    }

                    DataGridViewTextBoxCell paymentMode = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxPaymentMode"];
                    paymentMode.Value = historique.PaymentMode;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl paymentModeControl)
                    {
                        paymentModeControl.Text = historique.PaymentMode;
                    }

                    DataGridViewComboBoxCell state = (DataGridViewComboBoxCell)dataGridViewHistorique.Rows[index].Cells["comboBoxState"];
                    state.Value = historique.State;
                    if (dataGridViewHistorique.EditingControl is DataGridViewComboBoxEditingControl stateControl)
                    {
                        stateControl.Text = Tools.GetEnumDescription(historique.State);
                    }

                    DataGridViewCheckBoxCell verification = (DataGridViewCheckBoxCell)dataGridViewHistorique.Rows[index].Cells["checkBoxVerification"];
                    verification.Value = historique.Verification;

                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridViewHistorique.Rows.Count)
                {
                    DataGridViewRow selectedRow = dataGridViewHistorique.Rows[e.RowIndex];

                    if (selectedRow != null &&
                        e.ColumnIndex == selectedRow.Cells["textBoxPriseEnCharge"].ColumnIndex)
                    {
                        int id = (selectedRow.Cells["textBoxId"].Value as int?).Value;
                        HistoriqueOneView historique = Historiques.First(x => x.Id == id);
                        string filePath = $@"{Paths.TakeOverPdfsPath}\PriseEnCharge_{(historique.PriseEnCharge < 10 ? $"0{historique.PriseEnCharge.ToString()}" : historique.PriseEnCharge.ToString())}_{historique.Date.Replace("/", "")}.pdf";
                        if (System.IO.File.Exists(filePath))
                        {
                            Process.Start(filePath);
                        }
                        using (PriseEnChargePathForm dialog = new PriseEnChargePathForm(filePath))
                        {
                            dialog.ShowDialog();
                        }
                    }
                    else if (selectedRow != null &&
                        e.ColumnIndex == selectedRow.Cells["textBoxFacture"].ColumnIndex &&
                        (selectedRow.Cells["textBoxId"].Value as int?).HasValue)
                    {
                        int id = (selectedRow.Cells["textBoxId"].Value as int?).Value;
                        int invoiceid = (selectedRow.Cells["textBoxInvoiceId"].Value as int?).Value;
                        if (invoiceid != 0)
                        {
                            HistoriqueOneView historique = Historiques.First(x => x.Id == id && x.FactureNumero == invoiceid);
                            string filePath = $@"{Paths.InvoicePdfsPath}\Facture_{(historique.FactureNumero < 10 ? $"0{historique.FactureNumero}" : historique.FactureNumero.ToString())}_PriseEnCharge_{(historique.PriseEnCharge < 10 ? $"0{historique.PriseEnCharge.ToString()}" : historique.PriseEnCharge.ToString())}_{historique.Date.Replace("/", "")}.pdf";
                            if (System.IO.File.Exists(filePath))
                            {
                                Process.Start(filePath);
                            }
                            using (InvoicePathForm dialog = new InvoicePathForm(filePath))
                            {
                                dialog.ShowDialog();
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
        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                int? firstIndex = null;
                int lastIndex = 0;
                decimal total = 0;
                foreach (DataGridViewRow row in dataGridViewHistorique.Rows)
                {
                    if (!row.IsNewRow && (row.Cells["textBoxId"].Value as int?).HasValue)
                    {
                        int index = 0;
                        Historiques.ForEach(x =>
                        {
                            if (x.Id == (row.Cells["textBoxId"].Value as int?).Value)
                            {
                                if (!firstIndex.HasValue)
                                    firstIndex = index;
                                x.State = (row.Cells["comboBoxState"].Value as TakeOverState?).Value;
                                x.Price = (row.Cells["textBoxPrice"].Value as string);
                                x.Paid = (row.Cells["textBoxPaid"].Value as string);
                                x.Verification = (row.Cells["checkBoxVerification"].Value as bool?).Value;
                                total += (Tools.ToNullableDecimal(x.Price) ?? 0) - (Tools.ToNullableDecimal(x.Remise) ?? 0);
                                lastIndex = index;
                            }
                            index++;
                        });
                    }
                }
                for (int i = firstIndex.Value; i <= lastIndex; i++)
                {
                    Historiques[i].ResteDu = (total - (Tools.ToNullableDecimal(Historiques[i].Paid) ?? 0) - (Tools.ToNullableDecimal(Historiques[i].Accompte) ?? 0)).ToString();
                    Historiques[i].Total = total.ToString();
                }

                // Indiquer que le formulaire doit se fermer avec un résultat valide (OK)
                this.DialogResult = DialogResult.OK;
                // Fermer le formulaire
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        private void buttonGenerateInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                List<MEData> mEDatas = MEDatasDS.Where(x => Historiques.FirstOrDefault(y => y.PriseEnCharge == x.TakeOverId) != null).ToList();

                int invoiceId = MEDatasDS.OrderByDescending(x => x.InvoiceId ?? 0)?.FirstOrDefault()?.InvoiceId ?? 0;
                using (GenerateInvoiceForm dialog = new GenerateInvoiceForm(mEDatas, Customer, MarquesDS, ModelesDS, TakeOversDS, invoiceId))
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

                        List<string> items = new List<string>() { "Numéro de prise en charge;Date;Numéro de facture;Numéro du client;Numéro de la marque;Numéro du modèle;IMEI;Numéro du type de réparation;Numéro du type de déblocage;Numéro de l'article;Quantité;Prix;Garantie;Remise;Accompte;Reste dû;Payé;Total;Numéro de mode de paiement;Etat;Id;Vérification" };
                        items.AddRange(MEDatasDS.Select(x => $"{x.TakeOverId};{x.Date};{x.InvoiceId};{x.CustomerId};{x.MarqueId};{x.ModeleId};{x.IMEI};{x.RepairTypeId};{x.UnlockTypeId};{x.ArticleId};{x.Quantity};{x.Price};{x.Garantie};{x.Remise};{x.Accompte};{x.ResteDu};{x.Paid};{x.Total};{x.PaymentMode};{(int)x.State};{x.Id};{(x.Verification ? "Oui" : "Non")}"));
                        Tools.RewriteDataToFile(items, path, false);

                        decimal payeAFacturer = factureACreer.First().Paid.Value;
                        decimal accompteAFacturer = factureACreer.First().Accompte.Value;
                        decimal totalAFacturer = 0;
                        Dictionary<int, string> repairTypeNames = new Dictionary<int, string>();
                        Dictionary<int, string> unlockTypeNames = new Dictionary<int, string>();
                        Dictionary<int, string> articles = new Dictionary<int, string>();
                        Dictionary<int, string> marques = new Dictionary<int, string>();
                        Dictionary<int, string> modeles = new Dictionary<int, string>();
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

                            Marque marque = MarquesDS.First(x => afacturer.MarqueId == x.Id);
                            Modele modele = ModelesDS.First(x => afacturer.ModeleId == x.Id);

                            RepairType repairType = TakeOversDS.FirstOrDefault(x => x.Id == 0).RepairTypes.FirstOrDefault(x => x.Id == afacturer.RepairTypeId);
                            if (repairType != null)
                            {
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
                            }

                            UnlockType unlockType = TakeOversDS.FirstOrDefault(x => x.Id == 1).UnlockTypes.FirstOrDefault(x => x.Id == afacturer.UnlockTypeId);
                            if (unlockType != null)
                            {
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
                            }

                            Article article = TakeOversDS.FirstOrDefault(x => x.Id == 2).Articles.FirstOrDefault(x => x.Id == afacturer.ArticleId);
                            if (article != null)
                            {
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
                            }
                        }
                        DialogResult acccompteResult = MessageBox.Show($"Voulez-vous prendre en compte l'accompte de {accompteAFacturer.ToString()} versé ?", "Information", MessageBoxButtons.YesNo);
                        if (acccompteResult == DialogResult.Yes)
                        {
                            payeAFacturer += accompteAFacturer;
                        }
                        (bool cb, bool espece, bool virement) = Tools.GetPaymentModeFromInt(factureACreer.First().PaymentMode);
                        // générer pdf
                        string customerName = (
                            !string.IsNullOrWhiteSpace(Customer.LastName) && !string.IsNullOrWhiteSpace(Customer.FirstName) ? Customer.LastName + " " + Customer.FirstName :
                            !string.IsNullOrWhiteSpace(Customer.LastName) && string.IsNullOrWhiteSpace(Customer.FirstName) ? Customer.LastName :
                            Customer.FirstName);
                        string filePath = Tools.GeneratePdfFromHtml(
                            type: 1,
                            logo: Paths.LogoPath,
                            customerName: (Customer.Sexe == Sexe.Femme ? $"Madame {customerName}" : Customer.Sexe == Sexe.Homme ? $"Monsieur {customerName}" : customerName),
                            customerPhone: Customer.PhoneNumber,
                            customerEmail: Customer.EmailAddress,
                            takeOverDate: factureACreer.First().Date.ToString("dddd dd MMMM yyyy"),
                            takeOverNumber: factureACreer.First().TakeOverId.ToString(),
                            accompte: null,
                            paid: payeAFacturer,
                            carteBleu: cb,
                            espece: espece,
                            virement: virement,
                            mEDatas: factureACreer,
                            path: $@"{Paths.InvoicePdfsPath}\Facture_{(invoiceId < 10 ? $"0{invoiceId.ToString()}" : invoiceId.ToString())}_PriseEnCharge_{(factureACreer.First().TakeOverId < 10 ? $"0{factureACreer.First().TakeOverId.ToString()}" : factureACreer.First().TakeOverId.ToString())}_{factureACreer.First().Date.ToString("ddMMyyyy HHmmss")}.pdf",
                            repairTypeNames: repairTypeNames,
                            unlockTypeNames: unlockTypeNames,
                            articles: articles,
                            marques: marques,
                            modeles: modeles);
                        // Vérifier si le fichier existe
                        if (System.IO.File.Exists(filePath))
                        {
                            // Ouvrir le fichier PDF dans une fenêtre externe
                            Process.Start(filePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        public List<HistoriqueOneView> GetResult()
        {
            return Historiques.ToList();
        }

        private void DataGridView_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int columnIndex = dataGridViewHistorique.CurrentCell.ColumnIndex;
                int rowIndex = dataGridViewHistorique.CurrentCell.RowIndex;
                if (columnIndex >= 0 && rowIndex >= 0 && !dataGridViewHistorique.Rows[rowIndex].IsNewRow)
                {
                    DataGridViewTextBoxCell textBox = (sender as DataGridView).CurrentCell as DataGridViewTextBoxCell;
                    DataGridViewTextBoxEditingControl textBoxEditingControl = (sender as DataGridView).EditingControl as DataGridViewTextBoxEditingControl;
                    if (textBox != null && !string.IsNullOrWhiteSpace(textBox.Value as string) && textBoxEditingControl != null)
                    {
                        if (columnIndex == dataGridViewHistorique.Columns["textBoxPaid"].Index)
                        {
                            int priseEnchargeId = (dataGridViewHistorique.Rows[rowIndex].Cells["textBoxPriseEnCharge"].Value as int?).Value;
                            string paid = textBox.Value as string;
                            for (int i = rowIndex; !dataGridViewHistorique.Rows[i].IsNewRow && (dataGridViewHistorique.Rows[i].Cells["textBoxPriseEnCharge"].Value as int?).Value == priseEnchargeId; i++)
                            {
                                dataGridViewHistorique.Rows[i].Cells["textBoxPaid"].Value = paid;
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
    }
}
