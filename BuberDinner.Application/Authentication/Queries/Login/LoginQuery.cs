
using ErrorOr;
using MediatR;

using BuberDinner.Application.Authentication.Common;


namespace BuberDinner.Application.Authentication.Queries.Login;


public record LoginQuery(
    string Email,
    string Password
) : IRequest<ErrorOr<AuthenticationResult>>;