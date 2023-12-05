using Application.Interfaces;
using Application.Repositories;
using Application.Transactions.Commands;
using Domain.Entities;
using Domain.Common.Errors;

using ErrorOr;
using MediatR;

namespace Application.Transactions.CommandHandlers;

public class UpdateTransactionHandler : IRequestHandler<UpdateTransaction, ErrorOr<Transaction>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly ITransactionRepository _transactionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public UpdateTransactionHandler(IAccountRepository accountRepository,
        ITransactionRepository transactionRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _accountRepository = accountRepository;
        _transactionRepository = transactionRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }
    public async Task<ErrorOr<Transaction>> Handle(UpdateTransaction request, CancellationToken cancellationToken)
    {
        var transaction = await _transactionRepository.Get(request.Id);

        if (transaction == null)
        {
            return Errors.Account.AccountNotFound;
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

        var transactionsTags = await _transactionRepository.GetTransactionTags();

        if (!(transactionsTags.Any(t => t.Id == request.TagId)))
        {
            return Errors.Transaction.WrongTransactionTag;
        }

        var updated = new Transaction
        {
            Id = request.Id,
            AccountId = account.Id,
            Count = request.Count,
            DateTime = request.DateTime,
            Description = request.Description,
            TagId = request.TagId,
        };

        var result = await _transactionRepository.Update(updated);

        if (transaction.TypeId == TransactionType.Income.Id)
        {
            account.Balance -= transaction.Count;

            account.Balance += updated.Count;
        }
        else if (transaction.TypeId == TransactionType.Expence.Id)
        {
            account.Balance += transaction.Count;

            account.Balance -= updated.Count;

        }

        await _unitOfWork.SaveAsync();

        return result;
    }
}
