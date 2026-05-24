using DE.Forms.Infrastructure;
using DE.Forms.Models;
using DE.Forms.Services;
using Xunit;

namespace DE.Forms.IntegrationTests;

internal static class UserWorkflowAssertions
{
    public static async Task RunAsync(IUserRepository userRepository)
    {
        var passwordService = new PasswordService();
        var authenticationService = new AuthenticationService(userRepository, passwordService);
        var userService = new UserService(userRepository, passwordService);

        var adminResult = await authenticationService.AuthenticateAsync("admin", "admin");
        Assert.True(adminResult.IsSuccess);
        Assert.Equal(RoleNames.Administrator, adminResult.User?.RoleName);

        var firstWrongPassword = await authenticationService.AuthenticateAsync("user", "wrong-password-1");
        Assert.False(firstWrongPassword.IsSuccess);
        Assert.False(firstWrongPassword.IsBlocked);

        await authenticationService.AuthenticateAsync("user", "wrong-password-2");
        var thirdWrongPassword = await authenticationService.AuthenticateAsync("user", "wrong-password-3");
        Assert.True(thirdWrongPassword.IsBlocked);

        var blockedLogin = await authenticationService.AuthenticateAsync("user", "user");
        Assert.True(blockedLogin.IsBlocked);

        var seededUser = await userRepository.GetByLoginAsync("user");
        Assert.NotNull(seededUser);

        var unblockResult = await userService.UnblockUserAsync(seededUser.UserId);
        Assert.True(unblockResult.IsSuccess);

        var unblockedLogin = await authenticationService.AuthenticateAsync("user", "user");
        Assert.True(unblockedLogin.IsSuccess);

        var createResult = await userService.CreateUserAsync(new CreateUserRequest
        {
            FullName = "Интеграционный пользователь",
            Login = "integration_user",
            Password = "integration-password",
            RoleName = RoleNames.User
        });
        Assert.True(createResult.IsSuccess);

        var duplicateResult = await userService.CreateUserAsync(new CreateUserRequest
        {
            FullName = "Дубликат",
            Login = "integration_user",
            Password = "integration-password",
            RoleName = RoleNames.User
        });
        Assert.False(duplicateResult.IsSuccess);

        var createdUser = await userRepository.GetByLoginAsync("integration_user");
        Assert.NotNull(createdUser);

        var updateResult = await userService.UpdateUserAsync(new UpdateUserRequest
        {
            UserId = createdUser.UserId,
            FullName = "Обновленный интеграционный пользователь",
            Login = createdUser.Login,
            NewPassword = "new-integration-password",
            RoleName = RoleNames.Administrator,
            IsBlocked = true
        });
        Assert.True(updateResult.IsSuccess);

        var blockedCreatedUser = await authenticationService.AuthenticateAsync("integration_user", "new-integration-password");
        Assert.True(blockedCreatedUser.IsBlocked);

        var unblockCreatedUser = await userService.UnblockUserAsync(createdUser.UserId);
        Assert.True(unblockCreatedUser.IsSuccess);

        var updatedLogin = await authenticationService.AuthenticateAsync("integration_user", "new-integration-password");
        Assert.True(updatedLogin.IsSuccess);
        Assert.Equal(RoleNames.Administrator, updatedLogin.User?.RoleName);
    }
}
