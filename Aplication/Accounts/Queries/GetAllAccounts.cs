using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Accounts.Queries;

public record GetAllAccounts(Guid UserId): IRequest<ErrorOr<List<Account>>>;
