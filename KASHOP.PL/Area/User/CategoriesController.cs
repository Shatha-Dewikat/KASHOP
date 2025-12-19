using KASHOP.DAL.DTO.Request;
using KASHOP.LLB.Service;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Area.User
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _category;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public CategoriesController(
            ICategoryService category,
            IStringLocalizer<SharedResource> localizer)
        {
            _category = category;
            _localizer = localizer;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var response = _category.GetAllCategories();
            return Ok(new
            {
                message = _localizer["Success"].Value,
                response
            });
        }
    }


}
