﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Auth
{
	public class ResitPasswordDtos
	{
		[Required]
		public string NewPassword { get; set; }
	//	public string Email { get; set; }
	//	public string Token { get; set; }
	}
}
