﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MobileExpress
{
    public partial class GarantieForm : Form
    {
        private List<Garantie> GarantiesTemp { get; set; } = new List<Garantie>();

        public GarantieForm(List<Garantie>  garantiesTemp)
        {
            InitializeComponent();

            GarantiesTemp = garantiesTemp;
            DataGridView_Load();
        }
        private void ClearDataGridView()
        {
            dataGridViewGarantie.DataSource = null;
            dataGridViewGarantie.Rows.Clear();
            dataGridViewGarantie.Columns.Clear();
            dataGridViewGarantie.AutoGenerateColumns = false;
            dataGridViewGarantie.AllowUserToDeleteRows = false;

            DataGridViewTextBoxColumn textBoxTabName = new DataGridViewTextBoxColumn
            {
                HeaderText = "Type",
                Name = "dataGridViewGarantieTabName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                ReadOnly = true,
            };
            dataGridViewGarantie.Columns.Add(textBoxTabName);

            DataGridViewTextBoxColumn textBoxId = new DataGridViewTextBoxColumn
            {
                Name = "dataGridViewGarantieId",
                ValueType = typeof(int),
                ReadOnly = true,
                Visible = false,
            };
            dataGridViewGarantie.Columns.Add(textBoxId);

            DataGridViewTextBoxColumn textBoxProductName = new DataGridViewTextBoxColumn
            {
                HeaderText = "Produit",
                Name = "dataGridViewGarantieProductName",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(string),
                ReadOnly = true,
            };
            dataGridViewGarantie.Columns.Add(textBoxProductName);

            DataGridViewTextBoxColumn textBoxMonths = new DataGridViewTextBoxColumn
            {
                HeaderText = "Mois garanties",
                Name = "dataGridViewGarantieMonths",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                DefaultCellStyle = new DataGridViewCellStyle { ForeColor = Color.Black },
                ValueType = typeof(int?),
                ReadOnly = false,
            };
            dataGridViewGarantie.Columns.Add(textBoxMonths);

            DataGridViewCheckBoxColumn checkBoxAdd = new DataGridViewCheckBoxColumn
            {
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                HeaderText = "Sélection",
                Name = "dataGridViewGarantieOption",
                TrueValue = true,
                ValueType = typeof(bool),
            };
            dataGridViewGarantie.Columns.Add(checkBoxAdd);
        }
        private void DataGridView_Load()
        {
            try
            {
                if (GarantiesTemp == null || !GarantiesTemp.Any())
                {
                    MessageBox.Show("Il n'y a pas de garantie à gérer. Veuillez reprendre la prise en charge ou la facture.", "Alerte", MessageBoxButtons.OK);
                    // Indiquer que le formulaire doit se fermer avec un résultat valide (OK)
                    this.DialogResult = DialogResult.Cancel;
                    // Fermer le formulaire
                    this.Close();
                    return;
                }

                ClearDataGridView();

                int index = 0;
                foreach (Garantie garantie in GarantiesTemp)
                {
                    dataGridViewGarantie.Rows.Add();

                    DataGridViewTextBoxCell textBoxTabName = (DataGridViewTextBoxCell)dataGridViewGarantie.Rows[index].Cells["dataGridViewGarantieTabName"];
                    textBoxTabName.Value = garantie.TabName;

                    DataGridViewTextBoxCell textBoxId = (DataGridViewTextBoxCell)dataGridViewGarantie.Rows[index].Cells["dataGridViewGarantieId"];
                    textBoxId.Value = garantie.Id;

                    DataGridViewTextBoxCell textBoxProductName = (DataGridViewTextBoxCell)dataGridViewGarantie.Rows[index].Cells["dataGridViewGarantieProductName"];
                    textBoxProductName.Value = garantie.ProductName;

                    DataGridViewTextBoxCell textBoxGarantieMonths = (DataGridViewTextBoxCell)dataGridViewGarantie.Rows[index].Cells["dataGridViewGarantieMonths"];
                    textBoxGarantieMonths.Value = garantie.Months;

                    DataGridViewCheckBoxCell checkBoxGarantieOption = (DataGridViewCheckBoxCell)dataGridViewGarantie.Rows[index].Cells["dataGridViewGarantieOption"];
                    checkBoxGarantieOption.Value = garantie.Option;

                    index++;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Erreur", MessageBoxButtons.OK);
            }
        }
        public List<Garantie> GetResult()
        {
            return GarantiesTemp;
        }
        private void buttonValidate_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dataGridViewGarantie.Rows)
                {
                    if (!row.IsNewRow &&
                        (row.Cells["dataGridViewGarantieId"].Value as int?).HasValue &&
                        (row.Cells["dataGridViewGarantieId"].Value as int?).Value > 0)
                    {
                        int id = (row.Cells["dataGridViewGarantieId"].Value as int?).Value;
                        int? garantieMonths = row.Cells["dataGridViewGarantieMonths"].Value as int?;
                        bool garantieOption = (row.Cells["dataGridViewGarantieOption"].Value as bool?) ?? false;

                        for (int i = 0; i < GarantiesTemp.Count; i++)
                        {
                            if (GarantiesTemp[i].Id == id)
                            {
                                GarantiesTemp[i].Months = garantieMonths;
                                GarantiesTemp[i].Option = garantieOption;
                            }
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
