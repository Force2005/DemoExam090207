using DE.Forms.Models;
using DE.Forms.Services;

namespace DE.Forms.Forms;

public sealed partial class AdminDashboardForm : Form
{
    private UserService? _userService;
    private AppUser? _currentUser;

    public AdminDashboardForm()
    {
        InitializeComponent();
        RegisterEvents();
    }

    public AdminDashboardForm(UserService userService, AppUser currentUser)
        : this()
    {
        _userService = userService;
        _currentUser = currentUser;
        currentUserLabel.Text = $"Вы вошли как: {_currentUser.Login} ({_currentUser.RoleName})";
    }

    private void RegisterEvents()
    {
        Shown += AdminDashboardForm_Shown;
        searchTextBox.KeyDown += SearchTextBox_KeyDown;
        searchButton.Click += async (_, _) => await LoadUsersAsync(searchTextBox.Text);
        refreshButton.Click += RefreshButton_Click;
        addButton.Click += AddButton_Click;
        editButton.Click += EditButton_Click;
        unblockButton.Click += UnblockButton_Click;
        exitButton.Click += (_, _) => Close();
    }

    private async void AdminDashboardForm_Shown(object? sender, EventArgs e)
    {
        if (_userService is not null)
        {
            await LoadUsersAsync();
        }
    }

    private async void RefreshButton_Click(object? sender, EventArgs e)
    {
        searchTextBox.Clear();
        await LoadUsersAsync();
    }

    private async Task LoadUsersAsync(string? searchText = null)
    {
        if (_userService is null)
        {
            summaryLabel.Text = "Форма открыта в конструкторе.";
            return;
        }

        try
        {
            UseWaitCursor = true;
            var users = await _userService.SearchUsersAsync(searchText);
            usersGridView.DataSource = users.ToList();
            summaryLabel.Text = $"Найдено пользователей: {users.Count}";
        }
        catch (Exception exception)
        {
            MessageBox.Show(this, $"Не удалось загрузить пользователей.{Environment.NewLine}{exception.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            UseWaitCursor = false;
        }
    }

    private async void AddButton_Click(object? sender, EventArgs e)
    {
        if (_userService is null)
        {
            return;
        }

        using var form = new UserEditForm(_userService);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await LoadUsersAsync(searchTextBox.Text);
        }
    }

    private async void EditButton_Click(object? sender, EventArgs e)
    {
        if (_userService is null)
        {
            return;
        }

        var selectedUser = GetSelectedUser();
        if (selectedUser is null)
        {
            MessageBox.Show(this, "Выберите пользователя для редактирования.", "Редактирование", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        using var form = new UserEditForm(_userService, selectedUser);
        if (form.ShowDialog(this) == DialogResult.OK)
        {
            await LoadUsersAsync(searchTextBox.Text);
        }
    }

    private async void UnblockButton_Click(object? sender, EventArgs e)
    {
        if (_userService is null)
        {
            return;
        }

        var selectedUser = GetSelectedUser();
        if (selectedUser is null)
        {
            MessageBox.Show(this, "Выберите пользователя для снятия блокировки.", "Снятие блокировки", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        if (!selectedUser.IsBlocked)
        {
            MessageBox.Show(this, "Выбранный пользователь не заблокирован.", "Снятие блокировки", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        var confirmation = MessageBox.Show(
            this,
            $"Снять блокировку с пользователя «{selectedUser.Login}»?",
            "Подтверждение",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

        if (confirmation != DialogResult.Yes)
        {
            return;
        }

        try
        {
            var result = await _userService.UnblockUserAsync(selectedUser.UserId);
            MessageBox.Show(
                this,
                result.Message,
                result.IsSuccess ? "Готово" : "Ошибка",
                MessageBoxButtons.OK,
                result.IsSuccess ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (result.IsSuccess)
            {
                await LoadUsersAsync(searchTextBox.Text);
            }
        }
        catch (Exception exception)
        {
            MessageBox.Show(this, $"Не удалось снять блокировку.{Environment.NewLine}{exception.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private async void SearchTextBox_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode != Keys.Enter)
        {
            return;
        }

        e.SuppressKeyPress = true;
        await LoadUsersAsync(searchTextBox.Text);
    }

    private AppUser? GetSelectedUser()
    {
        return usersGridView.CurrentRow?.DataBoundItem as AppUser;
    }
}
