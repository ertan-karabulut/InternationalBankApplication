using AutoMapper;
using BusinessLayer.Abstrack;
using BusinessLayer.Concreate.Component;
using BusinessLayer.Dto;
using BusinessLayer.Dto.Account;
using BusinessLayer.Mappers;
using BusinessLayer.Validation;
using BusinessLayer.Validation.AccontValidation;
using CoreLayer.BusinessLayer.Concreate;
using CoreLayer.DataAccess.Concrete.DataRequest;
using CoreLayer.Utilities.Aspect;
using CoreLayer.Utilities.Exception;
using CoreLayer.Utilities.Messages;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using DataAccessLayer.Abstract;
using EntityLayer.Models.EntityFremework;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concreate.WorkFlow
{
    public class AccountWorkFlow : AccountComponent, IAccountWorkFlow
    {
        #region Variables
        private readonly IUnitOfWork _unow;
        private readonly IMapper _mapper;
        #endregion
        #region constructor
        public AccountWorkFlow(IUnitOfWork unow, IMapper mapper) : base(unow)
        {
            this._unow = unow;
            this._mapper = mapper;
        }
        #endregion

        [ValidatorAspect(typeof(DataTableValidation))]
        public async Task<IResult<DataTableResponse<MyAccountDto>>> GetMyAccountList(DataTableRequest request)
        {
            IResult<DataTableResponse<MyAccountDto>> result = ResultInjection.Result<DataTableResponse<MyAccountDto>>();
            int customerId = base.helperWorkFlow.GetUserId();

            if(!request.Filter.Any(x=>x.Name == "CustomerId"))
                request.Filter.Add(new NameValuePair { Name = "CustomerId", Value = customerId.ToString() });

            if (!request.Filter.Any(x => x.Name == "IsActive"))
                request.Filter.Add(new NameValuePair { Name = "IsActive", Value = "True" });

            var resultAccont = await base.GetMyAccountListComponent(request);
            if (resultAccont.ResultStatus)
            {
                result.ResultObje = new DataTableResponse<MyAccountDto>();
                result.ResultObje.Data = this._mapper.Map<List<MyAccountDto>>(resultAccont.ResultObje.Data);
                result.ResultObje.TotalCount = resultAccont.ResultObje.TotalCount;
                result.SetTrue();
            }

            return result;
        }

        public async Task<IResult<AccountDetailDto>> AccountDetail(int accountId)
        {
            IResult<AccountDetailDto> result = ResultInjection.Result<AccountDetailDto>();
            StringBuilder logText = new StringBuilder();
            var account = await base.AccountDetailComponent(accountId);
            if (account.ResultStatus)
            {
                logText.AppendLine($"Hesap bulundu. IBAN {account.ResultObje.Iban}");
                int customerId = base.helperWorkFlow.GetUserId();
                if (account.ResultObje.CustomerId == customerId)
                {
                    result.ResultObje = this._mapper.Map<AccountDetailDto>(account.ResultObje);
                    result.SetTrue();
                }
                else
                    logText.AppendLine("Hesap size ait değil. İşlem başarısız oldu.");
            }
            else
                logText.AppendLine("Hesap bulunamadı.");
            base.LogMessage.InsertLog(logText.ToString(), "AccountDetail", "AccountWorkFlow.cs");
            return result;
        }

        [ValidatorAspect(typeof(DataTableValidation))]
        public async Task<IResult<DataTableResponse<AccountHistoryDto>>> AccountHistory(DataTableRequest request)
        {
            var result = ResultInjection.Result<DataTableResponse<AccountHistoryDto>>();
            if (request.Filter.Any(x => x.Name == "AccountId") && request.Filter.FirstOrDefault(x => x.Name == "AccountId").Value != null)
            {
                var history = await base.AccountHistoryComponent(request);
                if (history.ResultStatus)
                {
                    result.ResultObje = new DataTableResponse<AccountHistoryDto>();
                    result.ResultObje.Data = this._mapper.Map<List<AccountHistoryDto>>(history.ResultObje.Data);
                    result.ResultObje.TotalCount = history.ResultObje.TotalCount;
                    result.SetTrue();
                }
            }
            else
            {
                result.ResultInnerMessage = "AccountId girilmelidir.";
                result.SetFalse(StaticMessage.DefaultUIMessageCode);
            }

            return result;
        }

        [ValidatorAspect(typeof(CreateAccountListValidation))]
        public async Task<IResult<List<AccountDto>>> AddAsync(AccountCreatDtoList entity)
        {
            IResult<List<AccountDto>> result = ResultInjection.Result<List<AccountDto>>();
            var account = this._mapper.Map<List<Account>>(entity.AccountList);
            List<int> branchIdList = account.Select(x => x.BranchId).ToList();

            var branchList = this._unow.BranchRepository.Get(x => branchIdList.Contains(x.Id)).ToList();

            if (!object.Equals(branchList,null))
            {
                account.ForEach(x => {
                    x.AccountNumber = CreateAccountNumber();
                    x.Iban = CreateIban(x.AccountNumber, branchList.Where(y=>y.Id == x.BranchId).Select(y=>y.BranchNumber).FirstOrDefault());
                });
                var addResult = await this._unow.AccountRepository.AddAsync(account);
                if (addResult.ResultStatus && await this._unow.SaveChangesAsync())
                {
                    result.ResultObje = this._mapper.Map<List<AccountDto>>(addResult.ResultObje);
                    result.SetTrue();
                }
            }
            return result;
        }

        [ValidatorAspect(typeof(CreateAccountValidation))]
        public async Task<IResult<AccountDto>> AddAsync(AccountCreatDto entity)
        {
            IResult<AccountDto> result = ResultInjection.Result<AccountDto>();
            var account = this._mapper.Map<Account>(entity);
            var branch = this._unow.BranchRepository.Get(x=> x.Id == account.BranchId).FirstOrDefault();
            if (branch != null)
            {
                account.AccountNumber = this.CreateAccountNumber();
                account.Iban = this.CreateIban(account.AccountNumber, branch.BranchNumber);

                var resultUpdate = await this._unow.AccountRepository.AddAsync(account);
                if (resultUpdate.ResultStatus && await this._unow.SaveChangesAsync())
                {
                    result.ResultObje = this._mapper.Map<AccountDto>(resultUpdate.ResultObje);
                    result.SetTrue();
                }
            }
            else
            {
                result.SetFalse();
                result.ResultInnerMessage = "Hatalı şube kodu";
            }

            return result;
        }

        [ValidatorAspect(typeof(UpdateAccountListValidation))]
        public async Task<IResult> DeleteAsync(AccountUpdateDtoList entity)
        {
            IResult result = ResultInjection.Result();
            var account = this._mapper.Map<List<Account>>(entity.AccountList);
            var deteResult = this._unow.AccountRepository.Delete(account);
            if (deteResult.ResultStatus && await this._unow.SaveChangesAsync())
            {
                result.SetTrue();
            }
            return result;
        }

        [ValidatorAspect(typeof(UpdateAccountValidation))]
        public async Task<IResult> DeleteAsync(AccountUpdateDto entity)
        {
            IResult result = ResultInjection.Result();
            var account = this._mapper.Map<Account>(entity);
            var deteResult = this._unow.AccountRepository.Delete(account);
            if (deteResult.ResultStatus && await this._unow.SaveChangesAsync())
            {
                result.SetTrue();
            }
            return result;
        }

        [ValidatorAspect(typeof(UpdateAccountValidation))]
        public async Task<IResult> UpdateAsync(AccountUpdateDto entity)
        {
            IResult result = ResultInjection.Result();
            var updateAccount = await this._unow.AccountRepository.Get(x => true).Where(x => x.Id == entity.Id).FirstOrDefaultAsync();

            if (!object.Equals(updateAccount, null))
            {
                EntityToModelMapping(updateAccount, entity);
                var resultUpdate = this._unow.AccountRepository.Update(updateAccount);
                if (resultUpdate.ResultStatus && await this._unow.SaveChangesAsync())
                {
                    result.SetTrue();
                }
            }
            return result;
        }

        [ValidatorAspect(typeof(UpdateAccountListValidation))]
        public async Task<IResult> UpdateAsync(AccountUpdateDtoList entity)
        {
            IResult result = ResultInjection.Result();
            List<int> accountIdList = entity.AccountList.Select(x => x.Id).ToList();
            var resultList = await this._unow.AccountRepository.Get(x => accountIdList.Contains(x.Id)).ToListAsync();

            if (resultList.Count == entity.AccountList.Count)
            {
                resultList.ForEach(x => EntityToModelMapping(x, entity.AccountList.Find(y => y.Id == x.Id)));
                var resultUpdate = this._unow.AccountRepository.Update(resultList);
                if (resultUpdate.ResultStatus && await this._unow.SaveChangesAsync())
                {
                    result.SetTrue();
                }
            }
            else
            {
                var errorMessageList = 
                    entity.AccountList.Where(x => resultList.Any(y => y.Id != x.Id)).Select(x=> $"Hesap bulunamadı {x.Id.ToString()}").ToList();

                throw new CustomValidationException(StaticMessage.DefaultValidationMessage, errorMessageList);
            }
            return result;
        }

        [ValidatorAspect(typeof(UpdateAccountValidation))]
        public async Task<IResult> CloseAccount(AccountUpdateDto account)
        {
            StringBuilder logText = new StringBuilder();
            var result = ResultInjection.Result();
            var closeAccount = await base.CloseAccountComponent(account);
            
            if (closeAccount.ResultStatus)
            {
                logText.AppendLine($"Kapatılacak hesap bulundu. Id:{closeAccount.ResultObje.Id} IBAN: {closeAccount.ResultObje.Iban}");
                if ((closeAccount.ResultObje.AccountBalances.Count <= 0) || (closeAccount.ResultObje.AccountBalances.Count > 0 && closeAccount.ResultObje.AccountBalances.FirstOrDefault().Balance == 0))
                {
                    closeAccount.ResultObje.IsActive = false;
                    var closed = this._unow.AccountRepository.Update(closeAccount.ResultObje);
                    if (closed.ResultStatus && await this._unow.SaveChangesAsync())
                    {
                        logText.AppendLine("Hesap başarıyla kapatıldı.");
                        result.SetTrue();
                    }
                }
                else
                {
                    logText.AppendLine($"Hesap bakiyesi mevcut. Kapatılamaz.");
                    result.ResultInnerMessage = StaticMessage.AccountBalanceError;
                    result.SetFalse(StaticMessage.DefaultUIMessageCode);
                }
            }
            else
            {
                logText.AppendLine("Hesap bulunamadı.");
                result.ResultInnerMessage = "Kapatılacak hesap bulunamadı.";
                result.SetFalse(StaticMessage.DefaultUIMessageCode);
            }
            base.LogMessage.InsertLog(logText.ToString(), "CloseAccount", "AccountWorkFlow.cs");
            return result;
        }

        private void EntityToModelMapping(Account account, AccountUpdateDto accountUpdateDto)
        {
            if (!string.IsNullOrEmpty(accountUpdateDto.Iban))
                account.Iban = accountUpdateDto.Iban;
            if (accountUpdateDto.BranchId.HasValue)
                account.BranchId = accountUpdateDto.BranchId.Value;
            if (accountUpdateDto.CustomerId.HasValue)
                account.CustomerId = accountUpdateDto.CustomerId.Value;
            if (accountUpdateDto.CurrencyUnitId.HasValue)
                account.CurrencyUnitId = accountUpdateDto.CurrencyUnitId.Value;
            account.AccountName = accountUpdateDto.AccountName == string.Empty? null : accountUpdateDto.AccountName;
            if (!string.IsNullOrEmpty(accountUpdateDto.AccountNumber))
                account.AccountNumber = accountUpdateDto.AccountNumber;
            if (accountUpdateDto.IsActive.HasValue)
                account.IsActive = accountUpdateDto.IsActive.Value;
            if (accountUpdateDto.TypeId.HasValue)
                account.TypeId = accountUpdateDto.TypeId.Value;
        }

        private string CreateAccountNumber()
        {
            Random random = new Random();
            return random.Next(10000000).ToString().PadRight(7,'0');
        }
        private string CreateIban(string accountNumber, string branchNumber)
        {
            Random random = new Random();
            return $"TR{random.Next(100).ToString().PadLeft(2,'0')}000620{(!string.IsNullOrEmpty(branchNumber) ? branchNumber.PadLeft(5, '0') :"".PadLeft(5,'0'))}{accountNumber.PadLeft(11,'0')}";
        }
    }
}
