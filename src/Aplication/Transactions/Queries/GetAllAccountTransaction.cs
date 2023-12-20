using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Transactions.Queries;

public record GetAllAccountTransaction(Guid AccountId): IRequest<ErrorOr<List<Transaction>>>;
