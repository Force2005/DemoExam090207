using DE6.Forms.Models;

namespace DE6.Forms
{
    public partial class Form1 : Form
    {
        private string? _fullName;
        private IReadOnlyList<TestCaseResult> _testResults = [];

        public Form1()
        {
            InitializeComponent();
        }

        private async void GetDataButton_Click(object sender, EventArgs e)
        {
            try
            {
                SetLoadingState(isLoading: true);
                fullNameLabel.Text = "Загрузка...";
                validationResultLabel.Text = string.Empty;

                _fullName = await FullNameClient.GetFullNameAsync();
                _testResults = FullNameValidator.Validate(_fullName);

                fullNameLabel.Text = _fullName;
                validationResultLabel.Text = FullNameValidator.HasForbiddenCharacters(_testResults)
                    ? "ФИО содержит запрещенные символы"
                    : "ФИО не содержит запрещенные символы";
                sendResultButton.Enabled = true;
            }
            catch (Exception ex)
            {
                _fullName = null;
                _testResults = [];
                fullNameLabel.Text = "Ошибка получения данных";
                validationResultLabel.Text = string.Empty;
                sendResultButton.Enabled = false;

                MessageBox.Show(
                    ex.Message,
                    "Ошибка",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(isLoading: false);
            }
        }

        private void SendResultButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_fullName) || _testResults.Count == 0)
            {
                MessageBox.Show(
                    "Сначала получите данные из эмулятора.",
                    "Нет данных",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return;
            }

            try
            {
                string templatePath = Path.Combine(AppContext.BaseDirectory, "ТестКейс.docx");
                string outputPath = Path.Combine(AppContext.BaseDirectory, "ТестКейс_результат.docx");

                TestCaseDocumentWriter.SaveResults(templatePath, outputPath, _testResults);

                MessageBox.Show(
                    $"Результат проверки записан в документ:{Environment.NewLine}{outputPath}",
                    "Готово",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    ex.Message,
                    "Ошибка записи результата",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            getDataButton.Enabled = !isLoading;

            if (isLoading)
            {
                sendResultButton.Enabled = false;
            }
            else if (!string.IsNullOrWhiteSpace(_fullName))
            {
                sendResultButton.Enabled = true;
            }
        }
    }
}
