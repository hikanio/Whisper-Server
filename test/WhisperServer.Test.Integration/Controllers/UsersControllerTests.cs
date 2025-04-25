using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using WhisperServer.Api.Commands;
using WhisperServer.Api.Dto;
using WhisperServer.Api.Entities;
using WhisperServer.Api.Security;
using WhisperServer.Test.Integration.Helpers;

namespace WhisperServer.Test.Integration.Controllers;

public sealed class UsersControllerTests : BaseControllerTests, IDisposable
{
    [Fact]
    public async Task given_valid_command_post_signup_should_return_200_ok_status_code()
    {
        var command = new SignUp(ValidUsername, ValidEmail, ValidPassword, ValidPublicKey);

        var response = await Client.PostAsJsonAsync(SignUpEndpoint, command);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task given_valid_command_post_signin_should_return_200_ok_status_code_and_jwt()
    {
        await CreateUserAsync(ValidUsername, ValidEmail, ValidPassword, ValidPublicKey);

        var command = new SignIn(ValidEmail, ValidPassword, ValidPublicKey, false);
        var response = await Client.PostAsJsonAsync(SignInEndpoint, command);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var jwt = await response.Content.ReadFromJsonAsync<JwtDto>();
        Assert.NotNull(jwt);
        Assert.False(string.IsNullOrWhiteSpace(jwt.AccessToken));
    }

    [Fact]
    public async Task get_all_users_by_query_get_search_should_return_200_ok_status_code_and_list_of_userdto()
    {
        var user = await CreateUserAsync(ValidUsername, ValidEmail, ValidPassword, ValidPublicKey);
        Authorize(user.Id, user.Username, user.Email);
        const string query = $"?name={ValidUsername}&limitResults=1";

        var response = await Client.GetAsync(SearchUserEndpoint + query);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var users = await response.Content.ReadFromJsonAsync<List<UserDto>>();
        Assert.NotNull(users);
        Assert.NotEmpty(users);
        Assert.Contains(users, u => u.Id == user.Id);
    }

    #region Arrange

    private const string SignUpEndpoint = "/users/sign-up";
    private const string SignInEndpoint = "/users/sign-in";
    private const string SearchUserEndpoint = "/users/search";
    private const string ValidEmail = "test@test.com";
    private const string ValidUsername = "username";
    private const string ValidPassword = "superstrongpassword";
    private const string ValidPublicKey = "public key";

    private readonly TestDatabase _testDatabase;

    public UsersControllerTests(OptionsProvider optionsProvider) : base(optionsProvider)
    {
        _testDatabase = new TestDatabase(optionsProvider);
    }

    private async Task<User> CreateUserAsync(string username, string email, string password, string publicKey)
    {
        var userId = Guid.NewGuid();
        var passwordManager = new PasswordManager(new PasswordHasher<User>());
        var key = new Key(Guid.NewGuid(), userId, publicKey, DateTime.UtcNow, DateTime.UtcNow);
        var user = new User(userId, username, email, passwordManager.Secure(password), key, DateTime.UtcNow);
        await _testDatabase.DbContext.Users.AddAsync(user);
        await _testDatabase.DbContext.SaveChangesAsync();

        return user;
    }

    #endregion

    public void Dispose()
    {
        _testDatabase.Dispose();
    }
}