using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using WhisperServer.Api.Dto;
using WhisperServer.Api.Entities;
using WhisperServer.Api.Security;
using WhisperServer.Api.ValueObjects;
using WhisperServer.Test.Integration.Helpers;

namespace WhisperServer.Test.Integration.Controllers;

public sealed class ConversationsControllerTests : BaseControllerTests, IDisposable
{
    [Fact]
    public async Task get_all_conversations_should_return_200_ok_status_code_and_list_of_conversationdto()
    {
        CreateConversation(out var user, out var message);
        Authorize(user.Id, user.Username, user.Email);

        var response = await Client.GetAsync(ConversationsEndpoint);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var conversations = await response.Content.ReadFromJsonAsync<List<ConversationDto>>();
        Assert.NotNull(conversations);
        Assert.NotEmpty(conversations);
        Assert.Contains(conversations, c => c.UserId == user.Id);
    }

    [Fact]
    public async Task get_all_messages_from_conversation_should_return_200_ok_status_code_and_list_of_messagedto()
    {
        CreateConversation(out var user, out var message);
        Authorize(user.Id, user.Username, user.Email);

        var response = await Client.GetAsync(ConversationsEndpoint + "/" + user.Id + MessagesEndpoint);

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var messages = await response.Content.ReadFromJsonAsync<List<MessageDto>>();
        Assert.NotNull(messages);
        Assert.NotEmpty(messages);
        Assert.Contains(messages, m => m.Id == message.Id);
    }

    #region Arrange

    private const string ConversationsEndpoint = "/conversations";
    private const string MessagesEndpoint = "/messages";
    private readonly TestDatabase _testDatabase;

    public ConversationsControllerTests(OptionsProvider optionsProvider) : base(optionsProvider)
    {
        _testDatabase = new TestDatabase(optionsProvider);
    }

    private void CreateConversation(out User user, out Message message)
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