﻿using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Account
        {
            public static Error AccountNotFound => Error.NotFound(
            code: "User.AccountNotFound",
            description: "Account not found.");
        }
    }
}
