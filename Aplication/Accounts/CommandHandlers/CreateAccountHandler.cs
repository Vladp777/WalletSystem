using Application.Accounts.Commands;
using Application.Repositories;
using Domain.Entities;
using MediatR;

namespace Application.Accounts.CommandHandlers
{
    public class CreateAccountHandler : IRequestHandler<CreateAccount, Account>
    {
        private readonly IAccountRepository _context;

        public CreateAccountHandler(IAccountRepository context, IUnitOfWork unitOfWork)
        {
            _context = context;
        }

        public Task<Account> Handle(CreateAccount request, CancellationToken cancellationToken)
        {
            var entity = new Account()
            {
                Id = new Guid(),
                ApplicationUserId = request.ApplicationUserId,
                Name = request.Name,
                Balance = request.Balance
            };

            return _context.Create(entity);
        }
    }
}
