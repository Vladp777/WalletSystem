using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Transactions.Queries;

public record GetTransactionById(Guid Id): IRequest<ErrorOr<Transaction>>;
