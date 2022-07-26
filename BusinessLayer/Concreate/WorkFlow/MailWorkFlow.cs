using AutoMapper;
using BusinessLayer.Abstrack;
using BusinessLayer.Concreate.Component;
using BusinessLayer.Dto.Mail;
using BusinessLayer.Validation;
using BusinessLayer.Validation.MailValidation;
using CoreLayer.DataAccess.Concrete.DataRequest;
using CoreLayer.Utilities.Aspect;
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

namespace BusinessLayer.Concreate.WorkFlow
{
    public class MailWorkFlow : MailComponent, IMailWorkFlow
    {
        #region Variables
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unow;
        #endregion
        #region Constructor
        public MailWorkFlow(IUnitOfWork unow, IMapper mapper) : base(unow)
        {
            this._mapper = mapper;
            this._unow = unow;
        }
        #endregion

        [ValidatorAspect(typeof(DataTableValidation))]
        public async Task<IResult<DataTableResponse<MailDto>>> CustomerMailList(DataTableRequest request)
        {
            var result = ResultInjection.Result<DataTableResponse<MailDto>>();

            if (!request.Filter.Any(x => x.Name == "CustomerId"))
            {
                int customerId = base.helperWorkFlow.GetUserId();
                request.Filter.Add(new NameValuePair { Name = "CustomerId", Value = customerId.ToString() });
            }

            if (!request.Filter.Any(x => x.Name == "IsActive"))
                request.Filter.Add(new NameValuePair { Name = "IsActive", Value = "True" });

            if (request.Order.Column != "IsFavorite")
            {
                request.Order.Column = "IsFavorite";
                request.Order.Short = "desc";
            }

            var mailList = await base.CustomerMailListComponent(request);
            if (mailList.ResultStatus)
            {
                result.ResultObje = new DataTableResponse<MailDto>();

                result.ResultObje.Data = this._mapper.Map<List<MailDto>>(mailList.ResultObje.Data);
                result.ResultObje.TotalCount = mailList.ResultObje.TotalCount;
                result.SetTrue();
            }

            return result;
        }

        [ValidatorAspect(typeof(UpdateMailValidation))]
        public async Task<IResult> UpdateMail(MailUpdateDto mail)
        {
            this._unow.BeginTransaction();
            if (mail.IsFavorite.HasValue && mail.IsFavorite.Value)
            {
                var favoriteMail= await this._unow.MailRespository.Get(x => x.CustomerId == mail.CustomerId && x.IsFavorite).FirstOrDefaultAsync();
                if (favoriteMail != null && favoriteMail.Id != mail.Id)
                {
                    favoriteMail.IsFavorite = false;
                    this._unow.MailRespository.Update(favoriteMail);
                }
            }
            var updateMail = await base.UpdateMailComponent(mail);
            if (updateMail.ResultStatus)
                await this._unow.CommitAsync();
            else
                await this._unow.RollbackAsync();
            return updateMail;
        }

        [ValidatorAspect(typeof(UpdateMailValidation))]
        public async Task<IResult> DeleteMail(MailUpdateDto mail)
        {
            IResult result = ResultInjection.Result();
            bool IsMail = this._unow.MailRespository.Get(x => true).Any(x => x.CustomerId == mail.CustomerId && x.IsActive);

            if (IsMail)
            {
                result = await base.DeleteMailComponent(mail);
            }

            return result;
        }

        [ValidatorAspect(typeof(CreateMailValidation))]
        public async Task<IResult<MailDto>> AddMail(MailCreateDto mail)
        {
            var result = ResultInjection.Result<MailDto>();
            mail.CustomerId = base.helperWorkFlow.GetUserId();

            if (mail.IsFavorite)
            {
                var favoriteMail = await this._unow.MailRespository.Get(x => x.CustomerId == mail.CustomerId && x.IsActive && x.IsFavorite).FirstOrDefaultAsync();
                if (favoriteMail != null)
                {
                    favoriteMail.IsFavorite = false;
                    if (!(this._unow.MailRespository.Update(favoriteMail).ResultStatus && await this._unow.SaveChangesAsync()))
                    {
                        result.SetFalse();
                    }
                }
            }

            var mailEntity = this._mapper.Map<EMail>(mail);
            var mailAdd = await base.AddMailComponent(mailEntity);
            if (mailAdd.ResultStatus)
            {
                result.SetTrue();
                result.ResultObje = this._mapper.Map<MailDto>(mailAdd.ResultObje);
            }
            return result;
        }
    }
}
