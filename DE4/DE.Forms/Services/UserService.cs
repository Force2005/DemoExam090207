using DE.Forms.Infrastructure;
using DE.Forms.Models;

namespace DE.Forms.Services;

public sealed class UserService
{
    private readonly IUserRepository _userRepository;
    private readonly PasswordService _passwordService;

    public UserService(IUserRepository userRepository, PasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public Task<IReadOnlyList<AppUser>> SearchUsersAsync(string? searchText)
    {
        return _userRepository.SearchAsync(searchText);
    }

    public async Task<OperationResult> CreateUserAsync(CreateUserRequest request)
    {
        var validationResult = ValidateCreateRequest(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var normalizedLogin = request.Login.Trim();
        if (await _userRepository.LoginExistsAsync(normalizedLogin))
        {
            return OperationResult.Failure("Пользователь с указанным логином уже существует.");
        }

        var user = new AppUser
        {
            FullName = request.FullName.Trim(),
            Login = normalizedLogin,
            PasswordHash = _passwordService.HashPassword(request.Password.Trim()),
            RoleName = request.RoleName,
            FailedAttempts = 0,
            IsBlocked = false
        };

        await _userRepository.CreateAsync(user);
        return OperationResult.Success("Пользователь успешно добавлен.");
    }

    public async Task<OperationResult> UpdateUserAsync(UpdateUserRequest request)
    {
        var validationResult = ValidateUpdateRequest(request);
        if (!validationResult.IsSuccess)
        {
            return validationResult;
        }

        var existingUser = await _userRepository.GetByIdAsync(request.UserId);
        if (existingUser is null)
        {
            return OperationResult.Failure("Пользователь не найден.");
        }

        var normalizedLogin = request.Login.Trim();
        if (await _userRepository.LoginExistsAsync(normalizedLogin, request.UserId))
        {
            return OperationResult.Failure("Пользователь с указанным логином уже существует.");
        }

        existingUser.FullName = request.FullName.Trim();
        existingUser.Login = normalizedLogin;
        existingUser.RoleName = request.RoleName;
        existingUser.IsBlocked = request.IsBlocked;

        if (!request.IsBlocked)
        {
            existingUser.FailedAttempts = 0;
        }

        if (!string.IsNullOrWhiteSpace(request.NewPassword))
        {
            existingUser.PasswordHash = _passwordService.HashPassword(request.NewPassword.Trim());
        }

        await _userRepository.UpdateAsync(existingUser);
        return OperationResult.Success("Данные пользователя успешно обновлены.");
    }

    public async Task<OperationResult> UnblockUserAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            return OperationResult.Failure("Пользователь не найден.");
        }

        await _userRepository.SetBlockedStatusAsync(userId, false);
        return OperationResult.Success("Учетная запись успешно разблокирована.");
    }

    private static OperationResult ValidateCreateRequest(CreateUserRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            return OperationResult.Failure("Введите ФИО пользователя.");
        }

        if (string.IsNullOrWhiteSpace(request.Login))
        {
            return OperationResult.Failure("Введите логин пользователя.");
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            return OperationResult.Failure("Введите пароль пользователя.");
        }

        if (!RoleNames.All.Contains(request.RoleName))
        {
            return OperationResult.Failure("Выберите корректную роль пользователя.");
        }

        return OperationResult.Success(string.Empty);
    }

    private static OperationResult ValidateUpdateRequest(UpdateUserRequest request)
    {
        if (request.UserId <= 0)
        {
            return OperationResult.Failure("Выберите пользователя для редактирования.");
        }

        if (string.IsNullOrWhiteSpace(request.FullName))
        {
            return OperationResult.Failure("Введите ФИО пользователя.");
        }

        if (string.IsNullOrWhiteSpace(request.Login))
        {
            return OperationResult.Failure("Введите логин пользователя.");
        }

        if (!RoleNames.All.Contains(request.RoleName))
        {
            return OperationResult.Failure("Выберите корректную роль пользователя.");
        }

        return OperationResult.Success(string.Empty);
    }
}
