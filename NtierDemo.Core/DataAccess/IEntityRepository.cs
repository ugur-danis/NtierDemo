﻿using NtierDemo.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace NtierDemo.Core.DataAccess
{
    public interface IEntityRepository<T> where T : class, IEntity, new()
    {
        IList<T> GetList(Expression<Func<T, bool>> filter = null);
        T Get(Expression<Func<T, bool>> filter);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
