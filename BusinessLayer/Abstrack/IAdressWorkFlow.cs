using BusinessLayer.Dto;
using BusinessLayer.Dto.Adress;
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
    public interface IAdressWorkFlow 
    {
        Task<IResult<DataTableResponse<AdressDto>>> CustomerAdressList(DataTableRequest request);
        Task<IResult<DataResponse<AdressDetailDto>>> GetAdressDetail(DataRequest request);
        Task<IResult<List<SelectListItemDto>>> GetCountryDropDownList(bool? IsActive);
        Task<IResult<List<SelectListItemDto>>> GetCityDropDownList(bool ?IsActive,int CountryId = 0);
        Task<IResult<List<SelectListItemDto>>> GetDistrictDropDownList(bool ?IsActive,int CityId = 0);
        Task<IResult> UpdateAdress(AdressUpdateDto adress);
        Task<IResult> DeleteAdress(AdressUpdateDto adress);
        Task<IResult<AdressDto>> AddAdress(AdressCreateDto adress);
    }
}
