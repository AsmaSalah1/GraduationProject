using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Likes
{
	public class GetLikesDto
	{
		public int LikeId { get; set; }
		public int UserId { get; set; }
		public string UserName { get; set; }
	}
}
