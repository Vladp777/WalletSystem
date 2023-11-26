using Domain.Entities;
using MediatR;

namespace Application.Accounts.Queries;

public record GetAccountById(Guid Id): IRequest<Account?>;
