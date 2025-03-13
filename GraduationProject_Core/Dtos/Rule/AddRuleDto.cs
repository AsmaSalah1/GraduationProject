using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Rule
{
	public class AddRuleDto
	{
		[Required]
		public string Title { get; set; }
		[Required]
		public List<string> Description { get; set; } = new List<string>();
	}
}
