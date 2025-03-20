using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Auth
{
	public class ChangePasswordDtos
	{
		[DataType(DataType.Password)]
		public string OldPassword { get; set; }
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }
		//public string token { get; set; }
	}
}
