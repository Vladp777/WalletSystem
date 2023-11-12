using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.Commands;

public record CreateAccount(Guid ApplicationUserId,
    string Name,
    double Balance
    ) : IRequest<Account>;



