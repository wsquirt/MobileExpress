namespace MobileExpress
{
    partial class GenerateInvoiceForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonGenerateInvoice = new System.Windows.Forms.Button();
            this.dataGridViewGenerateInvoice = new System.Windows.Forms.DataGridView();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGenerateInvoice)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.buttonGenerateInvoice, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 384);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(776, 54);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonGenerateInvoice
            // 
            this.buttonGenerateInvoice.Location = new System.Drawing.Point(3, 3);
            this.buttonGenerateInvoice.Name = "buttonGenerateInvoice";
            this.buttonGenerateInvoice.Size = new System.Drawing.Size(770, 48);
            this.buttonGenerateInvoice.TabIndex = 1;
            this.buttonGenerateInvoice.Text = "Générer une facture";
            this.buttonGenerateInvoice.UseVisualStyleBackColor = true;
            this.buttonGenerateInvoice.Click += new System.EventHandler(this.buttonGenerateInvoice_Click);
            // 
            // dataGridViewGenerateInvoice
            // 
            this.dataGridViewGenerateInvoice.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewGenerateInvoice.Location = new System.Drawing.Point(12, 12);
            this.dataGridViewGenerateInvoice.Name = "dataGridViewGenerateInvoice";
            this.dataGridViewGenerateInvoice.Size = new System.Drawing.Size(776, 366);
            this.dataGridViewGenerateInvoice.TabIndex = 1;
            // 
            // GenerateInvoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewGenerateInvoice);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "GenerateInvoiceForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewGenerateInvoice)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonGenerateInvoice;
        private System.Windows.Forms.DataGridView dataGridViewGenerateInvoice;
    }
}