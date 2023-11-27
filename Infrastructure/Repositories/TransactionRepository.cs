using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Repositories;
using Domain.Entities;

namespace Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;

        public TransactionRepository(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Transaction> Create(Transaction entity)
        {
            var result = _context.Transactions.Add(entity);

            await _unitOfWork.SaveAsync();

            return result.Entity;
        }

        public async Task<Transaction> Delete(Guid Id)
        {
            var deleted = _context.Transactions.First(x => x.Id == Id);

            _context.Transactions.Remove(deleted);

            await _unitOfWork.SaveAsync();

            return deleted;
        }

        public Task<bool> DeleteAllTransactions(Guid accountId)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> Get(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Transaction>> GetAll(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetTransactionsByTag(Guid accountId, TransactionTag tag)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetTransactionsByType(Guid accountId, TransactionType type)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Transaction>> GetTransactionsByTypeAndTag(Guid accountId, TransactionType type, TransactionTag tag)
        {
            throw new NotImplementedException();
        }

        public Task<Transaction> Update(Transaction entity)
        {
            throw new NotImplementedException();
        }
    }
}
