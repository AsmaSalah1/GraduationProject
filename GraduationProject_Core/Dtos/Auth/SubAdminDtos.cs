using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Auth
{
	public class SubAdminDtos
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string? UniversityName { get; set; }
		public Gender Gender { get; set; }
		public string? Image { get; set; } // استقبال الصورة عند التحديث
	    public string Role {  get; set; }
	}
}
