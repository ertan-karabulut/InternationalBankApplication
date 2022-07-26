using CoreLayer.BusinessLayer.Concreate;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using DataAccessLayer.Abstract;
using EntityLayer.Models.EntityFremework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concreate.Component
{
    public class BranchComponent : ComponentBase
    {
        #region Variables
        private readonly IUnitOfWork _unow;
        #endregion

        #region Constructor
        public BranchComponent(IUnitOfWork unow)
        {
            this._unow = unow;
        }
        #endregion

        #region protected method
        protected async Task<IResult<List<Branch>>> BranchListComponent(bool IsActive = true)
        {
            var result = ResultInjection.Result<List<Branch>>();
            var query = this._unow.BranchRepository.Get(x => x.IsActive == IsActive);
            query = query.Include(x => x.City).Include(x => x.District);

            var branchList = await query.ToListAsync();
            if (branchList.Count > 0)
            {
                result.ResultObje = branchList;
                result.SetTrue();
            }

            return result;
        }
        #endregion
    }
}
