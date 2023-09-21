using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

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

            dataGridViewHistorique.CellDoubleClick += new DataGridViewCellEventHandler(DataGridViewHistorique_CellDoubleClick);
            dataGridViewHistorique.CellValidated += new DataGridViewCellEventHandler(DataGridViewHistorique_CellValidated);
            DataGridViewHistorique_Load();
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
                ValueType = typeof(int),
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
                ValueType = typeof(int),
                Visible = true,
                ReadOnly = true,
            };
            dataGridViewHistorique.Columns.Add(quantity);
            DataGridViewTextBoxColumn price = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Prix unitaire",
                Name = "textBoxPrice",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(decimal),
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
                ValueType = typeof(int),
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
                ValueType = typeof(decimal),
                Visible = true,
                ReadOnly = false,
            };
            dataGridViewHistorique.Columns.Add(remise);
            DataGridViewTextBoxColumn accompte = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Accompte",
                Name = "textBoxAccompte",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(decimal),
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
                ValueType = typeof(decimal),
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
                ValueType = typeof(decimal),
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
                ValueType = typeof(decimal),
                Visible = true,
                ReadOnly = false,
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
                                            .Select(status => new KeyValuePair<string, TakeOverState>(Tools.GetEnumDescriptionFromEnum<TakeOverState>(status), status))
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
        private void DataGridViewHistorique_Load()
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
                    price.Value = Tools.ToNullableDecimal(historique.Price) ?? 0;
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
                    remise.Value = Tools.ToNullableDecimal(historique.Remise) ?? 0;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl remiseControl)
                    {
                        remiseControl.Text = historique.Remise;
                    }

                    DataGridViewTextBoxCell accompte = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxAccompte"];
                    accompte.Value = Tools.ToNullableDecimal(historique.Accompte) ?? 0;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl accompteControl)
                    {
                        accompteControl.Text = historique.Accompte;
                    }

                    DataGridViewTextBoxCell resteDu = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxResteDu"];
                    resteDu.Value = Tools.ToNullableDecimal(historique.ResteDu) ?? 0;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl resteDuControl)
                    {
                        resteDuControl.Text = historique.ResteDu;
                    }

                    DataGridViewTextBoxCell paid = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxPaid"];
                    paid.Value = Tools.ToNullableDecimal(historique.Paid) ?? 0;
                    if (dataGridViewHistorique.EditingControl is DataGridViewTextBoxEditingControl paidControl)
                    {
                        paidControl.Text = historique.Paid;
                    }

                    DataGridViewTextBoxCell total = (DataGridViewTextBoxCell)dataGridViewHistorique.Rows[index].Cells["textBoxTotal"];
                    total.Value = Tools.ToNullableDecimal(historique.Total) ?? 0;
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
                        stateControl.Text = Tools.GetEnumDescriptionFromEnum(historique.State);
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
        private void DataGridViewHistorique_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.RowIndex < dataGridViewHistorique.Rows.Count)
                {
                    DataGridViewRow selectedRow = dataGridViewHistorique.Rows[e.RowIndex];

                    if (selectedRow != null &&
                        e.ColumnIndex == selectedRow.Cells["textBoxPriseEnCharge"].ColumnIndex &&
                        (selectedRow.Cells["textBoxId"].Value as int?).HasValue)
                    {
                        int id = (selectedRow.Cells["textBoxId"].Value as int?).Value;
                        HistoriqueOneView historique = Historiques.First(x => x.Id == id);
                        string filePath = $@"{Paths.PriseEnChargeDirectory}\PriseEnCharge_{(historique.PriseEnCharge < 10 ? $"0{historique.PriseEnCharge.ToString()}" : historique.PriseEnCharge.ToString())}_{historique.Date.Replace("/", "")}.docx";
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
                            string filePath = $@"{Paths.FactureDirectory}\Facture_{(historique.FactureNumero < 10 ? $"0{historique.FactureNumero}" : historique.FactureNumero.ToString())}_PriseEnCharge_{(historique.PriseEnCharge < 10 ? $"0{historique.PriseEnCharge.ToString()}" : historique.PriseEnCharge.ToString())}_{historique.Date.Replace("/", "")}.docx";
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
        private void DataGridViewHistorique_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int currentColumnIndex = dataGridViewHistorique.CurrentCell.ColumnIndex;
                int currentRowIndex = dataGridViewHistorique.CurrentCell.RowIndex;
                if (currentColumnIndex >= 0 && currentRowIndex >= 0 && !dataGridViewHistorique.Rows[currentRowIndex].IsNewRow)
                {
                    DataGridViewTextBoxCell textBox = (sender as DataGridView).CurrentCell as DataGridViewTextBoxCell;
                    DataGridViewTextBoxEditingControl textBoxEditingControl = (sender as DataGridView).EditingControl as DataGridViewTextBoxEditingControl;
                    if (textBox != null && (textBox.Value as decimal?).HasValue && textBoxEditingControl != null)
                    {
                        if (currentColumnIndex == dataGridViewHistorique.Columns["textBoxPaid"].Index)
                        {
                            int? priseEnChargeId = (dataGridViewHistorique.Rows[currentRowIndex].Cells["textBoxPriseEnCharge"].Value as int?);
                            decimal? paid = dataGridViewHistorique.Rows[currentRowIndex].Cells["textBoxPaid"].Value as decimal?;
                            decimal? total = dataGridViewHistorique.Rows[currentRowIndex].Cells["textBoxTotal"].Value as decimal?;
                            for (int i = 0; i < dataGridViewHistorique.RowCount; i++)
                            {
                                if (!dataGridViewHistorique.Rows[i].IsNewRow &&
                                    priseEnChargeId.HasValue &&
                                    priseEnChargeId.Value == (dataGridViewHistorique.Rows[i].Cells["textBoxPriseEnCharge"].Value as int?).Value)
                                {
                                    dataGridViewHistorique.Rows[i].Cells["textBoxPaid"].Value = paid;
                                    dataGridViewHistorique.Text = paid.HasValue ? paid.Value.ToString() : "0";

                                    dataGridViewHistorique.Rows[i].Cells["textBoxResteDu"].Value = (total ?? 0) - (paid ?? 0);
                                    dataGridViewHistorique.Text = paid.HasValue && total.HasValue ? ((total ?? 0) - (paid ?? 0)).ToString() : "0";
                                }
                            }
                        }
                        else if (currentColumnIndex == dataGridViewHistorique.Columns["textBoxPrice"].Index)
                        {
                            int? priseEnChargeId = (dataGridViewHistorique.Rows[currentRowIndex].Cells["textBoxPriseEnCharge"].Value as int?);
                            decimal? paid = dataGridViewHistorique.Rows[currentRowIndex].Cells["textBoxPaid"].Value as decimal?;
                            decimal total = 0;
                            for (int i = 0; i < dataGridViewHistorique.RowCount; i++)
                            {
                                if (!dataGridViewHistorique.Rows[i].IsNewRow &&
                                    priseEnChargeId.HasValue &&
                                    priseEnChargeId.Value == (dataGridViewHistorique.Rows[i].Cells["textBoxPriseEnCharge"].Value as int?).Value)
                                {
                                    decimal? price = dataGridViewHistorique.Rows[i].Cells["textBoxPrice"].Value as decimal?;
                                    decimal? remise = dataGridViewHistorique.Rows[i].Cells["textBoxRemise"].Value as decimal?;
                                    int? quantity = dataGridViewHistorique.Rows[i].Cells["textBoxQuantity"].Value as int?;
                                    total += ((price ?? 0) * (quantity ?? 1)) - (remise ?? 0);
                                }
                            }
                            for (int i = 0; i < dataGridViewHistorique.RowCount; i++)
                            {
                                if (!dataGridViewHistorique.Rows[i].IsNewRow &&
                                    priseEnChargeId.HasValue &&
                                    priseEnChargeId.Value == (dataGridViewHistorique.Rows[i].Cells["textBoxPriseEnCharge"].Value as int?).Value)
                                {
                                    dataGridViewHistorique.Rows[i].Cells["textBoxResteDu"].Value = total - (paid ?? 0);
                                    dataGridViewHistorique.Text = paid.HasValue ? (total - (paid ?? 0)).ToString() : "0";

                                    dataGridViewHistorique.Rows[i].Cells["textBoxTotal"].Value = total - (paid ?? 0);
                                    dataGridViewHistorique.Text = total.ToString();
                                }
                            }
                        }
                        else if (currentColumnIndex == dataGridViewHistorique.Columns["textBoxRemise"].Index)
                        {
                            int? priseEnChargeId = (dataGridViewHistorique.Rows[currentRowIndex].Cells["textBoxPriseEnCharge"].Value as int?);
                            decimal? paid = Tools.ToNullableDecimal(dataGridViewHistorique.Rows[currentRowIndex].Cells["textBoxPaid"].Value as string);
                            decimal total = 0;
                            for (int i = 0; i < dataGridViewHistorique.RowCount; i++)
                            {
                                if (!dataGridViewHistorique.Rows[i].IsNewRow &&
                                    priseEnChargeId.HasValue &&
                                    priseEnChargeId.Value == (dataGridViewHistorique.Rows[i].Cells["textBoxPriseEnCharge"].Value as int?).Value)
                                {
                                    decimal? price = Tools.ToNullableDecimal(dataGridViewHistorique.Rows[i].Cells["textBoxPrice"].Value as string);
                                    decimal? remise = dataGridViewHistorique.Rows[i].Cells["textBoxRemise"].Value as decimal?;
                                    int? quantity = dataGridViewHistorique.Rows[i].Cells["textBoxQuantity"].Value as int?;
                                    total += ((price ?? 0) * (quantity ?? 1)) - (remise ?? 0);
                                }
                            }
                            for (int i = 0; i < dataGridViewHistorique.RowCount; i++)
                            {
                                if (!dataGridViewHistorique.Rows[i].IsNewRow &&
                                    priseEnChargeId.HasValue &&
                                    priseEnChargeId.Value == (dataGridViewHistorique.Rows[i].Cells["textBoxPriseEnCharge"].Value as int?).Value)
                                {
                                    dataGridViewHistorique.Rows[i].Cells["textBoxResteDu"].Value = total - (paid ?? 0);
                                    dataGridViewHistorique.Text = paid.HasValue ? (total - (paid ?? 0)).ToString() : "0";

                                    dataGridViewHistorique.Rows[i].Cells["textBoxTotal"].Value = total - (paid ?? 0);
                                    dataGridViewHistorique.Text = total.ToString();
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
        private void buttonSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<int> priseEnChargeIds = new List<int>();
                List<MEData> mEDatas = new List<MEData>();
                foreach (DataGridViewRow row in dataGridViewHistorique.Rows)
                {
                    if (!row.IsNewRow && (row.Cells["textBoxId"].Value as int?).HasValue)
                    {
                        string typeItem = row.Cells["textBoxTypeItem"].Value as string;
                        string typeText = row.Cells["textBoxReference"].Value as string;

                        int takeOverId = (row.Cells["textBoxPriseEnCharge"].Value as int?).Value;
                        DateTime date = DateTime.Parse(row.Cells["textBoxDate"].Value as string);
                        int? invoiceId = (row.Cells["textBoxInvoiceId"].Value as int?).Value;
                        int customerId = Customer.Id;
                        int? marqueId = MarquesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), (row.Cells["textBoxMarque"].Value as string).ToLower()) == 0)?.Id;
                        int? modeleId = ModelesDS.FirstOrDefault(x => string.Compare(x.Name.ToLower(), (row.Cells["textBoxModele"].Value as string).ToLower()) == 0)?.Id;
                        string imei = row.Cells["textBoxIMEI"].Value as string;
                        int? repairTypeId = typeItem == "Réparation" ? TakeOversDS.First(t => t.Id == 0).RepairTypes.FirstOrDefault(x => string.Compare(x.Name.ToLower(), typeText?.ToLower()) == 0)?.Id : null;
                        int? unlockTypeId = typeItem == "Déblocage" ? TakeOversDS.First(t => t.Id == 1).UnlockTypes.FirstOrDefault(x => string.Compare(x.Name.ToLower(), typeText?.ToLower()) == 0)?.Id : null;
                        int? articleId = typeItem == "Achat" ? TakeOversDS.First(t => t.Id == 2).Articles.FirstOrDefault(x => string.Compare(x.Name.ToLower(), typeText?.ToLower()) == 0)?.Id : null;
                        int quantity = (row.Cells["textBoxQuantity"].Value as int?).Value;
                        decimal price = (row.Cells["textBoxPrice"].Value as decimal?) ?? 0;
                        int? monthsGarantie = (row.Cells["textBoxGarantie"].Value as string) == "Non" ? null : (int?)int.Parse((row.Cells["textBoxGarantie"].Value as string).Split(' ')[0]);
                        decimal? remise = (row.Cells["textBoxRemise"].Value as decimal?) ?? 0;
                        decimal? accompte = (row.Cells["textBoxAccompte"].Value as decimal?) ?? 0;
                        decimal? resteDu = (row.Cells["textBoxResteDu"].Value as decimal?) ?? 0;
                        decimal? paid = (row.Cells["textBoxPaid"].Value as decimal?) ?? 0;
                        decimal total = (row.Cells["textBoxTotal"].Value as decimal?) ?? 0;
                        PaymentMode paymentMode = Tools.GetEnumFromDescription<PaymentMode>(row.Cells["textBoxPaymentMode"].Value as string);
                        TakeOverState state = (row.Cells["comboBoxState"].Value as TakeOverState?).Value;
                        int id = (row.Cells["textBoxId"].Value as int?).Value;
                        bool verification = (row.Cells["checkBoxVerification"].Value as bool?).Value;
                        
                        if (!priseEnChargeIds.Any(x => x == takeOverId))
                        {
                            priseEnChargeIds.Add(takeOverId);
                        }
                        mEDatas.Add(new MEData(
                        takeOverId, date, invoiceId, customerId, marqueId, modeleId, imei, repairTypeId, unlockTypeId, articleId,
                        quantity, price, monthsGarantie, remise, accompte, resteDu, paid, total, paymentMode, state, id, verification));
                    }
                }

                foreach (int priseEnChargeId in priseEnChargeIds)
                {
                    decimal total = 0;
                    decimal paid = 0;
                    bool isPaidCounted = false;
                    Historiques.ForEach(h =>
                    {
                        if (h.PriseEnCharge == priseEnChargeId)
                        {
                            MEData mEData = mEDatas.First(me => me.Id == h.Id);
                            h.Price = mEData.Price.ToString();
                            h.Quantity = mEData.Quantity;
                            h.Accompte = mEData.Accompte.ToString();
                            h.Paid = mEData.Paid.ToString();
                            h.Remise = mEData.Remise.ToString();
                            h.State = mEData.State;
                            h.Verification = mEData.Verification;
                            total += ((mEData.Price ?? 0) * mEData.Quantity) - (mEData.Remise ?? 0);
                            if (!isPaidCounted)
                            {
                                paid += (mEData.Paid ?? 0) + (mEData.Accompte ?? 0);
                                isPaidCounted = true;
                            }
                        }
                    });
                    Historiques.ForEach(h =>
                    {
                        if (h.PriseEnCharge == priseEnChargeId)
                        {
                            h.ResteDu = (total - paid).ToString();
                            h.Total = total.ToString();
                        }
                    });
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
                        items.AddRange(MEDatasDS.Select(x => $"{x.TakeOverId};{x.Date};{x.InvoiceId};{x.CustomerId};{x.MarqueId};{x.ModeleId};{x.IMEI};{x.RepairTypeId};{x.UnlockTypeId};{x.ArticleId};{x.Quantity};{x.Price};{x.Garantie};{x.Remise};{x.Accompte};{x.ResteDu};{x.Paid};{x.Total};{Tools.GetEnumDescriptionFromEnum<PaymentMode>((PaymentMode)x.PaymentMode)};{(int)x.State};{x.Id};{(x.Verification ? "Oui" : "Non")}"));
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
                        (bool cb, bool espece, bool virement) = Tools.GetBoolFromPaymentMode(factureACreer.First().PaymentMode);
                        // générer pdf
                        string customerName = (
                            !string.IsNullOrWhiteSpace(Customer.LastName) && !string.IsNullOrWhiteSpace(Customer.FirstName) ? Customer.LastName + " " + Customer.FirstName :
                            !string.IsNullOrWhiteSpace(Customer.LastName) && string.IsNullOrWhiteSpace(Customer.FirstName) ? Customer.LastName :
                            Customer.FirstName);
                        string title = $@"Facture_{(invoiceId < 10 ? $"0{invoiceId}" : invoiceId.ToString())}_PriseEnCharge_{(factureACreer.First().TakeOverId < 10 ? $"0{factureACreer.First().TakeOverId}" : factureACreer.First().TakeOverId.ToString())}_{factureACreer.First().Date.ToString("ddMMyyyyHHmmss")}";
                        Tools.GenerateDocx(
                            type: 1,
                            logo: Paths.LogoPath,
                            customerName: (Customer.Sexe == Sexe.Femme ? $"Madame {customerName}" : Customer.Sexe == Sexe.Homme ? $"Monsieur {customerName}" : customerName),
                            customerPhone: Customer.PhoneNumber,
                            customerEmail: Customer.EmailAddress,
                            takeOverDate: factureACreer.First().Date.ToString("dd/MM/yyyy"),
                            takeOverNumber: factureACreer.First().TakeOverId.ToString(),
                            accompte: null,
                            paid: payeAFacturer,
                            carteBleu: cb,
                            espece: espece,
                            virement: virement,
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
        public List<HistoriqueOneView> GetResult()
        {
            return Historiques.ToList();
        }

    }
}
