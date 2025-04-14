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
		Task<bool> AddTeamAsync(CreateTeamDto dto, int competitionId, int userId);
		Task<IEnumerable<TeamDetailsDto>> GetAllTeamsAsync();
		Task<bool> DeleteTeamAsync(int teamId, int userId);
		Task<bool> UpdateTeamAsync(int competitionId, int teamId, UpdateTeamDto dto, int userId);
		Task<IEnumerable<TeamsDetailsDto>> GetTeamsByParticipantNameAsync(string participantName);
		Task<IEnumerable<TeamsDetailsDto>> GetTeamsByCompetitionIdAsync(int competitionId);
		//Task<bool> RemoveTeamFromCompetitionAsync(int teamId, int competitionId);
		Task<bool> RemoveTeamFromCompetitionAsync(int competitionId, int teamId);
		Task<bool> LinkTeamToCompetitionAsync(int competitionId, int teamId, LinkTeamToCompetitionDto dto);
		Task<IEnumerable<TeamsDetailsDto>> GetTeamsByTeamNameAsync(string teamName);
		Task<string> AddTeamAsmaa(CreateTeamDto dto, int competitionId, int userId);

	}
}
