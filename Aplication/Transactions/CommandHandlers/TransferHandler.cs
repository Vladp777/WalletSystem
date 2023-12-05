using Application.Interfaces;
using Application.Repositories;
using Application.Transactions.Commands;
using Domain.Entities;
using Domain.Common.Errors;
using ErrorOr;
using MediatR;

namespace Application.Transactions.CommandHandlers;

public class TransferHandler : IRequestHandler<TransferCommand, ErrorOr<Transaction>>
{
    private readonly IAccountRepository _accountRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITransactionRepository _transactionRepository;
    private readonly ICurrentUserService _currentUserService;

    public TransferHandler(ITransactionRepository transactionRepository,
        ICurrentUserService currentUserService,
        IAccountRepository accountRepository,
        IUnitOfWork unitOfWork)
    {
        _transactionRepository = transactionRepository;
        _currentUserService = currentUserService;
        _accountRepository = accountRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task<ErrorOr<Transaction>> Handle(TransferCommand request, CancellationToken cancellationToken)
    {
        var fromAccount = await _accountRepository.Get(request.FromId);

        var toAccount = await _accountRepository.Get(request.ToId);

        if (fromAccount == null || toAccount == null)
        {
            return Errors.Account.AccountNotFound;
        }

        if (fromAccount.UserId != _currentUserService.UserId
                || toAccount.UserId != _currentUserService.UserId)
        {
            return Errors.User.Unauthorized;
        }

        var fromTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = request.FromId,
            TypeId = TransactionType.Expence.Id,
            Description = $"Transfer to '{toAccount.Name}' account",
            Count = request.Count,
            DateTime = DateTime.Now,
            TagId = TransactionTag.Transfer.Id
        };

        var toTransaction = new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = request.ToId,
            TypeId = TransactionType.Income.Id,
            Description = $"Transfer from '{fromAccount.Name}' account",
            Count = request.Count,
            DateTime = DateTime.Now,
            TagId = TransactionTag.Transfer.Id
        };

        var resultFrom = await _transactionRepository.Create(fromTransaction);
        var resultTo = await _transactionRepository.Create(toTransaction);

        fromAccount.Balance -= request.Count;
        toAccount.Balance += request.Count;

        await _unitOfWork.SaveAsync();

        return resultFrom;
    }
}
