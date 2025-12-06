using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responce;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.LLB.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryReposiroty;

        public CategoryService(ICategoryRepository categoryReposiroty)
        {
            _categoryReposiroty = categoryReposiroty;
        }

        public CategoryResponce CreateCategory(CategoryRequest Request)
        {
            var category = Request.Adapt<Category>();
            _categoryReposiroty.Create(category);

            return category.Adapt<CategoryResponce>();
        }

        public List<CategoryResponce> GetAllCategories()
        {
            var categories = _categoryReposiroty.GetALL();

            var response = categories.Adapt<List<CategoryResponce>>();
            return response;
        }

    }
}
