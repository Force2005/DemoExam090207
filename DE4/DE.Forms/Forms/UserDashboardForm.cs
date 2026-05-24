using DE.Forms.Models;

namespace DE.Forms.Forms;

public sealed partial class UserDashboardForm : Form
{
    public UserDashboardForm()
    {
        InitializeComponent();
        closeButton.Click += (_, _) => Close();
    }

    public UserDashboardForm(AppUser user)
        : this()
    {
        greetingLabel.Text = $"Здравствуйте, {user.FullName}!";
        loginInfoLabel.Text = $"Логин: {user.Login}";
        roleInfoLabel.Text = $"Роль: {user.RoleName}";
    }
}
