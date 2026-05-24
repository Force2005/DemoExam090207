using DE.Forms.Models;
using DE.Forms.Services;

namespace DE.Forms.Forms;

public sealed partial class UserEditForm : Form
{
    private UserService? _userService;
    private AppUser? _editingUser;

    public UserEditForm()
    {
        InitializeComponent();
        if (roleComboBox.Items.Count == 0)
        {
            roleComboBox.Items.AddRange(RoleNames.All.Cast<object>().ToArray());
        }

        roleComboBox.SelectedItem = RoleNames.User;
        saveButton.Click += SaveButton_Click;
        cancelButton.Click += (_, _) => DialogResult = DialogResult.Cancel;
        ConfigureMode(null);
    }

    public UserEditForm(UserService userService, AppUser? editingUser = null)
        : this()
    {
        _userService = userService;
        _editingUser = editingUser;
        ConfigureMode(editingUser);
    }

    private void ConfigureMode(AppUser? user)
    {
        Text = user is null ? "Добавление пользователя" : $"Редактирование пользователя: {user.Login}";
        fieldsGroupBox.Text = user is null ? "Данные нового пользователя" : "Данные пользователя";
        passwordLabel.Text = user is null ? "Пароль:" : "Новый пароль:";
        passwordHintLabel.Text = user is null
            ? "Пароль обязателен для нового пользователя."
            : "Оставьте поля пароля пустыми, если пароль менять не нужно.";
        isBlockedCheckBox.Enabled = user is not null;

        if (user is null)
        {
            fullNameTextBox.Clear();
            loginTextBox.Clear();
            passwordTextBox.Clear();
            confirmPasswordTextBox.Clear();
            roleComboBox.SelectedItem = RoleNames.User;
            isBlockedCheckBox.Checked = false;
            return;
        }

        fullNameTextBox.Text = user.FullName;
        loginTextBox.Text = user.Login;
        passwordTextBox.Clear();
        confirmPasswordTextBox.Clear();
        roleComboBox.SelectedItem = user.RoleName;
        isBlockedCheckBox.Checked = user.IsBlocked;
    }

    private async void SaveButton_Click(object? sender, EventArgs e)
    {
        if (_userService is null)
        {
            MessageBox.Show(this, "Форма открыта в конструкторе. Запустите приложение, чтобы сохранять данные.", "Сохранение", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        try
        {
            var result = _editingUser is null
                ? await CreateUserAsync()
                : await UpdateUserAsync();

            MessageBox.Show(
                this,
                result.Message,
                result.IsSuccess ? "Готово" : "Проверка данных",
                MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (result.IsSuccess)
            {
                DialogResult = DialogResult.OK;
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(this, $"Не удалось сохранить пользователя.{Environment.NewLine}{exception.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private Task<OperationResult> CreateUserAsync()
    {
        if (_userService is null)
        {
            return Task.FromResult(OperationResult.Failure("Сервис пользователей не задан."));
        }

        var passwordResult = ReadPassword(required: true, out var password);
        if (!passwordResult.IsSuccess)
        {
            return Task.FromResult(passwordResult);
        }

        return _userService.CreateUserAsync(new CreateUserRequest
        {
            FullName = fullNameTextBox.Text,
            Login = loginTextBox.Text,
            Password = password,
            RoleName = roleComboBox.SelectedItem?.ToString() ?? string.Empty
        });
    }

    private Task<OperationResult> UpdateUserAsync()
    {
        if (_userService is null)
        {
            return Task.FromResult(OperationResult.Failure("Сервис пользователей не задан."));
        }

        if (_editingUser is null)
        {
            return Task.FromResult(OperationResult.Failure("Пользователь не выбран."));
        }

        var passwordResult = ReadPassword(required: false, out var password);
        if (!passwordResult.IsSuccess)
        {
            return Task.FromResult(passwordResult);
        }

        return _userService.UpdateUserAsync(new UpdateUserRequest
        {
            UserId = _editingUser.UserId,
            FullName = fullNameTextBox.Text,
            Login = loginTextBox.Text,
            NewPassword = password,
            RoleName = roleComboBox.SelectedItem?.ToString() ?? string.Empty,
            IsBlocked = isBlockedCheckBox.Checked
        });
    }

    private OperationResult ReadPassword(bool required, out string password)
    {
        password = passwordTextBox.Text.Trim();
        var confirmation = confirmPasswordTextBox.Text.Trim();

        if (required && string.IsNullOrWhiteSpace(password))
        {
            return OperationResult.Failure("Введите пароль пользователя.");
        }

        if (!string.IsNullOrWhiteSpace(password) || !string.IsNullOrWhiteSpace(confirmation))
        {
            if (password != confirmation)
            {
                return OperationResult.Failure("Пароли не совпадают.");
            }
        }

        return OperationResult.Success(string.Empty);
    }
}
