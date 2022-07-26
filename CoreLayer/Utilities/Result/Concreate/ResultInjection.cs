using CoreLayer.Utilities.Result.Abstrack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreLayer.Utilities.Result.Concreate
{
    public class ResultInjection
    {
        //buraya hangi IResult verilirse projenin tümünde o result kullanılır.
        public static IResult<T> Result<T>()
        {
            Result<T> result = new Result<T>();
            result.SetFalse();
            return result;
        }
        public static IResult Result()
        {
            Result result = new Result();
            result.SetFalse();
            return result;
        }
    }
}
