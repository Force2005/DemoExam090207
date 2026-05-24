namespace DE.Forms
{
    partial class Form1
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
            authGroupBox = new GroupBox();
            authLayout = new TableLayoutPanel();
            titleLabel = new Label();
            loginPanel = new Panel();
            loginTextBox = new TextBox();
            loginLabel = new Label();
            passwordPanel = new Panel();
            passwordTextBox = new TextBox();
            passwordLabel = new Label();
            noteLabel = new Label();
            buttonsPanel = new FlowLayoutPanel();
            loginButton = new Button();
            closeButton = new Button();
            captchaGroupBox = new GroupBox();
            puzzleCaptchaControl = new DE.Forms.Controls.PuzzleCaptchaControl();
            rootLayout.SuspendLayout();
            authGroupBox.SuspendLayout();
            authLayout.SuspendLayout();
            loginPanel.SuspendLayout();
            passwordPanel.SuspendLayout();
            buttonsPanel.SuspendLayout();
            captchaGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // rootLayout
            // 
            rootLayout.ColumnCount = 2;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 380F));
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.Controls.Add(authGroupBox, 0, 0);
            rootLayout.Controls.Add(captchaGroupBox, 1, 0);
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Location = new Point(0, 0);
            rootLayout.Name = "rootLayout";
            rootLayout.Padding = new Padding(20);
            rootLayout.RowCount = 1;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.Size = new Size(1160, 720);
            rootLayout.TabIndex = 0;
            // 
            // authGroupBox
            // 
            authGroupBox.BackColor = Color.White;
            authGroupBox.Controls.Add(authLayout);
            authGroupBox.Dock = DockStyle.Fill;
            authGroupBox.Location = new Point(23, 23);
            authGroupBox.Name = "authGroupBox";
            authGroupBox.Padding = new Padding(14, 22, 14, 14);
            authGroupBox.Size = new Size(374, 674);
            authGroupBox.TabIndex = 0;
            authGroupBox.TabStop = false;
            authGroupBox.Text = "Вход в систему";
            // 
            // authLayout
            // 
            authLayout.ColumnCount = 1;
            authLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            authLayout.Controls.Add(titleLabel, 0, 0);
            authLayout.Controls.Add(loginPanel, 0, 1);
            authLayout.Controls.Add(passwordPanel, 0, 2);
            authLayout.Controls.Add(noteLabel, 0, 3);
            authLayout.Controls.Add(buttonsPanel, 0, 5);
            authLayout.Dock = DockStyle.Fill;
            authLayout.Location = new Point(14, 38);
            authLayout.Name = "authLayout";
            authLayout.RowCount = 6;
            authLayout.RowStyles.Add(new RowStyle());
            authLayout.RowStyles.Add(new RowStyle());
            authLayout.RowStyles.Add(new RowStyle());
            authLayout.RowStyles.Add(new RowStyle());
            authLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            authLayout.RowStyles.Add(new RowStyle());
            authLayout.Size = new Size(346, 622);
            authLayout.TabIndex = 0;
            // 
            // titleLabel
            // 
            titleLabel.AutoSize = true;
            titleLabel.Dock = DockStyle.Top;
            titleLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            titleLabel.Location = new Point(3, 0);
            titleLabel.Margin = new Padding(3, 0, 3, 18);
            titleLabel.Name = "titleLabel";
            titleLabel.Size = new Size(340, 30);
            titleLabel.TabIndex = 0;
            titleLabel.Text = "Информационная система";
            // 
            // loginPanel
            // 
            loginPanel.Controls.Add(loginTextBox);
            loginPanel.Controls.Add(loginLabel);
            loginPanel.Dock = DockStyle.Top;
            loginPanel.Location = new Point(3, 51);
            loginPanel.Margin = new Padding(3, 3, 3, 8);
            loginPanel.Name = "loginPanel";
            loginPanel.Size = new Size(340, 72);
            loginPanel.TabIndex = 1;
            // 
            // loginTextBox
            // 
            loginTextBox.Dock = DockStyle.Top;
            loginTextBox.Location = new Point(0, 26);
            loginTextBox.Name = "loginTextBox";
            loginTextBox.Size = new Size(340, 23);
            loginTextBox.TabIndex = 1;
            // 
            // loginLabel
            // 
            loginLabel.Dock = DockStyle.Top;
            loginLabel.Location = new Point(0, 0);
            loginLabel.Name = "loginLabel";
            loginLabel.Padding = new Padding(0, 0, 0, 6);
            loginLabel.Size = new Size(340, 26);
            loginLabel.TabIndex = 0;
            loginLabel.Text = "Логин";
            // 
            // passwordPanel
            // 
            passwordPanel.Controls.Add(passwordTextBox);
            passwordPanel.Controls.Add(passwordLabel);
            passwordPanel.Dock = DockStyle.Top;
            passwordPanel.Location = new Point(3, 134);
            passwordPanel.Margin = new Padding(3, 3, 3, 8);
            passwordPanel.Name = "passwordPanel";
            passwordPanel.Size = new Size(340, 72);
            passwordPanel.TabIndex = 2;
            // 
            // passwordTextBox
            // 
            passwordTextBox.Dock = DockStyle.Top;
            passwordTextBox.Location = new Point(0, 26);
            passwordTextBox.Name = "passwordTextBox";
            passwordTextBox.Size = new Size(340, 23);
            passwordTextBox.TabIndex = 1;
            passwordTextBox.UseSystemPasswordChar = true;
            // 
            // passwordLabel
            // 
            passwordLabel.Dock = DockStyle.Top;
            passwordLabel.Location = new Point(0, 0);
            passwordLabel.Name = "passwordLabel";
            passwordLabel.Padding = new Padding(0, 0, 0, 6);
            passwordLabel.Size = new Size(340, 26);
            passwordLabel.TabIndex = 0;
            passwordLabel.Text = "Пароль";
            // 
            // noteLabel
            // 
            noteLabel.AutoSize = true;
            noteLabel.Dock = DockStyle.Top;
            noteLabel.ForeColor = Color.FromArgb(91, 98, 111);
            noteLabel.Location = new Point(3, 214);
            noteLabel.Margin = new Padding(3, 0, 3, 16);
            noteLabel.Name = "noteLabel";
            noteLabel.Size = new Size(340, 30);
            noteLabel.TabIndex = 3;
            noteLabel.Text = "Для авторизации необходимо собрать пазл-капчу и нажать кнопку входа.";
            // 
            // buttonsPanel
            // 
            buttonsPanel.AutoSize = true;
            buttonsPanel.Controls.Add(loginButton);
            buttonsPanel.Controls.Add(closeButton);
            buttonsPanel.Dock = DockStyle.Top;
            buttonsPanel.Location = new Point(3, 577);
            buttonsPanel.Name = "buttonsPanel";
            buttonsPanel.Size = new Size(340, 42);
            buttonsPanel.TabIndex = 4;
            buttonsPanel.WrapContents = false;
            // 
            // loginButton
            // 
            loginButton.BackColor = Color.FromArgb(35, 102, 180);
            loginButton.FlatAppearance.BorderSize = 0;
            loginButton.FlatStyle = FlatStyle.Flat;
            loginButton.ForeColor = Color.White;
            loginButton.Location = new Point(0, 0);
            loginButton.Margin = new Padding(0, 0, 10, 0);
            loginButton.Name = "loginButton";
            loginButton.Size = new Size(150, 42);
            loginButton.TabIndex = 0;
            loginButton.Text = "Войти";
            loginButton.UseVisualStyleBackColor = false;
            // 
            // closeButton
            // 
            closeButton.BackColor = Color.FromArgb(232, 238, 246);
            closeButton.FlatAppearance.BorderColor = Color.FromArgb(218, 224, 232);
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.ForeColor = Color.FromArgb(26, 42, 64);
            closeButton.Location = new Point(160, 0);
            closeButton.Margin = new Padding(0);
            closeButton.Name = "closeButton";
            closeButton.Size = new Size(150, 42);
            closeButton.TabIndex = 1;
            closeButton.Text = "Закрыть";
            closeButton.UseVisualStyleBackColor = false;
            // 
            // captchaGroupBox
            // 
            captchaGroupBox.BackColor = Color.White;
            captchaGroupBox.Controls.Add(puzzleCaptchaControl);
            captchaGroupBox.Dock = DockStyle.Fill;
            captchaGroupBox.Location = new Point(403, 23);
            captchaGroupBox.Name = "captchaGroupBox";
            captchaGroupBox.Padding = new Padding(14, 22, 14, 14);
            captchaGroupBox.Size = new Size(734, 674);
            captchaGroupBox.TabIndex = 1;
            captchaGroupBox.TabStop = false;
            captchaGroupBox.Text = "Пазл-капча";
            // 
            // puzzleCaptchaControl
            // 
            puzzleCaptchaControl.BackColor = Color.White;
            puzzleCaptchaControl.Dock = DockStyle.Fill;
            puzzleCaptchaControl.Location = new Point(14, 38);
            puzzleCaptchaControl.Name = "puzzleCaptchaControl";
            puzzleCaptchaControl.Size = new Size(706, 622);
            puzzleCaptchaControl.TabIndex = 0;
            puzzleCaptchaControl.Load += puzzleCaptchaControl_Load;
            // 
            // Form1
            // 
            AcceptButton = loginButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            CancelButton = closeButton;
            ClientSize = new Size(1160, 720);
            Controls.Add(rootLayout);
            MinimumSize = new Size(1040, 660);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "DE - Авторизация";
            rootLayout.ResumeLayout(false);
            authGroupBox.ResumeLayout(false);
            authLayout.ResumeLayout(false);
            authLayout.PerformLayout();
            loginPanel.ResumeLayout(false);
            loginPanel.PerformLayout();
            passwordPanel.ResumeLayout(false);
            passwordPanel.PerformLayout();
            buttonsPanel.ResumeLayout(false);
            captchaGroupBox.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel rootLayout;
        private GroupBox authGroupBox;
        private TableLayoutPanel authLayout;
        private Label titleLabel;
        private Panel loginPanel;
        private TextBox loginTextBox;
        private Label loginLabel;
        private Panel passwordPanel;
        private TextBox passwordTextBox;
        private Label passwordLabel;
        private Label noteLabel;
        private FlowLayoutPanel buttonsPanel;
        private Button loginButton;
        private Button closeButton;
        private GroupBox captchaGroupBox;
        private Controls.PuzzleCaptchaControl puzzleCaptchaControl;
    }
}
