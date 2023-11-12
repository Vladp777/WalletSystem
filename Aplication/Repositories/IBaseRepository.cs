using Domain.Common;
using Domain.Entities;

namespace Application.Repositories;

internal interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<T> Delete(T entity);
    Task<T> Get(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAll(Guid id, CancellationToken cancellationToken);
}
