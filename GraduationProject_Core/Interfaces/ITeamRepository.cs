using GraduationProject_Core.Dtos.Team;
using GraduationProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
	public interface ITeamRepository
	{
		 Task<bool> AddTeamAsync(CreateTeamDto dto, int userId);
		Task<IEnumerable<TeamDetailsDto>> GetAllTeamsAsync();
		Task<bool> DeleteTeamAsync(int teamId, int userId);
		Task<bool> UpdateTeamAsync(int teamId, UpdateTeamDto dto, int userId);
		Task<IEnumerable<TeamDetailsDto>> GetTeamsByParticipantNameAsync(string participantName);

	}
}
