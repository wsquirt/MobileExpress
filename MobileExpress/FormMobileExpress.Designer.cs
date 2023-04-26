using System;
using System.Windows.Forms;

namespace MobileExpress
{
    partial class FormMobileExpress
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMobileExpress));
            this.printDialogCustomerOneInvoice = new System.Windows.Forms.PrintDialog();
            this.printPreviewDialogCustomerOneInvoice = new System.Windows.Forms.PrintPreviewDialog();
            this.printDocumentCustomerOneInvoice = new System.Drawing.Printing.PrintDocument();
            this.stockTabPage = new System.Windows.Forms.TabPage();
            this.dataGridViewStock = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonStockAddArticle = new System.Windows.Forms.Button();
            this.label23 = new System.Windows.Forms.Label();
            this.textBoxStockNewName = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.numericUpDownStockNewQuantity = new System.Windows.Forms.NumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.textBoxStockNewDescription = new System.Windows.Forms.TextBox();
            this.numericUpDownStockNewPrice = new System.Windows.Forms.NumericUpDown();
            this.textBoxStockSearch = new System.Windows.Forms.TextBox();
            this.label46 = new System.Windows.Forms.Label();
            this.customerRelationTabPage = new System.Windows.Forms.TabPage();
            this.dataGridViewCustomerRelationAll = new System.Windows.Forms.DataGridView();
            this.dataGridViewCustomerRelationOne = new System.Windows.Forms.DataGridView();
            this.labelCustomerRelationFullName = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.textBoxCustomerRelationLastName = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.textBoxCustomerRelationFirstName = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.textBoxCustomerRelationPhone = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.textBoxCustomerRelationEmailAddress = new System.Windows.Forms.TextBox();
            this.buttonCustomerRelationSaveCustomerData = new System.Windows.Forms.Button();
            this.label38 = new System.Windows.Forms.Label();
            this.labelCustomerOneInvoiceNumber = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.labelCustomerOneInvoicePrice = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.labelCustomerOneInvoiceType = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.labelCustomerOneInvoiceDate = new System.Windows.Forms.Label();
            this.buttonCustomerRelationGetInvoicePath = new System.Windows.Forms.Button();
            this.buttonCustomerRelationPrintInvoice = new System.Windows.Forms.Button();
            this.buttonCustomerRelationImportInvoice = new System.Windows.Forms.Button();
            this.textBoxCustomerSearchAll = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.tabPageTakeOver = new System.Windows.Forms.TabPage();
            this.label45 = new System.Windows.Forms.Label();
            this.dateTimePickerTakeOverDate = new System.Windows.Forms.DateTimePicker();
            this.label43 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.textBoxTakeOverLastName = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.textBoxTakeOverFirstName = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.textBoxTakeOverPhoneNumber = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.textBoxTakeOverTotalPrice = new System.Windows.Forms.TextBox();
            this.buttonTakeOverSave = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.buttonTakeOverGenerateInvoice = new System.Windows.Forms.Button();
            this.textBoxTakeOverEmailAddress = new System.Windows.Forms.TextBox();
            this.buttonTakeOverPrint = new System.Windows.Forms.Button();
            this.tabControlTakeOver = new System.Windows.Forms.TabControl();
            this.tabPageTakeOverInvoice = new System.Windows.Forms.TabPage();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.tabPageTakeOverReceipt = new System.Windows.Forms.TabPage();
            this.dataGridViewTakeOverReceipt = new System.Windows.Forms.DataGridView();
            this.invoiceNumberTakeOver = new System.Windows.Forms.TextBox();
            this.label41 = new System.Windows.Forms.Label();
            this.textBoxTakeOverAccompte = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.textBoxTakeOverResteDu = new System.Windows.Forms.TextBox();
            this.label48 = new System.Windows.Forms.Label();
            this.textBoxTakeOverPaid = new System.Windows.Forms.TextBox();
            this.tabControlAll = new System.Windows.Forms.TabControl();
            this.stockTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStock)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStockNewQuantity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStockNewPrice)).BeginInit();
            this.customerRelationTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomerRelationAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomerRelationOne)).BeginInit();
            this.tabPageTakeOver.SuspendLayout();
            this.tabControlTakeOver.SuspendLayout();
            this.tabPageTakeOverInvoice.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.tabPageTakeOverReceipt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTakeOverReceipt)).BeginInit();
            this.tabControlAll.SuspendLayout();
            this.SuspendLayout();
            // 
            // printDialogCustomerOneInvoice
            // 
            this.printDialogCustomerOneInvoice.UseEXDialog = true;
            // 
            // printPreviewDialogCustomerOneInvoice
            // 
            this.printPreviewDialogCustomerOneInvoice.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialogCustomerOneInvoice.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialogCustomerOneInvoice.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialogCustomerOneInvoice.Enabled = true;
            this.printPreviewDialogCustomerOneInvoice.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialogCustomerOneInvoice.Icon")));
            this.printPreviewDialogCustomerOneInvoice.Name = "printPreviewDialogCustomerOneInvoice";
            this.printPreviewDialogCustomerOneInvoice.Visible = false;
            // 
            // stockTabPage
            // 
            this.stockTabPage.Controls.Add(this.label46);
            this.stockTabPage.Controls.Add(this.textBoxStockSearch);
            this.stockTabPage.Controls.Add(this.groupBox1);
            this.stockTabPage.Controls.Add(this.dataGridViewStock);
            this.stockTabPage.Location = new System.Drawing.Point(4, 34);
            this.stockTabPage.Name = "stockTabPage";
            this.stockTabPage.Size = new System.Drawing.Size(1720, 713);
            this.stockTabPage.TabIndex = 3;
            this.stockTabPage.Text = "Stock";
            this.stockTabPage.UseVisualStyleBackColor = true;
            // 
            // dataGridViewStock
            // 
            this.dataGridViewStock.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewStock.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridViewStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStock.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewStock.Location = new System.Drawing.Point(3, 44);
            this.dataGridViewStock.Name = "dataGridViewStock";
            this.dataGridViewStock.Size = new System.Drawing.Size(1708, 543);
            this.dataGridViewStock.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.numericUpDownStockNewPrice);
            this.groupBox1.Controls.Add(this.textBoxStockNewDescription);
            this.groupBox1.Controls.Add(this.label31);
            this.groupBox1.Controls.Add(this.label30);
            this.groupBox1.Controls.Add(this.numericUpDownStockNewQuantity);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.textBoxStockNewName);
            this.groupBox1.Controls.Add(this.label23);
            this.groupBox1.Controls.Add(this.buttonStockAddArticle);
            this.groupBox1.Location = new System.Drawing.Point(3, 593);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1708, 122);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Ajouter un article";
            // 
            // buttonStockAddArticle
            // 
            this.buttonStockAddArticle.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonStockAddArticle.Location = new System.Drawing.Point(1476, 15);
            this.buttonStockAddArticle.Name = "buttonStockAddArticle";
            this.buttonStockAddArticle.Size = new System.Drawing.Size(226, 47);
            this.buttonStockAddArticle.TabIndex = 1;
            this.buttonStockAddArticle.Text = "Ajouter";
            this.buttonStockAddArticle.UseVisualStyleBackColor = true;
            this.buttonStockAddArticle.Click += new System.EventHandler(this.buttonStockAddArticle_Click);
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(6, 42);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(56, 25);
            this.label23.TabIndex = 8;
            this.label23.Text = "Nom";
            // 
            // textBoxStockNewName
            // 
            this.textBoxStockNewName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStockNewName.Location = new System.Drawing.Point(112, 39);
            this.textBoxStockNewName.Name = "textBoxStockNewName";
            this.textBoxStockNewName.Size = new System.Drawing.Size(523, 30);
            this.textBoxStockNewName.TabIndex = 9;
            // 
            // label22
            // 
            this.label22.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(732, 79);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(49, 25);
            this.label22.TabIndex = 10;
            this.label22.Text = "Prix";
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.button1.Location = new System.Drawing.Point(1476, 69);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(226, 47);
            this.button1.TabIndex = 12;
            this.button1.Text = "Scanner";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // numericUpDownStockNewQuantity
            // 
            this.numericUpDownStockNewQuantity.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numericUpDownStockNewQuantity.Location = new System.Drawing.Point(833, 39);
            this.numericUpDownStockNewQuantity.Name = "numericUpDownStockNewQuantity";
            this.numericUpDownStockNewQuantity.Size = new System.Drawing.Size(116, 30);
            this.numericUpDownStockNewQuantity.TabIndex = 63;
            // 
            // label30
            // 
            this.label30.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(732, 42);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(94, 25);
            this.label30.TabIndex = 64;
            this.label30.Text = "Quantité";
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(6, 82);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(120, 25);
            this.label31.TabIndex = 65;
            this.label31.Text = "Description";
            // 
            // textBoxStockNewDescription
            // 
            this.textBoxStockNewDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxStockNewDescription.Location = new System.Drawing.Point(112, 79);
            this.textBoxStockNewDescription.Name = "textBoxStockNewDescription";
            this.textBoxStockNewDescription.Size = new System.Drawing.Size(523, 30);
            this.textBoxStockNewDescription.TabIndex = 66;
            // 
            // numericUpDownStockNewPrice
            // 
            this.numericUpDownStockNewPrice.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.numericUpDownStockNewPrice.Location = new System.Drawing.Point(833, 77);
            this.numericUpDownStockNewPrice.Name = "numericUpDownStockNewPrice";
            this.numericUpDownStockNewPrice.Size = new System.Drawing.Size(116, 30);
            this.numericUpDownStockNewPrice.TabIndex = 67;
            // 
            // textBoxStockSearch
            // 
            this.textBoxStockSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStockSearch.Location = new System.Drawing.Point(283, 9);
            this.textBoxStockSearch.Name = "textBoxStockSearch";
            this.textBoxStockSearch.Size = new System.Drawing.Size(336, 29);
            this.textBoxStockSearch.TabIndex = 35;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.Location = new System.Drawing.Point(3, 12);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(274, 24);
            this.label46.TabIndex = 36;
            this.label46.Text = "Recherche par Nom de l\'Article";
            // 
            // customerRelationTabPage
            // 
            this.customerRelationTabPage.Controls.Add(this.label40);
            this.customerRelationTabPage.Controls.Add(this.textBoxCustomerSearchAll);
            this.customerRelationTabPage.Controls.Add(this.textBoxCustomerRelationEmailAddress);
            this.customerRelationTabPage.Controls.Add(this.textBoxCustomerRelationPhone);
            this.customerRelationTabPage.Controls.Add(this.textBoxCustomerRelationFirstName);
            this.customerRelationTabPage.Controls.Add(this.textBoxCustomerRelationLastName);
            this.customerRelationTabPage.Controls.Add(this.buttonCustomerRelationImportInvoice);
            this.customerRelationTabPage.Controls.Add(this.buttonCustomerRelationPrintInvoice);
            this.customerRelationTabPage.Controls.Add(this.buttonCustomerRelationGetInvoicePath);
            this.customerRelationTabPage.Controls.Add(this.labelCustomerOneInvoiceDate);
            this.customerRelationTabPage.Controls.Add(this.label47);
            this.customerRelationTabPage.Controls.Add(this.labelCustomerOneInvoiceType);
            this.customerRelationTabPage.Controls.Add(this.label44);
            this.customerRelationTabPage.Controls.Add(this.labelCustomerOneInvoicePrice);
            this.customerRelationTabPage.Controls.Add(this.label39);
            this.customerRelationTabPage.Controls.Add(this.labelCustomerOneInvoiceNumber);
            this.customerRelationTabPage.Controls.Add(this.label38);
            this.customerRelationTabPage.Controls.Add(this.buttonCustomerRelationSaveCustomerData);
            this.customerRelationTabPage.Controls.Add(this.label35);
            this.customerRelationTabPage.Controls.Add(this.label34);
            this.customerRelationTabPage.Controls.Add(this.label33);
            this.customerRelationTabPage.Controls.Add(this.label32);
            this.customerRelationTabPage.Controls.Add(this.labelCustomerRelationFullName);
            this.customerRelationTabPage.Controls.Add(this.dataGridViewCustomerRelationOne);
            this.customerRelationTabPage.Controls.Add(this.dataGridViewCustomerRelationAll);
            this.customerRelationTabPage.Location = new System.Drawing.Point(4, 34);
            this.customerRelationTabPage.Name = "customerRelationTabPage";
            this.customerRelationTabPage.Size = new System.Drawing.Size(1720, 713);
            this.customerRelationTabPage.TabIndex = 2;
            this.customerRelationTabPage.Text = "Relation Client";
            this.customerRelationTabPage.UseVisualStyleBackColor = true;
            // 
            // dataGridViewCustomerRelationAll
            // 
            this.dataGridViewCustomerRelationAll.AllowUserToAddRows = false;
            this.dataGridViewCustomerRelationAll.AllowUserToDeleteRows = false;
            this.dataGridViewCustomerRelationAll.AllowUserToOrderColumns = true;
            this.dataGridViewCustomerRelationAll.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewCustomerRelationAll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCustomerRelationAll.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewCustomerRelationAll.Location = new System.Drawing.Point(3, 56);
            this.dataGridViewCustomerRelationAll.Name = "dataGridViewCustomerRelationAll";
            this.dataGridViewCustomerRelationAll.ReadOnly = true;
            this.dataGridViewCustomerRelationAll.ShowCellToolTips = false;
            this.dataGridViewCustomerRelationAll.ShowEditingIcon = false;
            this.dataGridViewCustomerRelationAll.Size = new System.Drawing.Size(1304, 300);
            this.dataGridViewCustomerRelationAll.TabIndex = 0;
            // 
            // dataGridViewCustomerRelationOne
            // 
            this.dataGridViewCustomerRelationOne.AllowUserToAddRows = false;
            this.dataGridViewCustomerRelationOne.AllowUserToDeleteRows = false;
            this.dataGridViewCustomerRelationOne.AllowUserToOrderColumns = true;
            this.dataGridViewCustomerRelationOne.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridViewCustomerRelationOne.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCustomerRelationOne.Location = new System.Drawing.Point(3, 371);
            this.dataGridViewCustomerRelationOne.Name = "dataGridViewCustomerRelationOne";
            this.dataGridViewCustomerRelationOne.ReadOnly = true;
            this.dataGridViewCustomerRelationOne.ShowCellToolTips = false;
            this.dataGridViewCustomerRelationOne.ShowEditingIcon = false;
            this.dataGridViewCustomerRelationOne.Size = new System.Drawing.Size(1304, 344);
            this.dataGridViewCustomerRelationOne.TabIndex = 1;
            // 
            // labelCustomerRelationFullName
            // 
            this.labelCustomerRelationFullName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCustomerRelationFullName.AutoSize = true;
            this.labelCustomerRelationFullName.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomerRelationFullName.Location = new System.Drawing.Point(1337, 56);
            this.labelCustomerRelationFullName.Name = "labelCustomerRelationFullName";
            this.labelCustomerRelationFullName.Size = new System.Drawing.Size(146, 26);
            this.labelCustomerRelationFullName.TabIndex = 2;
            this.labelCustomerRelationFullName.Text = "Prénom NOM";
            // 
            // label32
            // 
            this.label32.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(1338, 119);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(51, 24);
            this.label32.TabIndex = 3;
            this.label32.Text = "Nom";
            // 
            // textBoxCustomerRelationLastName
            // 
            this.textBoxCustomerRelationLastName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxCustomerRelationLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCustomerRelationLastName.Location = new System.Drawing.Point(1463, 116);
            this.textBoxCustomerRelationLastName.Name = "textBoxCustomerRelationLastName";
            this.textBoxCustomerRelationLastName.Size = new System.Drawing.Size(254, 29);
            this.textBoxCustomerRelationLastName.TabIndex = 4;
            // 
            // label33
            // 
            this.label33.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(1338, 168);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(77, 24);
            this.label33.TabIndex = 5;
            this.label33.Text = "Prénom";
            // 
            // textBoxCustomerRelationFirstName
            // 
            this.textBoxCustomerRelationFirstName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxCustomerRelationFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCustomerRelationFirstName.Location = new System.Drawing.Point(1463, 165);
            this.textBoxCustomerRelationFirstName.Name = "textBoxCustomerRelationFirstName";
            this.textBoxCustomerRelationFirstName.Size = new System.Drawing.Size(254, 29);
            this.textBoxCustomerRelationFirstName.TabIndex = 6;
            // 
            // label34
            // 
            this.label34.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(1338, 217);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(103, 24);
            this.label34.TabIndex = 7;
            this.label34.Text = "Téléphone";
            // 
            // textBoxCustomerRelationPhone
            // 
            this.textBoxCustomerRelationPhone.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxCustomerRelationPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCustomerRelationPhone.Location = new System.Drawing.Point(1463, 214);
            this.textBoxCustomerRelationPhone.Name = "textBoxCustomerRelationPhone";
            this.textBoxCustomerRelationPhone.Size = new System.Drawing.Size(254, 29);
            this.textBoxCustomerRelationPhone.TabIndex = 8;
            // 
            // label35
            // 
            this.label35.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(1338, 267);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(119, 24);
            this.label35.TabIndex = 9;
            this.label35.Text = "Adresse mail";
            // 
            // textBoxCustomerRelationEmailAddress
            // 
            this.textBoxCustomerRelationEmailAddress.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxCustomerRelationEmailAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCustomerRelationEmailAddress.Location = new System.Drawing.Point(1463, 264);
            this.textBoxCustomerRelationEmailAddress.Name = "textBoxCustomerRelationEmailAddress";
            this.textBoxCustomerRelationEmailAddress.Size = new System.Drawing.Size(254, 29);
            this.textBoxCustomerRelationEmailAddress.TabIndex = 10;
            // 
            // buttonCustomerRelationSaveCustomerData
            // 
            this.buttonCustomerRelationSaveCustomerData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomerRelationSaveCustomerData.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCustomerRelationSaveCustomerData.Location = new System.Drawing.Point(1342, 310);
            this.buttonCustomerRelationSaveCustomerData.Name = "buttonCustomerRelationSaveCustomerData";
            this.buttonCustomerRelationSaveCustomerData.Size = new System.Drawing.Size(312, 46);
            this.buttonCustomerRelationSaveCustomerData.TabIndex = 11;
            this.buttonCustomerRelationSaveCustomerData.Text = "Sauvegarder";
            this.buttonCustomerRelationSaveCustomerData.UseVisualStyleBackColor = true;
            this.buttonCustomerRelationSaveCustomerData.Click += new System.EventHandler(this.buttonCustomerRelationSaveCustomerData_Click);
            // 
            // label38
            // 
            this.label38.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.Location = new System.Drawing.Point(1337, 369);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(147, 26);
            this.label38.TabIndex = 12;
            this.label38.Text = "Facturation n°";
            // 
            // labelCustomerOneInvoiceNumber
            // 
            this.labelCustomerOneInvoiceNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCustomerOneInvoiceNumber.AutoSize = true;
            this.labelCustomerOneInvoiceNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomerOneInvoiceNumber.Location = new System.Drawing.Point(1490, 371);
            this.labelCustomerOneInvoiceNumber.Name = "labelCustomerOneInvoiceNumber";
            this.labelCustomerOneInvoiceNumber.Size = new System.Drawing.Size(24, 26);
            this.labelCustomerOneInvoiceNumber.TabIndex = 13;
            this.labelCustomerOneInvoiceNumber.Text = "0";
            // 
            // label39
            // 
            this.label39.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.Location = new System.Drawing.Point(1338, 408);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(57, 24);
            this.label39.TabIndex = 14;
            this.label39.Text = "Prix : ";
            // 
            // labelCustomerOneInvoicePrice
            // 
            this.labelCustomerOneInvoicePrice.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCustomerOneInvoicePrice.AutoSize = true;
            this.labelCustomerOneInvoicePrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomerOneInvoicePrice.Location = new System.Drawing.Point(1492, 408);
            this.labelCustomerOneInvoicePrice.Name = "labelCustomerOneInvoicePrice";
            this.labelCustomerOneInvoicePrice.Size = new System.Drawing.Size(35, 24);
            this.labelCustomerOneInvoicePrice.TabIndex = 15;
            this.labelCustomerOneInvoicePrice.Text = "0.0";
            // 
            // label44
            // 
            this.label44.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.Location = new System.Drawing.Point(1338, 441);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(156, 24);
            this.label44.TabIndex = 17;
            this.label44.Text = "Type de facture : ";
            // 
            // labelCustomerOneInvoiceType
            // 
            this.labelCustomerOneInvoiceType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCustomerOneInvoiceType.AutoSize = true;
            this.labelCustomerOneInvoiceType.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomerOneInvoiceType.Location = new System.Drawing.Point(1500, 441);
            this.labelCustomerOneInvoiceType.Name = "labelCustomerOneInvoiceType";
            this.labelCustomerOneInvoiceType.Size = new System.Drawing.Size(53, 24);
            this.labelCustomerOneInvoiceType.TabIndex = 18;
            this.labelCustomerOneInvoiceType.Text = "Type";
            // 
            // label47
            // 
            this.label47.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label47.AutoSize = true;
            this.label47.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label47.Location = new System.Drawing.Point(1338, 478);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(210, 24);
            this.label47.TabIndex = 20;
            this.label47.Text = "Date de la commande : ";
            // 
            // labelCustomerOneInvoiceDate
            // 
            this.labelCustomerOneInvoiceDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelCustomerOneInvoiceDate.AutoSize = true;
            this.labelCustomerOneInvoiceDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCustomerOneInvoiceDate.Location = new System.Drawing.Point(1554, 478);
            this.labelCustomerOneInvoiceDate.Name = "labelCustomerOneInvoiceDate";
            this.labelCustomerOneInvoiceDate.Size = new System.Drawing.Size(100, 24);
            this.labelCustomerOneInvoiceDate.TabIndex = 21;
            this.labelCustomerOneInvoiceDate.Text = "01/01/2000";
            // 
            // buttonCustomerRelationGetInvoicePath
            // 
            this.buttonCustomerRelationGetInvoicePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomerRelationGetInvoicePath.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCustomerRelationGetInvoicePath.Location = new System.Drawing.Point(1342, 515);
            this.buttonCustomerRelationGetInvoicePath.Name = "buttonCustomerRelationGetInvoicePath";
            this.buttonCustomerRelationGetInvoicePath.Size = new System.Drawing.Size(312, 42);
            this.buttonCustomerRelationGetInvoicePath.TabIndex = 30;
            this.buttonCustomerRelationGetInvoicePath.Text = "Chemin vers facture";
            this.buttonCustomerRelationGetInvoicePath.UseVisualStyleBackColor = true;
            this.buttonCustomerRelationGetInvoicePath.Click += new System.EventHandler(this.buttonCustomerRelationGetInvoicePath_Click);
            // 
            // buttonCustomerRelationPrintInvoice
            // 
            this.buttonCustomerRelationPrintInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomerRelationPrintInvoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCustomerRelationPrintInvoice.Location = new System.Drawing.Point(1342, 563);
            this.buttonCustomerRelationPrintInvoice.Name = "buttonCustomerRelationPrintInvoice";
            this.buttonCustomerRelationPrintInvoice.Size = new System.Drawing.Size(312, 42);
            this.buttonCustomerRelationPrintInvoice.TabIndex = 31;
            this.buttonCustomerRelationPrintInvoice.Text = "Imprimer la facture";
            this.buttonCustomerRelationPrintInvoice.UseVisualStyleBackColor = true;
            this.buttonCustomerRelationPrintInvoice.Click += new System.EventHandler(this.buttonCustomerRelationPrintInvoice_Click);
            // 
            // buttonCustomerRelationImportInvoice
            // 
            this.buttonCustomerRelationImportInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomerRelationImportInvoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCustomerRelationImportInvoice.Location = new System.Drawing.Point(1342, 611);
            this.buttonCustomerRelationImportInvoice.Name = "buttonCustomerRelationImportInvoice";
            this.buttonCustomerRelationImportInvoice.Size = new System.Drawing.Size(312, 42);
            this.buttonCustomerRelationImportInvoice.TabIndex = 32;
            this.buttonCustomerRelationImportInvoice.Text = "Importer une facture";
            this.buttonCustomerRelationImportInvoice.UseVisualStyleBackColor = true;
            // 
            // textBoxCustomerSearchAll
            // 
            this.textBoxCustomerSearchAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCustomerSearchAll.Location = new System.Drawing.Point(469, 17);
            this.textBoxCustomerSearchAll.Name = "textBoxCustomerSearchAll";
            this.textBoxCustomerSearchAll.Size = new System.Drawing.Size(336, 29);
            this.textBoxCustomerSearchAll.TabIndex = 33;
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.Location = new System.Drawing.Point(3, 20);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(460, 24);
            this.label40.TabIndex = 34;
            this.label40.Text = "Recherche par (nom, prénom, numéro de téléphone)";
            // 
            // tabPageTakeOver
            // 
            this.tabPageTakeOver.Controls.Add(this.textBoxTakeOverPaid);
            this.tabPageTakeOver.Controls.Add(this.textBoxTakeOverResteDu);
            this.tabPageTakeOver.Controls.Add(this.textBoxTakeOverAccompte);
            this.tabPageTakeOver.Controls.Add(this.invoiceNumberTakeOver);
            this.tabPageTakeOver.Controls.Add(this.textBoxTakeOverEmailAddress);
            this.tabPageTakeOver.Controls.Add(this.textBoxTakeOverTotalPrice);
            this.tabPageTakeOver.Controls.Add(this.textBoxTakeOverPhoneNumber);
            this.tabPageTakeOver.Controls.Add(this.textBoxTakeOverFirstName);
            this.tabPageTakeOver.Controls.Add(this.textBoxTakeOverLastName);
            this.tabPageTakeOver.Controls.Add(this.label48);
            this.tabPageTakeOver.Controls.Add(this.label17);
            this.tabPageTakeOver.Controls.Add(this.label41);
            this.tabPageTakeOver.Controls.Add(this.tabControlTakeOver);
            this.tabPageTakeOver.Controls.Add(this.buttonTakeOverPrint);
            this.tabPageTakeOver.Controls.Add(this.buttonTakeOverGenerateInvoice);
            this.tabPageTakeOver.Controls.Add(this.label16);
            this.tabPageTakeOver.Controls.Add(this.buttonTakeOverSave);
            this.tabPageTakeOver.Controls.Add(this.label18);
            this.tabPageTakeOver.Controls.Add(this.label20);
            this.tabPageTakeOver.Controls.Add(this.label21);
            this.tabPageTakeOver.Controls.Add(this.label27);
            this.tabPageTakeOver.Controls.Add(this.label43);
            this.tabPageTakeOver.Controls.Add(this.dateTimePickerTakeOverDate);
            this.tabPageTakeOver.Controls.Add(this.label45);
            this.tabPageTakeOver.Location = new System.Drawing.Point(4, 34);
            this.tabPageTakeOver.Name = "tabPageTakeOver";
            this.tabPageTakeOver.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTakeOver.Size = new System.Drawing.Size(1720, 713);
            this.tabPageTakeOver.TabIndex = 5;
            this.tabPageTakeOver.Text = "Prise en charge";
            this.tabPageTakeOver.UseVisualStyleBackColor = true;
            // 
            // label45
            // 
            this.label45.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.Location = new System.Drawing.Point(7, 77);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(165, 24);
            this.label45.TabIndex = 68;
            this.label45.Text = "Date de facturation";
            // 
            // dateTimePickerTakeOverDate
            // 
            this.dateTimePickerTakeOverDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerTakeOverDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerTakeOverDate.Location = new System.Drawing.Point(178, 73);
            this.dateTimePickerTakeOverDate.Name = "dateTimePickerTakeOverDate";
            this.dateTimePickerTakeOverDate.Size = new System.Drawing.Size(297, 29);
            this.dateTimePickerTakeOverDate.TabIndex = 69;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.Location = new System.Drawing.Point(749, 12);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(191, 26);
            this.label43.TabIndex = 70;
            this.label43.Text = "Prise en charge n°";
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(7, 152);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(51, 24);
            this.label27.TabIndex = 72;
            this.label27.Text = "Nom";
            // 
            // textBoxTakeOverLastName
            // 
            this.textBoxTakeOverLastName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTakeOverLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTakeOverLastName.Location = new System.Drawing.Point(178, 149);
            this.textBoxTakeOverLastName.Name = "textBoxTakeOverLastName";
            this.textBoxTakeOverLastName.Size = new System.Drawing.Size(297, 29);
            this.textBoxTakeOverLastName.TabIndex = 73;
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(6, 199);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(77, 24);
            this.label21.TabIndex = 74;
            this.label21.Text = "Prénom";
            // 
            // textBoxTakeOverFirstName
            // 
            this.textBoxTakeOverFirstName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTakeOverFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTakeOverFirstName.Location = new System.Drawing.Point(178, 196);
            this.textBoxTakeOverFirstName.Name = "textBoxTakeOverFirstName";
            this.textBoxTakeOverFirstName.Size = new System.Drawing.Size(297, 29);
            this.textBoxTakeOverFirstName.TabIndex = 75;
            // 
            // label20
            // 
            this.label20.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.Location = new System.Drawing.Point(7, 241);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(79, 24);
            this.label20.TabIndex = 76;
            this.label20.Text = "Numéro";
            // 
            // textBoxTakeOverPhoneNumber
            // 
            this.textBoxTakeOverPhoneNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTakeOverPhoneNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTakeOverPhoneNumber.Location = new System.Drawing.Point(178, 238);
            this.textBoxTakeOverPhoneNumber.Name = "textBoxTakeOverPhoneNumber";
            this.textBoxTakeOverPhoneNumber.Size = new System.Drawing.Size(297, 29);
            this.textBoxTakeOverPhoneNumber.TabIndex = 77;
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(7, 547);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(80, 24);
            this.label18.TabIndex = 78;
            this.label18.Text = "Prix total";
            // 
            // textBoxTakeOverTotalPrice
            // 
            this.textBoxTakeOverTotalPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTakeOverTotalPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTakeOverTotalPrice.Location = new System.Drawing.Point(179, 544);
            this.textBoxTakeOverTotalPrice.Name = "textBoxTakeOverTotalPrice";
            this.textBoxTakeOverTotalPrice.Size = new System.Drawing.Size(296, 29);
            this.textBoxTakeOverTotalPrice.TabIndex = 79;
            this.textBoxTakeOverTotalPrice.Text = "0";
            // 
            // buttonTakeOverSave
            // 
            this.buttonTakeOverSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTakeOverSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTakeOverSave.Location = new System.Drawing.Point(581, 651);
            this.buttonTakeOverSave.Name = "buttonTakeOverSave";
            this.buttonTakeOverSave.Size = new System.Drawing.Size(191, 58);
            this.buttonTakeOverSave.TabIndex = 82;
            this.buttonTakeOverSave.Text = "Sauvegarder";
            this.buttonTakeOverSave.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(6, 286);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(119, 24);
            this.label16.TabIndex = 84;
            this.label16.Text = "Adresse mail";
            // 
            // buttonTakeOverGenerateInvoice
            // 
            this.buttonTakeOverGenerateInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTakeOverGenerateInvoice.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTakeOverGenerateInvoice.Location = new System.Drawing.Point(778, 651);
            this.buttonTakeOverGenerateInvoice.Name = "buttonTakeOverGenerateInvoice";
            this.buttonTakeOverGenerateInvoice.Size = new System.Drawing.Size(191, 58);
            this.buttonTakeOverGenerateInvoice.TabIndex = 86;
            this.buttonTakeOverGenerateInvoice.Text = "Générer une facture";
            this.buttonTakeOverGenerateInvoice.UseVisualStyleBackColor = true;
            // 
            // textBoxTakeOverEmailAddress
            // 
            this.textBoxTakeOverEmailAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTakeOverEmailAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTakeOverEmailAddress.Location = new System.Drawing.Point(178, 283);
            this.textBoxTakeOverEmailAddress.Name = "textBoxTakeOverEmailAddress";
            this.textBoxTakeOverEmailAddress.Size = new System.Drawing.Size(297, 29);
            this.textBoxTakeOverEmailAddress.TabIndex = 85;
            // 
            // buttonTakeOverPrint
            // 
            this.buttonTakeOverPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTakeOverPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTakeOverPrint.Location = new System.Drawing.Point(975, 651);
            this.buttonTakeOverPrint.Name = "buttonTakeOverPrint";
            this.buttonTakeOverPrint.Size = new System.Drawing.Size(191, 57);
            this.buttonTakeOverPrint.TabIndex = 87;
            this.buttonTakeOverPrint.Text = "Imprimer";
            this.buttonTakeOverPrint.UseVisualStyleBackColor = true;
            // 
            // tabControlTakeOver
            // 
            this.tabControlTakeOver.Controls.Add(this.tabPageTakeOverReceipt);
            this.tabControlTakeOver.Controls.Add(this.tabPageTakeOverInvoice);
            this.tabControlTakeOver.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlTakeOver.Location = new System.Drawing.Point(481, 48);
            this.tabControlTakeOver.Name = "tabControlTakeOver";
            this.tabControlTakeOver.SelectedIndex = 0;
            this.tabControlTakeOver.Size = new System.Drawing.Size(1233, 587);
            this.tabControlTakeOver.TabIndex = 88;
            // 
            // tabPageTakeOverInvoice
            // 
            this.tabPageTakeOverInvoice.Controls.Add(this.dataGridView2);
            this.tabPageTakeOverInvoice.Location = new System.Drawing.Point(4, 29);
            this.tabPageTakeOverInvoice.Name = "tabPageTakeOverInvoice";
            this.tabPageTakeOverInvoice.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTakeOverInvoice.Size = new System.Drawing.Size(1225, 554);
            this.tabPageTakeOverInvoice.TabIndex = 1;
            this.tabPageTakeOverInvoice.Text = "Facturation";
            this.tabPageTakeOverInvoice.UseVisualStyleBackColor = true;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(6, 6);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(1213, 537);
            this.dataGridView2.TabIndex = 1;
            // 
            // tabPageTakeOverReceipt
            // 
            this.tabPageTakeOverReceipt.Controls.Add(this.dataGridViewTakeOverReceipt);
            this.tabPageTakeOverReceipt.ForeColor = System.Drawing.Color.White;
            this.tabPageTakeOverReceipt.Location = new System.Drawing.Point(4, 29);
            this.tabPageTakeOverReceipt.Name = "tabPageTakeOverReceipt";
            this.tabPageTakeOverReceipt.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTakeOverReceipt.Size = new System.Drawing.Size(1225, 554);
            this.tabPageTakeOverReceipt.TabIndex = 0;
            this.tabPageTakeOverReceipt.Text = "Récépicé";
            this.tabPageTakeOverReceipt.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTakeOverReceipt
            // 
            this.dataGridViewTakeOverReceipt.BackgroundColor = System.Drawing.Color.LightGray;
            this.dataGridViewTakeOverReceipt.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTakeOverReceipt.Location = new System.Drawing.Point(3, 6);
            this.dataGridViewTakeOverReceipt.Name = "dataGridViewTakeOverReceipt";
            this.dataGridViewTakeOverReceipt.Size = new System.Drawing.Size(1216, 537);
            this.dataGridViewTakeOverReceipt.TabIndex = 0;
            this.dataGridViewTakeOverReceipt.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewTakeOverReceipt_CellValueChanged);
            this.dataGridViewTakeOverReceipt.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridViewTakeOverReceipt_CurrentCellDirtyStateChanged);
            // 
            // invoiceNumberTakeOver
            // 
            this.invoiceNumberTakeOver.Location = new System.Drawing.Point(946, 12);
            this.invoiceNumberTakeOver.Name = "invoiceNumberTakeOver";
            this.invoiceNumberTakeOver.Size = new System.Drawing.Size(100, 30);
            this.invoiceNumberTakeOver.TabIndex = 89;
            this.invoiceNumberTakeOver.Text = "0";
            // 
            // label41
            // 
            this.label41.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(6, 405);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(96, 24);
            this.label41.TabIndex = 90;
            this.label41.Text = "Accompte";
            // 
            // textBoxTakeOverAccompte
            // 
            this.textBoxTakeOverAccompte.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTakeOverAccompte.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTakeOverAccompte.Location = new System.Drawing.Point(178, 402);
            this.textBoxTakeOverAccompte.Name = "textBoxTakeOverAccompte";
            this.textBoxTakeOverAccompte.Size = new System.Drawing.Size(297, 29);
            this.textBoxTakeOverAccompte.TabIndex = 91;
            this.textBoxTakeOverAccompte.Text = "0";
            this.textBoxTakeOverAccompte.TextChanged += new System.EventHandler(this.TextBoxTakeOverAccompte_TextChanged);
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(6, 453);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(85, 24);
            this.label17.TabIndex = 92;
            this.label17.Text = "Reste dû";
            // 
            // textBoxTakeOverResteDu
            // 
            this.textBoxTakeOverResteDu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTakeOverResteDu.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTakeOverResteDu.Location = new System.Drawing.Point(179, 450);
            this.textBoxTakeOverResteDu.Name = "textBoxTakeOverResteDu";
            this.textBoxTakeOverResteDu.Size = new System.Drawing.Size(296, 29);
            this.textBoxTakeOverResteDu.TabIndex = 93;
            this.textBoxTakeOverResteDu.Text = "0";
            // 
            // label48
            // 
            this.label48.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label48.AutoSize = true;
            this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label48.Location = new System.Drawing.Point(6, 500);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(52, 24);
            this.label48.TabIndex = 94;
            this.label48.Text = "Payé";
            // 
            // textBoxTakeOverPaid
            // 
            this.textBoxTakeOverPaid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTakeOverPaid.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTakeOverPaid.Location = new System.Drawing.Point(179, 497);
            this.textBoxTakeOverPaid.Name = "textBoxTakeOverPaid";
            this.textBoxTakeOverPaid.Size = new System.Drawing.Size(296, 29);
            this.textBoxTakeOverPaid.TabIndex = 95;
            this.textBoxTakeOverPaid.Text = "0";
            this.textBoxTakeOverPaid.TextChanged += new System.EventHandler(this.TextBoxTakeOverPaid_TextChanged);
            // 
            // tabControlAll
            // 
            this.tabControlAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlAll.Controls.Add(this.tabPageTakeOver);
            this.tabControlAll.Controls.Add(this.customerRelationTabPage);
            this.tabControlAll.Controls.Add(this.stockTabPage);
            this.tabControlAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlAll.Location = new System.Drawing.Point(12, 12);
            this.tabControlAll.Name = "tabControlAll";
            this.tabControlAll.SelectedIndex = 0;
            this.tabControlAll.Size = new System.Drawing.Size(1728, 751);
            this.tabControlAll.TabIndex = 0;
            // 
            // FormMobileExpress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1752, 775);
            this.Controls.Add(this.tabControlAll);
            this.Name = "FormMobileExpress";
            this.Text = "Form1";
            this.stockTabPage.ResumeLayout(false);
            this.stockTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStock)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStockNewQuantity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownStockNewPrice)).EndInit();
            this.customerRelationTabPage.ResumeLayout(false);
            this.customerRelationTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomerRelationAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomerRelationOne)).EndInit();
            this.tabPageTakeOver.ResumeLayout(false);
            this.tabPageTakeOver.PerformLayout();
            this.tabControlTakeOver.ResumeLayout(false);
            this.tabPageTakeOverInvoice.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.tabPageTakeOverReceipt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTakeOverReceipt)).EndInit();
            this.tabControlAll.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private PrintDialog printDialogCustomerOneInvoice;
        private PrintPreviewDialog printPreviewDialogCustomerOneInvoice;
        private System.Drawing.Printing.PrintDocument printDocumentCustomerOneInvoice;
        private TabPage stockTabPage;
        private Label label46;
        private TextBox textBoxStockSearch;
        private GroupBox groupBox1;
        private NumericUpDown numericUpDownStockNewPrice;
        private TextBox textBoxStockNewDescription;
        private Label label31;
        private Label label30;
        private NumericUpDown numericUpDownStockNewQuantity;
        private Button button1;
        private Label label22;
        private TextBox textBoxStockNewName;
        private Label label23;
        private Button buttonStockAddArticle;
        private DataGridView dataGridViewStock;
        private TabPage customerRelationTabPage;
        private Label label40;
        private TextBox textBoxCustomerSearchAll;
        private TextBox textBoxCustomerRelationEmailAddress;
        private TextBox textBoxCustomerRelationPhone;
        private TextBox textBoxCustomerRelationFirstName;
        private TextBox textBoxCustomerRelationLastName;
        private Button buttonCustomerRelationImportInvoice;
        private Button buttonCustomerRelationPrintInvoice;
        private Button buttonCustomerRelationGetInvoicePath;
        private Label labelCustomerOneInvoiceDate;
        private Label label47;
        private Label labelCustomerOneInvoiceType;
        private Label label44;
        private Label labelCustomerOneInvoicePrice;
        private Label label39;
        private Label labelCustomerOneInvoiceNumber;
        private Label label38;
        private Button buttonCustomerRelationSaveCustomerData;
        private Label label35;
        private Label label34;
        private Label label33;
        private Label label32;
        private Label labelCustomerRelationFullName;
        private DataGridView dataGridViewCustomerRelationOne;
        private DataGridView dataGridViewCustomerRelationAll;
        private TabPage tabPageTakeOver;
        private TextBox textBoxTakeOverPaid;
        private TextBox textBoxTakeOverResteDu;
        private TextBox textBoxTakeOverAccompte;
        private TextBox invoiceNumberTakeOver;
        private TextBox textBoxTakeOverEmailAddress;
        private TextBox textBoxTakeOverTotalPrice;
        private TextBox textBoxTakeOverPhoneNumber;
        private TextBox textBoxTakeOverFirstName;
        private TextBox textBoxTakeOverLastName;
        private Label label48;
        private Label label17;
        private Label label41;
        private TabControl tabControlTakeOver;
        private TabPage tabPageTakeOverReceipt;
        private DataGridView dataGridViewTakeOverReceipt;
        private TabPage tabPageTakeOverInvoice;
        private DataGridView dataGridView2;
        private Button buttonTakeOverPrint;
        private Button buttonTakeOverGenerateInvoice;
        private Label label16;
        private Button buttonTakeOverSave;
        private Label label18;
        private Label label20;
        private Label label21;
        private Label label27;
        private Label label43;
        private DateTimePicker dateTimePickerTakeOverDate;
        private Label label45;
        private TabControl tabControlAll;
    }
}

