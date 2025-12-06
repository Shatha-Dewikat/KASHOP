using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.DTO.Responce
{
    public class RegisterResponce

    {
      
        public bool Success { get; set; }



         public string Message { get; set; }

        public List<string>? Errors { get; set; }


    }
}
