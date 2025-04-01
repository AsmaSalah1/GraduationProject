using GraduationProject_Core.Dtos.Auth;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
				var encodedToken = WebUtility.UrlEncode(token);
				var encodedEmail = WebUtility.UrlEncode(user.Email);
				//Console.WriteLine("Generated Token: " + token); // اختبار وإظهار التوكن في الـ console أو الـ logs

				var ConfirmEmailURL = $"https://localhost:7024/Auths/confirm-email?token={encodedToken}&email={encodedEmail}";

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
			return await CreateToken(user);
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
			if (user is null)
			{
				return "Email not registered";
			}

			//ابعث توكن عشان التشفير و اعرف الايميل لاي يوزر
			var token = await userManager.GeneratePasswordResetTokenAsync(user);
			//var resetPasswordLink = $"https://localhost:7024/Auths/reset-password?token={token}&email={Useremail}";
			var encodedToken = WebUtility.UrlEncode(token);
			var encodedEmail = WebUtility.UrlEncode(user.Email);
			var resetPasswordLink = $"https://yourfrontend.com/reset-password?token={token}&email={user.Email}";
			var email = new Email()
			{
				Subject = "Reset Password",
				Recivers = Useremail,
				Body = $"Hello,\n\nYou requested to reset your password. Please click the link below to proceed:\n\n" +
					   $"{resetPasswordLink}\n\nIf you did not request this, please ignore this email.\n\nThanks.",
			};
			EmailHealper.SendEmail(email);
			//	string[] ret=[encodedToken,encodedEmail];
			//return ret;
			return $"token={encodedToken}&email={encodedEmail}";
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
		private async Task<string> CreateToken(User user)
		{
			var roles = await userManager.GetRolesAsync(user);

			var claims = new List<Claim>
	{
		new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
		new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
	};

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				configuration["jwt:audience"],
				configuration["jwt:issure"],
				claims,
				expires: DateTime.UtcNow.AddDays(14),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
		//private string CreateToken(User user)
		//{
		//	var roles =  userManager.GetRolesAsync(user);

		//	var claims = new List<Claim>()
		//	{
		//	new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
		//	// = new Claim (ClaimTypes.GivenName,user.UserName),
		//	new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),

		//	//new Claim(ClaimTypes.Role,role)
		//};			
		//		foreach (var role in roles)
		//		{
		//			claims.Add(new Claim(ClaimTypes.Role, role));
		//		}

		//	var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwt:key"]));
		//	//=	var key2 = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("jwt")["key"]));
		//	var crad = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		//	var token = new JwtSecurityToken(
		//		configuration["jwt:audience"],
		//		configuration["jwt:issure"],
		//		claims,
		//		signingCredentials: crad,
		//		expires: DateTime.UtcNow.AddDays(14)
		//		);

		//	return new JwtSecurityTokenHandler().WriteToken(token);
		//}

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

		public async Task<string> CreateSubAdminAccountAsync(User user, string password)
		{
			var result = await userManager.CreateAsync(user, password);
			if (result.Succeeded)
			{
				await userManager.AddToRoleAsync(user, "SuperAdmin");
				return "SubAdmin registered successfully!";
			}
			var errorMessage = result.Errors.Select(e => e.Description).ToList();
			return string.Join(',', errorMessage);
		}

		public async Task<List<SubAdminDtos>> GetAllSubAdminAsync()
		{
			var users = await dbContext.Users.AsNoTracking().ToListAsync();
			// تجهيز الـ Dtos التي ستحتوي على اسم المستخدم واسم الرول
			var subAdminDtos = new List<SubAdminDtos>();

			foreach (var user in users)
			{
				var role = await userManager.GetRolesAsync(user);  // جلب كل الرولات للمستخدم
				var subAdminDto = new SubAdminDtos
				{
					Id=user.Id,
					Name = user.UserName,     // اسم المستخدم
					Role = role.FirstOrDefault(),  // اسم الرول (الافتراضي هو SubAdmin)
					Email = user.Email,
					Gender = user.Gender,
					Image = user.Image ?? "/default image/Man default image.png"
				};
				subAdminDtos.Add(subAdminDto);
			}
			var subAdmins= new List<SubAdminDtos>();
			foreach(var admin in subAdminDtos)
			{
				if(admin.Role== "SuperAdmin")
				{
					subAdmins.Add(admin);
				}
			}
			return subAdmins;
		}

		public async Task<string> DeleteSubAdminAsync(int userId)
		{
			var user = await userManager.FindByIdAsync(userId.ToString());
			if (user == null)
			{
				return "User not found";
			}
			var role = await userManager.GetRolesAsync(user);  // جلب كل الرولات للمستخدم

			// تحقق من أن المستخدم لديه الصلاحية (SuperAdmin في هذه الحالة)
			bool isSubAdmin = role.Contains("SuperAdmin"); 
			if (!isSubAdmin)
			{
				return "User is not a SubAdmin";
			}
			foreach (var r in role)
			{
				await userManager.RemoveFromRoleAsync(user, r);
			}
			// حذف المستخدم
			var result = await userManager.DeleteAsync(user);
			if (result.Succeeded)
			{
				return "SubAdmin deleted successfully";
			}

			return string.Join(",", result.Errors.Select(e => e.Description));
		}

		public async Task<string> ContactUsAsync(ContactUsDto dto,int UserId)
		{
			// 1. التحقق من صلاحية البريد الإلكتروني (البريد الإلكتروني صالح إذا كان له تنسيق صحيح)
			var isValidEmail = new EmailAddressAttribute().IsValid(dto.Email);
			if (!isValidEmail)
			{
				return "Invalid email format"; // إرجاع رسالة في حال كان التنسيق غير صحيح
			}
			var user = await userManager.FindByIdAsync(UserId.ToString());
			var email = new Email()
			{
				Sender=user.Email,
				SenderPassword=user.PasswordHash,
				Subject = "PCPC Website Contact Us",
				Recivers = "mustafaalrifaya3@gmail.com",
				Body = $"{dto.Message}",
			};
			EmailHealper.SendEmail2(email);
			return "Email sent successfully.";
		}

	}
}
