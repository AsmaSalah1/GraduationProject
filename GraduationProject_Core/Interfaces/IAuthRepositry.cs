using GraduationProject_Core.Dtos.Auth;
using GraduationProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
	public interface IAuthRepositry
	{
		Task<string> RegisterAsync(User user , string password);
		Task<string> ForgetPassword(string email);
		Task<string> ResetPassword(string email, string password,string token);
		Task<string> ConfirmEmail (string email, string token);
		Task<string> LogInAsync(string email, string password);
		Task<string> ChangePassword(ChangePasswordDtos dtos,int userId);

	}
}
