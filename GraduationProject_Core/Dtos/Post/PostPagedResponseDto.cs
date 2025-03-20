using GraduationProject_Core.Dtos.PersonalExperiance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Post
{
	public class PostPagedResponseDto<GetPostDto>
	{
		public int TotalPosts { get; set; }
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public IEnumerable<GetPostDto> Posts { get; set; }
	}
}
