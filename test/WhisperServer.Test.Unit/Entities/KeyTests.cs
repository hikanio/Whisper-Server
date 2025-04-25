using WhisperServer.Api.Entities;
using WhisperServer.Api.Exceptions;

namespace WhisperServer.Test.Unit.Entities;

public class KeyTests
{
    [Fact]
    public void given_valid_data_key_entity_creation_should_succeed()
    {
        var exception = Record.Exception(() => new Key(Guid.NewGuid(), Guid.NewGuid(), ValidPublicKey, DateTime.UtcNow, DateTime.UtcNow));
        
        Assert.Null(exception);
    }
    
    [Fact]
    public void given_valid_publickey_key_entity_update_should_succeed()
    {
        const string publicKey = "updated valid PublicKey";
        var key = new Key(Guid.NewGuid(), Guid.NewGuid(), ValidPublicKey, DateTime.UtcNow, DateTime.UtcNow);

        var exception = Record.Exception(() => key.Update(publicKey));

        Assert.Null(exception);
    }
    
    [Fact]
    public void given_invalid_publickey_key_entity_update_should_fail()
    {
        
        var key = new Key(Guid.NewGuid(), Guid.NewGuid(), ValidPublicKey, DateTime.UtcNow, DateTime.UtcNow);

        var exception = Record.Exception(() => key.Update(InvalidPublicKey));

        Assert.NotNull(exception);
        Assert.IsType<InvalidPublicKeyException>(exception);
    }

    #region Arrange

    private const string InvalidPublicKey = "";
    private const string ValidPublicKey = "public key";

    #endregion
}