﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.QAA
{
	public class GetQAADto
	{
		public int Id { get; set; }
		public string Question { get; set; }
		public string Answer { get; set; }
	}
}
