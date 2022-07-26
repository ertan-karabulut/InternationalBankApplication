using AutoMapper;
using BusinessLayer.Abstrack;
using BusinessLayer.Concreate.Component;
using BusinessLayer.Dto.PhpneNumber;
using BusinessLayer.Validation;
using BusinessLayer.Validation.PhoneNumberValidation;
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
    public class PhoneNumberWorkFlow : PhoneNumberComponent, IPhoneNumberWorkFlow
    {
        #region Variables
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unow;
        #endregion
        #region Constructor
        public PhoneNumberWorkFlow(IUnitOfWork unow, IMapper mapper) : base(unow)
        {
            this._mapper = mapper;
            this._unow = unow;
        }
        #endregion
        [ValidatorAspect(typeof(DataTableValidation))]
        public async Task<IResult<DataTableResponse<PhoneNumberDto>>> CustomerPhoneList(DataTableRequest request)
        {
            var result = ResultInjection.Result<DataTableResponse<PhoneNumberDto>>();

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

            var mailList = await base.CustomerPhoneNumberComponent(request);
            if (mailList.ResultStatus)
            {
                result.ResultObje = new DataTableResponse<PhoneNumberDto>();

                result.ResultObje.Data = this._mapper.Map<List<PhoneNumberDto>>(mailList.ResultObje.Data);
                result.ResultObje.TotalCount = mailList.ResultObje.TotalCount;
                result.SetTrue();
            }

            return result;
        }

        [ValidatorAspect(typeof(UpdatePhoneNumberValidation))]
        public async Task<IResult> UpdatePhoneNumber(PhoneNumberUpdateDto phone)
        {
            this._unow.BeginTransaction();
            if (phone.IsFavorite.HasValue && phone.IsFavorite.Value)
            {
                var favoritePhone = await this._unow.PhoneNumberRepository.Get(x => x.CustomerId == phone.CustomerId && x.IsFavorite).FirstOrDefaultAsync();
                if (favoritePhone != null && favoritePhone.Id != phone.Id)
                {
                    favoritePhone.IsFavorite = false;
                    this._unow.PhoneNumberRepository.Update(favoritePhone);
                }
            }
            var updatePhone = await base.UpdatePhoneNumberComponent(phone);
            if (updatePhone.ResultStatus)
                await this._unow.CommitAsync();
            else
                await this._unow.RollbackAsync();
            return updatePhone;
        }

        [ValidatorAspect(typeof(CreatePhoneNumberValidation))]
        public async Task<IResult<PhoneNumberDto>> AddPhoneNumber(PhoneNumberCreateDto phone)
        {
            var result = ResultInjection.Result<PhoneNumberDto>();
            phone.CustomerId = base.helperWorkFlow.GetUserId();

            if (phone.IsFavorite)
            {
                var favoritePhone = await this._unow.PhoneNumberRepository.Get(x => x.CustomerId == phone.CustomerId && x.IsActive && x.IsFavorite).FirstOrDefaultAsync();
                if (favoritePhone != null)
                {
                    favoritePhone.IsFavorite = false;
                    if (!(this._unow.PhoneNumberRepository.Update(favoritePhone).ResultStatus && await this._unow.SaveChangesAsync()))
                    {
                        result.SetFalse();
                    }
                }
            }

            var phoneEntity = this._mapper.Map<PhoneNumber>(phone);
            var phoneAdd = await base.AddPhoneNumberComponent(phoneEntity);
            if (phoneAdd.ResultStatus)
            {
                result.SetTrue();
                result.ResultObje = this._mapper.Map<PhoneNumberDto>(phoneAdd.ResultObje);
            }
            return result;
        }

        [ValidatorAspect(typeof(UpdatePhoneNumberValidation))]
        public async Task<IResult> DeletePhoneNumber(PhoneNumberUpdateDto phone)
        {
            IResult result = ResultInjection.Result();
            bool IsPhone = this._unow.PhoneNumberRepository.Get(x => true).Any(x => x.CustomerId == phone.CustomerId && x.IsActive);

            if (IsPhone)
            {
                result = await base.DeletePhoneNumberComponent(phone);
            }

            return result;
        }
    }
}
