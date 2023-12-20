using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Transactions.Commands;

public record UpdateTransaction(Guid Id,
    string Description,
    double Count,
    DateTime DateTime,
    int TagId): IRequest<ErrorOr<Transaction>>;
