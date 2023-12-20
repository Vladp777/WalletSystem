using Application.Interfaces;
using Application.Repositories;
using Application.Transactions.Commands;
using Domain.Entities;
using Domain.Common.Errors;

using ErrorOr;
using MediatR;

namespace Application.Transactions.CommandHandlers
{
    public class DeleteTransactionHandler : IRequestHandler<DeleteTransaction, ErrorOr<Transaction>>
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteTransactionHandler(IAccountRepository accountRepository, 
            ITransactionRepository transactionRepository, 
            IUnitOfWork unitOfWork, 
            ICurrentUserService currentUserService)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<ErrorOr<Transaction>> Handle(DeleteTransaction request, CancellationToken cancellationToken)
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

            var result = await _transactionRepository.Delete(transaction.Id);

            if(transaction.TypeId == TransactionType.Income.Id)
            {
                account.Balance -= transaction.Count;
            }
            else if (transaction.TypeId == TransactionType.Expence.Id)
            {
                account.Balance += transaction.Count;
            }

            await _unitOfWork.SaveAsync();

            return result;
        }
    }
}
