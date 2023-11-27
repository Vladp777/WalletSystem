using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Transactions.Commands;

public record UpdateTransaction(Guid Id,
    Guid AccountId,
    TransactionType Type,
    string Description,
    double Count,
    DateTime DateTime,
    double Result_Balance,
    TransactionTag Tag): IRequest<ErrorOr<Transaction>>;
