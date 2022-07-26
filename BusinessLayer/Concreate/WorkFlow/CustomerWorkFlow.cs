using BusinessLayer.Abstrack;
using CoreLayer.BusinessLayer.Concreate;
using CoreLayer.Utilities.Enum;
using CoreLayer.Utilities.Ioc;
using CoreLayer.Utilities.Messages;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using DataAccessLayer.Abstract;
using EntityLayer.Models.EntityFremework;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using CoreLayer.DataAccess.Concrete;
using BusinessLayer.Concreate.Component;
using BusinessLayer.Dto;
using CoreLayer.DataAccess.Concrete.DataRequest;
using BusinessLayer.Dto.Adress;
using AutoMapper;
using CoreLayer.Utilities.Aspect;
using BusinessLayer.Validation;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Validation.AdressValidation;
using Microsoft.AspNetCore.Http;
using CoreLayer.BusinessLayer.Model;

namespace BusinessLayer.Concreate.WorkFlow
{
    public class CustomerWorkFlow : CustomerComponent, ICustomerWorkFlow
    {
        #region Variables
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unow;
        #endregion
        #region Constructor
        public CustomerWorkFlow(IHostingEnvironment env, IUnitOfWork unow, IMapper mapper) : base(env, unow)
        {
            this._mapper = mapper;
            this._unow = unow;
        }
        #endregion
        public async Task<IResult<ClaimDto>> GetClaim()
        {
            StringBuilder logText = new StringBuilder();
            IResult<ClaimDto> result = ResultInjection.Result<ClaimDto>();
            result.ResultObje = new ClaimDto();
            logText.AppendLine("Claims okuma işlemi başladı.");
            base.helperWorkFlow.GetTokenClaims(result.ResultObje);
            if (!object.Equals(result.ResultObje, null))
            {
                string filePath = await base.GetUserPhoto(result.ResultObje.Photo);
                if (!string.IsNullOrEmpty(filePath))
                {
                    result.ResultObje.Photo = filePath;
                    result.SetTrue();
                }
            }
            logText.AppendLine("İşlem sonucu " + (result.ResultStatus ? "başarılı." : "başarısız."));
            base.LogMessage.InsertLog(logText.ToString(), "GetClaim", "CustomerWorkFlow.cs");
            return result;
        }

        [ValidatorAspect(typeof(FileModelValidation))]
        public async Task<IResult> UpdateProfilePhoto(FileModel file)
        {
            string newFileName = string.Empty;
            try
            {
                StringBuilder logText = new StringBuilder();
                var result = ResultInjection.Result();
                var nameResult = await base.SaveProfilPhoto(file);
                if (nameResult.ResultStatus)
                {
                    logText.AppendLine($"Profil resmi kaydedildi. Dosya adı : {nameResult.ResultObje}");
                    int userId = base.helperWorkFlow.GetUserId();
                    var customer = await this._unow.CustomerRepository.Get(x => x.Id == userId, QueryTrackingBehavior.TrackAll).FirstOrDefaultAsync();
                    if (customer != null)
                    {
                        logText.AppendLine("Kullanıcı bilgileri bulundu.");
                        string fileName = customer.Photo;
                        newFileName = nameResult.ResultObje;
                        customer.Photo = nameResult.ResultObje;
                        await this._unow.BeginTransactionAsync();
                        if (this._unow.CustomerRepository.Update(customer).ResultStatus && await this._unow.SaveChangesAsync())
                        {
                            if (base.DeleteProfilePhoto(fileName).ResultStatus)
                                logText.AppendLine("Eski profil resmi silindi.");
                            logText.AppendLine("Kullanıcı güncellendi. İşlem Başarılı.");
                            result.SetTrue();
                            await this._unow.CommitAsync();
                        }
                        else
                        {
                            await this._unow.RollbackAsync();
                            if (base.DeleteProfilePhoto(nameResult.ResultObje).ResultStatus)
                                logText.AppendLine("Gönderilen dosya silindi.");
                            logText.AppendLine("Kullanıcı bilgileri güncellenemedi. İşlemler geri alındı.");
                        }
                    }
                }

                base.LogMessage.InsertLog(logText.ToString(), "UpdateProfilePhoto", "CustomerWorkFlow.cs");
                return result;
            }
            catch (Exception ex)
            {
                base.DeleteProfilePhoto(newFileName);
                await this._unow.RollbackAsync();
                throw ex;
            }
        }
    }
}
