using BusinessLayer.Dto.Card;
using CoreLayer.DataAccess.Concrete.DataRequest;
using CoreLayer.Utilities.Result.Abstrack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstrack
{
    public interface ICardWorkFlow
    {
        Task<IResult<DataTableResponse<CreditCardDto>>> CustomerCrediCardList(DataRequestBase request);
        Task<IResult<DataTableResponse<AtmCardDto>>> CustomerAtmCardList(DataRequestBase request);
    }
}
