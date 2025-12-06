using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responce;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.LLB.Service
{
    public interface ICategoryService
    {
         List<CategoryResponce> GetAllCategories();

        CategoryResponce CreateCategory(CategoryRequest Request);

    }
}
