using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MobileExpress
{
    public partial class GenerateInvoiceForm : Form
    {
        List<MEData> MEDatas = new List<MEData>();
        Customer Customer { get; set; }
        List<Marque> MarquesDS { get; set; }
        List<Modele> ModelesDS { get; set; }
        List<TakeOverType> TakeOversDS { get; set; }
        int InvoiceId { get; set; }
        public GenerateInvoiceForm(List<MEData> receipts, Customer customer, List<Marque> marquesDS, List<Modele> modelesDS, List<TakeOverType> takeOversDS, int invoiceId)
        {
            InitializeComponent();
            MEDatas = receipts;
            Customer = customer;
            MarquesDS = marquesDS;
            ModelesDS = modelesDS;
            TakeOversDS = takeOversDS;
            InvoiceId = invoiceId;
            DataGridView_Load();
        }

        private void ClearDataGridView()
        {
            dataGridViewGenerateInvoice.DataSource = null;
            dataGridViewGenerateInvoice.Rows.Clear();
            dataGridViewGenerateInvoice.Columns.Clear();
            dataGridViewGenerateInvoice.AutoGenerateColumns = false;
            dataGridViewGenerateInvoice.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;

            DataGridViewCheckBoxColumn checkBoxAdd = new DataGridViewCheckBoxColumn
            {
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                HeaderText = "Sélection",
                Name = "dataGridViewGenerateInvoiceAddItem",
                TrueValue = true,
                ValueType = typeof(bool),
            };
            dataGridViewGenerateInvoice.Columns.Add(checkBoxAdd);
            
            DataGridViewTextBoxColumn textBoxTakeoverId = new DataGridViewTextBoxColumn
            {
                HeaderText = "Prise en charge",
                Name = "dataGridViewGenerateInvoiceTakeoverId",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ValueType = typeof(int),
                ReadOnly = true,
                Visible = true,
            };
            dataGridViewGenerateInvoice.Columns.Add(textBoxTakeoverId);

            DataGridViewTextBoxColumn textBoxReference = new DataGridViewTextBoxColumn
            {
                HeaderText = "Référence",
                Name = "dataGridViewGenerateInvoiceReference",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                ReadOnly = true,
            };
            dataGridViewGenerateInvoice.Columns.Add(textBoxReference);

            DataGridViewTextBoxColumn textBoxProduct = new DataGridViewTextBoxColumn
            {
                HeaderText = "Produit",
                Name = "dataGridViewGenerateInvoiceProduct",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                ReadOnly = true,
            };
            dataGridViewGenerateInvoice.Columns.Add(textBoxProduct);

            DataGridViewTextBoxColumn textBoxType = new DataGridViewTextBoxColumn
            {
                HeaderText = "Type",
                Name = "dataGridViewGenerateInvoiceType",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                ReadOnly = true,
            };
            dataGridViewGenerateInvoice.Columns.Add(textBoxType);

            DataGridViewTextBoxColumn textBoxIMEI = new DataGridViewTextBoxColumn
            {
                HeaderText = "IMEI",
                Name = "dataGridViewGenerateInvoiceIMEI",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                ReadOnly = true,
            };
            dataGridViewGenerateInvoice.Columns.Add(textBoxIMEI);
        }
        private void DataGridView_Load()
        {
            try
            {
                if (MEDatas == null || !MEDatas.Any())
                {
                    MessageBox.Show("Il n'y a pas d'items sélectionnés. Veuillez reprendre la prise en charge ou la facture.", "Alerte", MessageBoxButtons.OK);
                    return;
                }

                if (Customer == null)
                {
                    MessageBox.Show("Il y a un problème avec les données clientes. Veuillez réessayer.", "Alerte", MessageBoxButtons.OK);
                    return;
                }

                ClearDataGridView();
                
                int index = 0;
                foreach (MEData mEData in MEDatas/*.Where(x => !x.InvoiceId.HasValue)*/)
                {
                    dataGridViewGenerateInvoice.Rows.Add();

                    DataGridViewCheckBoxCell addItemCheckBoxCell = (DataGridViewCheckBoxCell)dataGridViewGenerateInvoice.Rows[index].Cells["dataGridViewGenerateInvoiceAddItem"];
                    addItemCheckBoxCell.Value = false;

                    DataGridViewTextBoxCell takeoverIdTextBoxCell = (DataGridViewTextBoxCell)dataGridViewGenerateInvoice.Rows[index].Cells["dataGridViewGenerateInvoiceTakeoverId"];
                    takeoverIdTextBoxCell.Value = mEData.TakeOverId;

                    if (mEData.MarqueId.HasValue)
                    {
                        string reference = string.Empty;

                        int marqueId = (mEData.MarqueId.Value as int?).Value;
                        Marque marque = MarquesDS.Find(x => x.Id == marqueId);
                        reference += marque.Name;

                        if (mEData.ModeleId.HasValue)
                        {
                            int modeleId = (mEData.ModeleId.Value as int?).Value;
                            Modele modele = ModelesDS.Find(x => x.Id == modeleId);
                            reference += " " + modele.Name;
                        }

                        DataGridViewTextBoxCell referenceComboBoxCell = (DataGridViewTextBoxCell)dataGridViewGenerateInvoice.Rows[index].Cells["dataGridViewGenerateInvoiceReference"];
                        referenceComboBoxCell.Value = reference;
                    }

                    if (mEData.RepairTypeId.HasValue)
                    {
                        DataGridViewTextBoxCell typeTextBoxCell = (DataGridViewTextBoxCell)dataGridViewGenerateInvoice.Rows[index].Cells["dataGridViewGenerateInvoiceType"];
                        typeTextBoxCell.Value = "Réparation";

                        int product = (mEData.RepairTypeId.Value as int?).Value;
                        DataGridViewTextBoxCell productTextBoxCell = (DataGridViewTextBoxCell)dataGridViewGenerateInvoice.Rows[index].Cells["dataGridViewGenerateInvoiceProduct"];
                        RepairType repairType = TakeOversDS.Find(x => x.Id == 0).RepairTypes.Find(x => x.Id == product);
                        productTextBoxCell.Value = repairType.Name;
                    }
                    else if (mEData.UnlockTypeId.HasValue)
                    {
                        DataGridViewTextBoxCell typeTextBoxCell = (DataGridViewTextBoxCell)dataGridViewGenerateInvoice.Rows[index].Cells["dataGridViewGenerateInvoiceType"];
                        typeTextBoxCell.Value = "Déblocage";

                        int product = (mEData.UnlockTypeId.Value as int?).Value;
                        DataGridViewTextBoxCell productTextBoxCell = (DataGridViewTextBoxCell)dataGridViewGenerateInvoice.Rows[index].Cells["dataGridViewGenerateInvoiceProduct"];
                        UnlockType unlockType = TakeOversDS.Find(x => x.Id == 1).UnlockTypes.Find(x => x.Id == product);
                        productTextBoxCell.Value = unlockType.Name;
                    }
                    else if (mEData.ArticleId.HasValue)
                    {
                        DataGridViewTextBoxCell typeTextBoxCell = (DataGridViewTextBoxCell)dataGridViewGenerateInvoice.Rows[index].Cells["dataGridViewGenerateInvoiceType"];
                        typeTextBoxCell.Value = "Achat";

                        int product = (mEData.ArticleId.Value as int?).Value;
                        DataGridViewTextBoxCell productTextBoxCell = (DataGridViewTextBoxCell)dataGridViewGenerateInvoice.Rows[index].Cells["dataGridViewGenerateInvoiceProduct"];
                        Article article = TakeOversDS.Find(x => x.Id == 2).Articles.Find(x => x.Id == product);
                        productTextBoxCell.Value = article.Produit;
                    }

                    DataGridViewTextBoxCell imeiTextBoxCell = (DataGridViewTextBoxCell)dataGridViewGenerateInvoice.Rows[index].Cells["dataGridViewGenerateInvoiceIMEI"];
                    imeiTextBoxCell.Value = mEData.IMEI;

                    index++;
                }
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
                foreach (DataGridViewRow line in dataGridViewGenerateInvoice.Rows)
                {
                    if (!line.IsNewRow &&
                        (line.Cells["dataGridViewGenerateInvoiceAddItem"].Value as bool?).HasValue &&
                        (line.Cells["dataGridViewGenerateInvoiceAddItem"].Value as bool?).Value)
                    {
                        string type = (string)line.Cells["dataGridViewGenerateInvoiceType"].Value;
                        RepairType repairType = null;
                        UnlockType unlockType = null;
                        Article article = null;
                        if (string.Compare("Réparation", type) == 0)
                        {
                            repairType = TakeOversDS.Find(x => x.Id == 0).RepairTypes.Find(x => string.Compare(x.Name, (string)line.Cells["dataGridViewGenerateInvoiceProduct"].Value) == 0);
                        }
                        else if (string.Compare("Déblocage", type) == 0)
                        {
                            unlockType = TakeOversDS.Find(x => x.Id == 1).UnlockTypes.Find(x => string.Compare(x.Name, (string)line.Cells["dataGridViewGenerateInvoiceProduct"].Value) == 0);
                        }
                        else if (string.Compare("Achat", type) == 0)
                        {
                            article = TakeOversDS.Find(x => x.Id == 2).Articles.Find(x => string.Compare(x.Produit, (string)line.Cells["dataGridViewGenerateInvoiceProduct"].Value) == 0);
                        }
                        foreach (MEData mEData in MEDatas)
                        {
                            if (mEData.TakeOverId == (int)line.Cells["dataGridViewGenerateInvoiceTakeoverId"].Value &&
                                ((repairType != null && mEData.RepairTypeId == repairType.Id) ||
                                (unlockType != null && mEData.UnlockTypeId == unlockType.Id) ||
                                (article != null && mEData.ArticleId == article.Id)))
                            {
                                mEData.InvoiceId = InvoiceId;
                                mEData.State = mEData.State == TakeOverState.InProgress || mEData.State == TakeOverState.Done ? TakeOverState.PickedUp : mEData.State;
                            }
                        }
                    }
                }
                // Indiquer que le formulaire doit se fermer avec un résultat valide (OK)
                this.DialogResult = DialogResult.Yes;
                // Fermer le formulaire
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }

        public List<MEData> GetResult()
        {
            return MEDatas.Where(x => x.InvoiceId == InvoiceId)?.ToList();
        }
    }
}
