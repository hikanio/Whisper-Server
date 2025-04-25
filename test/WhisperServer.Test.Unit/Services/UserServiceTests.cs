using Microsoft.AspNetCore.Identity;
using WhisperServer.Api.Commands;
using WhisperServer.Api.Dto;
using WhisperServer.Api.Entities;
using WhisperServer.Api.Exceptions;
using WhisperServer.Api.Repositories;
using WhisperServer.Api.Security;
using WhisperServer.Api.Security.Auth;
using WhisperServer.Api.Services;
using WhisperServer.Test.Unit.Stubs;

namespace WhisperServer.Test.Unit.Services;

public class UserServiceTests
{
    [Fact]
    public async Task given_valid_data_signup_user_should_succeed()
    {
        var command = new SignUp(ValidUsername, ValidEmail, ValidPassword, ValidPublicKey);
        var exception = await Record.ExceptionAsync(() => _userService.SignUp(command));
        
        Assert.Null(exception);
    }
    
    [Fact]
    public async Task given_invalid_data_signup_user_should_fail()
    {
        var command = new SignUp(InvalidUsername, InvalidEmail, InvalidPassword, InvalidPublicKey);
        var exception = await Record.ExceptionAsync(() => _userService.SignUp(command));
        
        Assert.NotNull(exception);
    }
    
    [Fact]
    public async Task given_valid_data_signin_user_should_succeed()
    {
        var signUpCommand = new SignUp(ValidUsername, ValidEmail, ValidPassword, ValidPublicKey);
        await _userService.SignUp(signUpCommand);
        var command = new SignIn(ValidEmail, ValidPassword, ValidPublicKey, false);
        
        var token = await _userService.SignIn(command);
        
        Assert.NotNull(token);
        Assert.IsType<JwtDto>(token);
    }
    
    [Fact]
    public async Task given_invalid_password_signin_user_should_fail()
    {
        var signUpCommand = new SignUp(ValidUsername, ValidEmail, ValidPassword, ValidPublicKey);
        await _userService.SignUp(signUpCommand);
        const string wrongValidPassword = "wrongpassword";
        var command = new SignIn(ValidEmail, wrongValidPassword, ValidPublicKey, false);
        
        var exception = await Record.ExceptionAsync(() => _userService.SignIn(command));

        Assert.NotNull(exception);
        Assert.IsType<InvalidCredentialsException>(exception);
    }

    [Theory]
    [InlineData("user")]
    [InlineData("test")]
    public async Task given_query_find_all_users_by_username_or_email_should_succeed(string query)
    {
        var command = new SignUp(ValidUsername, ValidEmail, ValidPassword, ValidPublicKey);
        await _userService.SignUp(command);

        var users = await _userService.FindAllByUsernameOrEmail(query, 1);

        Assert.NotNull(users);
        Assert.NotEmpty(users);
        Assert.IsAssignableFrom<IEnumerable<UserDto>>(users);
    }
    
    [Theory]
    [InlineData("abc")]
    [InlineData("invalid")]
    public async Task given_invalid_query_find_all_users_by_username_or_email_should_fail(string query)
    {
        var signUpCommand = new SignUp(ValidUsername, ValidEmail, ValidPassword, ValidPublicKey);
        await _userService.SignUp(signUpCommand);

        var users = await _userService.FindAllByUsernameOrEmail(query, null);

        Assert.NotNull(users);
        Assert.Empty(users);
        Assert.IsAssignableFrom<IEnumerable<UserDto>>(users);
    }
    
    #region Arrange

    private readonly UserService _userService;
    
    private const string ValidEmail = "test@test.com";
    private const string ValidUsername = "username";
    private const string ValidPassword = "superstrongpassword";
    private const string ValidPublicKey = "public key";
    private const string InvalidEmail = "test@";
    private const string InvalidUsername = "us";
    private const string InvalidPassword = "pass";
    private const string InvalidPublicKey = "";

    public UserServiceTests()
    {
        IUserRepository userRepository = new TestUserRepository();
        IPasswordManager passwordManager = new PasswordManager(new PasswordHasher<User>());
        IAuthenticator authenticator = new TestAuthenticator();
        _userService = new UserService(userRepository, passwordManager, authenticator);
    }

    #endregion
}