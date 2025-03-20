using GraduationProject_Core.Dtos.Participant;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ParticipantsController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;

		public ParticipantsController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}


		[HttpPost("Add-Participant-To-Team")]
		public async Task<IActionResult> AddParticipantToTeam([FromQuery] int teamId, [FromBody] AddParticipantOnly request)
		{
			if(!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			if (string.IsNullOrEmpty(token))
				return Unauthorized("Token is missing");

			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
				return Unauthorized("Invalid user token");

			var result = await unitOfWork.iparticipantRepository.AddParticipantToTeamAsync(teamId, request.ParticipantName, request.Year);
			if (result)
				return Ok("Participant added to team successfully");

			return BadRequest("Error adding participant to team");
		}


		// حذف مشارك من فريق

		[HttpDelete("Remove-Participant-From-Team")]
		public async Task<IActionResult> RemoveParticipantFromTeam(int teamId, int participantId)
		{
			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			if (string.IsNullOrEmpty(token))
				return Unauthorized("Token is missing");

			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
				return Unauthorized("Invalid user token");

			var result = await unitOfWork.iparticipantRepository.RemoveParticipantFromTeamAsync(teamId, participantId);
			if (result)
				return Ok("Participant removed from team successfully");

			return BadRequest("Error removing participant from team");
		}
	}

}

