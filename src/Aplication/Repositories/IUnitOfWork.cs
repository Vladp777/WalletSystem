﻿namespace Application.Repositories;

public interface IUnitOfWork
{
    Task SaveAsync();
}
