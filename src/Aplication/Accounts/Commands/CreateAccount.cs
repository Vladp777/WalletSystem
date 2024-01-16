using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Accounts.Commands;

public record CreateAccount(Guid UserId,
    string Name,
    double Balance
    ) : IRequest<ErrorOr<Account>>;



