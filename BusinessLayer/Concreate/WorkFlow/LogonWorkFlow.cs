using BusinessLayer.Abstrack;
using BusinessLayer.Validation;
using CoreLayer.BusinessLayer.Concreate;
using CoreLayer.BusinessLayer.Model;
using CoreLayer.Security;
using CoreLayer.Utilities.Aspect;
using CoreLayer.Utilities.Enum;
using CoreLayer.Utilities.Ioc;
using CoreLayer.Utilities.Messages;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using DataAccessLayer.Abstract;
using EntityLayer.Models.EntityFremework;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DataAccessLayer.Concreate;
using BusinessLayer.Concreate.Component;
using BusinessLayer.Dto;

namespace BusinessLayer.Concreate.WorkFlow
{
    public class LogonWorkFlow : LogonComponen , ILogonWorkFlow
    {
        #region Variables
        private readonly IUnitOfWork _unow;
        #endregion

        #region constructor
        public LogonWorkFlow(IUnitOfWork unow):base(unow)
        {
            this._unow = unow;
        }
        #endregion
        public async Task<IResult<TokenResponseDto>> Logon(TokenRequestDto tokenModel)
        {

            IResult<TokenResponseDto> result = ResultInjection.Result<TokenResponseDto>();
            result.ResultObje = new TokenResponseDto();
            string saltPassword = Hashing.CreateSHA512(tokenModel.Password);
            string logMessage = string.Empty;
            var claimResult = await base.GetUserPassword(tokenModel.User, saltPassword);
            if (claimResult.ResultStatus)
            {
                logMessage += "Kullanıcı adı şifre doğru";
                string token = this.CretateToken(claimResult.ResultObje);
                if (!string.IsNullOrEmpty(token))
                {
                    logMessage = "Token oluşturma başarılı.";
                    result.ResultObje.AccessToken = token;
                    result.ResultObje.RefreshToken = Hashing.CreateSHA512($"{token}{base.Configuration["TokenKey"]}");
                    result.SetTrue();
                }
            }
            else
            {
                result.SetFalse();
                result.ResultInnerMessage = "Kullanıcı adı veya şifre hatalı.";
                logMessage = "Kullanıcı adı veya şifre hatalı.";
            }

            base.LogMessage.InsertLog(logMessage, "Logon", "LogonWorkFlow.cs");
            return result;
        }

        public IResult<TokenResponseDto> RefreshToken(RefreshTokenRequestDto model)
        {
            IResult<TokenResponseDto> result = ResultInjection.Result<TokenResponseDto>();
            result.ResultObje = new TokenResponseDto();
            StringBuilder logText = new StringBuilder();
            logText.AppendLine($"RefreshToken işlemi başladı.");
            if (model.RefreshToken == Hashing.CreateSHA512($"{model.AccessToken}{base.Configuration["TokenKey"]}"))
            {
                logText.AppendLine("RefreshToken doğrulandı.");
                JwtSecurityTokenHandler toJwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken toSecurityToken = toJwtSecurityTokenHandler.ReadJwtToken(model.AccessToken);
                string refreshTokenExTime = base.Configuration["RefreshTokenExTime"];
                double extTime;
                double.TryParse(refreshTokenExTime, out extTime);
                if (toSecurityToken.IssuedAt.ToLocalTime().AddMinutes(extTime) >= DateTime.Now)
                {
                    var claims = toSecurityToken.Claims.ToList();
                    string key = base.Configuration["TokenKey"];
                    result.ResultObje.AccessToken = base.helperWorkFlow.CreateToken(claims: claims, key: key, IssuedAt: toSecurityToken.IssuedAt);
                    result.ResultObje.RefreshToken = Hashing.CreateSHA512($"{result.ResultObje.AccessToken}{base.Configuration["TokenKey"]}");
                }
                else
                    logText.AppendLine("RefreshToken süresi sonlanmış.");
            }
            else
                logText.AppendLine("RefreshToken doğrulanamadı.");
            return result;
        }

        #region TokenOperation
        string CretateToken(ClaimDto claim, DateTime? IssuedAt = null)
        {
            string key = base.Configuration["TokenKey"];
            List<Claim> claims = new List<Claim>()
            {
                        new Claim(ClaimTypes.Name,claim.Name),
                        new Claim(ClaimTypes.Surname,claim.Surname),
                        new Claim("Id",claim.Id.ToString()),
                        new Claim("Photo",claim.Photo),
                        new Claim("CustomerNumber",claim.CustomerNumber)
            };
            foreach (var item in claim.RoleList)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }
            return base.helperWorkFlow.CreateToken(claims: claims,key: key);
        }
        #endregion
    }
}
