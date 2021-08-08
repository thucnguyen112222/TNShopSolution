using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNShopSolution.ViewModels.Common
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {
        public ApiSuccessResult()
        {
            IsSuccessed = true;
        }

        public ApiSuccessResult(T resultObj) 
        {
            IsSuccessed = true;
            ResultObject = resultObj;
        }
    }
}
