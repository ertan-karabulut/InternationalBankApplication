using BusinessLayer.Dto.Mail;
using CoreLayer.BusinessLayer.Concreate;
using CoreLayer.DataAccess.Concrete.DataRequest;
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

namespace BusinessLayer.Concreate.Component
{
    public class MailComponent : ComponentBase
    {
        #region Variables
        private readonly IUnitOfWork _unow;
        #endregion
        #region Constructor
        public MailComponent(IUnitOfWork unow)
        {
            this._unow = unow;
        }
        #endregion
        #region protected method
        protected async Task<IResult<DataTableResponse<EMail>>> CustomerMailListComponent(DataTableRequest request)
        {
            var result = ResultInjection.Result<DataTableResponse<EMail>>();
            result.ResultObje = new DataTableResponse<EMail>();
            var query = this._unow.MailRespository.Get(x => true);
            query = SetFilter(request.Filter, query);
            query = SetOrder(request.Order, query);

            int count = await query.CountAsync();
            if (count > 0)
            {
                var data = await query.Skip(request.Skip).Take(request.Take).AsQueryable().ToListAsync();

                result.ResultObje.Data = data;
                result.ResultObje.TotalCount = count;
                result.SetTrue();
            }
            return result;
        }

        protected async Task<IResult> UpdateMailComponent(MailUpdateDto mail)
        {
            IResult result = ResultInjection.Result();
            var mailEntity = await this._unow.MailRespository.Get(x => x.Id == mail.Id).FirstOrDefaultAsync();

            if (mailEntity != null)
            {
                this.MailUpdateMapping(mailEntity, mail);
                if (this._unow.MailRespository.Update(mailEntity).ResultStatus && await this._unow.SaveChangesAsync())
                    result.SetTrue();
            }

            return result;
        }

        protected async Task<IResult> DeleteMailComponent(MailUpdateDto mail)
        {
            IResult result = ResultInjection.Result();
            var mailEntity = await this._unow.MailRespository.Get(x => x.Id == mail.Id).FirstOrDefaultAsync();

            if (mailEntity != null)
            {
                mailEntity.IsActive = false;
                var update = this._unow.MailRespository.Update(mailEntity);
                if (update.ResultStatus && await this._unow.SaveChangesAsync())
                    result.SetTrue();
            }

            return result;
        }

        protected async Task<IResult<EMail>> AddMailComponent(EMail mail)
        {
            var result = ResultInjection.Result<EMail>();
            var addResult = await this._unow.MailRespository.AddAsync(mail);
            if (addResult.ResultStatus && await this._unow.SaveChangesAsync())
            {
                result.ResultObje = addResult.ResultObje;
                result.SetTrue();
            }

            return result;
        }
        #endregion

        #region private method
        private IQueryable<EMail> SetOrder(OrderModel order, IQueryable<EMail> query)
        {
            switch (order.Column)
            {
                case "EMail":
                    if (order.Short.ToUpper() == "DESC")
                        query = query.OrderByDescending(x => x.EMail1);
                    else
                        query = query.OrderBy(x => x.EMail1);
                    break;
                case "CustomerId":
                    if (order.Short.ToUpper() == "DESC")
                        query = query.OrderByDescending(x => x.CustomerId);
                    else
                        query = query.OrderBy(x => x.CustomerId);
                    break;
                case "IsFavorite":
                    if (order.Short.ToUpper() == "DESC")
                        query = query.OrderByDescending(x => x.IsFavorite);
                    else
                        query = query.OrderBy(x => x.IsFavorite);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreateDate);
                    break;
            }
            return query;
        }
        private IQueryable<EMail> SetFilter(List<NameValuePair> valuePairs, IQueryable<EMail> query)
        {
            if (valuePairs.Any(x => x.Name == "Id") && valuePairs.FirstOrDefault(x => x.Name == "Id").Value != null)
            {
                string value = valuePairs.FirstOrDefault(x => x.Name == "Id").Value;
                int Id;
                int.TryParse(value, out Id);
                query = query.Where(x => x.Id == Id);
            }
            if (valuePairs.Any(x => x.Name == "CustomerId") && valuePairs.FirstOrDefault(x => x.Name == "CustomerId").Value != null)
            {
                string value = valuePairs.FirstOrDefault(x => x.Name == "CustomerId").Value;
                int CustomerId;
                int.TryParse(value, out CustomerId);
                query = query.Where(x => x.CustomerId == CustomerId);
            }
            if (valuePairs.Any(x => x.Name == "EMail") && valuePairs.FirstOrDefault(x => x.Name == "EMail").Value != null)
            {
                string value = valuePairs.FirstOrDefault(x => x.Name == "EMail").Value;
                query = query.Where(x => value.Contains(x.EMail1));
            }
            if (valuePairs.Any(x => x.Name == "IsActive") && valuePairs.FirstOrDefault(x => x.Name == "IsActive").Value != null)
            {
                string value = valuePairs.FirstOrDefault(x => x.Name == "IsActive").Value;
                bool IsActive;
                bool.TryParse(value, out IsActive);
                query = query.Where(x => x.IsActive == IsActive);
            }

            return query;
        }
        private void MailUpdateMapping(EMail mail, MailUpdateDto updateDto)
        {
            if (!string.IsNullOrEmpty(updateDto.EMail))
                mail.EMail1 = updateDto.EMail;
            if (updateDto.IsFavorite.HasValue)
                mail.IsFavorite = updateDto.IsFavorite.Value;
            if (updateDto.IsActive.HasValue)
                mail.IsActive = updateDto.IsActive.Value;
        }
        #endregion
    }
}
