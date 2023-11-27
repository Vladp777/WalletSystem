using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Transactions.Commands;

public record DeleteTransaction(Guid Id) : IRequest<ErrorOr<Transaction>>;

