using KASHOP.DAL.DTO.Request;
using KASHOP.DAL.DTO.Responce;
using KASHOP.DAL.Models;
using KASHOP.LLB.Service;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Service
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            _emailSender = emailSender;
        }

        // ===================== LOGIN =====================
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                var result = await _signInManager.PasswordSignInAsync(
                    user,
                    request.Password,
                    false,
                    true
                );

                if (!result.Succeeded)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        Message = "Invalid email or password"
                    };
                }

                var token = await GenerateAccessToken(user);

                return new LoginResponse
                {
                    Success = true,
                    Message = "Login successful",
                    AccessToken = token
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse
                {
                    Success = false,
                    Message = "Unexpected error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // ===================== REGISTER =====================
        public async Task<RegisterResponce> RegisterAsync(RegisterRequest request)
        {
            try
            {
                var user = request.Adapt<ApplicationUser>();

                var result = await _userManager.CreateAsync(user, request.Password);

                if (!result.Succeeded)
                {
                    return new RegisterResponce
                    {
                        Success = false,
                        Message = "Registration failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                await _userManager.AddToRoleAsync(user, "User");

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var confirmUrl =
                    $"https://localhost:7061/api/Account/confirmemail?email={user.Email}&token={Uri.EscapeDataString(token)}";

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Confirm Email",
                    $"<h3>Welcome {user.UserName}</h3>" +
                    $"<a href='{confirmUrl}'>Confirm Email</a>"
                );

                return new RegisterResponce
                {
                    Success = true,
                    Message = "Registration successful, check your email"
                };
            }
            catch (Exception ex)
            {
                return new RegisterResponce
                {
                    Success = false,
                    Message = "Unexpected error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // ===================== CONFIRM EMAIL =====================
        public async Task<string> ConfirmEmailAsync(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return "User not found";

            var result = await _userManager.ConfirmEmailAsync(user, token);
            return result.Succeeded ? "Email confirmed successfully" : "Email confirmation failed";
        }

        // ===================== FORGOT PASSWORD =====================
        public async Task<ForgotPasswordResponse> RequestPasswordReset(ForgotPasswordRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    return new ForgotPasswordResponse
                    {
                        Success = true,
                        Message = "If the email exists, a reset link was sent"
                    };
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                var resetUrl =
                    $"https://localhost:7061/api/Account/reset-password?email={user.Email}&token={Uri.EscapeDataString(token)}";

                await _emailSender.SendEmailAsync(
                    user.Email,
                    "Reset Password",
                    $"<p>Click below to reset your password:</p>" +
                    $"<a href='{resetUrl}'>Reset Password</a>"
                );

                return new ForgotPasswordResponse
                {
                    Success = true,
                    Message = "Password reset link sent"
                };
            }
            catch (Exception ex)
            {
                return new ForgotPasswordResponse
                {
                    Success = false,
                    Message = "Unexpected error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // ===================== RESET PASSWORD =====================
        public async Task<ForgotPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                if (user == null)
                {
                    return new ForgotPasswordResponse
                    {
                        Success = false,
                        Message = "Invalid request"
                    };
                }

                var result = await _userManager.ResetPasswordAsync(
                    user,
                    request.Token,
                    request.NewPassword
                );

                if (!result.Succeeded)
                {
                    return new ForgotPasswordResponse
                    {
                        Success = false,
                        Message = "Password reset failed",
                        Errors = result.Errors.Select(e => e.Description).ToList()
                    };
                }

                return new ForgotPasswordResponse
                {
                    Success = true,
                    Message = "Password reset successfully"
                };
            }
            catch (Exception ex)
            {
                return new ForgotPasswordResponse
                {
                    Success = false,
                    Message = "Unexpected error",
                    Errors = new List<string> { ex.Message }
                };
            }
        }

        // ===================== JWT =====================
        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var roles = await _userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!)
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
