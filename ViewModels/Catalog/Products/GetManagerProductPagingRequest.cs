using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNShopSolution.ViewModels.Common;

namespace TNShopSolution.ViewModels.Catalog.Products
{
    public class GetManagerProductPagingRequest: PagingRequestBase
    {
        public string Keywork { get; set; }
        public List<int> CategoryIDs { get; set; }
    }
}
