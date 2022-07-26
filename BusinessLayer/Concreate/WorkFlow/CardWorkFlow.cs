using AutoMapper;
using BusinessLayer.Abstrack;
using BusinessLayer.Concreate.Component;
using BusinessLayer.Dto.Card;
using BusinessLayer.Validation;
using CoreLayer.DataAccess.Concrete.DataRequest;
using CoreLayer.Utilities.Aspect;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concreate.WorkFlow
{
    public class CardWorkFlow : CardComponent, ICardWorkFlow
    {
        #region Variables
        private readonly IMapper _mapper;
        #endregion
        #region Constructor
        public CardWorkFlow(IUnitOfWork unow, IMapper mapper) : base(unow)
        {
            this._mapper = mapper;
        }
        #endregion

        public async Task<IResult<DataTableResponse<CreditCardDto>>> CustomerCrediCardList(DataRequestBase request)
        {
            var result = ResultInjection.Result<DataTableResponse<CreditCardDto>>();

            if (!request.Filter.Any(x => x.Name == "CustomerId"))
            {
                int customerId = base.helperWorkFlow.GetUserId();
                request.Filter.Add(new NameValuePair { Name = "CustomerId", Value = customerId.ToString() });
            }

            if (!request.Filter.Any(x => x.Name == "IsActive"))
                request.Filter.Add(new NameValuePair { Name = "IsActive", Value = "True" });

            var cardList = await base.CustomerCrediCardListComponent(request);
            if (cardList.ResultStatus)
            {
                result.ResultObje = new DataTableResponse<CreditCardDto>();

                result.ResultObje.Data = this._mapper.Map<List<CreditCardDto>>(cardList.ResultObje.Data);
                result.ResultObje.TotalCount = cardList.ResultObje.TotalCount;
                result.SetTrue();
            }

            return result;
        }

        public async Task<IResult<DataTableResponse<AtmCardDto>>> CustomerAtmCardList(DataRequestBase request)
        {
            var result = ResultInjection.Result<DataTableResponse<AtmCardDto>>();

            if (!request.Filter.Any(x => x.Name == "CustomerId"))
            {
                int customerId = base.helperWorkFlow.GetUserId();
                request.Filter.Add(new NameValuePair { Name = "CustomerId", Value = customerId.ToString() });
            }

            if (!request.Filter.Any(x => x.Name == "IsActive"))
                request.Filter.Add(new NameValuePair { Name = "IsActive", Value = "True" });

            var cardList = await base.CustomerAtmCardListComponent(request);
            if (cardList.ResultStatus)
            {
                result.ResultObje = new DataTableResponse<AtmCardDto>();

                result.ResultObje.Data = this._mapper.Map<List<AtmCardDto>>(cardList.ResultObje.Data);
                result.ResultObje.TotalCount = cardList.ResultObje.TotalCount;
                result.SetTrue();
            }

            return result;
        }
    }
}
