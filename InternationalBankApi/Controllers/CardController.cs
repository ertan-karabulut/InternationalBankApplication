﻿using BusinessLayer.Abstrack;
using CoreLayer.DataAccess.Concrete.DataRequest;
using InternationalBankApi.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace InternationalBankApi.Controllers
{
    public class CardController : BaseController
    {
        #region Variables
        private readonly ICardWorkFlow _cardWorkFlow;
        #endregion
        #region Constructor
        public CardController(ICardWorkFlow cardWorkFlow)
        {
            this._cardWorkFlow = cardWorkFlow;
        }
        #endregion

        [HttpPost, Route("CustomerCrediCardList"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerCrediCardList([FromBody] DataRequestBase request)
        {
            var result = await this._cardWorkFlow.CustomerCrediCardList(request);
            return Ok(result);
        }

        [HttpPost, Route("CustomerAtmCardList"), Authorize(Roles = "Customer")]
        public async Task<IActionResult> CustomerAtmCardList([FromBody] DataRequestBase request)
        {
            var result = await this._cardWorkFlow.CustomerAtmCardList(request);
            return Ok(result);
        }
    }
}
