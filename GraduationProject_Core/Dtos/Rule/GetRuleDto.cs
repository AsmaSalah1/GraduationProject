using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Rule
{
	public class GetRuleDto
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public List<string> Description { get; set; } = new List<string>();
	}
}
