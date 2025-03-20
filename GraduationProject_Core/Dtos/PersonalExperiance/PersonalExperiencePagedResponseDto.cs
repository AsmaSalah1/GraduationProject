using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.PersonalExperiance
{
	public class PersonalExperiencePagedResponseDto<GetPersonalExperienceDto>
	{
		public int TotalPersonalExperienc {  get; set; }
		public int PageIndex { get; set; }
		public int PageSize { get; set; }
		public IEnumerable<GetPersonalExperienceDto> PersonalExperiances {  get; set; }

	}
}
