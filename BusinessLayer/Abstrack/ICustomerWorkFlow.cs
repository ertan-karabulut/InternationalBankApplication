using BusinessLayer.Dto;
using BusinessLayer.Dto.Adress;
using CoreLayer.BusinessLayer.Model;
using CoreLayer.DataAccess.Concrete.DataRequest;
using CoreLayer.Utilities.Result.Abstrack;
using EntityLayer.Models.EntityFremework;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstrack
{
    public interface ICustomerWorkFlow 
    {
        Task<IResult<ClaimDto>> GetClaim();
        Task<IResult> UpdateProfilePhoto(FileModel file);
    }
}
