﻿using CoreLayer.DataAccess.Abstrack;
using CoreLayer.DataAccess.Concrete;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using DataAccessLayer.Abstract;
using EntityLayer.Models.EntityFremework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DataAccessLayer.Concreate
{
    public class CityRepository : RepositoryBase<City>, ICityRepository
    {
        public CityRepository(MyBankContext context) : base(context)
        {
        }
    }
}
