using GraduationProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Post
{
	public class GetPostDto
	{
		public string ProfileImage { get; set; }
		public string PosterName { get; set; }
		public string university { get;set; }
		public string PostImage { get;set;}
		public string Title { get; set; }
		public string Description { get; set; }
		//public List<Comment> Comments { get; set; }

	}
}
