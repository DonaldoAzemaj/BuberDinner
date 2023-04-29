
using ErrorOr;
using MediatR;

using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Entities;
using BuberDinner.Application.Common.Errors;

namespace BuberDinner.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public LoginQueryHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery query, CancellationToken cancellationToken)
    {
        // to avoid warning 'This async method lacks await'
        await Task.CompletedTask;

        // Validate user exists
        if (_userRepository.GetUserByEmail(query.Email) is not User user)
        {
            return ApplicationErrors.Authentication.InvalidedCredentials;
        }

        // Validate password is correct
        if (user.Password != query.Password)
        {
            return new[] { ApplicationErrors.Authentication.InvalidedCredentials };
        }

        // Create Jwt token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}