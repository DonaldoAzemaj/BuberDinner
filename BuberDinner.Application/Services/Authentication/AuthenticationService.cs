
using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Common.Interfaces.Persistence;
using BuberDinner.Domain.Common.Errors;
using BuberDinner.Domain.Entities;
using ErrorOr;

namespace BuberDinner.Application.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUserRepository _userRepository;

        public AuthenticationService(IJwtTokenGenerator jwtTokenGenerator,
                                     IUserRepository userRepository)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
            _userRepository = userRepository;
        }

        public ErrorOr<AuthenticationResult> Register(string firstName, string lastName, string email, string password)
        {
            // check if user exists
            if(_userRepository.GetUserByEmail(email) is not null)
            {
                return ApplicationErrors.Authentication.DuplicateEmail;
            }

            // create user (generate unique id)
            var user = new User{
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Password = password
            };
            _userRepository.Add(user);

            //create Jwt token
            var token = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }

        public ErrorOr<AuthenticationResult> Login(string email, string password)
        {
            // Validate user exists
            if(_userRepository.GetUserByEmail(email) is not User user)
            {
                return ApplicationErrors.Authentication.InvalidedCredentials;
            }

            // Validate password is correct
            if(user.Password != password)
            {
                 return new [] { ApplicationErrors.Authentication.InvalidedCredentials };
            }

            // Create Jwt token
            var token  = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }



}