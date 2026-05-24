namespace DE.Forms.Forms
{
    partial class UserDashboardForm
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
            profileGroupBox = new GroupBox();
            profileLayout = new TableLayoutPanel();
            greetingLabel = new Label();
            loginInfoLabel = new Label();
            roleInfoLabel = new Label();
            buttonsPanel = new FlowLayoutPanel();
            closeButton = new Button();
            rootLayout.SuspendLayout();
            profileGroupBox.SuspendLayout();
            profileLayout.SuspendLayout();
            buttonsPanel.SuspendLayout();
            SuspendLayout();
            // 
            // rootLayout
            // 
            rootLayout.ColumnCount = 1;
            rootLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rootLayout.Controls.Add(profileGroupBox, 0, 0);
            rootLayout.Controls.Add(buttonsPanel, 0, 1);
            rootLayout.Dock = DockStyle.Fill;
            rootLayout.Location = new Point(0, 0);
            rootLayout.Name = "rootLayout";
            rootLayout.Padding = new Padding(24);
            rootLayout.RowCount = 2;
            rootLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rootLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            rootLayout.Size = new Size(760, 420);
            rootLayout.TabIndex = 0;
            // 
            // profileGroupBox
            // 
            profileGroupBox.BackColor = Color.White;
            profileGroupBox.Controls.Add(profileLayout);
            profileGroupBox.Dock = DockStyle.Fill;
            profileGroupBox.Location = new Point(27, 27);
            profileGroupBox.Name = "profileGroupBox";
            profileGroupBox.Padding = new Padding(14, 22, 14, 14);
            profileGroupBox.Size = new Size(706, 324);
            profileGroupBox.TabIndex = 0;
            profileGroupBox.TabStop = false;
            profileGroupBox.Text = "Профиль пользователя";
            // 
            // profileLayout
            // 
            profileLayout.ColumnCount = 1;
            profileLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            profileLayout.Controls.Add(greetingLabel, 0, 0);
            profileLayout.Controls.Add(loginInfoLabel, 0, 1);
            profileLayout.Controls.Add(roleInfoLabel, 0, 2);
            profileLayout.Dock = DockStyle.Fill;
            profileLayout.Location = new Point(14, 38);
            profileLayout.Name = "profileLayout";
            profileLayout.RowCount = 4;
            profileLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            profileLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            profileLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            profileLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            profileLayout.Size = new Size(678, 272);
            profileLayout.TabIndex = 0;
            // 
            // greetingLabel
            // 
            greetingLabel.AutoSize = true;
            greetingLabel.Dock = DockStyle.Top;
            greetingLabel.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            greetingLabel.Location = new Point(3, 0);
            greetingLabel.Margin = new Padding(3, 0, 3, 18);
            greetingLabel.Name = "greetingLabel";
            greetingLabel.Size = new Size(672, 30);
            greetingLabel.TabIndex = 0;
            greetingLabel.Text = "Здравствуйте, пользователь!";
            // 
            // loginInfoLabel
            // 
            loginInfoLabel.AutoSize = true;
            loginInfoLabel.Dock = DockStyle.Top;
            loginInfoLabel.Location = new Point(3, 48);
            loginInfoLabel.Margin = new Padding(3, 0, 3, 10);
            loginInfoLabel.Name = "loginInfoLabel";
            loginInfoLabel.Size = new Size(672, 15);
            loginInfoLabel.TabIndex = 1;
            loginInfoLabel.Text = "Логин: user";
            // 
            // roleInfoLabel
            // 
            roleInfoLabel.AutoSize = true;
            roleInfoLabel.Dock = DockStyle.Top;
            roleInfoLabel.Location = new Point(3, 73);
            roleInfoLabel.Margin = new Padding(3, 0, 3, 10);
            roleInfoLabel.Name = "roleInfoLabel";
            roleInfoLabel.Size = new Size(672, 15);
            roleInfoLabel.TabIndex = 2;
            roleInfoLabel.Text = "Роль: Пользователь";
            // 
            // buttonsPanel
            // 
            buttonsPanel.AutoSize = true;
            buttonsPanel.Controls.Add(closeButton);
            buttonsPanel.Dock = DockStyle.Top;
            buttonsPanel.FlowDirection = FlowDirection.RightToLeft;
            buttonsPanel.Location = new Point(27, 357);
            buttonsPanel.Name = "buttonsPanel";
            buttonsPanel.Size = new Size(706, 36);
            buttonsPanel.TabIndex = 1;
            // 
            // closeButton
            // 
            closeButton.BackColor = Color.FromArgb(35, 102, 180);
            closeButton.FlatAppearance.BorderSize = 0;
            closeButton.FlatStyle = FlatStyle.Flat;
            closeButton.ForeColor = Color.White;
            closeButton.Location = new Point(606, 0);
            closeButton.Margin = new Padding(0);
            closeButton.Name = "closeButton";
            closeButton.Padding = new Padding(18, 8, 18, 8);
            closeButton.Size = new Size(100, 36);
            closeButton.TabIndex = 0;
            closeButton.Text = "Выход";
            closeButton.UseVisualStyleBackColor = false;
            // 
            // UserDashboardForm
            // 
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            ClientSize = new Size(760, 420);
            Controls.Add(rootLayout);
            MinimumSize = new Size(640, 360);
            Name = "UserDashboardForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "DE - Рабочий стол пользователя";
            rootLayout.ResumeLayout(false);
            rootLayout.PerformLayout();
            profileGroupBox.ResumeLayout(false);
            profileLayout.ResumeLayout(false);
            profileLayout.PerformLayout();
            buttonsPanel.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel rootLayout;
        private GroupBox profileGroupBox;
        private TableLayoutPanel profileLayout;
        private Label greetingLabel;
        private Label loginInfoLabel;
        private Label roleInfoLabel;
        private FlowLayoutPanel buttonsPanel;
        private Button closeButton;
    }
}
