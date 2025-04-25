using WhisperServer.Api.Entities;
using WhisperServer.Api.Exceptions;
using WhisperServer.Api.ValueObjects;

namespace WhisperServer.Test.Unit.Entities;

public class MessageTests
{
    [Fact]
    public void given_invalid_status_message_entity_creation_should_fail()
    {
        var exception = Record.Exception(() => new Message(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), ValidMessage,
            ValidMessage, InvalidStatus,
            DateTime.UtcNow, DateTime.UtcNow));

        Assert.NotNull(exception);
        Assert.IsType<InvalidMessageStatusException>(exception);
    }

    [Fact]
    public void given_invalid_status_message_entity_update_should_fail()
    {
        var message = new Message(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), ValidMessage, ValidMessage,
            ValidStatus, DateTime.UtcNow, DateTime.UtcNow);

        var exception = Record.Exception(() => message.UpdateStatus(InvalidStatus));

        Assert.NotNull(exception);
        Assert.IsType<InvalidMessageStatusException>(exception);
    }

    [Theory]
    [InlineData("")]
    [InlineData("    ")]
    public void given_invalid_message_content_message_entity_creation_should_fail(string invalidMessageContent)
    {
        var exception = Record.Exception(() => new Message(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(),
            invalidMessageContent,
            invalidMessageContent, ValidStatus,
            DateTime.UtcNow, DateTime.UtcNow));

        Assert.NotNull(exception);
        Assert.IsType<InvalidMessageContentException>(exception);
    }

    #region Arrange

    private const string ValidMessage = "message";
    private const string ValidStatus = MessageStatus.Sent;
    private const string InvalidStatus = "invalid status";

    #endregion
}