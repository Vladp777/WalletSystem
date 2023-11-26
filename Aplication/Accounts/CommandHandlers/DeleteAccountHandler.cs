using Application.Accounts.Commands;
using Application.Interfaces;
using Application.Repositories;
using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Accounts.CommandHandlers;

public class DeleteAccountHandler : IRequestHandler<DeleteAccount, ErrorOr<Account>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ICurrentUserService _currentUserService;

    public DeleteAccountHandler(IAccountRepository accountRepository, ICurrentUserService currentUserService)
    {
        _accountRepository = accountRepository;
        _currentUserService = currentUserService;
    }
    public async Task<ErrorOr<Account>> Handle(DeleteAccount request, CancellationToken cancellationToken)
    {
        var accountToDelete = 
        return await _accountRepository.Delete(request.Id);
    }
}
