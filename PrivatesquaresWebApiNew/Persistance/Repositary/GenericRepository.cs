using PrivatesquaresWebApiNew.Persistance.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PrivatesquaresWebApiNew.Persistance.Repositary
{
    public abstract class GenericRepository<C> : IGenericRepository<C> where C : EWT_PSQNEWEntities, new()
    {
        public C Context { get; set; } = new C();

    }

    public interface IGenericRepository<C> where C : EWT_PSQNEWEntities, new()
    {
    }
}