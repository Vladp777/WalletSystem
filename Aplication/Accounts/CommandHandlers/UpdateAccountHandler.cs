using Application.Accounts.Commands;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.CommandHandlers;

public class UpdateAccountHandler : IRequestHandler<UpdateAccount, Account>
{
    private readonly IAccountRepository _repository;

    public UpdateAccountHandler(IAccountRepository repository)
    {
        _repository = repository;
    }
    public Task<Account> Handle(UpdateAccount request, CancellationToken cancellationToken)
    {
        var entity = new Account
        {
            Id = request.Id,
            Name = request.Name,
            Balance = request.Balance
        };
        return _repository.Update(entity);
    }
}
