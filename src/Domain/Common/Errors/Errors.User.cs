using ErrorOr;

namespace Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DuplicateEmail",
            description: "Email is already in use.");

        public static Error InvalidEmailOrPassword => Error.Conflict(
            code: "User.InvalidEmail",
            description: "Email/password is wrong");

        public static Error Unauthorized => Error.Unauthorized(
            code: "User.Unauthorized",
            description: "No access to this action.");
    }
}
