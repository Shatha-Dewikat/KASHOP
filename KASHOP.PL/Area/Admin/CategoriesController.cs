using KASHOP.DAL.DTO.Request;
using KASHOP.LLB.Service;
using KASHOP.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace KASHOP.PL.Area.Admin
{
    [ApiController]
    [Route("api/admin/categories")]
    [Authorize] 
    public class AdminCategoriesController : ControllerBase
    {
        private readonly ICategoryService _category;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public AdminCategoriesController(
            ICategoryService category,
            IStringLocalizer<SharedResource> localizer)
        {
            _category = category;
            _localizer = localizer;
        }

        
        [HttpPost]
        public IActionResult Create(CategoryRequest request)
        {
            var response = _category.CreateCategory(request);

            return Ok(new
            {
                message = _localizer["Success"].Value
            });
        }
    }

}
