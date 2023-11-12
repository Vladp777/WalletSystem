using Application.Accounts.Commands;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.CommandHandlers;

public class DeleteAccountHandler : IRequestHandler<DeleteAccount, Account>
{
    private readonly IAccountRepository _repository;

    public DeleteAccountHandler(IAccountRepository repository)
    {
        _repository = repository;
    }
    public Task<Account> Handle(DeleteAccount request, CancellationToken cancellationToken)
    {
        return _repository.Delete(request.Id);
    }
}
