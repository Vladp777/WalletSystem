using ErrorOr;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Transaction
        {
            public static Error WrongTransactionType => Error.Conflict(
            code: "Transaction.WrongTransactionType",
            description: "Wrong transaction type");
        }
    }
}
