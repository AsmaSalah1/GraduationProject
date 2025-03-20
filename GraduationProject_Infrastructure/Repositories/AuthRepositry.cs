using GraduationProject_Core.Dtos.Auth;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Infrastructure.Repositories
{
	public class AuthRepositry : IAuthRepositry
	{
		private readonly UserManager<User> userManager;
		private readonly IConfiguration configuration;
		private readonly SignInManager<User> signInManager;
		private readonly ApplicationDbContext dbContext;

		public AuthRepositry(UserManager<User> userManager , IConfiguration configuration
			, SignInManager<User> signInManager , ApplicationDbContext dbContext)
		{
			this.userManager = userManager;
			this.configuration = configuration;
			this.signInManager = signInManager;
			this.dbContext = dbContext;
		}

		public async Task<string> RegisterAsync(User user, string password)
		{
			var result = await userManager.CreateAsync(user,password);
			if (result.Succeeded) {
				await userManager.AddToRoleAsync(user, "User");
				var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
			//	var token = CreateToken(user);
				//var encodedToken = WebUtility.UrlEncode(token);
				//var encodedEmail = WebUtility.UrlEncode(user.Email);
				Console.WriteLine("Generated Token: " + token); // اختبار وإظهار التوكن في الـ console أو الـ logs

				var ConfirmEmailURL = $"https://localhost:7024/Auths/confirm-email?token={token}&email={user.Email}";

				var email = new Email()
				{
					Subject = "Confirm your email",
					Recivers = user.Email,
					Body = $"Confirm your email by Click here {ConfirmEmailURL}",
				};
				EmailHealper.SendEmail(email);
				return "User registered successfully! Please check your email to confirm your account.";
			}
			var errorMessage = result.Errors.Select(e=>e.Description).ToList();
			return string.Join(',', errorMessage);
		}

		public async Task<string> ConfirmEmail(string email, string token)
		{
			var user = await userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return "Invalid email";
			}
			var result = await userManager.ConfirmEmailAsync(user, token);
			if (result.Succeeded)
			{
				return "Email confirmed successfully!";
			}
			return "Invalid token or email.";
		}

		public async Task<string> LogInAsync(string email, string password)
		{
			var user = await userManager.FindByEmailAsync(email);
			if (user == null)
			{
				return "Invalid user name or password";
			}
			var result = await signInManager.PasswordSignInAsync(user, password, false, false);
			if (!result.Succeeded)
			{
				return null;
			}
			return CreateToken(user);
		}

		public async Task<string> ForgetPassword(string Useremail)
		{
			// 1. التحقق من صلاحية البريد الإلكتروني (البريد الإلكتروني صالح إذا كان له تنسيق صحيح)
			var isValidEmail = new EmailAddressAttribute().IsValid(Useremail);
			if (!isValidEmail)
			{
				return "Invalid email format"; // إرجاع رسالة في حال كان التنسيق غير صحيح
			}
			var user = await userManager.FindByEmailAsync(Useremail);
			if(user is null)
			{
				return "Email not registered";
			}
			//ابعث توكن عشان التشفير و اعرف الايميل لاي يوزر
			var token = await userManager.GeneratePasswordResetTokenAsync(user);
			//var resetPasswordLink = $"https://localhost:7024/Auths/reset-password?token={token}&email={Useremail}";
			var resetPasswordLink = $"https://yourfrontend.com/reset-password?token={token}&email={Useremail}";

			var email = new Email()
			{
				Subject = "Reset Password",
				Recivers = Useremail,
				Body = $"Hello,\n\nYou requested to reset your password. Please click the link below to proceed:\n\n" +
		               $"{resetPasswordLink}\n\nIf you did not request this, please ignore this email.\n\nThanks.",
			};
			EmailHealper.SendEmail(email);
			return "Email Sent successfully";
		}

		public async Task<string> ResetPassword(string email, string password, string token)
		{
			var user2 = await userManager.FindByEmailAsync(email);

			//اذا موجود بعمل ابديت للباسوورد - الفنكشن جاهز ما بحتاج اعمل اشي -- 
			if (user2 is not null)
			{
				var result = await userManager.ResetPasswordAsync(user2, token, password);
				if (result.Succeeded)
				{
					return "Password has been successfully reset.";
				}
				// 🔴 التحقق من الخطأ المتعلق بانتهاء صلاحية التوكن
				if (result.Errors.Any(e => e.Code == "InvalidToken"))
				{
					return "The reset password link has expired or is invalid. Please request a new one.";
				}
			}

			return "There is an error";
		}

		private string CreateToken(User user)
		{
			var claims = new[]
			{
			new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
			// = new Claim (ClaimTypes.GivenName,user.UserName),
			new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
		};
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
			//=	var key2 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("jwt")["key"]));
			var crad = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var token = new JwtSecurityToken(
				configuration["jwt:audience"],
				configuration["jwt:issure"],
				claims,
				signingCredentials: crad,
				expires: DateTime.UtcNow.AddDays(14)
				);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public async Task<string> ChangePassword(ChangePasswordDtos dtos, int userId)
		{
			var user = await userManager.FindByIdAsync(userId.ToString());
			var user2=await dbContext.Users.FirstOrDefaultAsync(x => x.Id == userId);	
			if(user2 == null)
			{
				return "Invalid user";
			}
			var result = await userManager.ChangePasswordAsync(user,dtos.OldPassword, dtos.NewPassword);
			if (!result.Succeeded)
			{
				return string.Join(", ", result.Errors.Select(e => e.Description)); // إرجاع الأخطاء
 			}
				return "Password changed successfully.";
		}
	}
}
