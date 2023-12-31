﻿using Domain.Entities;

namespace Application.Repositories;

public interface IBaseRepository<T> where T : BaseEntity
{
    Task<T> Create(T entity);
    Task<T> Update(T entity);
    Task<T> Delete(Guid Id);
    Task<T?> Get(Guid id);
    Task<List<T>> GetAll(Guid id);
}
