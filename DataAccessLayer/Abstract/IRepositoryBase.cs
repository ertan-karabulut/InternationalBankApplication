using CoreLayer.DataAccess.Abstrack;
using CoreLayer.DataAccess.Concrete;
using CoreLayer.Utilities.Result.Abstrack;
using EntityLayer.Models.EntityFremework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IRepositoryBase<TEntity> : IEntityRepository<TEntity,MyBankContext>  where TEntity : class
    {
    }
}
