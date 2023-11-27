using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Accounts.Queries;

public record GetAccountById(Guid Id): IRequest<ErrorOr<Account>>;
