
using BuberDinner.Application.Common.Interfaces.Authentication;
using BuberDinner.Application.Services.Persistence;
using BuberDinner.Domain.Entities;

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

        public AuthenticationResult Register(string firstName, string lastName, string email, string password)
        {
            // check if user exists
            if(_userRepository.GetUserByEmail(email) is not null)
            {
                throw new Exception("User already exists");
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

            // return AuthenticationResult
            return new AuthenticationResult(user, token);
        }

        public AuthenticationResult Login(string email, string password)
        {
            // Validate user exists
            if(_userRepository.GetUserByEmail(email) is not User user)
            {
                throw new Exception("User does not exist");
            }

            // Validate password is correct
            if(user.Password != password)
            {
                throw new Exception("Invalid password");
            }

            // Create Jwt token
            var token  = _jwtTokenGenerator.GenerateToken(user);

            return new AuthenticationResult(user, token);
        }
    }



}