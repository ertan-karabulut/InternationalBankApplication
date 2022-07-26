using BusinessLayer.Dto;
using BusinessLayer.Dto.Account;
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
    public interface IAccountWorkFlow
    {
        #region Ekleme metodları
        Task<IResult<List<AccountDto>>> AddAsync(AccountCreatDtoList entity);
        Task<IResult<AccountDto>> AddAsync(AccountCreatDto entity);
        #endregion

        #region Güncelleme metodları
        Task<IResult> UpdateAsync(AccountUpdateDto entity);
        Task<IResult> UpdateAsync(AccountUpdateDtoList entity);
        Task<IResult> CloseAccount(AccountUpdateDto account);
        #endregion

        #region Silme medotları
        Task<IResult> DeleteAsync(AccountUpdateDtoList entity);
        Task<IResult> DeleteAsync(AccountUpdateDto entity);
        #endregion
        Task<IResult<DataTableResponse<MyAccountDto>>> GetMyAccountList(DataTableRequest request);
        Task<IResult<AccountDetailDto>> AccountDetail(int accountId);
        Task<IResult<DataTableResponse<AccountHistoryDto>>> AccountHistory(DataTableRequest request);
    }
}
