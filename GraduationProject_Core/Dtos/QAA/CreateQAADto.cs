using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.QAA
{
	public class CreateQAADto
	{
		[Required]
		public string Question { get; set; }
		[Required]
		public string Answer { get; set; }
	}
}
