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

        public async Task<Account> Handle(CreateAccount request)
        {
            var entity = new Account()
            {
                Id = new Guid(),
                ApplicationUserId = request.ApplicationUserId,
                Name = request.Name,
                Balance = request.Balance
            };

            return await _context.Create(entity);
        }
    }
}
