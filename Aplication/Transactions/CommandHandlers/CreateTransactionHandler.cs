using Application.Interfaces;
using Application.Repositories;
using Application.Transactions.Commands;
using Domain.Common.Errors;
using Domain.Entities;

using ErrorOr;
using MediatR;

namespace Application.Transactions.CommandHandlers;

public class CreateTransactionHandler : IRequestHandler<CreateTransaction, ErrorOr<Transaction>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrentUserService _currentUserService;

    public CreateTransactionHandler(ITransactionRepository transactionRepository, 
        ICurrentUserService currentUserService, 
        IAccountRepository accountRepository, 
        IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _currentUserService = currentUserService;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ErrorOr<Transaction>> Handle(CreateTransaction request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.Get(request.AccountId);

        if (account == null)
        {
            return Errors.Account.AccountNotFound;
        }

        if(account.UserId != _currentUserService.UserId)
        {
            return Errors.User.Unauthorized;
        }

        double resultBalance;

        if (request.TypeId == TransactionType.Income.Id)
        {
            resultBalance = account.Balance + request.Count;
        }
        else if (request.TypeId == TransactionType.Expence.Id)
        {
            resultBalance = account.Balance - request.Count;
        }
        else
        {
            return Errors.Transaction.WrongTransactionType;
        }

        var transactionsTags = await _transactionRepository.GetTransactionTags();

        if (!(transactionsTags.Any(t => t.Id == request.TagId)))
        {
            return Errors.Transaction.WrongTransactionTag;
        }

        var transaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            TypeId = request.TypeId,
            Description = request.Description,
            Count = request.Count,
            DateTime = request.DateTime,
            Result_Balance = resultBalance,
            TagId = request.TagId
        };

        var result = await _transactionRepository.Create(transaction);

        account.Balance = resultBalance;

        await _unitOfWork.SaveAsync();

        return result;
    }
}
