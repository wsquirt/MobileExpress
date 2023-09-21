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
            this.stockTabPage = new System.Windows.Forms.TabPage();
            this.buttonStockAdd = new System.Windows.Forms.Button();
            this.label46 = new System.Windows.Forms.Label();
            this.textBoxStockSearch = new System.Windows.Forms.TextBox();
            this.dataGridViewStock = new System.Windows.Forms.DataGridView();
            this.customerRelationTabPage = new System.Windows.Forms.TabPage();
            this.dataGridViewCustomerRelationAll = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label35 = new System.Windows.Forms.Label();
            this.textBoxCustomerRelationEmailAddress = new System.Windows.Forms.TextBox();
            this.textBoxCustomerRelationPhone = new System.Windows.Forms.TextBox();
            this.label34 = new System.Windows.Forms.Label();
            this.textBoxCustomerRelationFirstName = new System.Windows.Forms.TextBox();
            this.textBoxCustomerRelationLastName = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxCustomerRelationSexe = new System.Windows.Forms.ComboBox();
            this.buttonCustomerRelationSaveCustomerData = new System.Windows.Forms.Button();
            this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxCustomerSearchAll = new System.Windows.Forms.TextBox();
            this.label40 = new System.Windows.Forms.Label();
            this.tabPageTakeOver = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.takeOverNumber = new System.Windows.Forms.TextBox();
            this.buttonTakeOverReset = new System.Windows.Forms.Button();
            this.label43 = new System.Windows.Forms.Label();
            this.buttonTakeOverSearch = new System.Windows.Forms.Button();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonTakeOverSave = new System.Windows.Forms.Button();
            this.buttonTakeOverManageRemise = new System.Windows.Forms.Button();
            this.buttonTakeOverManageGarantie = new System.Windows.Forms.Button();
            this.buttonTakeOverScanner = new System.Windows.Forms.Button();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label45 = new System.Windows.Forms.Label();
            this.textBoxTakeOverAccompte = new System.Windows.Forms.TextBox();
            this.textBoxTakeOverPaid = new System.Windows.Forms.TextBox();
            this.dateTimePickerTakeOverDate = new System.Windows.Forms.DateTimePicker();
            this.label41 = new System.Windows.Forms.Label();
            this.textBoxTakeOverTotalPrice = new System.Windows.Forms.TextBox();
            this.textBoxTakeOverResteDu = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.label48 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.tableLayoutPanel8 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonAddCustomer = new System.Windows.Forms.Button();
            this.buttonUpdateCustomer = new System.Windows.Forms.Button();
            this.checkBoxEspece = new System.Windows.Forms.CheckBox();
            this.checkBoxCb = new System.Windows.Forms.CheckBox();
            this.checkBoxVirement = new System.Windows.Forms.CheckBox();
            this.comboBoxTakeOverCustomer = new System.Windows.Forms.ComboBox();
            this.tabControlTakeOver = new System.Windows.Forms.TabControl();
            this.tabPageTakeOverRepair = new System.Windows.Forms.TabPage();
            this.dataGridViewTakeOverRepair = new System.Windows.Forms.DataGridView();
            this.tabPageTakeOverUnlock = new System.Windows.Forms.TabPage();
            this.dataGridViewTakeOverUnlock = new System.Windows.Forms.DataGridView();
            this.tabPageTakeOverAchat = new System.Windows.Forms.TabPage();
            this.dataGridViewTakeOverAchat = new System.Windows.Forms.DataGridView();
            this.tabControlAll = new System.Windows.Forms.TabControl();
            this.stockTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStock)).BeginInit();
            this.customerRelationTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomerRelationAll)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel7.SuspendLayout();
            this.tabPageTakeOver.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel8.SuspendLayout();
            this.tabControlTakeOver.SuspendLayout();
            this.tabPageTakeOverRepair.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTakeOverRepair)).BeginInit();
            this.tabPageTakeOverUnlock.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTakeOverUnlock)).BeginInit();
            this.tabPageTakeOverAchat.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTakeOverAchat)).BeginInit();
            this.tabControlAll.SuspendLayout();
            this.SuspendLayout();
            // 
            // stockTabPage
            // 
            this.stockTabPage.Controls.Add(this.buttonStockAdd);
            this.stockTabPage.Controls.Add(this.label46);
            this.stockTabPage.Controls.Add(this.textBoxStockSearch);
            this.stockTabPage.Controls.Add(this.dataGridViewStock);
            this.stockTabPage.Location = new System.Drawing.Point(4, 34);
            this.stockTabPage.Name = "stockTabPage";
            this.stockTabPage.Size = new System.Drawing.Size(1348, 661);
            this.stockTabPage.TabIndex = 3;
            this.stockTabPage.Text = "Stock";
            this.stockTabPage.UseVisualStyleBackColor = true;
            // 
            // buttonStockAdd
            // 
            this.buttonStockAdd.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonStockAdd.Location = new System.Drawing.Point(3, 587);
            this.buttonStockAdd.Name = "buttonStockAdd";
            this.buttonStockAdd.Size = new System.Drawing.Size(1342, 71);
            this.buttonStockAdd.TabIndex = 37;
            this.buttonStockAdd.Text = "Ajouter un article";
            this.buttonStockAdd.UseVisualStyleBackColor = true;
            this.buttonStockAdd.Click += new System.EventHandler(this.buttonStockAdd_Click);
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.Location = new System.Drawing.Point(3, 0);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(274, 24);
            this.label46.TabIndex = 36;
            this.label46.Text = "Recherche par Nom de l\'Article";
            // 
            // textBoxStockSearch
            // 
            this.textBoxStockSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStockSearch.Location = new System.Drawing.Point(283, 3);
            this.textBoxStockSearch.Name = "textBoxStockSearch";
            this.textBoxStockSearch.Size = new System.Drawing.Size(973, 29);
            this.textBoxStockSearch.TabIndex = 35;
            // 
            // dataGridViewStock
            // 
            this.dataGridViewStock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewStock.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.dataGridViewStock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStock.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridViewStock.Location = new System.Drawing.Point(3, 38);
            this.dataGridViewStock.Name = "dataGridViewStock";
            this.dataGridViewStock.Size = new System.Drawing.Size(1342, 543);
            this.dataGridViewStock.TabIndex = 0;
            // 
            // customerRelationTabPage
            // 
            this.customerRelationTabPage.Controls.Add(this.dataGridViewCustomerRelationAll);
            this.customerRelationTabPage.Controls.Add(this.flowLayoutPanel1);
            this.customerRelationTabPage.Controls.Add(this.tableLayoutPanel7);
            this.customerRelationTabPage.Location = new System.Drawing.Point(4, 34);
            this.customerRelationTabPage.Name = "customerRelationTabPage";
            this.customerRelationTabPage.Size = new System.Drawing.Size(1348, 661);
            this.customerRelationTabPage.TabIndex = 2;
            this.customerRelationTabPage.Text = "Relation Client";
            this.customerRelationTabPage.UseVisualStyleBackColor = true;
            // 
            // dataGridViewCustomerRelationAll
            // 
            this.dataGridViewCustomerRelationAll.AllowUserToAddRows = false;
            this.dataGridViewCustomerRelationAll.AllowUserToDeleteRows = false;
            this.dataGridViewCustomerRelationAll.AllowUserToOrderColumns = true;
            this.dataGridViewCustomerRelationAll.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewCustomerRelationAll.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCustomerRelationAll.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridViewCustomerRelationAll.Location = new System.Drawing.Point(3, 58);
            this.dataGridViewCustomerRelationAll.Name = "dataGridViewCustomerRelationAll";
            this.dataGridViewCustomerRelationAll.ReadOnly = true;
            this.dataGridViewCustomerRelationAll.ShowCellToolTips = false;
            this.dataGridViewCustomerRelationAll.ShowEditingIcon = false;
            this.dataGridViewCustomerRelationAll.Size = new System.Drawing.Size(828, 577);
            this.dataGridViewCustomerRelationAll.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.tableLayoutPanel2);
            this.flowLayoutPanel1.Controls.Add(this.buttonCustomerRelationSaveCustomerData);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(837, 9);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(439, 626);
            this.flowLayoutPanel1.TabIndex = 35;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 41.2993F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 58.7007F));
            this.tableLayoutPanel2.Controls.Add(this.label35, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.textBoxCustomerRelationEmailAddress, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.textBoxCustomerRelationPhone, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label34, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.textBoxCustomerRelationFirstName, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.textBoxCustomerRelationLastName, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label33, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label32, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.comboBoxCustomerRelationSexe, 1, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 52.7027F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 47.2973F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(431, 180);
            this.tableLayoutPanel2.TabIndex = 34;
            // 
            // label35
            // 
            this.label35.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.Location = new System.Drawing.Point(55, 150);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(119, 24);
            this.label35.TabIndex = 9;
            this.label35.Text = "Adresse mail";
            // 
            // textBoxCustomerRelationEmailAddress
            // 
            this.textBoxCustomerRelationEmailAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCustomerRelationEmailAddress.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCustomerRelationEmailAddress.Location = new System.Drawing.Point(180, 147);
            this.textBoxCustomerRelationEmailAddress.Name = "textBoxCustomerRelationEmailAddress";
            this.textBoxCustomerRelationEmailAddress.Size = new System.Drawing.Size(248, 29);
            this.textBoxCustomerRelationEmailAddress.TabIndex = 10;
            // 
            // textBoxCustomerRelationPhone
            // 
            this.textBoxCustomerRelationPhone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCustomerRelationPhone.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCustomerRelationPhone.Location = new System.Drawing.Point(180, 112);
            this.textBoxCustomerRelationPhone.Name = "textBoxCustomerRelationPhone";
            this.textBoxCustomerRelationPhone.Size = new System.Drawing.Size(248, 29);
            this.textBoxCustomerRelationPhone.TabIndex = 8;
            // 
            // label34
            // 
            this.label34.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.Location = new System.Drawing.Point(71, 114);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(103, 24);
            this.label34.TabIndex = 7;
            this.label34.Text = "Téléphone";
            // 
            // textBoxCustomerRelationFirstName
            // 
            this.textBoxCustomerRelationFirstName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCustomerRelationFirstName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCustomerRelationFirstName.Location = new System.Drawing.Point(180, 77);
            this.textBoxCustomerRelationFirstName.Name = "textBoxCustomerRelationFirstName";
            this.textBoxCustomerRelationFirstName.Size = new System.Drawing.Size(248, 29);
            this.textBoxCustomerRelationFirstName.TabIndex = 6;
            // 
            // textBoxCustomerRelationLastName
            // 
            this.textBoxCustomerRelationLastName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxCustomerRelationLastName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCustomerRelationLastName.Location = new System.Drawing.Point(180, 42);
            this.textBoxCustomerRelationLastName.Name = "textBoxCustomerRelationLastName";
            this.textBoxCustomerRelationLastName.Size = new System.Drawing.Size(248, 29);
            this.textBoxCustomerRelationLastName.TabIndex = 4;
            // 
            // label33
            // 
            this.label33.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label33.AutoSize = true;
            this.label33.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(97, 79);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(77, 24);
            this.label33.TabIndex = 5;
            this.label33.Text = "Prénom";
            // 
            // label32
            // 
            this.label32.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.Location = new System.Drawing.Point(123, 44);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(51, 24);
            this.label32.TabIndex = 3;
            this.label32.Text = "Nom";
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.label3.Location = new System.Drawing.Point(120, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 24);
            this.label3.TabIndex = 11;
            this.label3.Text = "Sexe";
            // 
            // comboBoxCustomerRelationSexe
            // 
            this.comboBoxCustomerRelationSexe.FormattingEnabled = true;
            this.comboBoxCustomerRelationSexe.Location = new System.Drawing.Point(180, 3);
            this.comboBoxCustomerRelationSexe.Name = "comboBoxCustomerRelationSexe";
            this.comboBoxCustomerRelationSexe.Size = new System.Drawing.Size(248, 33);
            this.comboBoxCustomerRelationSexe.TabIndex = 12;
            // 
            // buttonCustomerRelationSaveCustomerData
            // 
            this.buttonCustomerRelationSaveCustomerData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCustomerRelationSaveCustomerData.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCustomerRelationSaveCustomerData.Location = new System.Drawing.Point(3, 189);
            this.buttonCustomerRelationSaveCustomerData.Name = "buttonCustomerRelationSaveCustomerData";
            this.buttonCustomerRelationSaveCustomerData.Size = new System.Drawing.Size(426, 46);
            this.buttonCustomerRelationSaveCustomerData.TabIndex = 11;
            this.buttonCustomerRelationSaveCustomerData.Text = "Sauvegarder";
            this.buttonCustomerRelationSaveCustomerData.UseVisualStyleBackColor = true;
            this.buttonCustomerRelationSaveCustomerData.Click += new System.EventHandler(this.buttonCustomerRelationSaveCustomerData_Click);
            // 
            // tableLayoutPanel7
            // 
            this.tableLayoutPanel7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel7.ColumnCount = 2;
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Controls.Add(this.textBoxCustomerSearchAll, 1, 0);
            this.tableLayoutPanel7.Controls.Add(this.label40, 0, 0);
            this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel7.Name = "tableLayoutPanel7";
            this.tableLayoutPanel7.RowCount = 1;
            this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel7.Size = new System.Drawing.Size(831, 49);
            this.tableLayoutPanel7.TabIndex = 36;
            // 
            // textBoxCustomerSearchAll
            // 
            this.textBoxCustomerSearchAll.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.textBoxCustomerSearchAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxCustomerSearchAll.Location = new System.Drawing.Point(425, 3);
            this.textBoxCustomerSearchAll.Name = "textBoxCustomerSearchAll";
            this.textBoxCustomerSearchAll.Size = new System.Drawing.Size(396, 29);
            this.textBoxCustomerSearchAll.TabIndex = 33;
            // 
            // label40
            // 
            this.label40.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.Location = new System.Drawing.Point(3, 0);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(409, 48);
            this.label40.TabIndex = 34;
            this.label40.Text = "Recherche par (nom, prénom, numéro de téléphone)";
            // 
            // tabPageTakeOver
            // 
            this.tabPageTakeOver.Controls.Add(this.tableLayoutPanel6);
            this.tabPageTakeOver.Controls.Add(this.tableLayoutPanel4);
            this.tabPageTakeOver.Controls.Add(this.tableLayoutPanel3);
            this.tabPageTakeOver.Controls.Add(this.tabControlTakeOver);
            this.tabPageTakeOver.Location = new System.Drawing.Point(4, 34);
            this.tabPageTakeOver.Name = "tabPageTakeOver";
            this.tabPageTakeOver.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTakeOver.Size = new System.Drawing.Size(1348, 661);
            this.tabPageTakeOver.TabIndex = 5;
            this.tabPageTakeOver.Text = "Prise en charge";
            this.tabPageTakeOver.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 4;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 63.47518F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.52482F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 165F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 182F));
            this.tableLayoutPanel6.Controls.Add(this.takeOverNumber, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.buttonTakeOverReset, 3, 0);
            this.tableLayoutPanel6.Controls.Add(this.label43, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.buttonTakeOverSearch, 2, 0);
            this.tableLayoutPanel6.Location = new System.Drawing.Point(546, 6);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 1;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(673, 36);
            this.tableLayoutPanel6.TabIndex = 98;
            // 
            // takeOverNumber
            // 
            this.takeOverNumber.Location = new System.Drawing.Point(209, 3);
            this.takeOverNumber.Name = "takeOverNumber";
            this.takeOverNumber.Size = new System.Drawing.Size(97, 30);
            this.takeOverNumber.TabIndex = 95;
            this.takeOverNumber.Text = "0";
            // 
            // buttonTakeOverReset
            // 
            this.buttonTakeOverReset.Location = new System.Drawing.Point(493, 3);
            this.buttonTakeOverReset.Name = "buttonTakeOverReset";
            this.buttonTakeOverReset.Size = new System.Drawing.Size(147, 30);
            this.buttonTakeOverReset.TabIndex = 91;
            this.buttonTakeOverReset.Text = "Réinitialiser";
            this.buttonTakeOverReset.UseVisualStyleBackColor = true;
            this.buttonTakeOverReset.Click += new System.EventHandler(this.buttonTakeOverReset_Click);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.Location = new System.Drawing.Point(3, 0);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(191, 26);
            this.label43.TabIndex = 70;
            this.label43.Text = "Prise en charge n°";
            // 
            // buttonTakeOverSearch
            // 
            this.buttonTakeOverSearch.Location = new System.Drawing.Point(328, 3);
            this.buttonTakeOverSearch.Name = "buttonTakeOverSearch";
            this.buttonTakeOverSearch.Size = new System.Drawing.Size(150, 30);
            this.buttonTakeOverSearch.TabIndex = 95;
            this.buttonTakeOverSearch.Text = "Rechercher";
            this.buttonTakeOverSearch.UseVisualStyleBackColor = true;
            this.buttonTakeOverSearch.Click += new System.EventHandler(this.buttonTakeOverSearch_Click_1);
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tableLayoutPanel4.AutoSize = true;
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.78481F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 322F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 323F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 318F));
            this.tableLayoutPanel4.Controls.Add(this.buttonTakeOverSave, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonTakeOverManageRemise, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonTakeOverManageGarantie, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonTakeOverScanner, 3, 0);
            this.tableLayoutPanel4.Location = new System.Drawing.Point(42, 556);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(1281, 83);
            this.tableLayoutPanel4.TabIndex = 97;
            // 
            // buttonTakeOverSave
            // 
            this.buttonTakeOverSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTakeOverSave.AutoSize = true;
            this.buttonTakeOverSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTakeOverSave.Location = new System.Drawing.Point(3, 12);
            this.buttonTakeOverSave.Name = "buttonTakeOverSave";
            this.buttonTakeOverSave.Size = new System.Drawing.Size(312, 58);
            this.buttonTakeOverSave.TabIndex = 82;
            this.buttonTakeOverSave.Text = "Sauvegarder";
            this.buttonTakeOverSave.UseVisualStyleBackColor = true;
            this.buttonTakeOverSave.Click += new System.EventHandler(this.buttonTakeOverSave_Click);
            // 
            // buttonTakeOverManageRemise
            // 
            this.buttonTakeOverManageRemise.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTakeOverManageRemise.AutoSize = true;
            this.buttonTakeOverManageRemise.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTakeOverManageRemise.Location = new System.Drawing.Point(643, 12);
            this.buttonTakeOverManageRemise.Name = "buttonTakeOverManageRemise";
            this.buttonTakeOverManageRemise.Size = new System.Drawing.Size(317, 58);
            this.buttonTakeOverManageRemise.TabIndex = 88;
            this.buttonTakeOverManageRemise.Text = "Gérer les remises";
            this.buttonTakeOverManageRemise.UseVisualStyleBackColor = true;
            this.buttonTakeOverManageRemise.Click += new System.EventHandler(this.buttonTakeOverManageRemise_Click);
            // 
            // buttonTakeOverManageGarantie
            // 
            this.buttonTakeOverManageGarantie.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTakeOverManageGarantie.AutoSize = true;
            this.buttonTakeOverManageGarantie.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTakeOverManageGarantie.Location = new System.Drawing.Point(321, 12);
            this.buttonTakeOverManageGarantie.Name = "buttonTakeOverManageGarantie";
            this.buttonTakeOverManageGarantie.Size = new System.Drawing.Size(316, 58);
            this.buttonTakeOverManageGarantie.TabIndex = 87;
            this.buttonTakeOverManageGarantie.Text = "Gérer les garanties";
            this.buttonTakeOverManageGarantie.UseVisualStyleBackColor = true;
            this.buttonTakeOverManageGarantie.Click += new System.EventHandler(this.buttonTakeOverManageGarantie_Click);
            // 
            // buttonTakeOverScanner
            // 
            this.buttonTakeOverScanner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTakeOverScanner.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonTakeOverScanner.Location = new System.Drawing.Point(966, 12);
            this.buttonTakeOverScanner.Name = "buttonTakeOverScanner";
            this.buttonTakeOverScanner.Size = new System.Drawing.Size(312, 58);
            this.buttonTakeOverScanner.TabIndex = 89;
            this.buttonTakeOverScanner.Text = "Scanner un article";
            this.buttonTakeOverScanner.UseVisualStyleBackColor = true;
            this.buttonTakeOverScanner.Click += new System.EventHandler(this.ButtonTakeOverScanner_Click);
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 2;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.39806F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 66.60194F));
            this.tableLayoutPanel3.Controls.Add(this.label45, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.textBoxTakeOverAccompte, 1, 3);
            this.tableLayoutPanel3.Controls.Add(this.textBoxTakeOverPaid, 1, 5);
            this.tableLayoutPanel3.Controls.Add(this.dateTimePickerTakeOverDate, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.label41, 0, 3);
            this.tableLayoutPanel3.Controls.Add(this.textBoxTakeOverTotalPrice, 1, 6);
            this.tableLayoutPanel3.Controls.Add(this.textBoxTakeOverResteDu, 1, 4);
            this.tableLayoutPanel3.Controls.Add(this.label27, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.label48, 0, 5);
            this.tableLayoutPanel3.Controls.Add(this.label18, 0, 6);
            this.tableLayoutPanel3.Controls.Add(this.label17, 0, 4);
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel8, 1, 2);
            this.tableLayoutPanel3.Controls.Add(this.checkBoxEspece, 1, 8);
            this.tableLayoutPanel3.Controls.Add(this.checkBoxCb, 1, 7);
            this.tableLayoutPanel3.Controls.Add(this.checkBoxVirement, 1, 9);
            this.tableLayoutPanel3.Controls.Add(this.comboBoxTakeOverCustomer, 1, 1);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(6, 48);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 10;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.54945F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 49.45055F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 35F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 33F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(534, 383);
            this.tableLayoutPanel3.TabIndex = 96;
            // 
            // label45
            // 
            this.label45.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label45.AutoSize = true;
            this.label45.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.Location = new System.Drawing.Point(3, 11);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(172, 24);
            this.label45.TabIndex = 68;
            this.label45.Text = "Date de facturation";
            // 
            // textBoxTakeOverAccompte
            // 
            this.textBoxTakeOverAccompte.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTakeOverAccompte.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTakeOverAccompte.Location = new System.Drawing.Point(181, 144);
            this.textBoxTakeOverAccompte.Name = "textBoxTakeOverAccompte";
            this.textBoxTakeOverAccompte.Size = new System.Drawing.Size(350, 29);
            this.textBoxTakeOverAccompte.TabIndex = 91;
            this.textBoxTakeOverAccompte.Text = "0";
            this.textBoxTakeOverAccompte.TextChanged += new System.EventHandler(this.TextBoxTakeOverAccompte_TextChanged);
            // 
            // textBoxTakeOverPaid
            // 
            this.textBoxTakeOverPaid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTakeOverPaid.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTakeOverPaid.Location = new System.Drawing.Point(181, 216);
            this.textBoxTakeOverPaid.Name = "textBoxTakeOverPaid";
            this.textBoxTakeOverPaid.Size = new System.Drawing.Size(350, 29);
            this.textBoxTakeOverPaid.TabIndex = 95;
            this.textBoxTakeOverPaid.Text = "0";
            this.textBoxTakeOverPaid.TextChanged += new System.EventHandler(this.TextBoxTakeOverPaid_TextChanged);
            // 
            // dateTimePickerTakeOverDate
            // 
            this.dateTimePickerTakeOverDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimePickerTakeOverDate.CustomFormat = "dd/MM/yyyy HH:mm:ss";
            this.dateTimePickerTakeOverDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dateTimePickerTakeOverDate.Location = new System.Drawing.Point(181, 8);
            this.dateTimePickerTakeOverDate.Name = "dateTimePickerTakeOverDate";
            this.dateTimePickerTakeOverDate.Size = new System.Drawing.Size(350, 29);
            this.dateTimePickerTakeOverDate.TabIndex = 69;
            // 
            // label41
            // 
            this.label41.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(3, 146);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(172, 24);
            this.label41.TabIndex = 90;
            this.label41.Text = "Accompte";
            // 
            // textBoxTakeOverTotalPrice
            // 
            this.textBoxTakeOverTotalPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTakeOverTotalPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTakeOverTotalPrice.Location = new System.Drawing.Point(181, 251);
            this.textBoxTakeOverTotalPrice.Name = "textBoxTakeOverTotalPrice";
            this.textBoxTakeOverTotalPrice.Size = new System.Drawing.Size(350, 29);
            this.textBoxTakeOverTotalPrice.TabIndex = 79;
            this.textBoxTakeOverTotalPrice.Text = "0";
            // 
            // textBoxTakeOverResteDu
            // 
            this.textBoxTakeOverResteDu.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxTakeOverResteDu.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxTakeOverResteDu.Location = new System.Drawing.Point(181, 180);
            this.textBoxTakeOverResteDu.Name = "textBoxTakeOverResteDu";
            this.textBoxTakeOverResteDu.Size = new System.Drawing.Size(350, 29);
            this.textBoxTakeOverResteDu.TabIndex = 93;
            this.textBoxTakeOverResteDu.Text = "0";
            // 
            // label27
            // 
            this.label27.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label27.AutoSize = true;
            this.label27.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(3, 56);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(172, 24);
            this.label27.TabIndex = 72;
            this.label27.Text = "Client";
            // 
            // label48
            // 
            this.label48.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label48.AutoSize = true;
            this.label48.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label48.Location = new System.Drawing.Point(3, 218);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(172, 24);
            this.label48.TabIndex = 94;
            this.label48.Text = "Payé";
            // 
            // label18
            // 
            this.label18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label18.AutoSize = true;
            this.label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(3, 254);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(172, 24);
            this.label18.TabIndex = 78;
            this.label18.Text = "Prix total";
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.Location = new System.Drawing.Point(3, 183);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(172, 24);
            this.label17.TabIndex = 92;
            this.label17.Text = "Reste dû";
            // 
            // tableLayoutPanel8
            // 
            this.tableLayoutPanel8.ColumnCount = 2;
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 173F));
            this.tableLayoutPanel8.Controls.Add(this.buttonAddCustomer, 0, 0);
            this.tableLayoutPanel8.Controls.Add(this.buttonUpdateCustomer, 1, 0);
            this.tableLayoutPanel8.Location = new System.Drawing.Point(181, 94);
            this.tableLayoutPanel8.Name = "tableLayoutPanel8";
            this.tableLayoutPanel8.RowCount = 1;
            this.tableLayoutPanel8.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel8.Size = new System.Drawing.Size(350, 43);
            this.tableLayoutPanel8.TabIndex = 99;
            // 
            // buttonAddCustomer
            // 
            this.buttonAddCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAddCustomer.Location = new System.Drawing.Point(3, 3);
            this.buttonAddCustomer.Name = "buttonAddCustomer";
            this.buttonAddCustomer.Size = new System.Drawing.Size(171, 37);
            this.buttonAddCustomer.TabIndex = 96;
            this.buttonAddCustomer.Text = "Ajouter un client";
            this.buttonAddCustomer.UseVisualStyleBackColor = true;
            this.buttonAddCustomer.Click += new System.EventHandler(this.buttonAddCustomer_Click);
            // 
            // buttonUpdateCustomer
            // 
            this.buttonUpdateCustomer.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUpdateCustomer.Location = new System.Drawing.Point(180, 3);
            this.buttonUpdateCustomer.Name = "buttonUpdateCustomer";
            this.buttonUpdateCustomer.Size = new System.Drawing.Size(167, 37);
            this.buttonUpdateCustomer.TabIndex = 97;
            this.buttonUpdateCustomer.Text = "Modifier un client";
            this.buttonUpdateCustomer.UseVisualStyleBackColor = true;
            this.buttonUpdateCustomer.Click += new System.EventHandler(this.buttonUpdateCustomer_Click);
            // 
            // checkBoxEspece
            // 
            this.checkBoxEspece.AutoSize = true;
            this.checkBoxEspece.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEspece.Location = new System.Drawing.Point(181, 320);
            this.checkBoxEspece.Name = "checkBoxEspece";
            this.checkBoxEspece.Size = new System.Drawing.Size(203, 26);
            this.checkBoxEspece.TabIndex = 102;
            this.checkBoxEspece.Text = "Paiement en espèce";
            this.checkBoxEspece.UseVisualStyleBackColor = true;
            // 
            // checkBoxCb
            // 
            this.checkBoxCb.AutoSize = true;
            this.checkBoxCb.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxCb.Location = new System.Drawing.Point(181, 287);
            this.checkBoxCb.Name = "checkBoxCb";
            this.checkBoxCb.Size = new System.Drawing.Size(165, 27);
            this.checkBoxCb.TabIndex = 101;
            this.checkBoxCb.Text = "Paiement en CB";
            this.checkBoxCb.UseVisualStyleBackColor = true;
            // 
            // checkBoxVirement
            // 
            this.checkBoxVirement.AutoSize = true;
            this.checkBoxVirement.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxVirement.Location = new System.Drawing.Point(181, 352);
            this.checkBoxVirement.Name = "checkBoxVirement";
            this.checkBoxVirement.Size = new System.Drawing.Size(217, 28);
            this.checkBoxVirement.TabIndex = 103;
            this.checkBoxVirement.Text = "Paiement par virement";
            this.checkBoxVirement.UseVisualStyleBackColor = true;
            // 
            // comboBoxTakeOverCustomer
            // 
            this.comboBoxTakeOverCustomer.FormattingEnabled = true;
            this.comboBoxTakeOverCustomer.Location = new System.Drawing.Point(181, 49);
            this.comboBoxTakeOverCustomer.Name = "comboBoxTakeOverCustomer";
            this.comboBoxTakeOverCustomer.Size = new System.Drawing.Size(350, 33);
            this.comboBoxTakeOverCustomer.TabIndex = 104;
            // 
            // tabControlTakeOver
            // 
            this.tabControlTakeOver.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlTakeOver.Controls.Add(this.tabPageTakeOverRepair);
            this.tabControlTakeOver.Controls.Add(this.tabPageTakeOverUnlock);
            this.tabControlTakeOver.Controls.Add(this.tabPageTakeOverAchat);
            this.tabControlTakeOver.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlTakeOver.Location = new System.Drawing.Point(546, 48);
            this.tabControlTakeOver.Name = "tabControlTakeOver";
            this.tabControlTakeOver.SelectedIndex = 0;
            this.tabControlTakeOver.Size = new System.Drawing.Size(812, 502);
            this.tabControlTakeOver.TabIndex = 88;
            // 
            // tabPageTakeOverRepair
            // 
            this.tabPageTakeOverRepair.Controls.Add(this.dataGridViewTakeOverRepair);
            this.tabPageTakeOverRepair.ForeColor = System.Drawing.Color.White;
            this.tabPageTakeOverRepair.Location = new System.Drawing.Point(4, 29);
            this.tabPageTakeOverRepair.Name = "tabPageTakeOverRepair";
            this.tabPageTakeOverRepair.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTakeOverRepair.Size = new System.Drawing.Size(804, 469);
            this.tabPageTakeOverRepair.TabIndex = 0;
            this.tabPageTakeOverRepair.Text = "Réparation";
            this.tabPageTakeOverRepair.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTakeOverRepair
            // 
            this.dataGridViewTakeOverRepair.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTakeOverRepair.BackgroundColor = System.Drawing.Color.LightGray;
            this.dataGridViewTakeOverRepair.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTakeOverRepair.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewTakeOverRepair.Name = "dataGridViewTakeOverRepair";
            this.dataGridViewTakeOverRepair.Size = new System.Drawing.Size(792, 457);
            this.dataGridViewTakeOverRepair.TabIndex = 0;
            this.dataGridViewTakeOverRepair.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.DataGridViewTakeOverRepair_CellBeginEdit);
            this.dataGridViewTakeOverRepair.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewTakeOverRepair_CellClick);
            this.dataGridViewTakeOverRepair.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewTakeOverRepair_CellEndEdit);
            this.dataGridViewTakeOverRepair.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridViewTakeOverRepair_CurrentCellDirtyStateChanged);
            this.dataGridViewTakeOverRepair.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.DataGridViewTakeOverRepair_EditingControlShowing);
            // 
            // tabPageTakeOverUnlock
            // 
            this.tabPageTakeOverUnlock.Controls.Add(this.dataGridViewTakeOverUnlock);
            this.tabPageTakeOverUnlock.Location = new System.Drawing.Point(4, 29);
            this.tabPageTakeOverUnlock.Name = "tabPageTakeOverUnlock";
            this.tabPageTakeOverUnlock.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTakeOverUnlock.Size = new System.Drawing.Size(804, 469);
            this.tabPageTakeOverUnlock.TabIndex = 1;
            this.tabPageTakeOverUnlock.Text = "Déblocage";
            this.tabPageTakeOverUnlock.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTakeOverUnlock
            // 
            this.dataGridViewTakeOverUnlock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTakeOverUnlock.BackgroundColor = System.Drawing.Color.LightGray;
            this.dataGridViewTakeOverUnlock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTakeOverUnlock.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewTakeOverUnlock.Name = "dataGridViewTakeOverUnlock";
            this.dataGridViewTakeOverUnlock.Size = new System.Drawing.Size(792, 457);
            this.dataGridViewTakeOverUnlock.TabIndex = 0;
            this.dataGridViewTakeOverUnlock.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.DataGridViewTakeOverUnlock_CellBeginEdit);
            this.dataGridViewTakeOverUnlock.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewTakeOverUnlock_CellClick);
            this.dataGridViewTakeOverUnlock.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewTakeOverUnlock_CellEndEdit);
            this.dataGridViewTakeOverUnlock.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridViewTakeOverUnlock_CurrentCellDirtyStateChanged);
            this.dataGridViewTakeOverUnlock.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.DataGridViewTakeOverUnlock_EditingControlShowing);
            // 
            // tabPageTakeOverAchat
            // 
            this.tabPageTakeOverAchat.Controls.Add(this.dataGridViewTakeOverAchat);
            this.tabPageTakeOverAchat.Location = new System.Drawing.Point(4, 29);
            this.tabPageTakeOverAchat.Name = "tabPageTakeOverAchat";
            this.tabPageTakeOverAchat.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTakeOverAchat.Size = new System.Drawing.Size(804, 469);
            this.tabPageTakeOverAchat.TabIndex = 2;
            this.tabPageTakeOverAchat.Text = "Achat";
            this.tabPageTakeOverAchat.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTakeOverAchat
            // 
            this.dataGridViewTakeOverAchat.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTakeOverAchat.BackgroundColor = System.Drawing.Color.LightGray;
            this.dataGridViewTakeOverAchat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTakeOverAchat.Location = new System.Drawing.Point(6, 6);
            this.dataGridViewTakeOverAchat.Name = "dataGridViewTakeOverAchat";
            this.dataGridViewTakeOverAchat.Size = new System.Drawing.Size(792, 457);
            this.dataGridViewTakeOverAchat.TabIndex = 0;
            this.dataGridViewTakeOverAchat.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.DataGridViewTakeOverAchat_CellBeginEdit);
            this.dataGridViewTakeOverAchat.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewTakeOverAchat_CellClick);
            this.dataGridViewTakeOverAchat.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.DataGridViewTakeOverAchat_CellEndEdit);
            this.dataGridViewTakeOverAchat.CurrentCellDirtyStateChanged += new System.EventHandler(this.DataGridViewTakeOverAchat_CurrentCellDirtyStateChanged);
            this.dataGridViewTakeOverAchat.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.DataGridViewTakeOverAchat_EditingControlShowing);
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
            this.tabControlAll.Size = new System.Drawing.Size(1356, 699);
            this.tabControlAll.TabIndex = 0;
            // 
            // FormMobileExpress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1380, 723);
            this.Controls.Add(this.tabControlAll);
            this.Name = "FormMobileExpress";
            this.Text = "Mobile Express";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.stockTabPage.ResumeLayout(false);
            this.stockTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStock)).EndInit();
            this.customerRelationTabPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCustomerRelationAll)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel7.ResumeLayout(false);
            this.tableLayoutPanel7.PerformLayout();
            this.tabPageTakeOver.ResumeLayout(false);
            this.tabPageTakeOver.PerformLayout();
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel8.ResumeLayout(false);
            this.tabControlTakeOver.ResumeLayout(false);
            this.tabPageTakeOverRepair.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTakeOverRepair)).EndInit();
            this.tabPageTakeOverUnlock.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTakeOverUnlock)).EndInit();
            this.tabPageTakeOverAchat.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTakeOverAchat)).EndInit();
            this.tabControlAll.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private TabPage stockTabPage;
        private Label label46;
        private TextBox textBoxStockSearch;
        private DataGridView dataGridViewStock;
        private TabPage customerRelationTabPage;
        private Label label40;
        private TextBox textBoxCustomerSearchAll;
        private TextBox textBoxCustomerRelationPhone;
        private TextBox textBoxCustomerRelationFirstName;
        private TextBox textBoxCustomerRelationLastName;
        private Button buttonCustomerRelationSaveCustomerData;
        private Label label34;
        private Label label33;
        private Label label32;
        private DataGridView dataGridViewCustomerRelationAll;
        private TabPage tabPageTakeOver;
        private TextBox textBoxTakeOverPaid;
        private TextBox textBoxTakeOverResteDu;
        private TextBox textBoxTakeOverAccompte;
        private TextBox textBoxTakeOverTotalPrice;
        private Label label48;
        private Label label17;
        private Label label41;
        private TabControl tabControlTakeOver;
        private TabPage tabPageTakeOverRepair;
        private DataGridView dataGridViewTakeOverRepair;
        private Button buttonTakeOverSave;
        private Label label18;
        private Label label27;
        private Label label43;
        private DateTimePicker dateTimePickerTakeOverDate;
        private Label label45;
        private TabControl tabControlAll;
        private TabPage tabPageTakeOverUnlock;
        private TabPage tabPageTakeOverAchat;
        private DataGridView dataGridViewTakeOverUnlock;
        private DataGridView dataGridViewTakeOverAchat;
        private FlowLayoutPanel flowLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel4;
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel tableLayoutPanel7;
        private TableLayoutPanel tableLayoutPanel6;
        private Button buttonTakeOverManageGarantie;
        private TextBox takeOverNumber;
        private Button buttonTakeOverReset;
        private Button buttonTakeOverSearch;
        private Button buttonAddCustomer;
        private Button buttonUpdateCustomer;
        private TableLayoutPanel tableLayoutPanel8;
        private Button buttonTakeOverManageRemise;
        private CheckBox checkBoxEspece;
        private CheckBox checkBoxCb;
        private CheckBox checkBoxVirement;
        private Label label35;
        private TextBox textBoxCustomerRelationEmailAddress;
        private Label label3;
        private ComboBox comboBoxCustomerRelationSexe;
        private Button buttonTakeOverScanner;
        private Button buttonStockAdd;
        private ComboBox comboBoxTakeOverCustomer;
    }
}

