using AutoMapper;
using BusinessLayer.Abstrack;
using BusinessLayer.Concreate.Component;
using BusinessLayer.Dto.Branch;
using CoreLayer.Utilities.Aspect;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concreate.WorkFlow
{
    public class BranchWorkFlow : BranchComponent, IBranchWorkFlow
    {
        #region Variables
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public BranchWorkFlow(IUnitOfWork unow, IMapper mapper) : base(unow)
        {
            this._mapper = mapper;
        }
        #endregion

        [CacheAspect]
        public async Task<IResult<List<BranchDto>>> BranchList(bool IsActive = true)
        {
            var result = ResultInjection.Result<List<BranchDto>>();
            var branchList = await base.BranchListComponent(IsActive);
            if (branchList.ResultStatus)
            {
                result.ResultObje = this._mapper.Map<List<BranchDto>>(branchList.ResultObje);
                result.SetTrue();
            }

            return result;
        }
    }
}
