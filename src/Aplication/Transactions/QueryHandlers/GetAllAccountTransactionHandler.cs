using Application.Interfaces;
using Application.Repositories;
using Application.Transactions.Queries;
using Domain.Entities;
using Domain.Common.Errors;
using ErrorOr;
using MediatR;


namespace Application.Transactions.QueryHandlers;

public class GetAllAccountTransactionHandler : IRequestHandler<GetAllAccountTransaction, ErrorOr<List<Transaction>>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetAllAccountTransactionHandler(IAccountRepository accountRepository, 
        ITransactionRepository transactionRepository,
        ICurrentUserService currentUserService)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _currentUserService = currentUserService;
    }
    public async Task<ErrorOr<List<Transaction>>> Handle(GetAllAccountTransaction request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.Get(request.AccountId);

        if (account == null)
        {
            return Errors.Account.AccountNotFound;
        }

        if (account.UserId != _currentUserService.UserId)
        {
            return Errors.User.Unauthorized;
        }

        var transactions = await _transactionRepository.GetAll(request.AccountId);

        return transactions;
    }
}
