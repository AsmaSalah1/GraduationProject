using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.PersonalExperiance
{
	public class GetPersonalExperienceDto
	{
		public string UserName { get; set; }  // اسم المستخدم
		public string ImageName { get; set; }     // صورة المستخدم
		public string Content { get; set; }   // محتوى التجربة الشخصية
	    public DateTime DateTime { get; set; }
	}
}
