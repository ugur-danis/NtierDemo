﻿using NtierDemo.Entities.Concrete;
using NtierDemo.Core.DataAccess;

namespace NtierDemo.DataAccess.Abstract
{
    public interface IAuthorDal : IEntityRepository<Author>
    {
    }
}
