namespace DE6.Forms
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            getDataButton = new Button();
            sendResultButton = new Button();
            fullNameLabel = new Label();
            validationResultLabel = new Label();
            SuspendLayout();
            // 
            // getDataButton
            // 
            getDataButton.Location = new Point(37, 38);
            getDataButton.Name = "getDataButton";
            getDataButton.Size = new Size(250, 58);
            getDataButton.TabIndex = 0;
            getDataButton.Text = "Получить данные";
            getDataButton.UseVisualStyleBackColor = true;
            getDataButton.Click += GetDataButton_Click;
            // 
            // sendResultButton
            // 
            sendResultButton.Enabled = false;
            sendResultButton.Location = new Point(37, 128);
            sendResultButton.Name = "sendResultButton";
            sendResultButton.Size = new Size(250, 58);
            sendResultButton.TabIndex = 1;
            sendResultButton.Text = "Отправить результат теста";
            sendResultButton.UseVisualStyleBackColor = true;
            sendResultButton.Click += SendResultButton_Click;
            // 
            // fullNameLabel
            // 
            fullNameLabel.AutoSize = true;
            fullNameLabel.Font = new Font("Segoe UI", 12F);
            fullNameLabel.Location = new Point(330, 54);
            fullNameLabel.MaximumSize = new Size(420, 0);
            fullNameLabel.Name = "fullNameLabel";
            fullNameLabel.Size = new Size(156, 21);
            fullNameLabel.TabIndex = 2;
            fullNameLabel.Text = "Данные не получены";
            // 
            // validationResultLabel
            // 
            validationResultLabel.AutoSize = true;
            validationResultLabel.Font = new Font("Segoe UI", 12F);
            validationResultLabel.Location = new Point(330, 143);
            validationResultLabel.MaximumSize = new Size(420, 0);
            validationResultLabel.Name = "validationResultLabel";
            validationResultLabel.Size = new Size(217, 21);
            validationResultLabel.TabIndex = 3;
            validationResultLabel.Text = "Результат проверки не задан";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(790, 232);
            Controls.Add(validationResultLabel);
            Controls.Add(fullNameLabel);
            Controls.Add(sendResultButton);
            Controls.Add(getDataButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Валидация данных";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button getDataButton;
        private Button sendResultButton;
        private Label fullNameLabel;
        private Label validationResultLabel;
    }
}
