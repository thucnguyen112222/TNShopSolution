using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNShopSolution.ViewModels.Common
{
    public class ApiErrorResult<T> : ApiResult<T>
    {

        public string[] ValidationErrors { get; set; }
        public ApiErrorResult(string message)
        {
            IsSuccessed = false;
            Message = message;
        }
        public ApiErrorResult(string[] validationErrors)
        {
            IsSuccessed = false;
            ValidationErrors = validationErrors ?? throw new ArgumentNullException(nameof(validationErrors));
        }

        public ApiErrorResult()
        {
            IsSuccessed = false;
        }
    }
}
