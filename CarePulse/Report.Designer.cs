namespace CarePulse
{
    partial class Report
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.Label label1;
            this.panel8 = new System.Windows.Forms.Panel();
            this.panel11 = new ReaLTaiizor.Controls.Panel();
            this.btnStartPage = new System.Windows.Forms.Button();
            this.btnPreviousPage = new System.Windows.Forms.Button();
            this.btnNextPage = new System.Windows.Forms.Button();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.panel10 = new ReaLTaiizor.Controls.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.btnRollBack = new System.Windows.Forms.Button();
            this.txtboxSearch = new ReaLTaiizor.Controls.HopeTextBox();
            this.panel16 = new ReaLTaiizor.Controls.Panel();
            this.comboBoxFilterMonth = new ReaLTaiizor.Controls.DungeonComboBox();
            this.btnClearSearchText = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.btnView = new System.Windows.Forms.Button();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnExportCSV = new System.Windows.Forms.Button();
            this.panel9 = new ReaLTaiizor.Controls.Panel();
            this.datagridReport = new ReaLTaiizor.Controls.PoisonDataGridView();
            this.cpID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpDatePeriod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpResponseTotal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpSurveyScore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpMonth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpYear = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpSurveyTemplate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpSurveyQuestionsAnswers = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cpPatientsFeedback = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblPageInfo = new System.Windows.Forms.Label();
            this.panelFilter = new ReaLTaiizor.Controls.Panel();
            this.btnViewAll = new ReaLTaiizor.Controls.HopeButton();
            this.btnFilterData = new ReaLTaiizor.Controls.HopeButton();
            this.parrotGradientPanel2 = new ReaLTaiizor.Controls.ParrotGradientPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.datePickerEnd = new CarePulse.CustomDatePicker();
            this.datePickerStart = new CarePulse.CustomDatePicker();
            label1 = new System.Windows.Forms.Label();
            this.panel8.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.datagridReport)).BeginInit();
            this.panelFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.lblPageInfo);
            this.panel8.Controls.Add(this.panel11);
            this.panel8.Controls.Add(this.btnStartPage);
            this.panel8.Controls.Add(this.btnPreviousPage);
            this.panel8.Controls.Add(this.btnNextPage);
            this.panel8.Controls.Add(this.btnLastPage);
            this.panel8.Controls.Add(this.panel10);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(0, 521);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(986, 42);
            this.panel8.TabIndex = 33;
            // 
            // panel11
            // 
            this.panel11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel11.BackColor = System.Drawing.Color.Gainsboro;
            this.panel11.EdgeColor = System.Drawing.Color.Gainsboro;
            this.panel11.Location = new System.Drawing.Point(777, 11);
            this.panel11.Name = "panel11";
            this.panel11.Padding = new System.Windows.Forms.Padding(5);
            this.panel11.Size = new System.Drawing.Size(1, 20);
            this.panel11.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panel11.TabIndex = 29;
            this.panel11.Text = "panel11";
            // 
            // btnStartPage
            // 
            this.btnStartPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartPage.BackColor = System.Drawing.Color.Transparent;
            this.btnStartPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStartPage.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnStartPage.FlatAppearance.BorderSize = 0;
            this.btnStartPage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnStartPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnStartPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            this.btnStartPage.Image = global::CarePulse.Properties.Resources.first_24px;
            this.btnStartPage.Location = new System.Drawing.Point(809, 5);
            this.btnStartPage.Name = "btnStartPage";
            this.btnStartPage.Size = new System.Drawing.Size(31, 31);
            this.btnStartPage.TabIndex = 28;
            this.btnStartPage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStartPage.UseVisualStyleBackColor = false;
            this.btnStartPage.Click += new System.EventHandler(this.btnStartPage_Click);
            // 
            // btnPreviousPage
            // 
            this.btnPreviousPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPreviousPage.BackColor = System.Drawing.Color.Transparent;
            this.btnPreviousPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPreviousPage.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnPreviousPage.FlatAppearance.BorderSize = 0;
            this.btnPreviousPage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.btnPreviousPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnPreviousPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPreviousPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            this.btnPreviousPage.Image = global::CarePulse.Properties.Resources.back_24px;
            this.btnPreviousPage.Location = new System.Drawing.Point(846, 5);
            this.btnPreviousPage.Name = "btnPreviousPage";
            this.btnPreviousPage.Size = new System.Drawing.Size(31, 31);
            this.btnPreviousPage.TabIndex = 27;
            this.btnPreviousPage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPreviousPage.UseVisualStyleBackColor = false;
            this.btnPreviousPage.Click += new System.EventHandler(this.btnPreviousPage_Click);
            // 
            // btnNextPage
            // 
            this.btnNextPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNextPage.BackColor = System.Drawing.Color.Transparent;
            this.btnNextPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNextPage.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnNextPage.FlatAppearance.BorderSize = 0;
            this.btnNextPage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNextPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnNextPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNextPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            this.btnNextPage.Image = global::CarePulse.Properties.Resources.forward_24px;
            this.btnNextPage.Location = new System.Drawing.Point(882, 5);
            this.btnNextPage.Name = "btnNextPage";
            this.btnNextPage.Size = new System.Drawing.Size(31, 31);
            this.btnNextPage.TabIndex = 26;
            this.btnNextPage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnNextPage.UseVisualStyleBackColor = false;
            this.btnNextPage.Click += new System.EventHandler(this.btnNextPage_Click);
            // 
            // btnLastPage
            // 
            this.btnLastPage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLastPage.BackColor = System.Drawing.Color.Transparent;
            this.btnLastPage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnLastPage.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnLastPage.FlatAppearance.BorderSize = 0;
            this.btnLastPage.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLastPage.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnLastPage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLastPage.Font = new System.Drawing.Font("Calibri", 9.25F, System.Drawing.FontStyle.Bold);
            this.btnLastPage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            this.btnLastPage.Image = global::CarePulse.Properties.Resources.last_24px;
            this.btnLastPage.Location = new System.Drawing.Point(919, 5);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(31, 31);
            this.btnLastPage.TabIndex = 25;
            this.btnLastPage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLastPage.UseVisualStyleBackColor = false;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel10.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel10.EdgeColor = System.Drawing.Color.WhiteSmoke;
            this.panel10.Location = new System.Drawing.Point(0, 0);
            this.panel10.Name = "panel10";
            this.panel10.Padding = new System.Windows.Forms.Padding(5);
            this.panel10.Size = new System.Drawing.Size(986, 1);
            this.panel10.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panel10.TabIndex = 15;
            this.panel10.Text = "panel10";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.btnRollBack);
            this.panel6.Controls.Add(this.txtboxSearch);
            this.panel6.Controls.Add(this.panel16);
            this.panel6.Controls.Add(this.comboBoxFilterMonth);
            this.panel6.Controls.Add(this.btnClearSearchText);
            this.panel6.Controls.Add(this.btnSearch);
            this.panel6.Controls.Add(this.btnView);
            this.panel6.Controls.Add(this.btnFilter);
            this.panel6.Controls.Add(this.btnExportCSV);
            this.panel6.Controls.Add(this.panel9);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(986, 57);
            this.panel6.TabIndex = 34;
            // 
            // btnRollBack
            // 
            this.btnRollBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRollBack.BackColor = System.Drawing.Color.Transparent;
            this.btnRollBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRollBack.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnRollBack.FlatAppearance.BorderSize = 0;
            this.btnRollBack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnRollBack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnRollBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRollBack.Font = new System.Drawing.Font("Calibri", 9.25F, System.Drawing.FontStyle.Bold);
            this.btnRollBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            this.btnRollBack.Image = global::CarePulse.Properties.Resources.rollback_24px;
            this.btnRollBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnRollBack.Location = new System.Drawing.Point(865, 11);
            this.btnRollBack.Name = "btnRollBack";
            this.btnRollBack.Size = new System.Drawing.Size(31, 31);
            this.btnRollBack.TabIndex = 433;
            this.btnRollBack.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnRollBack.UseVisualStyleBackColor = false;
            this.btnRollBack.Click += new System.EventHandler(this.btnRollBack_Click);
            // 
            // txtboxSearch
            // 
            this.txtboxSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtboxSearch.BackColor = System.Drawing.Color.White;
            this.txtboxSearch.BaseColor = System.Drawing.Color.White;
            this.txtboxSearch.BorderColorA = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(158)))), ((int)(((byte)(255)))));
            this.txtboxSearch.BorderColorB = System.Drawing.Color.DimGray;
            this.txtboxSearch.Font = new System.Drawing.Font("Calibri", 9.25F, System.Drawing.FontStyle.Bold);
            this.txtboxSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtboxSearch.Hint = "Search";
            this.txtboxSearch.Location = new System.Drawing.Point(470, 10);
            this.txtboxSearch.MaxLength = 32767;
            this.txtboxSearch.Multiline = false;
            this.txtboxSearch.Name = "txtboxSearch";
            this.txtboxSearch.PasswordChar = '\0';
            this.txtboxSearch.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.txtboxSearch.SelectedText = "";
            this.txtboxSearch.SelectionLength = 0;
            this.txtboxSearch.SelectionStart = 0;
            this.txtboxSearch.Size = new System.Drawing.Size(255, 32);
            this.txtboxSearch.TabIndex = 432;
            this.txtboxSearch.TabStop = false;
            this.txtboxSearch.UseSystemPasswordChar = false;
            // 
            // panel16
            // 
            this.panel16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel16.BackColor = System.Drawing.Color.Gainsboro;
            this.panel16.EdgeColor = System.Drawing.Color.Gainsboro;
            this.panel16.Location = new System.Drawing.Point(785, 18);
            this.panel16.Name = "panel16";
            this.panel16.Padding = new System.Windows.Forms.Padding(5);
            this.panel16.Size = new System.Drawing.Size(1, 20);
            this.panel16.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panel16.TabIndex = 399;
            this.panel16.Text = "panel16";
            // 
            // comboBoxFilterMonth
            // 
            this.comboBoxFilterMonth.BackColor = System.Drawing.Color.White;
            this.comboBoxFilterMonth.ColorA = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(67)))), ((int)(((byte)(60)))));
            this.comboBoxFilterMonth.ColorB = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(67)))), ((int)(((byte)(60)))));
            this.comboBoxFilterMonth.ColorC = System.Drawing.Color.White;
            this.comboBoxFilterMonth.ColorD = System.Drawing.Color.FromArgb(((int)(((byte)(253)))), ((int)(((byte)(252)))), ((int)(((byte)(252)))));
            this.comboBoxFilterMonth.ColorE = System.Drawing.Color.White;
            this.comboBoxFilterMonth.ColorF = System.Drawing.Color.Gainsboro;
            this.comboBoxFilterMonth.ColorG = System.Drawing.Color.FromArgb(((int)(((byte)(119)))), ((int)(((byte)(119)))), ((int)(((byte)(118)))));
            this.comboBoxFilterMonth.ColorH = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(222)))), ((int)(((byte)(220)))));
            this.comboBoxFilterMonth.ColorI = System.Drawing.Color.White;
            this.comboBoxFilterMonth.Cursor = System.Windows.Forms.Cursors.Hand;
            this.comboBoxFilterMonth.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.comboBoxFilterMonth.DropDownHeight = 100;
            this.comboBoxFilterMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFilterMonth.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxFilterMonth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            this.comboBoxFilterMonth.FormattingEnabled = true;
            this.comboBoxFilterMonth.HoverSelectionColor = System.Drawing.Color.Empty;
            this.comboBoxFilterMonth.IntegralHeight = false;
            this.comboBoxFilterMonth.ItemHeight = 20;
            this.comboBoxFilterMonth.Items.AddRange(new object[] {
            "January",
            "February",
            "March",
            "April",
            "May",
            "June",
            "July",
            "August",
            "September",
            "November",
            "December"});
            this.comboBoxFilterMonth.Location = new System.Drawing.Point(79, 13);
            this.comboBoxFilterMonth.Name = "comboBoxFilterMonth";
            this.comboBoxFilterMonth.Size = new System.Drawing.Size(196, 26);
            this.comboBoxFilterMonth.StartIndex = 0;
            this.comboBoxFilterMonth.TabIndex = 32;
            this.comboBoxFilterMonth.SelectedIndexChanged += new System.EventHandler(this.comboBoxFilterMonth_SelectedIndexChanged);
            // 
            // btnClearSearchText
            // 
            this.btnClearSearchText.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearSearchText.BackColor = System.Drawing.Color.Transparent;
            this.btnClearSearchText.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnClearSearchText.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnClearSearchText.FlatAppearance.BorderSize = 0;
            this.btnClearSearchText.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnClearSearchText.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnClearSearchText.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearSearchText.Font = new System.Drawing.Font("Calibri", 9.25F, System.Drawing.FontStyle.Bold);
            this.btnClearSearchText.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            this.btnClearSearchText.Image = global::CarePulse.Properties.Resources.broom_24px;
            this.btnClearSearchText.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClearSearchText.Location = new System.Drawing.Point(426, 11);
            this.btnClearSearchText.Name = "btnClearSearchText";
            this.btnClearSearchText.Size = new System.Drawing.Size(31, 31);
            this.btnClearSearchText.TabIndex = 398;
            this.btnClearSearchText.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnClearSearchText.UseVisualStyleBackColor = false;
            this.btnClearSearchText.Click += new System.EventHandler(this.btnClearSearchText_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnSearch.FlatAppearance.BorderSize = 0;
            this.btnSearch.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSearch.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("Calibri", 9.25F, System.Drawing.FontStyle.Bold);
            this.btnSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            this.btnSearch.Image = global::CarePulse.Properties.Resources.search_24px;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(735, 11);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(31, 31);
            this.btnSearch.TabIndex = 397;
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // btnView
            // 
            this.btnView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnView.BackColor = System.Drawing.Color.Transparent;
            this.btnView.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnView.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnView.FlatAppearance.BorderSize = 0;
            this.btnView.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnView.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnView.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnView.Font = new System.Drawing.Font("Calibri", 9.25F, System.Drawing.FontStyle.Bold);
            this.btnView.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            this.btnView.Image = global::CarePulse.Properties.Resources.binoculars_24px2;
            this.btnView.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnView.Location = new System.Drawing.Point(919, 11);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(31, 31);
            this.btnView.TabIndex = 32;
            this.btnView.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnView.UseVisualStyleBackColor = false;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnFilter
            // 
            this.btnFilter.BackColor = System.Drawing.Color.Transparent;
            this.btnFilter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFilter.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnFilter.FlatAppearance.BorderSize = 0;
            this.btnFilter.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFilter.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Font = new System.Drawing.Font("Calibri", 9.25F, System.Drawing.FontStyle.Bold);
            this.btnFilter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            this.btnFilter.Image = global::CarePulse.Properties.Resources.filtered_file_24px;
            this.btnFilter.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFilter.Location = new System.Drawing.Point(33, 11);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(31, 31);
            this.btnFilter.TabIndex = 26;
            this.btnFilter.UseVisualStyleBackColor = false;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // btnExportCSV
            // 
            this.btnExportCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportCSV.BackColor = System.Drawing.Color.Transparent;
            this.btnExportCSV.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnExportCSV.FlatAppearance.BorderColor = System.Drawing.Color.WhiteSmoke;
            this.btnExportCSV.FlatAppearance.BorderSize = 0;
            this.btnExportCSV.FlatAppearance.MouseDownBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnExportCSV.FlatAppearance.MouseOverBackColor = System.Drawing.Color.WhiteSmoke;
            this.btnExportCSV.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportCSV.Font = new System.Drawing.Font("Calibri", 9.25F, System.Drawing.FontStyle.Bold);
            this.btnExportCSV.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            this.btnExportCSV.Image = global::CarePulse.Properties.Resources.export_csv_24px;
            this.btnExportCSV.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportCSV.Location = new System.Drawing.Point(809, 11);
            this.btnExportCSV.Name = "btnExportCSV";
            this.btnExportCSV.Size = new System.Drawing.Size(31, 31);
            this.btnExportCSV.TabIndex = 25;
            this.btnExportCSV.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExportCSV.UseVisualStyleBackColor = false;
            this.btnExportCSV.Click += new System.EventHandler(this.btnExportCSV_Click);
            // 
            // panel9
            // 
            this.panel9.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel9.EdgeColor = System.Drawing.Color.WhiteSmoke;
            this.panel9.Location = new System.Drawing.Point(0, 56);
            this.panel9.Name = "panel9";
            this.panel9.Padding = new System.Windows.Forms.Padding(5);
            this.panel9.Size = new System.Drawing.Size(986, 1);
            this.panel9.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panel9.TabIndex = 14;
            this.panel9.Text = "panel9";
            // 
            // datagridReport
            // 
            this.datagridReport.AllowUserToAddRows = false;
            this.datagridReport.AllowUserToDeleteRows = false;
            this.datagridReport.AllowUserToResizeColumns = false;
            this.datagridReport.AllowUserToResizeRows = false;
            dataGridViewCellStyle17.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle17.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle17.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.datagridReport.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle17;
            this.datagridReport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.datagridReport.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.datagridReport.BackgroundColor = System.Drawing.Color.White;
            this.datagridReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.datagridReport.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.datagridReport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.datagridReport.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle18.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(35)))), ((int)(((byte)(29)))));
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(35)))), ((int)(((byte)(29)))));
            dataGridViewCellStyle18.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datagridReport.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.datagridReport.ColumnHeadersHeight = 48;
            this.datagridReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.datagridReport.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cpID,
            this.cpDatePeriod,
            this.cpName,
            this.cpResponseTotal,
            this.cpSurveyScore,
            this.cpMonth,
            this.cpYear,
            this.cpSurveyTemplate,
            this.cpSurveyQuestionsAnswers,
            this.cpPatientsFeedback});
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle19.BackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(58)))), ((int)(((byte)(51)))));
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.Color.WhiteSmoke;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.datagridReport.DefaultCellStyle = dataGridViewCellStyle19;
            this.datagridReport.EnableHeadersVisualStyles = false;
            this.datagridReport.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.datagridReport.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.datagridReport.Location = new System.Drawing.Point(33, 60);
            this.datagridReport.Name = "datagridReport";
            this.datagridReport.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle20.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle20.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle20.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle20.SelectionBackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle20.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle20.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.datagridReport.RowHeadersDefaultCellStyle = dataGridViewCellStyle20;
            this.datagridReport.RowHeadersWidth = 5;
            this.datagridReport.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.datagridReport.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.datagridReport.Size = new System.Drawing.Size(917, 455);
            this.datagridReport.TabIndex = 35;
            this.datagridReport.UseCustomBackColor = true;
            this.datagridReport.UseCustomForeColor = true;
            // 
            // cpID
            // 
            this.cpID.FillWeight = 25F;
            this.cpID.HeaderText = "CPID";
            this.cpID.Name = "cpID";
            // 
            // cpDatePeriod
            // 
            this.cpDatePeriod.FillWeight = 35F;
            this.cpDatePeriod.HeaderText = "Date Period";
            this.cpDatePeriod.Name = "cpDatePeriod";
            // 
            // cpName
            // 
            this.cpName.FillWeight = 70F;
            this.cpName.HeaderText = "Name";
            this.cpName.Name = "cpName";
            // 
            // cpResponseTotal
            // 
            this.cpResponseTotal.FillWeight = 60F;
            this.cpResponseTotal.HeaderText = "Response";
            this.cpResponseTotal.Name = "cpResponseTotal";
            // 
            // cpSurveyScore
            // 
            this.cpSurveyScore.FillWeight = 25F;
            this.cpSurveyScore.HeaderText = "Survey Score";
            this.cpSurveyScore.Name = "cpSurveyScore";
            // 
            // cpMonth
            // 
            this.cpMonth.DividerWidth = 15;
            this.cpMonth.HeaderText = "Month";
            this.cpMonth.Name = "cpMonth";
            this.cpMonth.Visible = false;
            // 
            // cpYear
            // 
            this.cpYear.DividerWidth = 15;
            this.cpYear.HeaderText = "Year";
            this.cpYear.Name = "cpYear";
            this.cpYear.Visible = false;
            // 
            // cpSurveyTemplate
            // 
            this.cpSurveyTemplate.DividerWidth = 30;
            this.cpSurveyTemplate.HeaderText = "Template Name";
            this.cpSurveyTemplate.Name = "cpSurveyTemplate";
            this.cpSurveyTemplate.Visible = false;
            // 
            // cpSurveyQuestionsAnswers
            // 
            this.cpSurveyQuestionsAnswers.DividerWidth = 30;
            this.cpSurveyQuestionsAnswers.HeaderText = "Survey Questions and Answers";
            this.cpSurveyQuestionsAnswers.Name = "cpSurveyQuestionsAnswers";
            this.cpSurveyQuestionsAnswers.Visible = false;
            // 
            // cpPatientsFeedback
            // 
            this.cpPatientsFeedback.FillWeight = 60F;
            this.cpPatientsFeedback.HeaderText = "Patients Feedback";
            this.cpPatientsFeedback.Name = "cpPatientsFeedback";
            this.cpPatientsFeedback.Visible = false;
            // 
            // lblPageInfo
            // 
            this.lblPageInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPageInfo.AutoSize = true;
            this.lblPageInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPageInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            this.lblPageInfo.Location = new System.Drawing.Point(678, 11);
            this.lblPageInfo.Name = "lblPageInfo";
            this.lblPageInfo.Size = new System.Drawing.Size(0, 17);
            this.lblPageInfo.TabIndex = 31;
            this.lblPageInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelFilter
            // 
            this.panelFilter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelFilter.Controls.Add(this.btnViewAll);
            this.panelFilter.Controls.Add(this.btnFilterData);
            this.panelFilter.Controls.Add(this.parrotGradientPanel2);
            this.panelFilter.Controls.Add(this.label3);
            this.panelFilter.Controls.Add(label1);
            this.panelFilter.Controls.Add(this.datePickerEnd);
            this.panelFilter.Controls.Add(this.datePickerStart);
            this.panelFilter.EdgeColor = System.Drawing.Color.White;
            this.panelFilter.Location = new System.Drawing.Point(33, 64);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Padding = new System.Windows.Forms.Padding(5);
            this.panelFilter.Size = new System.Drawing.Size(344, 275);
            this.panelFilter.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            this.panelFilter.TabIndex = 36;
            this.panelFilter.Text = "panel16";
            this.panelFilter.Visible = false;
            // 
            // btnViewAll
            // 
            this.btnViewAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnViewAll.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.btnViewAll.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            this.btnViewAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnViewAll.DangerColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnViewAll.DefaultColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnViewAll.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnViewAll.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnViewAll.InfoColor = System.Drawing.Color.White;
            this.btnViewAll.Location = new System.Drawing.Point(43, 183);
            this.btnViewAll.Name = "btnViewAll";
            this.btnViewAll.PrimaryColor = System.Drawing.Color.Peru;
            this.btnViewAll.Size = new System.Drawing.Size(259, 32);
            this.btnViewAll.SuccessColor = System.Drawing.Color.SandyBrown;
            this.btnViewAll.TabIndex = 400;
            this.btnViewAll.Text = "REMOVE FILTER";
            this.btnViewAll.TextColor = System.Drawing.Color.White;
            this.btnViewAll.WarningColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnViewAll.Click += new System.EventHandler(this.btnViewAll_Click);
            // 
            // btnFilterData
            // 
            this.btnFilterData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFilterData.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(223)))), ((int)(((byte)(230)))));
            this.btnFilterData.ButtonType = ReaLTaiizor.Util.HopeButtonType.Primary;
            this.btnFilterData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFilterData.DangerColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(108)))), ((int)(((byte)(108)))));
            this.btnFilterData.DefaultColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnFilterData.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFilterData.HoverTextColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnFilterData.InfoColor = System.Drawing.Color.White;
            this.btnFilterData.Location = new System.Drawing.Point(43, 132);
            this.btnFilterData.Name = "btnFilterData";
            this.btnFilterData.PrimaryColor = System.Drawing.Color.MediumSeaGreen;
            this.btnFilterData.Size = new System.Drawing.Size(259, 32);
            this.btnFilterData.SuccessColor = System.Drawing.Color.SeaGreen;
            this.btnFilterData.TabIndex = 399;
            this.btnFilterData.Text = "APPLY FILTER";
            this.btnFilterData.TextColor = System.Drawing.Color.White;
            this.btnFilterData.WarningColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.btnFilterData.Click += new System.EventHandler(this.btnFilterData_Click);
            // 
            // parrotGradientPanel2
            // 
            this.parrotGradientPanel2.BottomLeft = System.Drawing.Color.SeaGreen;
            this.parrotGradientPanel2.BottomRight = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.parrotGradientPanel2.CompositingQualityType = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            this.parrotGradientPanel2.InterpolationType = System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
            this.parrotGradientPanel2.Location = new System.Drawing.Point(43, 107);
            this.parrotGradientPanel2.Name = "parrotGradientPanel2";
            this.parrotGradientPanel2.PixelOffsetType = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            this.parrotGradientPanel2.PrimerColor = System.Drawing.Color.White;
            this.parrotGradientPanel2.Size = new System.Drawing.Size(259, 1);
            this.parrotGradientPanel2.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            this.parrotGradientPanel2.Style = ReaLTaiizor.Controls.ParrotGradientPanel.GradientStyle.Corners;
            this.parrotGradientPanel2.TabIndex = 397;
            this.parrotGradientPanel2.TextRenderingType = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            this.parrotGradientPanel2.TopLeft = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.parrotGradientPanel2.TopRight = System.Drawing.Color.Black;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(67)))), ((int)(((byte)(60)))));
            this.label3.Location = new System.Drawing.Point(249, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 15);
            this.label3.TabIndex = 396;
            this.label3.Text = "END DATE";
            // 
            // label1
            // 
            label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(67)))), ((int)(((byte)(60)))));
            label1.Location = new System.Drawing.Point(35, 35);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(83, 15);
            label1.TabIndex = 395;
            label1.Text = "STARTED DATE";
            // 
            // datePickerEnd
            // 
            this.datePickerEnd.BorderColor = System.Drawing.Color.Gray;
            this.datePickerEnd.BorderSize = 0;
            this.datePickerEnd.FillColor = System.Drawing.Color.WhiteSmoke;
            this.datePickerEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
            this.datePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerEnd.Location = new System.Drawing.Point(181, 63);
            this.datePickerEnd.MinimumSize = new System.Drawing.Size(4, 35);
            this.datePickerEnd.Name = "datePickerEnd";
            this.datePickerEnd.ShowIconOnly = false;
            this.datePickerEnd.Size = new System.Drawing.Size(132, 35);
            this.datePickerEnd.TabIndex = 33;
            this.datePickerEnd.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            // 
            // datePickerStart
            // 
            this.datePickerStart.BorderColor = System.Drawing.Color.Gray;
            this.datePickerStart.BorderSize = 0;
            this.datePickerStart.FillColor = System.Drawing.Color.WhiteSmoke;
            this.datePickerStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.5F);
            this.datePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.datePickerStart.Location = new System.Drawing.Point(25, 63);
            this.datePickerStart.MinimumSize = new System.Drawing.Size(4, 35);
            this.datePickerStart.Name = "datePickerStart";
            this.datePickerStart.ShowIconOnly = false;
            this.datePickerStart.Size = new System.Drawing.Size(132, 35);
            this.datePickerStart.TabIndex = 32;
            this.datePickerStart.TextColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(53)))), ((int)(((byte)(44)))));
            // 
            // Report
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panelFilter);
            this.Controls.Add(this.datagridReport);
            this.Controls.Add(this.panel6);
            this.Controls.Add(this.panel8);
            this.Name = "Report";
            this.Size = new System.Drawing.Size(986, 563);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.datagridReport)).EndInit();
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel8;
        private ReaLTaiizor.Controls.Panel panel11;
        private System.Windows.Forms.Button btnStartPage;
        private System.Windows.Forms.Button btnPreviousPage;
        private System.Windows.Forms.Button btnNextPage;
        private System.Windows.Forms.Button btnLastPage;
        private ReaLTaiizor.Controls.Panel panel10;
        private System.Windows.Forms.Panel panel6;
        private ReaLTaiizor.Controls.HopeTextBox txtboxSearch;
        private ReaLTaiizor.Controls.Panel panel16;
        private System.Windows.Forms.Button btnClearSearchText;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Button btnView;
        private System.Windows.Forms.Button btnExportCSV;
        private ReaLTaiizor.Controls.Panel panel9;
        private ReaLTaiizor.Controls.PoisonDataGridView datagridReport;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpDatePeriod;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpResponseTotal;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpSurveyScore;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpMonth;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpYear;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpSurveyTemplate;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpSurveyQuestionsAnswers;
        private System.Windows.Forms.DataGridViewTextBoxColumn cpPatientsFeedback;
        private System.Windows.Forms.Button btnRollBack;
        private ReaLTaiizor.Controls.DungeonComboBox comboBoxFilterMonth;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.Label lblPageInfo;
        private ReaLTaiizor.Controls.Panel panelFilter;
        private ReaLTaiizor.Controls.HopeButton btnViewAll;
        private ReaLTaiizor.Controls.HopeButton btnFilterData;
        private ReaLTaiizor.Controls.ParrotGradientPanel parrotGradientPanel2;
        private System.Windows.Forms.Label label3;
        private CustomDatePicker datePickerEnd;
        private CustomDatePicker datePickerStart;
    }
}
