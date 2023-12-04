using Application.Accounts.Queries;
using Application.Interfaces;
using Application.Repositories;
using Application.Transactions.Queries;
using Domain.Entities;
using Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace Application.Transactions.QueryHandlers;

public class GetTransactionByIdHandler : IRequestHandler<GetTransactionById, ErrorOr<Transaction>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetTransactionByIdHandler(IAccountRepository accountRepository, 
        ITransactionRepository transactionRepository, 
        ICurrentUserService currentUserService)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _currentUserService = currentUserService;
    }
    public async Task<ErrorOr<Transaction>> Handle(GetTransactionById request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.Get(request.Id);

        if (transaction == null)
        {
            return Errors.Transaction.TransactionNotFound;
        }

        var account = await _accountRepository.Get(transaction.AccountId);

        if (account == null)
        {
            return Errors.Account.AccountNotFound;
        }

        if (account.UserId != _currentUserService.UserId)
        {
            return Errors.User.Unauthorized;
        }

        return transaction;
    }
}
