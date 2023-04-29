
using Microsoft.AspNetCore.Mvc;
using ErrorOr;
using MediatR;

using BuberDinner.Contracts.Authentication;
using BuberDinner.Application.Common.Errors;
using BuberDinner.Application.Authentication.Common;
using BuberDinner.Application.Authentication.Commands.Register;
using BuberDinner.Application.Authentication.Queries.Login;
using MapsterMapper;

namespace BuberDinner.Api.Controllers
{

    [Route("auth")]
    public class AuthenticationController : ApiController
    {
        private readonly ISender _mediator;
        private readonly IMapper _mapper;

        public AuthenticationController(ISender mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var command = _mapper.Map<RegisterCommand>(request);
            ErrorOr<AuthenticationResult> authResultOrError = await _mediator.Send(command);

            return authResultOrError.Match(
                authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
                errors => Problem(errors)
            );
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = _mapper.Map<LoginQuery>(request);
            var authResultOrError = await _mediator.Send(query);

            if (authResultOrError.IsError && authResultOrError.FirstError == ApplicationErrors.Authentication.InvalidedCredentials)
                return Problem(
                    statusCode: StatusCodes.Status401Unauthorized,
                    title: authResultOrError.FirstError.Description
                );

            return authResultOrError.Match(
                authResult => Ok(_mapper.Map<AuthenticationResponse>(authResult)),
                errors => Problem(errors)
            );
        }

    }
}