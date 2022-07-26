﻿using BusinessLayer.Dto;
using CoreLayer.BusinessLayer.Concreate;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using DataAccessLayer.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concreate.Component
{
    public class LogonComponen : ComponentBase
    {
        #region Variables
        private readonly IUnitOfWork _unow;
        #endregion
        #region constructor
        public LogonComponen(IUnitOfWork unow)
        {
            this._unow = unow;
        }
        #endregion
        protected async Task<IResult<ClaimDto>> GetUserPassword(string customerNumber, string password)
        {
            IResult<ClaimDto> result = ResultInjection.Result<ClaimDto>();
            var query = this._unow.CustomerRepository.Get(x => true);
            query = query.Include(x => x.InternetPasswords).Include(x => x.CustomerRoles).ThenInclude(x => x.Role);
            query = query.Where(x => x.IsActive && (x.CustomerNumber == customerNumber || x.IdentityNumber == customerNumber) && x.InternetPasswords.Any(y => y.Password == password && y.IsActive));
            var data = await query.Select(x => new ClaimDto
            {
                Id = x.Id,
                Name = x.Name,
                Surname = x.Surname,
                Photo = x.Photo,
                RoleList = x.CustomerRoles.Where(x => x.IsActive && x.Role.IsActive).Select(x => x.Role.RoleName),
                CustomerNumber = x.CustomerNumber
            }).FirstOrDefaultAsync();

            if (data != null)
            {
                result.ResultObje = data;
                result.SetTrue();
            }

            return result;
        }
    }
}
