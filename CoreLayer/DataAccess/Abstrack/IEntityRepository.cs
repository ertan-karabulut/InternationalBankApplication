using CoreLayer.DataAccess.Concrete;
using CoreLayer.Utilities.Result.Abstrack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DataAccess.Abstrack
{
    public interface IEntityRepository<TEntity, TContext> where TEntity : class where TContext : DbContext , new()
    {
        #region Listeleme metodları
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, QueryTrackingBehavior isTracking = QueryTrackingBehavior.NoTracking);
        #endregion

        #region Ekleme metodları async
        Task<IResult<List<TEntity>>> AddAsync(List<TEntity> entity);
        Task<IResult<TEntity>> AddAsync(TEntity entity);
        #endregion

        #region Ekleme metodları
        IResult<List<TEntity>> Add(List<TEntity> entity);
        IResult<TEntity> Add(TEntity entity);
        #endregion

        #region Güncelleme metodları
        IResult Update(TEntity entity);
        IResult Update(List<TEntity> entity);
        #endregion

        #region Silme medotları
        IResult Delete(List<TEntity> entity);
        IResult Delete(TEntity entity);
        #endregion
    }
}
