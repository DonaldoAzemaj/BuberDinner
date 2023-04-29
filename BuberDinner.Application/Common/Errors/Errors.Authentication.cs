using ErrorOr;

namespace BuberDinner.Application.Common.Errors;

public static partial class ApplicationErrors
{
    public static class Authentication
    {
        public static Error InvalidedCredentials => Error.Validation(
            code: "ApplicationErrors.Authentication.InvalidedCredentials",
            description: "Credentials are invalid");

        public static Error DuplicateEmail => Error.Conflict(
            code: "DomainErrors.User.DuplicateEmail",
            description: "Email is already in use.");
    }

}