﻿using CoreLayer.DataAccess.Abstrack;
using CoreLayer.DataAccess.Concrete.Repository;
using EntityLayer.Models.Mongo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Concreate
{
    public class InformationLogRepository : MongoLogRepository<InformationLog>, ILogRepository
    {
    }
}
