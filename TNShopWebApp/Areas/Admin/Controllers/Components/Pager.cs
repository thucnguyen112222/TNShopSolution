using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TNShopSolution.ViewModels.Common;

namespace TNShopWebApp.Areas.Admin.Controllers.Components
{
    [ViewComponent]
    public class Pager : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PageResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }
}
