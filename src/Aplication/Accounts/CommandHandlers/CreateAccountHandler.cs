﻿using Application.Accounts.Commands;
using Application.Repositories;
using Domain.Entities;
using ErrorOr;
using MediatR;

namespace Application.Accounts.CommandHandlers
{
    public class CreateAccountHandler : IRequestHandler<CreateAccount, ErrorOr<Account>>
    {
        private readonly IAccountRepository _context;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAccountHandler(IAccountRepository context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Account>> Handle(CreateAccount request, CancellationToken cancellationToken)
        {
            var entity = new Account()
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId.ToString(),
                Name = request.Name,
                Balance = request.Balance
            };

            var result = await _context.Create(entity);

            await _unitOfWork.SaveAsync();

            return result;
        }
    }
}
