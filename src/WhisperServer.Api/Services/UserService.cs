using WhisperServer.Api.Commands;
using WhisperServer.Api.Dto;
using WhisperServer.Api.Entities;
using WhisperServer.Api.Exceptions;
using WhisperServer.Api.Repositories;
using WhisperServer.Api.Security;
using WhisperServer.Api.Security.Auth;
using WhisperServer.Api.ValueObjects;

namespace WhisperServer.Api.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordManager _passwordManager;
    private readonly IAuthenticator _authenticator;

    public UserService(IUserRepository userRepository, IPasswordManager passwordManager, IAuthenticator authenticator)
    {
        _userRepository = userRepository;
        _passwordManager = passwordManager;
        _authenticator = authenticator;
    }

    /// <summary>
    /// Registers a new user by saving their information in the database.
    /// </summary>
    /// <param name="command">The command containing the user's sign-up details.</param>
    /// <returns></returns>
    /// <exception cref="UserAlreadyExistsException">Thrown if a user with the same email already exists.</exception>
    public async Task SignUp(SignUp command)
    {
        if (await _userRepository.GetByEmailAsync(command.Email) is not null)
        {
            throw new UserAlreadyExistsException(command.Email);
        }

        var userId = Guid.NewGuid();
        var key = new Key(
            Guid.NewGuid(),
            userId,
            command.PublicKey,
            DateTime.UtcNow,
            DateTime.UtcNow);
        var user = new User(
            userId,
            command.Username,
            command.Email,
            _passwordManager.Secure(command.Password),
            key,
            DateTime.UtcNow);

        await _userRepository.AddAsync(user);
    }

    /// <summary>
    /// Authenticates a user by validating their credentials and returns a JWT token.
    /// </summary>
    /// <param name="command">The command containing the user's sign-in details.</param>
    /// <returns>Returns the JWT token as <see cref="JwtDto"/>.</returns>
    /// <exception cref="InvalidCredentialsException">Thrown if the credentials are invalid.</exception>
    /// <exception cref="PublicKeyConflictException">Thrown if there is a mismatch between the provided and stored public key. Set <c>OverwriteKey=true</c> to update public key.</exception>
    public async Task<JwtDto> SignIn(SignIn command)
    {
        var user = await _userRepository.GetByEmailWithKeyAsync(command.Email);
        if (user is null || _passwordManager.Validate(command.Password, user.Password) is false)
        {
            throw new InvalidCredentialsException();
        }

        var keyConflict = user.Key.PublicKey != command.PublicKey;
        if (keyConflict && !command.OverwriteKey)
        {
            throw new PublicKeyConflictException();
        }

        if (keyConflict && command.OverwriteKey)
        {
            user.Key.Update(command.PublicKey);
            await _userRepository.UpdateAsync(user);
        }

        var token = _authenticator.CreateToken(user.Id, user.Username, user.Email);

        return token;
    }

    /// <summary>
    /// Searches for users by username or email and returns a list of matching users.
    /// </summary>
    /// <param name="name">The username or email to search for.</param>
    /// <param name="limitResults">An optional parameter to limit the number of results.</param>
    /// <returns>A task representing the asynchronous operation that returns a list of <see cref="UserDto"/> objects.</returns>
    /// <exception cref="InvalidQueryParameterException">Thrown if the search parameter is invalid.</exception>
    public async Task<IEnumerable<UserDto>> FindAllByUsernameOrEmail(string name, int? limitResults)
    {
        if (name is null || name.Length < Username.MinLength)
        {
            throw new InvalidQueryParameterException(
                $"Parameter {nameof(name)} must be at least {Username.MinLength} characters.");
        }

        var users = await _userRepository.GetAllMatchingByQueryAsync(name, limitResults);
        return users.Select(x =>
                new UserDto()
                {
                    Id = x.Id,
                    Username = x.Username,
                    Email = x.Email,
                    PublicKey = x.Key.PublicKey
                })
            .ToList();
    }
}