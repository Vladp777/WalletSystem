using Domain.Entities;
using MediatR;

namespace Application.Accounts.Commands;

public record UpdateAccount(Guid Id, 
    string Name,
    double Balance
    ) : IRequest<Account>;

