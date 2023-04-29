using ErrorOr;
using MediatR;

using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Domain.Entities;
using BuberDinner.Application.Common.Errors;

namespace BuberDinner.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IUserRepository _userRepository;

    public RegisterCommandHandler(IJwtTokenGenerator jwtTokenGenerator, IUserRepository userRepository)
    {
        _jwtTokenGenerator = jwtTokenGenerator;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        // to avoid warning 'This async method lacks await'
        await Task.CompletedTask;

        // check if user exists
        if (_userRepository.GetUserByEmail(command.Email) is not null)
        {
            return ApplicationErrors.Authentication.DuplicateEmail;
        }

        // create user (generate unique id)
        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            Email = command.Email,
            Password = command.Password
        };
        _userRepository.Add(user);

        //create Jwt token
        var token = _jwtTokenGenerator.GenerateToken(user);

        return new AuthenticationResult(user, token);
    }
}


