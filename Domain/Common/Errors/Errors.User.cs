using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Errors;

public static class Errors
{
    public static class User
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "User.DuplicateEmail",
            description: "Email is already in use.");

        public static Error InvalidEmailPassword => Error.Conflict(
            code: "User.InvalidEmail",
            description: "Email/password is wrong");
    }
}
