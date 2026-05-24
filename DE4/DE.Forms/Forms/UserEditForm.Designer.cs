namespace DE.Forms.Forms
{
    partial class UserEditForm
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
            rootLayout = new TableLayoutPanel();
            fieldsGroupBox = new GroupBox();
            fieldsLayout = new TableLayoutPanel();
            fullNameLabel = new Label();
            fullNameTextBox = new TextBox();
            loginLabel = new Label();
            loginTextBox = new TextBox();
            roleLabel = new Label();
            roleComboBox = new ComboBox();
            passwordLabel = new Label();
            passwordTextBox = new TextBox();
            confirmPasswordLabel = new Label();
            confirmPasswordTextBox = new TextBox();
            passwordHintLabel = new Label();
            isBlockedCheckBox = new CheckBox();
            buttonsPanel = new FlowLayoutPanel();
            saveButton = new Button();
            cancelButton = new Button();
            rootLayout.SuspendLayout();
            fieldsGroupBox.SuspendLayout();
            fieldsLayout.SuspendLayout();
            buttonsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // rootLayout
            // 
            rootLayout.ColumnCount = 1;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.Controls.Add(fieldsGroupBox, 0, 0);
            rootLayout.Controls.Add(buttonsPanel, 0, 1);
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Location = new Point(0, 0);
            rootLayout.Name = "rootLayout";
            rootLayout.Padding = new Padding(18);
            rootLayout.RowCount = 2;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            rootLayout.Size = new Size(640, 480);
            rootLayout.TabIndex = 0;
            // 
            // fieldsGroupBox
            // 
            fieldsGroupBox.BackColor = Color.White;
            fieldsGroupBox.Controls.Add(fieldsLayout);
            fieldsGroupBox.Dock = DockStyle.Fill;
            fieldsGroupBox.Location = new Point(21, 21);
            fieldsGroupBox.Name = "fieldsGroupBox";
            fieldsGroupBox.Padding = new Padding(14, 22, 14, 14);
            fieldsGroupBox.Size = new Size(598, 390);
            fieldsGroupBox.TabIndex = 0;
            fieldsGroupBox.TabStop = false;
            fieldsGroupBox.Text = "Данные нового пользователя";
            // 
            // fieldsLayout
            // 
            fieldsLayout.AutoSize = true;
            fieldsLayout.ColumnCount = 2;
            fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
            fieldsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            fieldsLayout.Controls.Add(fullNameLabel, 0, 0);
            fieldsLayout.Controls.Add(fullNameTextBox, 1, 0);
            fieldsLayout.Controls.Add(loginLabel, 0, 1);
            fieldsLayout.Controls.Add(loginTextBox, 1, 1);
            fieldsLayout.Controls.Add(roleLabel, 0, 2);
            fieldsLayout.Controls.Add(roleComboBox, 1, 2);
            fieldsLayout.Controls.Add(passwordLabel, 0, 3);
            fieldsLayout.Controls.Add(passwordTextBox, 1, 3);
            fieldsLayout.Controls.Add(confirmPasswordLabel, 0, 4);
            fieldsLayout.Controls.Add(confirmPasswordTextBox, 1, 4);
            fieldsLayout.Controls.Add(passwordHintLabel, 1, 5);
            fieldsLayout.Controls.Add(isBlockedCheckBox, 1, 6);
            fieldsLayout.Dock = DockStyle.Fill;
            fieldsLayout.Location = new Point(14, 38);
            fieldsLayout.Name = "fieldsLayout";
            fieldsLayout.RowCount = 7;
            fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            fieldsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            fieldsLayout.Size = new Size(570, 338);
            fieldsLayout.TabIndex = 0;
            // 
            // fullNameLabel
            // 
            fullNameLabel.AutoSize = true;
            fullNameLabel.Dock = DockStyle.Fill;
            fullNameLabel.Location = new Point(3, 0);
            fullNameLabel.Name = "fullNameLabel";
            fullNameLabel.Padding = new Padding(0, 6, 12, 0);
            fullNameLabel.Size = new Size(164, 39);
            fullNameLabel.TabIndex = 0;
            fullNameLabel.Text = "ФИО:";
            // 
            // fullNameTextBox
            // 
            fullNameTextBox.Dock = DockStyle.Fill;
            fullNameTextBox.Location = new Point(173, 3);
            fullNameTextBox.Margin = new Padding(3, 3, 3, 13);
            fullNameTextBox.Name = "fullNameTextBox";
            fullNameTextBox.Size = new Size(394, 23);
            fullNameTextBox.TabIndex = 1;
            // 
            // loginLabel
            // 
            loginLabel.AutoSize = true;
            loginLabel.Dock = DockStyle.Fill;
            loginLabel.Location = new Point(3, 39);
            loginLabel.Name = "loginLabel";
            loginLabel.Padding = new Padding(0, 6, 12, 0);
            loginLabel.Size = new Size(164, 39);
            loginLabel.TabIndex = 2;
            loginLabel.Text = "Логин:";
            // 
            // loginTextBox
            // 
            loginTextBox.Dock = DockStyle.Fill;
            loginTextBox.Location = new Point(173, 42);
            loginTextBox.Margin = new Padding(3, 3, 3, 13);
            loginTextBox.Name = "loginTextBox";
            loginTextBox.Size = new Size(394, 23);
            loginTextBox.TabIndex = 3;
            // 
            // roleLabel
            // 
            roleLabel.AutoSize = true;
            roleLabel.Dock = DockStyle.Fill;
            roleLabel.Location = new Point(3, 78);
            roleLabel.Name = "roleLabel";
            roleLabel.Padding = new Padding(0, 6, 12, 0);
            roleLabel.Size = new Size(164, 39);
            roleLabel.TabIndex = 4;
            roleLabel.Text = "Роль:";
            // 
            // roleComboBox
            // 
            roleComboBox.Dock = DockStyle.Fill;
            roleComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            roleComboBox.FormattingEnabled = true;
            roleComboBox.Location = new Point(173, 81);
            roleComboBox.Margin = new Padding(3, 3, 3, 13);
            roleComboBox.Name = "roleComboBox";
            roleComboBox.Size = new Size(394, 23);
            roleComboBox.TabIndex = 5;
            // 
            // passwordLabel
            // 
            passwordLabel.AutoSize = true;
            passwordLabel.Dock = DockStyle.Fill;
            passwordLabel.Location = new Point(3, 117);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Padding = new Padding(0, 6, 12, 0);
            passwordLabel.Size = new Size(164, 39);
            passwordLabel.TabIndex = 6;
            passwordLabel.Text = "Пароль:";
            // 
            // passwordTextBox
            // 
            passwordTextBox.Dock = DockStyle.Fill;
            passwordTextBox.Location = new Point(173, 120);
            passwordTextBox.Margin = new Padding(3, 3, 3, 13);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new Size(394, 23);
            passwordTextBox.TabIndex = 7;
            passwordTextBox.UseSystemPasswordChar = true;
            // 
            // confirmPasswordLabel
            // 
            confirmPasswordLabel.AutoSize = true;
            confirmPasswordLabel.Dock = DockStyle.Fill;
            confirmPasswordLabel.Location = new Point(3, 156);
            confirmPasswordLabel.Name = "confirmPasswordLabel";
            confirmPasswordLabel.Padding = new Padding(0, 6, 12, 0);
            confirmPasswordLabel.Size = new Size(164, 39);
            confirmPasswordLabel.TabIndex = 8;
            confirmPasswordLabel.Text = "Повтор пароля:";
            // 
            // confirmPasswordTextBox
            // 
            confirmPasswordTextBox.Dock = DockStyle.Fill;
            confirmPasswordTextBox.Location = new Point(173, 159);
            confirmPasswordTextBox.Margin = new Padding(3, 3, 3, 13);
            confirmPasswordTextBox.Name = "confirmPasswordTextBox";
            confirmPasswordTextBox.Size = new Size(394, 23);
            confirmPasswordTextBox.TabIndex = 9;
            confirmPasswordTextBox.UseSystemPasswordChar = true;
            // 
            // passwordHintLabel
            // 
            passwordHintLabel.AutoSize = true;
            passwordHintLabel.Dock = DockStyle.Top;
            passwordHintLabel.ForeColor = Color.FromArgb(91, 98, 111);
            passwordHintLabel.Location = new Point(173, 195);
            passwordHintLabel.Margin = new Padding(3, 0, 3, 12);
            passwordHintLabel.Name = "passwordHintLabel";
            passwordHintLabel.Size = new Size(394, 15);
            passwordHintLabel.TabIndex = 10;
            passwordHintLabel.Text = "Пароль обязателен для нового пользователя.";
            // 
            // isBlockedCheckBox
            // 
            isBlockedCheckBox.AutoSize = true;
            isBlockedCheckBox.Dock = DockStyle.Top;
            isBlockedCheckBox.Enabled = false;
            isBlockedCheckBox.Location = new Point(173, 225);
            isBlockedCheckBox.Margin = new Padding(3, 3, 3, 13);
            isBlockedCheckBox.Name = "isBlockedCheckBox";
            isBlockedCheckBox.Size = new Size(394, 19);
            isBlockedCheckBox.TabIndex = 11;
            isBlockedCheckBox.Text = "Учетная запись заблокирована";
            isBlockedCheckBox.UseVisualStyleBackColor = true;
            // 
            // buttonsPanel
            // 
            buttonsPanel.AutoSize = true;
            buttonsPanel.Controls.Add(saveButton);
            buttonsPanel.Controls.Add(cancelButton);
            buttonsPanel.Dock = DockStyle.Top;
            buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonsPanel.Location = new Point(21, 417);
            buttonsPanel.Name = "buttonsPanel";
            buttonsPanel.Size = new Size(598, 42);
            buttonsPanel.TabIndex = 1;
            buttonsPanel.WrapContents = false;
            // 
            // saveButton
            // 
            saveButton.BackColor = Color.FromArgb(35, 102, 180);
            saveButton.FlatAppearance.BorderSize = 0;
            saveButton.FlatStyle = FlatStyle.Flat;
            saveButton.ForeColor = Color.White;
            saveButton.Location = new Point(470, 0);
            saveButton.Margin = new Padding(8, 0, 0, 0);
            saveButton.Name = "saveButton";
            saveButton.Padding = new Padding(18, 8, 18, 8);
            saveButton.Size = new Size(128, 42);
            saveButton.TabIndex = 0;
            saveButton.Text = "Сохранить";
            saveButton.UseVisualStyleBackColor = false;
            // 
            // cancelButton
            // 
            cancelButton.BackColor = Color.FromArgb(232, 238, 246);
            cancelButton.FlatAppearance.BorderColor = Color.FromArgb(218, 224, 232);
            cancelButton.FlatStyle = FlatStyle.Flat;
            cancelButton.ForeColor = Color.FromArgb(26, 42, 64);
            cancelButton.Location = new Point(356, 0);
            cancelButton.Margin = new Padding(0);
            cancelButton.Name = "cancelButton";
            cancelButton.Padding = new Padding(18, 8, 18, 8);
            cancelButton.Size = new Size(106, 42);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Отмена";
            cancelButton.UseVisualStyleBackColor = false;
            // 
            // UserEditForm
            // 
            AcceptButton = saveButton;
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            CancelButton = cancelButton;
            ClientSize = new Size(640, 480);
            Controls.Add(rootLayout);
            MinimumSize = new Size(560, 420);
            Name = "UserEditForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Добавление пользователя";
            rootLayout.ResumeLayout(false);
            rootLayout.PerformLayout();
            fieldsGroupBox.ResumeLayout(false);
            fieldsGroupBox.PerformLayout();
            fieldsLayout.ResumeLayout(false);
            fieldsLayout.PerformLayout();
            buttonsPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel rootLayout;
        private GroupBox fieldsGroupBox;
        private TableLayoutPanel fieldsLayout;
        private Label fullNameLabel;
        private TextBox fullNameTextBox;
        private Label loginLabel;
        private TextBox loginTextBox;
        private Label roleLabel;
        private ComboBox roleComboBox;
        private Label passwordLabel;
        private TextBox passwordTextBox;
        private Label confirmPasswordLabel;
        private TextBox confirmPasswordTextBox;
        private Label passwordHintLabel;
        private CheckBox isBlockedCheckBox;
        private FlowLayoutPanel buttonsPanel;
        private Button saveButton;
        private Button cancelButton;
    }
}
