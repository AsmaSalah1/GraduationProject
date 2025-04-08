using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Auth
{
	public class CreateSubAdminAccountDto
	{
		[Required]
		public string Name { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		// تغيير من UniversityName إلى UniversityId لربط الجامعة بشكل صحيح
		[Required]
		public int UniversityId { get; set; }
		[Required]
		public Gender Gender { get; set; }
		public IFormFile? Image { get; set; } // استقبال الصورة عند التحديث

	}

}

