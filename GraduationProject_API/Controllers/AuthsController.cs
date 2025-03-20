using GraduationProject_Core.Dtos.Auth;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Net;

namespace GraduationProject_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthsController : ControllerBase
	{
		private readonly IAuthRepositry authRepositry;

		public AuthsController(IAuthRepositry authRepositry)
		{
			this.authRepositry = authRepositry;
		}

		[HttpPost("Register")]
		public async Task<IActionResult> Register([FromBody] RegisterDtos registerDtos)
		{
			if (!ModelState.IsValid) { 
			   return BadRequest(ModelState);
			}
		
			var user = new User()
			{
				UserName = registerDtos.Name,
				Email = registerDtos.Email,
				Gender=registerDtos.Gender
			};
			// تعيين صورة افتراضية بناءً على الجنس
			if (string.IsNullOrEmpty(user.Image)) // فقط إذا لم يوفر المستخدم صورة
			{
				user.Image = user.Gender == Gender.Male
					? $"{Directory.GetCurrentDirectory()}\\wwwroot\\Images\\Man defult image.png"
					: $"{Directory.GetCurrentDirectory()}\\wwwroot\\Images\\Women defult image.png";
			
			}
			var result = await authRepositry.RegisterAsync(user,registerDtos.Password);
			if(result == "User registered successfully! Please check your email to confirm your account.")
			{
				return Ok(result);
			}
			return BadRequest(result);
		}
		//[HttpGet("confirm-email")]
		//public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
		//{
		//	if (!ModelState.IsValid) {
		//		return BadRequest(ModelState);
		//	}
		//    //var decodedEmail=WebUtility.UrlDecode(email);
		//	//var decodedToken = WebUtility.UrlDecode(token);

		//	var result =await authRepositry.ConfirmEmail(email, token);
		//	if(result == "Email confirmed successfully!")
		//	{
		//		return Ok(result);
		//	}
		//	return BadRequest(result);
		//}

		[HttpGet("confirm-email")]
		public async Task<IActionResult> ConfirmEmail([FromQuery] string email)
		{
			if (string.IsNullOrEmpty(email))
			{
				return BadRequest("Email is required.");
			}

			var authHeader = Request.Headers["Authorization"].ToString();

			if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
			{
				return Unauthorized("Missing or invalid Authorization header.");
			}

			var token = authHeader.Replace("Bearer ", "");

			var result = await authRepositry.ConfirmEmail(email, token);

			if (result == "Email confirmed successfully!")
				return Ok(result);

			return BadRequest("Invalid token or email confirmation failed.");
		}
		[HttpPost("LogIn")]
		//localhost/api/AuthController/LogIn
		public async Task<IActionResult> Login([FromBody] LogInDTOs logInDTOs)
		{
			if (!ModelState.IsValid)
			{
				return Unauthorized();
			}
			var token = await authRepositry.LogInAsync(logInDTOs.Email, logInDTOs.Password);
			if (token is null)
			{
				return Unauthorized(new { Message = "Invalid User Email or Password" });
			}
			return Ok(token);
		}

		[HttpPost("Forgot-password")]
		public async Task<IActionResult> FordotPassword(ForgetPassowrd forgetPassowrd)
		{
			if (!ModelState.IsValid) {
				return BadRequest(ModelState);
			}
			var result = await authRepositry.ForgetPassword(forgetPassowrd.Email);
			if(result == "Email Sent successfully")
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		[HttpPost("reset-password")]
		public async Task<IActionResult> ResetPassword([FromBody] ResitPasswordDtos request)
		{
			if (string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.NewPassword))
			{
				return BadRequest("Token and new password are required.");
			}

			// التحقق من الـ token (يمكنك تخزينه في قاعدة بيانات أو ذاكرة مؤقتة)
			var result = await authRepositry.ResetPassword(request.Email,request.NewPassword,request.Token);

			if (result == "Password has been successfully reset.")
			{
				return Ok(result);
			}
			return BadRequest(result);
		}

		//		عند النقر على رابط إعادة تعيين كلمة المرور في البريد الإلكتروني، فإن الطلب يتم كـ GET وليس POST.

		//لما المتخدم يكبس ع الرابط الي نبعثله بالايميل بوديه هون عشان اعمل فاليديشين للتوكن و الايميل
		//وهاد الايند بوينت بوديني ع صفحة الفرونت 
		[HttpGet("reset-password")]
		public IActionResult ResetPasswordPage([FromQuery] string token, [FromQuery] string email)
		{
			if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
			{
				return BadRequest("Invalid request");
			}

			// إعادة توجيه المستخدم إلى صفحة إعادة تعيين كلمة المرور في الواجهة الأمامية
			return Redirect($"https://yourfrontend.com/reset-password?token={token}&email={email}");
		}

		[Authorize]
		[HttpPost("Change-Password")]
		public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordDtos dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			//التوكن عباة عن سترينغ
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("Token is missing");
			}
			try
			{
				var userId = ExtractClaims.ExtractUserId(token);
				if (!userId.HasValue)
				{
					return Unauthorized("Invalid user token");
				}
				//لازم احط await
				//بدونها بعطيني ايرور لما يفحص الريزلت لانه بكون نوعها اشي تاني
				var result = await authRepositry.ChangePassword(dto, userId.Value);
				if (result == "Password changed successfully.")
				{
					return Ok(result);
				}
				else if (result == "Invalid user")
				{
					return Unauthorized(result);
				}
				else
				{
					return BadRequest(result); // أي خطأ آخر (كلمة مرور غير صحيحة مثلاً)
				}
			}
			catch (Exception ex)
			{
				return Unauthorized("Invalid Token " + ex.Message);
			}
		}
	}
}
