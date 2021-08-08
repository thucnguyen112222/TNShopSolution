﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNShopSolution.ViewModels.Common
{
    public class ApiResult<T>
    {

        public  bool IsSuccessed { get; set; }
        public  string Message { get; set; }
        public T ResultObject { get; set; }
    }
}
