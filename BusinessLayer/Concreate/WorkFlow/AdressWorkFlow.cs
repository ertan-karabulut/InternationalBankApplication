using AutoMapper;
using BusinessLayer.Abstrack;
using BusinessLayer.Concreate.Component;
using BusinessLayer.Dto;
using BusinessLayer.Dto.Adress;
using BusinessLayer.Validation;
using BusinessLayer.Validation.AdressValidation;
using CoreLayer.DataAccess.Concrete.DataRequest;
using CoreLayer.Utilities.Aspect;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using DataAccessLayer.Abstract;
using EntityLayer.Models.EntityFremework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concreate.WorkFlow
{
    public class AdressWorkFlow : AdressComponent, IAdressWorkFlow
    {
        #region Variables
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unow;
        #endregion

        #region Constructor
        public AdressWorkFlow(IUnitOfWork unow, IMapper mapper) : base(unow)
        {
            this._mapper = mapper;
            this._unow = unow;
        }
        #endregion

        [ValidatorAspect(typeof(DataValidation))]
        public async Task<IResult<DataResponse<AdressDetailDto>>> GetAdressDetail(DataRequest request)
        {
            IResult<DataResponse<AdressDetailDto>> result = ResultInjection.Result<DataResponse<AdressDetailDto>>();

            if (!request.Filter.Any(x => x.Name == "Id"))
                return result;

            var adress = await base.GetAdressDetailComponent(request);

            if (adress.ResultStatus)
            {
                result.ResultObje = new DataResponse<AdressDetailDto>();
                result.ResultObje.Data = new AdressDetailDto();
                result.ResultObje.Data.Adress = this._mapper.Map<AdressDto>(adress.ResultObje.Data);


                var countryList = await this.GetCountryDropDownList(true);
                if (countryList.ResultStatus)
                {
                    countryList.ResultObje.ForEach(x => x.Selected = int.Parse(x.Value) == result.ResultObje.Data.Adress.CountryId);
                    result.ResultObje.Data.CountrySelectList = countryList.ResultObje.OrderByDescending(x => x.Selected).ToList();
                }

                var cityList = await this.GetCityDropDownList(true, result.ResultObje.Data.Adress.CountryId);

                if (cityList.ResultStatus)
                {
                    cityList.ResultObje.ForEach(x => x.Selected = int.Parse(x.Value) == result.ResultObje.Data.Adress.CityId);
                    result.ResultObje.Data.CitySelectList = cityList.ResultObje.OrderByDescending(x => x.Selected).ToList();
                }

                var districtList = await this.GetDistrictDropDownList(true, result.ResultObje.Data.Adress.CityId);

                if (districtList.ResultStatus)
                {
                    districtList.ResultObje.ForEach(x => x.Selected = int.Parse(x.Value) == result.ResultObje.Data.Adress.DistrictId);
                    result.ResultObje.Data.DistrictSelectList = districtList.ResultObje.OrderByDescending(x => x.Selected).ToList();
                }

                result.SetTrue();
            }

            return result;
        }

        [CacheAspect]
        public async Task<IResult<List<SelectListItemDto>>> GetCountryDropDownList(bool? IsActive)
        {
            IResult<List<SelectListItemDto>> result = ResultInjection.Result<List<SelectListItemDto>>();

            var cityList = await base.GetCountryListComponent(IsActive);
            if (cityList.ResultStatus)
            {
                result.ResultObje = cityList.ResultObje.Select(x => new SelectListItemDto { Text = x.CountryName, Value = x.Id.ToString(),Selected = x.CountryCode=="TR" }).ToList();
                result.SetTrue();
            }
            return result;
        }

        [CacheAspect]
        public async Task<IResult<List<SelectListItemDto>>> GetCityDropDownList(bool? IsActive, int CountryId)
        {
            IResult<List<SelectListItemDto>> result = ResultInjection.Result<List<SelectListItemDto>>();

            var cityList = await base.GetCityListComponent(CountryId: CountryId, IsActive: IsActive);
            if (cityList.ResultStatus)
            {
                result.ResultObje = cityList.ResultObje.Select(x => new SelectListItemDto { Text = x.CityName, Value = x.Id.ToString() }).ToList();
                result.SetTrue();
            }
            return result;
        }

        [CacheAspect]
        public async Task<IResult<List<SelectListItemDto>>> GetDistrictDropDownList(bool? IsActive, int CityId)
        {
            IResult<List<SelectListItemDto>> result = ResultInjection.Result<List<SelectListItemDto>>();

            var cityList = await base.GetDistrictListComponent(CityId: CityId, IsActive: IsActive);
            if (cityList.ResultStatus)
            {
                result.ResultObje = cityList.ResultObje.Select(x => new SelectListItemDto { Text = x.DistrictName, Value = x.Id.ToString() }).ToList();
                result.SetTrue();
            }
            return result;
        }

        [ValidatorAspect(typeof(UpdateAdressValidation))]
        public async Task<IResult> UpdateAdress(AdressUpdateDto adress)
        {
            this._unow.BeginTransaction();
            if (adress.IsFavorite.HasValue)
            {
                var favoriteAdress = await this._unow.AdressRespository.Get(x => x.CustomerId == adress.CustomerId && x.IsFavorite == adress.IsFavorite.Value).FirstOrDefaultAsync();
                if (favoriteAdress != null && favoriteAdress.Id != adress.Id)
                {
                    favoriteAdress.IsFavorite = false;
                    this._unow.AdressRespository.Update(favoriteAdress);
                }
            }
            var adressUpdate = await base.UpdateAdressComponent(adress);
            if (adressUpdate.ResultStatus)
                await this._unow.CommitAsync();
            else
                await this._unow.RollbackAsync();
            return adressUpdate;
        }

        [ValidatorAspect(typeof(DataTableValidation))]
        public async Task<IResult<DataTableResponse<AdressDto>>> CustomerAdressList(DataTableRequest request)
        {
            IResult<DataTableResponse<AdressDto>> result = ResultInjection.Result<DataTableResponse<AdressDto>>();

            int customerId = base.helperWorkFlow.GetUserId();

            if (!request.Filter.Any(x => x.Name == "CustomerId"))
                request.Filter.Add(new NameValuePair { Name = "CustomerId", Value = customerId.ToString() });

            if (!request.Filter.Any(x => x.Name == "IsActive"))
                request.Filter.Add(new NameValuePair { Name = "IsActive", Value = "True" });

            if (request.Order.Column != "IsFavorite")
            {
                request.Order.Column = "IsFavorite";
                request.Order.Short = "desc";
            }

            var adress = await base.CustomerAdressListComponent(request);
            if (adress.ResultStatus)
            {
                result.ResultObje = new DataTableResponse<AdressDto>();

                result.ResultObje.Data = this._mapper.Map<List<AdressDto>>(adress.ResultObje.Data);
                result.ResultObje.TotalCount = adress.ResultObje.TotalCount;
                result.SetTrue();
            }
            return result;
        }

        [ValidatorAspect(typeof(UpdateAdressValidation))]
        public async Task<IResult> DeleteAdress(AdressUpdateDto adress)
        {
            IResult result = ResultInjection.Result();
            bool isAdress = this._unow.AdressRespository.Get(x => true).Any(x => x.CustomerId == adress.CustomerId && x.IsActive);

            if (isAdress)
            {
                result = await base.DeleteAdressComponent(adress);
            }

            return result;
        }

        [ValidatorAspect(typeof(CreateAdressValidation))]
        public async Task<IResult<AdressDto>> AddAdress(AdressCreateDto adress)
        {
            IResult<AdressDto> result = ResultInjection.Result<AdressDto>();
            adress.CustomerId = base.helperWorkFlow.GetUserId();

            if (adress.IsFavorite)
            {
                var favoriteAdress = await this._unow.AdressRespository.Get(x => x.CustomerId == adress.CustomerId && x.IsActive && x.IsFavorite).FirstOrDefaultAsync();
                if (favoriteAdress != null)
                {
                    favoriteAdress.IsFavorite = false;
                    if (!(this._unow.AdressRespository.Update(favoriteAdress).ResultStatus && await this._unow.SaveChangesAsync()))
                    {
                        result.SetFalse();
                    }
                }
            }

            var adressEntity = this._mapper.Map<Adress>(adress);
            var adressAdd = await base.AddAdressComponent(adressEntity);
            if (adressAdd.ResultStatus)
            {
                result.SetTrue();
                result.ResultObje = this._mapper.Map<AdressDto>(adressAdd.ResultObje);
            }
            return result;
        }
    }
}
