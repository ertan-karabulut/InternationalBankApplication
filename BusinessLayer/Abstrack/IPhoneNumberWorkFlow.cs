using BusinessLayer.Dto;
using BusinessLayer.Dto.Adress;
using BusinessLayer.Dto.Mail;
using BusinessLayer.Dto.PhpneNumber;
using CoreLayer.DataAccess.Concrete.DataRequest;
using CoreLayer.Utilities.Result.Abstrack;
using EntityLayer.Models.EntityFremework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstrack
{
    public interface IPhoneNumberWorkFlow 
    {
        Task<IResult<DataTableResponse<PhoneNumberDto>>> CustomerPhoneList(DataTableRequest request);
        Task<IResult> UpdatePhoneNumber(PhoneNumberUpdateDto phone);
        Task<IResult<PhoneNumberDto>> AddPhoneNumber(PhoneNumberCreateDto phone);
        Task<IResult> DeletePhoneNumber(PhoneNumberUpdateDto phone);
    }
}
