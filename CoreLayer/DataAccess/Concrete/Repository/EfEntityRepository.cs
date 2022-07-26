using CoreLayer.DataAccess.Abstrack;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CoreLayer.Utilities.Ioc;
using System.Linq.Expressions;

namespace CoreLayer.DataAccess.Concrete.Repository
{
    public class EfEntityRepository<TEntity, TContext> where TEntity : class, new() where TContext : DbContext, new()
    {
        private readonly TContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public EfEntityRepository(TContext context)
        {
            this._context = context;
            this._dbSet = this._context.Set<TEntity>();
        }
        /// <summary>
        /// Sorguyu döner
        /// </summary>
        virtual public IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate, QueryTrackingBehavior isTracking = QueryTrackingBehavior.NoTracking)
        {
            return this._dbSet.AsTracking(isTracking).Where(predicate);
        }
        /// <summary>
        /// Liste şeklinde gönderilen veri tabanı nesnesini ekler.
        /// </summary>
        virtual public async Task<IResult<List<TEntity>>> AddAsync(List<TEntity> entity)
        {
            IResult<List<TEntity>> result = ResultInjection.Result<List<TEntity>>();
            await this._dbSet.AddRangeAsync(entity);
            result.SetTrue();
            result.ResultObje = entity;
            return result;
        }
        /// <summary>
        /// Liste şeklinde gönderilen veri tabanı nesnesini ekler.
        /// </summary>
        virtual public IResult<List<TEntity>> Add(List<TEntity> entity)
        {
            IResult<List<TEntity>> result = ResultInjection.Result<List<TEntity>>();
            this._dbSet.AddRange(entity);
            result.SetTrue();
            result.ResultObje = entity;
            return result;
        }

        /// <summary>
        /// Veri tabanı nesnesini ekler.
        /// </summary>
        virtual public async Task<IResult<TEntity>> AddAsync(TEntity entity)
        {
            IResult<TEntity> result = ResultInjection.Result<TEntity>();
            await this._dbSet.AddAsync(entity);
            result.SetTrue();
            result.ResultObje = entity;
            return result;
        }
        /// <summary>
        /// Veri tabanı nesnesini ekler.
        /// </summary>
        virtual public IResult<TEntity> Add(TEntity entity)
        {
            IResult<TEntity> result = ResultInjection.Result<TEntity>();
            this._dbSet.Add(entity);
            result.SetTrue();
            result.ResultObje = entity;
            return result;
        }

        /// <summary>
        /// Veri tabanı nesnesini günceller.
        /// </summary>
        virtual public IResult Update(TEntity entity)
        {
            IResult result = ResultInjection.Result();
            this._dbSet.Update(entity);
            result.SetTrue();
            return result;
        }
        /// <summary>
        /// Liste şeklinde gönderilen veri tabanı nesnesini günceller.
        /// </summary>
        virtual public IResult Update(List<TEntity> entity)
        {
            IResult result = ResultInjection.Result();
            this._dbSet.UpdateRange(entity);
            result.SetTrue();
            return result;
        }
        /// <summary>
        /// Veritabanı nesnesini siler.
        /// </summary>
        virtual public IResult Delete(TEntity entity)
        {
            IResult result = ResultInjection.Result();
            this._dbSet.Remove(entity);
            result.SetTrue();
            return result;
        }
        /// <summary>
        /// Liste şeklinde gönderilen veri tabanı nesnesini siler.
        /// </summary>
        virtual public IResult Delete(List<TEntity> entity)
        {
            IResult result = ResultInjection.Result();
            this._dbSet.RemoveRange(entity);
            result.SetTrue();
            return result;
        }
    }
}
