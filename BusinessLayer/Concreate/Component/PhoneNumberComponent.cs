using BusinessLayer.Dto.PhpneNumber;
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
    public class PhoneNumberComponent : ComponentBase
    {
        #region Variables
        private readonly IUnitOfWork _unow;
        #endregion
        #region Constructor
        public PhoneNumberComponent(IUnitOfWork unow)
        {
            _unow = unow;
        }
        #endregion
        #region protected method
        protected async Task<IResult<DataTableResponse<PhoneNumber>>> CustomerPhoneNumberComponent(DataTableRequest request)
        {
            var result = ResultInjection.Result<DataTableResponse<PhoneNumber>>();
            result.ResultObje = new DataTableResponse<PhoneNumber>();
            var query = this._unow.PhoneNumberRepository.Get(x => true);
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

        protected async Task<IResult> UpdatePhoneNumberComponent(PhoneNumberUpdateDto phone)
        {
            IResult result = ResultInjection.Result();
            var phoneEntity = await this._unow.PhoneNumberRepository.Get(x => x.Id == phone.Id).FirstOrDefaultAsync();

            if (phoneEntity != null)
            {
                this.MailUpdateMapping(phoneEntity, phone);
                if (this._unow.PhoneNumberRepository.Update(phoneEntity).ResultStatus && await this._unow.SaveChangesAsync())
                    result.SetTrue();
            }

            return result;
        }
        protected async Task<IResult<PhoneNumber>> AddPhoneNumberComponent(PhoneNumber phone)
        {
            var result = ResultInjection.Result<PhoneNumber>();
            var addResult = await this._unow.PhoneNumberRepository.AddAsync(phone);
            if (addResult.ResultStatus && await this._unow.SaveChangesAsync())
            {
                result.ResultObje = addResult.ResultObje;
                result.SetTrue();
            }

            return result;
        }

        protected async Task<IResult> DeletePhoneNumberComponent(PhoneNumberUpdateDto phone)
        {
            IResult result = ResultInjection.Result();
            var phoneEntity = await this._unow.PhoneNumberRepository.Get(x => x.Id == phone.Id).FirstOrDefaultAsync();

            if (phoneEntity != null)
            {
                phoneEntity.IsActive = false;
                var update = this._unow.PhoneNumberRepository.Update(phoneEntity);
                if (update.ResultStatus && await this._unow.SaveChangesAsync())
                    result.SetTrue();
            }

            return result;
        }
        #endregion
        #region private method
        private IQueryable<PhoneNumber> SetOrder(OrderModel order, IQueryable<PhoneNumber> query)
        {
            switch (order.Column)
            {
                case "CustomerId":
                    if (order.Short.ToUpper() == "DESC")
                        query = query.OrderByDescending(x => x.CustomerId);
                    else
                        query = query.OrderBy(x => x.CustomerId);
                    break;
                case "Id":
                    if (order.Short.ToUpper() == "DESC")
                        query = query.OrderByDescending(x => x.Id);
                    else
                        query = query.OrderBy(x => x.Id);
                    break;
                case "IsFavorite":
                    if (order.Short.ToUpper() == "DESC")
                        query = query.OrderByDescending(x => x.IsFavorite);
                    else
                        query = query.OrderBy(x => x.IsFavorite);
                    break;
                case "NumberName":
                    if (order.Short.ToUpper() == "DESC")
                        query = query.OrderByDescending(x => x.NumberName);
                    else
                        query = query.OrderBy(x => x.NumberName);
                    break;
                case "PhoneNumber":
                    if (order.Short.ToUpper() == "DESC")
                        query = query.OrderByDescending(x => x.PhoneNumber1);
                    else
                        query = query.OrderBy(x => x.PhoneNumber1);
                    break;
                default:
                    query = query.OrderByDescending(x => x.CreateDate);
                    break;
            }
            return query;
        }
        private IQueryable<PhoneNumber> SetFilter(List<NameValuePair> valuePairs, IQueryable<PhoneNumber> query)
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
            if (valuePairs.Any(x => x.Name == "NumberName") && valuePairs.FirstOrDefault(x => x.Name == "NumberName").Value != null)
            {
                string value = valuePairs.FirstOrDefault(x => x.Name == "NumberName").Value;
                query = query.Where(x => value.Contains(x.NumberName));
            }
            if (valuePairs.Any(x => x.Name == "PhoneNumber") && valuePairs.FirstOrDefault(x => x.Name == "PhoneNumber").Value != null)
            {
                string value = valuePairs.FirstOrDefault(x => x.Name == "PhoneNumber").Value;
                query = query.Where(x => value.Contains(x.PhoneNumber1));
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
        private void MailUpdateMapping(PhoneNumber phone, PhoneNumberUpdateDto updateDto)
        {
            if (!string.IsNullOrEmpty(updateDto.PhoneNumber))
                phone.PhoneNumber1 = updateDto.PhoneNumber;
            phone.NumberName = updateDto.NumberName == string.Empty ? null: updateDto.NumberName;
            if (updateDto.IsFavorite.HasValue)
                phone.IsFavorite = updateDto.IsFavorite.Value;
            if (updateDto.IsActive.HasValue)
                phone.IsActive = updateDto.IsActive.Value;
        }
        #endregion
    }
}
