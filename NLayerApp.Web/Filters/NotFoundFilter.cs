using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NLayerApp.Core.DTOs;
using NLayerApp.Core.DTOs.ResponseDTOs;
using NLayerApp.Core.Entities;
using NLayerApp.Core.Services;

namespace NLayerApp.Web.Filters
{
    public class NotFoundFilter<Entity, Dto> : IAsyncActionFilter where Entity : BaseEntity where Dto : class
    {
        private readonly IService<Entity, Dto> _service;

        public NotFoundFilter(IService<Entity, Dto> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var idValue = context.ActionArguments["id"];
            if (idValue == null)
            {
                await next.Invoke();
                return;
            }

            var id = (int)idValue;
            CustomResponseDto<bool> response = await _service.AnyAsync(x => x.Id == id);

            if (response.Data)
            {
                await next.Invoke();
                return;
            }

            ErrorViewModel errorViewModel = new();
            errorViewModel.Errors?.Add($"{typeof(Entity).Name}({id}) not found! (reached notfoundfilter)");
            context.Result = new RedirectToActionResult("Error", "Home", errorViewModel);
        }
    }
}
