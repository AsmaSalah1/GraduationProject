using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Post
{
	public class CreatePostDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public IFormFile? Image { get; set; }
	}
}
