namespace DE.Forms.Forms
{
    partial class AdminDashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            rootLayout = new TableLayoutPanel();
            headerPanel = new Panel();
            currentUserLabel = new Label();
            titleLabel = new Label();
            toolbarGroupBox = new GroupBox();
            toolbarLayout = new TableLayoutPanel();
            searchTextBox = new TextBox();
            searchButton = new Button();
            refreshButton = new Button();
            addButton = new Button();
            editButton = new Button();
            unblockButton = new Button();
            exitButton = new Button();
            summaryLabel = new Label();
            gridGroupBox = new GroupBox();
            usersGridView = new DataGridView();
            fullNameColumn = new DataGridViewTextBoxColumn();
            loginColumn = new DataGridViewTextBoxColumn();
            roleColumn = new DataGridViewTextBoxColumn();
            failedAttemptsColumn = new DataGridViewTextBoxColumn();
            isBlockedColumn = new DataGridViewCheckBoxColumn();
            createdAtColumn = new DataGridViewTextBoxColumn();
            rootLayout.SuspendLayout();
            headerPanel.SuspendLayout();
            toolbarGroupBox.SuspendLayout();
            toolbarLayout.SuspendLayout();
            gridGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)usersGridView).BeginInit();
            SuspendLayout();
            // 
            // rootLayout
            // 
            rootLayout.ColumnCount = 1;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.Controls.Add(headerPanel, 0, 0);
            rootLayout.Controls.Add(toolbarGroupBox, 0, 1);
            rootLayout.Controls.Add(gridGroupBox, 0, 2);
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Location = new Point(0, 0);
            rootLayout.Name = "rootLayout";
            rootLayout.Padding = new Padding(20);
            rootLayout.RowCount = 3;
            rootLayout.RowStyles.Add(new RowStyle());
            rootLayout.RowStyles.Add(new RowStyle());
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.Size = new Size(1180, 780);
            rootLayout.TabIndex = 0;
            // 
            // headerPanel
            // 
            headerPanel.Controls.Add(currentUserLabel);
            headerPanel.Controls.Add(titleLabel);
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Location = new Point(23, 23);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(1134, 70);
            headerPanel.TabIndex = 0;
            // 
            // currentUserLabel
            // 
            currentUserLabel.Dock = DockStyle.Top;
            currentUserLabel.ForeColor = Color.FromArgb(91, 98, 111);
            currentUserLabel.Location = new Point(0, 36);
            currentUserLabel.Name = "currentUserLabel";
            currentUserLabel.Size = new Size(1134, 23);
            currentUserLabel.TabIndex = 1;
            currentUserLabel.Text = "Вы вошли как: пользователь";
            // 
            // titleLabel
            // 
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            titleLabel.Location = new Point(0, 0);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(1134, 36);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Панель администратора";
            // 
            // toolbarGroupBox
            // 
            toolbarGroupBox.BackColor = Color.White;
            toolbarGroupBox.Controls.Add(toolbarLayout);
            toolbarGroupBox.Dock = DockStyle.Top;
            toolbarGroupBox.Location = new Point(23, 99);
            toolbarGroupBox.Name = "toolbarGroupBox";
            toolbarGroupBox.Padding = new Padding(14, 22, 14, 14);
            toolbarGroupBox.Size = new Size(1134, 118);
            toolbarGroupBox.TabIndex = 1;
            toolbarGroupBox.TabStop = false;
            toolbarGroupBox.Text = "Управление пользователями";
            // 
            // toolbarLayout
            // 
            toolbarLayout.ColumnCount = 7;
            toolbarLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            toolbarLayout.ColumnStyles.Add(new ColumnStyle());
            toolbarLayout.ColumnStyles.Add(new ColumnStyle());
            toolbarLayout.ColumnStyles.Add(new ColumnStyle());
            toolbarLayout.ColumnStyles.Add(new ColumnStyle());
            toolbarLayout.ColumnStyles.Add(new ColumnStyle());
            toolbarLayout.ColumnStyles.Add(new ColumnStyle());
            toolbarLayout.Controls.Add(searchTextBox, 0, 0);
            toolbarLayout.Controls.Add(searchButton, 1, 0);
            toolbarLayout.Controls.Add(refreshButton, 2, 0);
            toolbarLayout.Controls.Add(addButton, 3, 0);
            toolbarLayout.Controls.Add(editButton, 4, 0);
            toolbarLayout.Controls.Add(unblockButton, 5, 0);
            toolbarLayout.Controls.Add(exitButton, 6, 0);
            toolbarLayout.Controls.Add(summaryLabel, 0, 1);
            toolbarLayout.Dock = DockStyle.Fill;
            toolbarLayout.Location = new Point(14, 38);
            toolbarLayout.Name = "toolbarLayout";
            toolbarLayout.RowCount = 2;
            toolbarLayout.RowStyles.Add(new RowStyle());
            toolbarLayout.RowStyles.Add(new RowStyle());
            toolbarLayout.Size = new Size(1106, 66);
            toolbarLayout.TabIndex = 0;
            // 
            // searchTextBox
            // 
            searchTextBox.Dock = DockStyle.Fill;
            searchTextBox.Location = new Point(3, 3);
            searchTextBox.Margin = new Padding(3, 3, 10, 8);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new Size(433, 23);
            searchTextBox.TabIndex = 0;
            // 
            // searchButton
            // 
            searchButton.BackColor = Color.FromArgb(35, 102, 180);
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.FlatStyle = FlatStyle.Flat;
            searchButton.ForeColor = Color.White;
            searchButton.Location = new Point(446, 3);
            searchButton.Margin = new Padding(0, 3, 8, 8);
            searchButton.Name = "searchButton";
            searchButton.Padding = new Padding(12, 6, 12, 6);
            searchButton.Size = new Size(75, 42);
            searchButton.TabIndex = 1;
            searchButton.Text = "Поиск";
            searchButton.UseVisualStyleBackColor = false;
            // 
            // refreshButton
            // 
            refreshButton.BackColor = Color.FromArgb(232, 238, 246);
            refreshButton.FlatAppearance.BorderColor = Color.FromArgb(218, 224, 232);
            refreshButton.FlatStyle = FlatStyle.Flat;
            refreshButton.ForeColor = Color.FromArgb(26, 42, 64);
            refreshButton.Location = new Point(529, 3);
            refreshButton.Margin = new Padding(0, 3, 8, 8);
            refreshButton.Name = "refreshButton";
            refreshButton.Padding = new Padding(12, 6, 12, 6);
            refreshButton.Size = new Size(98, 42);
            refreshButton.TabIndex = 2;
            refreshButton.Text = "Обновить";
            refreshButton.UseVisualStyleBackColor = false;
            // 
            // addButton
            // 
            addButton.BackColor = Color.FromArgb(35, 102, 180);
            addButton.FlatAppearance.BorderSize = 0;
            addButton.FlatStyle = FlatStyle.Flat;
            addButton.ForeColor = Color.White;
            addButton.Location = new Point(635, 3);
            addButton.Margin = new Padding(0, 3, 8, 8);
            addButton.Name = "addButton";
            addButton.Padding = new Padding(12, 6, 12, 6);
            addButton.Size = new Size(97, 42);
            addButton.TabIndex = 3;
            addButton.Text = "Добавить";
            addButton.UseVisualStyleBackColor = false;
            // 
            // editButton
            // 
            editButton.BackColor = Color.FromArgb(232, 238, 246);
            editButton.FlatAppearance.BorderColor = Color.FromArgb(218, 224, 232);
            editButton.FlatStyle = FlatStyle.Flat;
            editButton.ForeColor = Color.FromArgb(26, 42, 64);
            editButton.Location = new Point(740, 3);
            editButton.Margin = new Padding(0, 3, 8, 8);
            editButton.Name = "editButton";
            editButton.Padding = new Padding(12, 6, 12, 6);
            editButton.Size = new Size(126, 42);
            editButton.TabIndex = 4;
            editButton.Text = "Редактировать";
            editButton.UseVisualStyleBackColor = false;
            // 
            // unblockButton
            // 
            unblockButton.BackColor = Color.FromArgb(232, 238, 246);
            unblockButton.FlatAppearance.BorderColor = Color.FromArgb(218, 224, 232);
            unblockButton.FlatStyle = FlatStyle.Flat;
            unblockButton.ForeColor = Color.FromArgb(26, 42, 64);
            unblockButton.Location = new Point(874, 3);
            unblockButton.Margin = new Padding(0, 3, 8, 8);
            unblockButton.Name = "unblockButton";
            unblockButton.Padding = new Padding(12, 6, 12, 6);
            unblockButton.Size = new Size(144, 42);
            unblockButton.TabIndex = 5;
            unblockButton.Text = "Снять блокировку";
            unblockButton.UseVisualStyleBackColor = false;
            // 
            // exitButton
            // 
            exitButton.BackColor = Color.FromArgb(232, 238, 246);
            exitButton.FlatAppearance.BorderColor = Color.FromArgb(218, 224, 232);
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.ForeColor = Color.FromArgb(26, 42, 64);
            exitButton.Location = new Point(1026, 3);
            exitButton.Margin = new Padding(0, 3, 0, 8);
            exitButton.Name = "exitButton";
            exitButton.Padding = new Padding(12, 6, 12, 6);
            exitButton.Size = new Size(80, 42);
            exitButton.TabIndex = 6;
            exitButton.Text = "Выход";
            exitButton.UseVisualStyleBackColor = false;
            // 
            // summaryLabel
            // 
            summaryLabel.AutoSize = true;
            toolbarLayout.SetColumnSpan(summaryLabel, 7);
            summaryLabel.Dock = DockStyle.Top;
            summaryLabel.ForeColor = Color.FromArgb(91, 98, 111);
            summaryLabel.Location = new Point(3, 53);
            summaryLabel.Name = "summaryLabel";
            summaryLabel.Size = new Size(1100, 15);
            summaryLabel.TabIndex = 7;
            summaryLabel.Text = "Загрузка данных...";
            // 
            // gridGroupBox
            // 
            gridGroupBox.BackColor = Color.White;
            gridGroupBox.Controls.Add(usersGridView);
            gridGroupBox.Dock = DockStyle.Fill;
            gridGroupBox.Location = new Point(23, 223);
            gridGroupBox.Name = "gridGroupBox";
            gridGroupBox.Padding = new Padding(14, 22, 14, 14);
            gridGroupBox.Size = new Size(1134, 534);
            gridGroupBox.TabIndex = 2;
            gridGroupBox.TabStop = false;
            gridGroupBox.Text = "Список пользователей";
            // 
            // usersGridView
            // 
            usersGridView.AllowUserToAddRows = false;
            usersGridView.AllowUserToDeleteRows = false;
            usersGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            usersGridView.BackgroundColor = Color.White;
            usersGridView.BorderStyle = BorderStyle.None;
            usersGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            usersGridView.Columns.AddRange(new DataGridViewColumn[] { fullNameColumn, loginColumn, roleColumn, failedAttemptsColumn, isBlockedColumn, createdAtColumn });
            usersGridView.Dock = DockStyle.Fill;
            usersGridView.Location = new Point(14, 38);
            usersGridView.MultiSelect = false;
            usersGridView.Name = "usersGridView";
            usersGridView.ReadOnly = true;
            usersGridView.RowHeadersVisible = false;
            usersGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            usersGridView.Size = new Size(1106, 482);
            usersGridView.TabIndex = 0;
            // 
            // fullNameColumn
            // 
            fullNameColumn.DataPropertyName = "FullName";
            fullNameColumn.FillWeight = 180F;
            fullNameColumn.HeaderText = "ФИО";
            fullNameColumn.Name = "fullNameColumn";
            fullNameColumn.ReadOnly = true;
            // 
            // loginColumn
            // 
            loginColumn.DataPropertyName = "Login";
            loginColumn.HeaderText = "Логин";
            loginColumn.Name = "loginColumn";
            loginColumn.ReadOnly = true;
            // 
            // roleColumn
            // 
            roleColumn.DataPropertyName = "RoleName";
            roleColumn.FillWeight = 110F;
            roleColumn.HeaderText = "Роль";
            roleColumn.Name = "roleColumn";
            roleColumn.ReadOnly = true;
            // 
            // failedAttemptsColumn
            // 
            failedAttemptsColumn.DataPropertyName = "FailedAttempts";
            failedAttemptsColumn.FillWeight = 70F;
            failedAttemptsColumn.HeaderText = "Ошибок";
            failedAttemptsColumn.Name = "failedAttemptsColumn";
            failedAttemptsColumn.ReadOnly = true;
            // 
            // isBlockedColumn
            // 
            isBlockedColumn.DataPropertyName = "IsBlocked";
            isBlockedColumn.FillWeight = 90F;
            isBlockedColumn.HeaderText = "Заблокирован";
            isBlockedColumn.Name = "isBlockedColumn";
            isBlockedColumn.ReadOnly = true;
            // 
            // createdAtColumn
            // 
            createdAtColumn.DataPropertyName = "CreatedAt";
            dataGridViewCellStyle1.Format = "dd.MM.yyyy HH:mm";
            createdAtColumn.DefaultCellStyle = dataGridViewCellStyle1;
            createdAtColumn.FillWeight = 120F;
            createdAtColumn.HeaderText = "Создан";
            createdAtColumn.Name = "createdAtColumn";
            createdAtColumn.ReadOnly = true;
            // 
            // AdminDashboardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(1180, 780);
            Controls.Add(rootLayout);
            MinimumSize = new Size(980, 680);
            Name = "AdminDashboardForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "DE - Рабочий стол администратора";
            rootLayout.ResumeLayout(false);
            headerPanel.ResumeLayout(false);
            toolbarGroupBox.ResumeLayout(false);
            toolbarLayout.ResumeLayout(false);
            toolbarLayout.PerformLayout();
            gridGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)usersGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel rootLayout;
        private Panel headerPanel;
        private Label currentUserLabel;
        private Label titleLabel;
        private GroupBox toolbarGroupBox;
        private TableLayoutPanel toolbarLayout;
        private TextBox searchTextBox;
        private Button searchButton;
        private Button refreshButton;
        private Button addButton;
        private Button editButton;
        private Button unblockButton;
        private Button exitButton;
        private Label summaryLabel;
        private GroupBox gridGroupBox;
        private DataGridView usersGridView;
        private DataGridViewTextBoxColumn fullNameColumn;
        private DataGridViewTextBoxColumn loginColumn;
        private DataGridViewTextBoxColumn roleColumn;
        private DataGridViewTextBoxColumn failedAttemptsColumn;
        private DataGridViewCheckBoxColumn isBlockedColumn;
        private DataGridViewTextBoxColumn createdAtColumn;
    }
}
