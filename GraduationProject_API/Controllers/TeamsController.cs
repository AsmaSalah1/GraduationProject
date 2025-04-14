using GraduationProject_Core.Dtos.Team;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class TeamsController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;

		public TeamsController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		[HttpPost("Add-Team/{competitionId}")]
		public async Task<IActionResult> AddTeam(int competitionId, [FromBody] CreateTeamDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("Token is missing");
			}

			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
			{
				return Unauthorized("Invalid user token");
			}

			// هنا استقبلنا الـ competitionId كـ parameter في الـ URL
			// الـ competitionId سيأتي مع الـ URL، لذا لا تحتاج لإضافته في body

			var result = await unitOfWork.teamRepository.AddTeamAsync(dto, competitionId, userId.Value);  // تمرير الـ competitionId هنا
			if (result)
			{
				return Ok(new { Message = "Team added successfully" });
			}

			return BadRequest("Error adding team");
		}


		[HttpGet("GetAllTeamNames")]
		public async Task<IActionResult> GetAllTeamNames()
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			//التوكن عباة عن سترينغ
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("Token is missing");
			}
			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
			{
				return Unauthorized("Invalid user token");
			}
			var teams = await unitOfWork.teamRepository.GetAllTeamsAsync();
			if (teams == null || !teams.Any())
				return NotFound("No teams found.");

			var teamNames = teams.Select(t => new { TeamId = t.TeamId, TeamName = t.TeamName }).ToList();
			return Ok(teamNames);
		}


		[HttpGet("GetTeamsByCompetition/{competitionId}")]
		public async Task<IActionResult> GetTeamsByCompetition(int competitionId)
		{
			var teams = await unitOfWork.teamRepository.GetTeamsByCompetitionIdAsync(competitionId);
			if (teams == null || !teams.Any())
			{
				return NotFound("No teams found for this competition.");
			}
			return Ok(teams);
		}


		[HttpPost("Link-Team-To-Competition/{competitionId}/{teamId}")]
		public async Task<IActionResult> LinkTeamToCompetitio(int competitionId, int teamId, [FromBody] LinkTeamToCompetitionDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // تحقق من صحة البيانات المدخلة
			}

			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("Token is missing");
			}

			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
			{
				return Unauthorized("Invalid user token");
			}

			// ربط الفريق مع المسابقة
			var result = await unitOfWork.teamRepository.LinkTeamToCompetitionAsync(competitionId, teamId, dto);

			if (!result)
			{
				return BadRequest("Error linking team to competition. Please check if the team or competition exists.");
			}

			return Ok("Team successfully linked to competition.");
		}


		// عرض جميع الفرق
		[HttpGet("GetAllTeams")]
		public async Task<IActionResult> GetAllTeams()
		{
			var teams = await unitOfWork.teamRepository.GetAllTeamsAsync();
			if (teams == null || !teams.Any())
				return NotFound("No teams found.");
			return Ok(teams);
		}

		// البحث عن الفريق باستخدام الاسم
		[HttpGet("GetTeamByName/{teamName}")]
		public async Task<IActionResult> GetTeamByName(string teamName)
		{
			var teams = await unitOfWork.teamRepository.GetAllTeamsAsync();

			var matchingTeams = teams
				.Where(t => t.TeamName.StartsWith(teamName, StringComparison.OrdinalIgnoreCase)) // البحث بأول حرفين أو أكثر
				.Select(t => new
				{
					TeamId = t.TeamId,
					TeamName = t.TeamName
				})
				.ToList();

			if (!matchingTeams.Any())
			{
				return NotFound("No teams found with the given name.");
			}

			return Ok(matchingTeams); // إرجاع الفرق التي تطابق الاسم المدخل
		}



		[HttpDelete("Delete-Team/{teamId}")]
		public async Task<IActionResult> DeleteTeamById([FromRoute] int teamId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("Token is missing");
			}

			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
			{
				return Unauthorized("Invalid user token");
			}

			var result = await unitOfWork.teamRepository.DeleteTeamAsync(teamId, userId.Value);
			if (result)
				return Ok("Team deleted successfully");

			return NotFound("Team not found.");
		}

		// تحديث الفريق
		[HttpPut("Competition/{competitionId}/Update-Team/{teamId}")]
		public async Task<IActionResult> UpdateTeam(int competitionId, int teamId, [FromBody] UpdateTeamDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("Token is missing");
			}

			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
			{
				return Unauthorized("Invalid user token");
			}

			var result = await unitOfWork.teamRepository.UpdateTeamAsync(competitionId, teamId, dto, userId.Value);
			if (result)
				return Ok("Team updated successfully");

			return BadRequest("Error updating team");
		}


		[HttpGet("Get-Teams-By-ParticipantName/{participantName}")]
		public async Task<IActionResult> GetTeamsByParticipantName(string participantName)
		{
			var teams = await unitOfWork.teamRepository.GetTeamsByParticipantNameAsync(participantName);
			if (teams == null || !teams.Any())
				return NotFound($"No teams found for the participant: {participantName}");

			return Ok(teams);
		}

		[HttpDelete("Competition/{competitionId}/Remove-Team-From-Competition/{teamId}")]
		public async Task<IActionResult> RemoveTeamFromCompetition(int competitionId, int teamId)
		{
			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("Token is missing");
			}

			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
			{
				return Unauthorized("Invalid user token");
			}

			var result = await unitOfWork.teamRepository.RemoveTeamFromCompetitionAsync(competitionId, teamId);
			if (result)
				return Ok("Team successfully removed from competition.");

			return BadRequest("Error removing team from competition.");
		}
		[HttpGet("Get-Teams-By-TeamName/{teamName}")]
		public async Task<IActionResult> GetTeamsByTeamName(string teamName)
		{
			var teams = await unitOfWork.teamRepository.GetTeamsByTeamNameAsync(teamName);
			if (teams == null || !teams.Any())
				return NotFound($"No teams found for the team name: {teamName}");

			return Ok(teams);
		}




		[HttpPost("Add-Team-Asmaa/{competitionId}")]
		public async Task<IActionResult> AddTeamAsmaa(int competitionId, [FromBody] CreateTeamDto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			if (string.IsNullOrEmpty(token))
			{
				return Unauthorized("Token is missing");
			}

			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
			{
				return Unauthorized("Invalid user token");
			}

			// هنا استقبلنا الـ competitionId كـ parameter في الـ URL
			// الـ competitionId سيأتي مع الـ URL، لذا لا تحتاج لإضافته في body

			var result = await unitOfWork.teamRepository.AddTeamAsmaa(dto, competitionId, userId.Value);  // تمرير الـ competitionId هنا
			if (result== "Team and Participants added successfully" || result== "Team and Participants linked successfully")
			{
				return Ok(new { Message = "Team added successfully" });
			}

			return BadRequest("Error adding team");
		}


	}
}
