using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNShopSolution.Utilities.Utilities
{
    public class TNShopException : Exception
    {
        public TNShopException()
        {
        }

        public TNShopException(string message)
            : base(message)
        {
        }

        public TNShopException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
