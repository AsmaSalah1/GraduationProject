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
		[Authorize(Roles ="Admin")]
		[HttpPost("Add-Team")]
		public async Task<IActionResult> AddTeam([FromBody] CreateTeamDto dto)
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
			// if (dto.Participants == null || dto.Participants.Count < 1 || dto.Participants.Count > 3)
			//   return BadRequest("Participants must be between 1 and 3.");

			var result = await unitOfWork.teamRepository.AddTeamAsync(dto, userId.Value);
			if (result)
				return Ok(new { Message = "Team added successfully" });

			return BadRequest("Error adding team");
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
			.Where(t => t.TeamName.Equals(teamName, StringComparison.OrdinalIgnoreCase))
			.ToList();

			if (!matchingTeams.Any())
			{
				return NotFound("No teams found with the given name.");
			}

			return Ok(matchingTeams); ;
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
		[HttpPut("Update-Team/{teamId}")]
		public async Task<IActionResult> UpdateTeam(int teamId, [FromBody] UpdateTeamDto dto)
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

			var result = await unitOfWork.teamRepository.UpdateTeamAsync(teamId, dto, userId.Value);
			if (result)
				return Ok("Team updated successfully");
			return BadRequest("Error updating team");
		}

		[HttpGet("Get-Teams-By-ParticipantName/{participantName}")]
		public async Task<IActionResult> GetTeamsByParticipantName(string participantName)
		{
			var teams = await unitOfWork.teamRepository.GetTeamsByParticipantNameAsync(participantName);
			if (teams == null || !teams.Any())
				return NotFound($"No teams found for the participant: {participantName}");

			return Ok(teams);
		}
	}
	}
