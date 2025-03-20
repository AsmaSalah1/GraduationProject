using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Auth
{
	public class RegisterDtos
	{
		[Required]
		public string Name { get; set; }
		[Required]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
		public string? UniversityName { get; set; }
		[Required]
		public Gender Gender { get; set; }
	}
	public enum Gender
	{
		Male,// بتتخزن صفر بالداتا بيس
		Female//بتتخزن 1
	}
}
