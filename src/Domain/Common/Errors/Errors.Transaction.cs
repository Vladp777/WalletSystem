using ErrorOr;

namespace Domain.Common.Errors
{
    public static partial class Errors
    {
        public static class Transaction
        {
            public static Error WrongTransactionType => Error.Conflict(
                code: "Transaction.WrongTransactionType",
                description: "Wrong transaction type");

            public static Error TransactionNotFound => Error.NotFound(
                code: "User.TransactionNotFound",
                description: "Transaction not found.");

            public static Error WrongTransactionTag => Error.Conflict(
                code: "Transaction.WrongTransactionTag",
                description: "Wrong transaction tag");
        }
    }
}
