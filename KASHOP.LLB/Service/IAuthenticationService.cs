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
        Task<LoginResponse> LoginAsync(LoginRequest request);

        Task<RegisterResponce> RegisterAsync(RegisterRequest registerRequest);
        Task<string> ConfirmEmailAsync(string email, string token);
        Task<ForgotPasswordResponse> RequestPasswordReset(ForgotPasswordRequest request);
        Task<ForgotPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
    }
}
