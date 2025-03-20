using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Comment
{
	public class AddCommentDto
	{
		[Required(ErrorMessage = "Comment content is required.")] 
		public string Content { get; set; }
	}
}
