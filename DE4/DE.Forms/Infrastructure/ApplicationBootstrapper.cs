using DE.Forms.Forms;
using DE.Forms.Models;
using DE.Forms.Services;

namespace DE.Forms.Infrastructure;

public sealed class ApplicationBootstrapper
{
    private readonly AuthenticationService _authenticationService;
    private readonly UserService _userService;
    private readonly Func<Task> _initializeDatabaseAsync;

    public ApplicationBootstrapper()
    {
        var appSettings = AppSettingsLoader.Load();
        var passwordService = new PasswordService();

        IUserRepository userRepository;
        if (DatabaseProvider.IsSqlServer(appSettings.Database.Provider))
        {
            var connectionFactory = new SqlServerConnectionFactory(appSettings.Database);
            var databaseInitializer = new SqlServerDatabaseInitializer(connectionFactory, passwordService);
            userRepository = new SqlServerUserRepository(connectionFactory);
            _initializeDatabaseAsync = databaseInitializer.InitializeAsync;
        }
        else
        {
            var connectionFactory = new MySqlConnectionFactory(appSettings.Database);
            var databaseInitializer = new DatabaseInitializer(connectionFactory, passwordService);
            userRepository = new UserRepository(connectionFactory);
            _initializeDatabaseAsync = databaseInitializer.InitializeAsync;
        }

        _authenticationService = new AuthenticationService(userRepository, passwordService);
        _userService = new UserService(userRepository, passwordService);
    }

    public Task InitializeAsync() => _initializeDatabaseAsync();

    public Form1 CreateLoginForm()
    {
        return new Form1(_authenticationService, CreateWorkspaceForm);
    }

    private Form CreateWorkspaceForm(AppUser user)
    {
        return user.RoleName == RoleNames.Administrator
            ? new AdminDashboardForm(_userService, user)
            : new UserDashboardForm(user);
    }
}
