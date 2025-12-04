using KASHOP.DAL.Data;
using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responce;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using KASHOP.PL.Resources;
using Mapster;
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

        public ICategoryRepository _categoryRepository { get; }

        public CategoriesController( IStringLocalizer<SharedResource> localizer,ICategoryRepository categoryRepository)
        {
            
            _localizer = localizer;
            _categoryRepository = categoryRepository;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetALL();

            var response = categories.Adapt<List<CategoryResponce>>();

            return Ok(new
            {
                message = _localizer["Success"].Value,
                categories = response
            });
        }

      
         [HttpPost("")]

       public IActionResult Create(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            _categoryRepository.Create(category);
            return Ok(new { message = _localizer["Success"].Value });
        }



    }
}
