using Domain.Entities;
using ErrorOr;
using MediatR;


namespace Application.Transactions.Commands;

public record CreateTransaction(
    Guid AccountId,
    int TypeId,
    string Description,
    double Count,
    DateTime DateTime,
    int TagId) : IRequest<ErrorOr<Transaction>>;