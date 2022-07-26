using BusinessLayer.Dto;
using BusinessLayer.Dto.Account;
using BusinessLayer.Dto.Branch;
using CoreLayer.DataAccess.Concrete;
using CoreLayer.DataAccess.Concrete.DataRequest;
using CoreLayer.Utilities.Result.Abstrack;
using EntityLayer.Models.EntityFremework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstrack
{
    public interface IBranchWorkFlow
    {
        Task<IResult<List<BranchDto>>> BranchList(bool IsActive = true);
    }
}
