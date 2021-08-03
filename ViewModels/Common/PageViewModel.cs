using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNShopSolution.ViewModels.Common
{
    public class PageViewModel<T>
    {
        public List<T> Items { get; set; }
        public int TotalRecord { set; get; }
    }
}
