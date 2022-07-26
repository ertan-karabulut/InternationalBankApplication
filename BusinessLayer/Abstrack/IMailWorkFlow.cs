using BusinessLayer.Dto;
using BusinessLayer.Dto.Adress;
using BusinessLayer.Dto.Mail;
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
    public interface IMailWorkFlow 
    {
        Task<IResult<DataTableResponse<MailDto>>> CustomerMailList(DataTableRequest request);
        Task<IResult> UpdateMail(MailUpdateDto mail);
        Task<IResult> DeleteMail(MailUpdateDto mail);
        Task<IResult<MailDto>> AddMail(MailCreateDto mail);
    }
}
