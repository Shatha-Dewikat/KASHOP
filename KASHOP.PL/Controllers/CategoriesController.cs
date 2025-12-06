using KASHOP.DAL.Data;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responce;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using KASHOP.LLB.Service;
using KASHOP.PL.Resources;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Collections.Generic;

namespace KASHOP.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    

    public class CategoriesController : ControllerBase
    {
        
        private readonly IStringLocalizer<SharedResource> _localizer;

        public ICategoryService _categoryService { get; }

        public CategoriesController( IStringLocalizer<SharedResource> localizer, ICategoryService categoryRepository)
        {
            
            _localizer = localizer;
            _categoryService = categoryRepository;
        }

        [HttpGet("")]
        public IActionResult Index()
        {

            var response = _categoryService.GetAllCategories();
            return Ok(new
            {
                message = _localizer["Success"].Value,
                categories = response
            });
        }

      
         [HttpPost("")]
        [Authorize]
        public IActionResult Create(CategoryRequest request)
        {
            var responce = _categoryService.CreateCategory(request);

            return Ok(new { message = _localizer["Success"].Value });
        }



    }
}
