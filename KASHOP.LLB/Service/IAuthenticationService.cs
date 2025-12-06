using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responce;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace KASHOP.LLB.Service
{
    public interface IAuthenticationService
    {
        Task<RegisterResponce> RegisterAsync(RegisterRequest registerRequest);
    }
}
