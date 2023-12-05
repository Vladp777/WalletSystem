using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Accounts.Commands;

public record DeleteAccount(Guid Id): IRequest<ErrorOr<Account>>;