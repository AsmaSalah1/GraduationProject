using GraduationProject_Core.Dtos.PersonalExperiance;
using GraduationProject_Core.Dtos.UserProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
	public interface IUserProfileRepositry
	{
		Task<ViewProfileDtos> GetUserProfileAsync(int id);
		Task<bool> UpdateUserProfileAsync(int id, UpdateProfileDtos updateDto);

	}
}
