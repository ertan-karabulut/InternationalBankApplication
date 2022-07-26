using BusinessLayer.Dto;
using CoreLayer.BusinessLayer.Model;
using CoreLayer.Utilities.Result.Abstrack;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Abstrack
{
    public interface ILogonWorkFlow
    {
        Task<IResult<TokenResponseDto>> Logon(TokenRequestDto tokenModel);
        IResult<TokenResponseDto> RefreshToken(RefreshTokenRequestDto model);
    }
}
