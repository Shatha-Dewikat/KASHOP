using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO.Responce
{
    public class CategoryResponce
    {
        public Status Status { get; set; }

        public List<CategoryTranslationResponse> Translations { get; set; }
    }
}
