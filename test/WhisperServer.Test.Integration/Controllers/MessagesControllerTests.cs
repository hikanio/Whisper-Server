using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using WhisperServer.Api.Commands;
using WhisperServer.Api.Dto;
using WhisperServer.Api.Entities;
using WhisperServer.Api.Security;
using WhisperServer.Api.ValueObjects;
using WhisperServer.Test.Integration.Helpers;

namespace WhisperServer.Test.Integration.Controllers;

public sealed class MessagesControllerTests : BaseControllerTests, IDisposable
{
    [Fact]
    public async Task post_message_should_return_201_created_status_code_and_messagedto()
    {
        var user = await CreateUserAsync();
        var command = new CreateMessage(user.Id, user.Id, "message", "message", MessageStatus.Sent);
        Authorize(user.Id, user.Username, user.Email);

        var response = await Client.PostAsJsonAsync(MessageEndpoint, command);
        
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var message = await response.Content.ReadFromJsonAsync<MessageDto>();
        Assert.NotNull(message);
    }

    [Fact]
    public async Task get_message_by_id_should_return_200_ok_status_code_and_messagedto()
    {
        CreateMessage(out var user, out var message);
        Authorize(user.Id, user.Username, user.Email);

        var response = await Client.GetAsync(MessageEndpoint + "/" + message.Id);
        
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var responseMessage = await response.Content.ReadFromJsonAsync<MessageDto>();
        Assert.NotNull(responseMessage);
        Assert.Equal(message.Id, responseMessage.Id);
    }
    
    [Fact]
    public async Task patch_message_should_return_204_nocontent_status_code()
    {
        CreateMessage(out var user, out var message);
        var command = new UpdateMessageStatus(MessageStatus.Read);
        Authorize(user.Id, user.Username, user.Email);

        var response = await Client.PatchAsJsonAsync(MessageEndpoint + "/" + message.Id, command);
        
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }
    
    #region Arrange

    private const string MessageEndpoint = "/messages";
    private readonly TestDatabase _testDatabase;

    public MessagesControllerTests(OptionsProvider optionsProvider) : base(optionsProvider)
    {
        _testDatabase = new TestDatabase(optionsProvider);
    }

    private async Task<User> CreateUserAsync()
    {
        const string validPublicKey = "public key";
        const string validUsername = "username";
        const string validEmail = "test@test.com";
        const string validPassword = "test@test.com";
        
        var userId = Guid.NewGuid();
        var passwordManager = new PasswordManager(new PasswordHasher<User>());
        var key = new Key(Guid.NewGuid(), userId, validPublicKey, DateTime.UtcNow, DateTime.UtcNow);
        var user = new User(userId, validUsername, validEmail, passwordManager.Secure(validPassword), key, DateTime.UtcNow);

        await _testDatabase.DbContext.Users.AddAsync(user);
        await _testDatabase.DbContext.SaveChangesAsync();
        return user;
    }

    private void CreateMessage(out User user, out Message message)
    {
        const string validPublicKey = "public key";
        const string validUsername = "username";
        const string validEmail = "test@test.com";
        const string validPassword = "test@test.com";
        const string validMessage = "message";
        var userId = Guid.NewGuid();
        var passwordManager = new PasswordManager(new PasswordHasher<User>());

        var key = new Key(Guid.NewGuid(), userId, validPublicKey, DateTime.UtcNow, DateTime.UtcNow);
        user = new User(userId, validUsername, validEmail, passwordManager.Secure(validPassword), key, DateTime.UtcNow);
        message = new Message(Guid.NewGuid(), user.Id, user.Id, validMessage, validMessage, MessageStatus.Sent,
            DateTime.UtcNow, DateTime.UtcNow);

        _testDatabase.DbContext.Users.Add(user);
        _testDatabase.DbContext.Messages.Add(message);
        _testDatabase.DbContext.SaveChanges();
    }

    #endregion

    public void Dispose()
    {
        _testDatabase.Dispose();
    }
}