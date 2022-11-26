using ECommerceProject.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceProject.Interfaces
{
    public interface ISmallCartViewComponent
    {
       Task<IViewComponentResult> InvokeAsync();
    }
}
