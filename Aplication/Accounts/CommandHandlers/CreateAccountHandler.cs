using Application.Accounts.Commands;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.CommandHandlers
{
    public class CreateAccountHandler : IRequestHandler<CreateAccount, Account>
    {
        private readonly IAccountRepository _context;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAccountHandler(IAccountRepository context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Account> Handle(CreateAccount request, CancellationToken cancellationToken)
        {
            var entity = new Account()
            {
                Id = new Guid(),
                UserId = request.ApplicationUserId.ToString(),
                Name = request.Name,
                Balance = request.Balance
            };

            var result = await _context.Create(entity);

            await _unitOfWork.SaveAsync();

            return result;
        }
    }
}
