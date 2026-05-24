using DE.Forms.Models;
using DE.Forms.Services;

namespace DE.Forms
{
    public partial class Form1 : Form
    {
        private readonly AuthenticationService? _authenticationService;
        private readonly Func<AppUser, Form>? _workspaceFactory;

        public Form1()
        {
            InitializeComponent();
            loginButton.Click += LoginButton_Click;
            closeButton.Click += (_, _) => Close();
            puzzleCaptchaControl.PuzzleValidated += CaptchaControl_PuzzleValidated;
        }

        public Form1(AuthenticationService authenticationService, Func<AppUser, Form> workspaceFactory)
            : this()
        {
            _authenticationService = authenticationService;
            _workspaceFactory = workspaceFactory;
        }

        private async void LoginButton_Click(object? sender, EventArgs e)
        {
            if (_authenticationService is null || _workspaceFactory is null)
            {
                ShowMessage("Форма открыта без сервисов приложения. Запустите проект, чтобы выполнить авторизацию.", "Авторизация", MessageBoxIcon.Information);
                return;
            }

            var login = loginTextBox.Text.Trim();
            var password = passwordTextBox.Text;

            if (string.IsNullOrWhiteSpace(login))
            {
                ShowMessage("Поле «Логин» обязательно для заполнения.", "Ошибка авторизации", MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                ShowMessage("Поле «Пароль» обязательно для заполнения.", "Ошибка авторизации", MessageBoxIcon.Warning);
                return;
            }

            if (!puzzleCaptchaControl.IsSolved && !puzzleCaptchaControl.ValidatePuzzle())
            {
                return;
            }

            try
            {
                UseWaitCursor = true;
                var result = await _authenticationService.AuthenticateAsync(login, password);

                ShowMessage(
                    result.Message,
                    result.IsSuccess ? "Успешная авторизация" : "Ошибка авторизации",
                    result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                if (!result.IsSuccess || result.User is null)
                {
                    puzzleCaptchaControl.ShufflePuzzle();
                    return;
                }

                Hide();
                using var workspaceForm = _workspaceFactory(result.User);
                workspaceForm.ShowDialog(this);
                PrepareForNextSession();
                Show();
            }
            catch (Exception exception)
            {
                ShowMessage($"Не удалось выполнить авторизацию.{Environment.NewLine}{exception.Message}", "Ошибка", MessageBoxIcon.Error);
            }
            finally
            {
                UseWaitCursor = false;
            }
        }

        private async void CaptchaControl_PuzzleValidated(object? sender, CaptchaValidationResult result)
        {
            if (result.IsSuccess || _authenticationService is null)
            {
                return;
            }

            var login = loginTextBox.Text.Trim();
            if (string.IsNullOrWhiteSpace(login))
            {
                ShowMessage("Поле «Логин» обязательно для заполнения.", "Проверка капчи", MessageBoxIcon.Warning);
                puzzleCaptchaControl.ShufflePuzzle();
                return;
            }

            try
            {
                var authResult = await _authenticationService.RegisterFailedCaptchaAttemptAsync(login);
                ShowMessage(
                    authResult.IsBlocked ? authResult.Message : "Капча собрана неверно.",
                    authResult.IsBlocked ? "Учетная запись заблокирована" : "Ошибка капчи",
                    MessageBoxIcon.Warning);
            }
            catch (Exception exception)
            {
                ShowMessage($"Не удалось зафиксировать ошибку капчи.{Environment.NewLine}{exception.Message}", "Ошибка", MessageBoxIcon.Error);
            }
            finally
            {
                puzzleCaptchaControl.ShufflePuzzle();
            }
        }

        private void PrepareForNextSession()
        {
            passwordTextBox.Clear();
            puzzleCaptchaControl.ShufflePuzzle();
        }

        private void ShowMessage(string message, string title, MessageBoxIcon icon)
        {
            MessageBox.Show(this, message, title, MessageBoxButtons.OK, icon);
        }

        private void puzzleCaptchaControl_Load(object sender, EventArgs e)
        {

        }
    }
}
