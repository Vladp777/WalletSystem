using Domain.Entities;
using MediatR;

namespace Application.Accounts.Commands;

public record DeleteAccount(Guid Id): IRequest<Account>;