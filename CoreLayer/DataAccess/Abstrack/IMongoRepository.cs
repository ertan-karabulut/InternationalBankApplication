using CoreLayer.Utilities.Logger;
using CoreLayer.Utilities.Result.Abstrack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.DataAccess.Abstrack
{
    public interface IMongoRepository<TCollection>
    {
        IQueryable<TCollection> Get();
        Task<IResult<TCollection>> AddAsync(TCollection entity);
        IResult<TCollection> Add(TCollection entity);
        Task<IResult<List<TCollection>>> AddAsync(List<TCollection> entity);
        IResult<List<TCollection>> Add(List<TCollection> entity);
    }
}
