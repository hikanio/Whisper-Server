using WhisperServer.Api.Entities;
using WhisperServer.Api.Exceptions;

namespace WhisperServer.Test.Unit.Entities;

public class UserTests
{
    [Fact]
    public void given_valid_data_user_entity_creation_should_succeed()
    {
        var exception = Record.Exception(() => new User(_userId, ValidUsername, ValidEmail, ValidPassword, _key, DateTime.UtcNow));

        Assert.Null(exception);
    }
    
    [Theory]
    [InlineData("invalid email")]
    [InlineData("")]
    [InlineData("test@s.")]
    [InlineData("bfmllfreojtjiwiuvorkptyenrymivffozoeruptfklfrjitstibhdgetszferviojemockcswmnclitxehhvljwbatasadomskj@test.com")]
    public void given_invalid_email_user_entity_creation_should_fail(string invalidEmail)
    {
        var exception = Record.Exception(() =>
            new User(_userId, ValidUsername, invalidEmail, ValidPassword, _key, DateTime.UtcNow));

        Assert.NotNull(exception);
        Assert.IsType<InvalidEmailException>(exception);
    }

    [Theory]
    [InlineData("us")]
    [InlineData("")]
    [InlineData("bfmllfreojtjiwiuvorkptyenrymivffozoeruptfklfrjitstibhdgetszfervioje")]
    public void given_invalid_username_user_entity_creation_should_fail(string invalidUsername)
    {
        var exception = Record.Exception(() =>
            new User(_userId, invalidUsername, ValidEmail, ValidPassword, _key, DateTime.UtcNow));

        Assert.NotNull(exception);
        Assert.IsType<InvalidUsernameException>(exception);
    }

    [Theory]
    [InlineData("pass")]
    [InlineData("")]
    public void given_invalid_password_user_entity_creation_should_fail(string invalidPassword)
    {
        var exception = Record.Exception(() =>
            new User(_userId, ValidUsername, ValidEmail, invalidPassword, _key, DateTime.UtcNow));

        Assert.NotNull(exception);
        Assert.IsType<InvalidPasswordException>(exception);
    }

    #region Arrange

    private readonly Guid _userId;
    private readonly Key _key;
    
    private const string ValidEmail = "test@test.com";
    private const string ValidUsername = "username";
    private const string ValidPassword = "superstrongpassword";
    private const string ValidPublicKey = "public key";

    public UserTests()
    {
        _userId = Guid.NewGuid();
        _key = new Key(Guid.NewGuid(), _userId, ValidPublicKey, DateTime.UtcNow, DateTime.UtcNow);
    }

    #endregion
}