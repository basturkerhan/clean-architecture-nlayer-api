using Microsoft.AspNetCore.Mvc;
using NLayerApp.Core.DTOs.ResponseDTOs;

namespace NLayerApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
        {
            return (response.StatusCode) switch
            {
                204 => new ObjectResult(null) { StatusCode = response.StatusCode },
                _ => new ObjectResult(response) { StatusCode = response.StatusCode }
            };
        }
    }
}
