using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MobileExpress
{
    public partial class RemiseForm : Form
    {
        private List<Remise> RemisesTemp { get; set; } = new List<Remise>();

        public RemiseForm(List<Remise> remisesTemp)
        {
            InitializeComponent();

            RemisesTemp = remisesTemp;
            DataGridView_Load();
        }
        private void ClearDataGridView()
        {
            dataGridViewRemise.DataSource = null;
            dataGridViewRemise.Rows.Clear();
            dataGridViewRemise.Columns.Clear();
            dataGridViewRemise.AutoGenerateColumns = false;
            dataGridViewRemise.AllowUserToDeleteRows = false;

            DataGridViewTextBoxColumn textBoxTabName = new DataGridViewTextBoxColumn
            {
                HeaderText = "Type",
                Name = "dataGridViewRemiseTabName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                ReadOnly = true,
            };
            dataGridViewRemise.Columns.Add(textBoxTabName);

            DataGridViewTextBoxColumn textBoxId = new DataGridViewTextBoxColumn
            {
                Name = "dataGridViewRemiseId",
                ValueType = typeof(int),
                ReadOnly = true,
                Visible = false,
            };
            dataGridViewRemise.Columns.Add(textBoxId);

            DataGridViewTextBoxColumn textBoxProductName = new DataGridViewTextBoxColumn
            {
                HeaderText = "Produit",
                Name = "dataGridViewRemiseProductName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                ReadOnly = true,
            };
            dataGridViewRemise.Columns.Add(textBoxProductName);

            DataGridViewTextBoxColumn textBoxPrix = new DataGridViewTextBoxColumn
            {
                HeaderText = "Prix",
                Name = "dataGridViewRemisePrix",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(decimal?),
                ReadOnly = false,
            };
            dataGridViewRemise.Columns.Add(textBoxPrix);
        }
        private void DataGridView_Load()
        {
            try
            {
                if (RemisesTemp == null || !RemisesTemp.Any())
                {
                    MessageBox.Show("Il n'y a pas de remise à gérer. Veuillez reprendre la prise en charge ou la facture.", "Alerte", MessageBoxButtons.OK);
                    return;
                }

                ClearDataGridView();

                int index = 0;
                foreach (Remise remise in RemisesTemp)
                {
                    dataGridViewRemise.Rows.Add();

                    DataGridViewTextBoxCell textBoxTabName = (DataGridViewTextBoxCell)dataGridViewRemise.Rows[index].Cells["dataGridViewRemiseTabName"];
                    textBoxTabName.Value = remise.TabName;

                    DataGridViewTextBoxCell textBoxId = (DataGridViewTextBoxCell)dataGridViewRemise.Rows[index].Cells["dataGridViewRemiseId"];
                    textBoxId.Value = remise.Id;

                    DataGridViewTextBoxCell textBoxProductName = (DataGridViewTextBoxCell)dataGridViewRemise.Rows[index].Cells["dataGridViewRemiseProductName"];
                    textBoxProductName.Value = remise.ProductName;

                    DataGridViewTextBoxCell textBoxPrix = (DataGridViewTextBoxCell)dataGridViewRemise.Rows[index].Cells["dataGridViewRemisePrix"];
                    textBoxPrix.Value = remise.Prix;

                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        public List<Remise> GetResult()
        {
            return RemisesTemp;
        }
        private void buttonValidate_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridViewRemise.Rows)
                {
                    if (!row.IsNewRow &&
                        (row.Cells["dataGridViewRemisePrix"].Value as decimal?).HasValue &&
                        (row.Cells["dataGridViewRemisePrix"].Value as decimal?).Value > 0)
                    {
                        int id = (row.Cells["dataGridViewRemiseId"].Value as int?).Value;
                        decimal? remisePrix = row.Cells["dataGridViewRemisePrix"].Value as decimal?;

                        Remise remise = RemisesTemp.FirstOrDefault(x => x.Id == id);
                        if (remise != null)
                        {
                            remise.Prix = remisePrix;
                        }
                    }
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
    }
}
