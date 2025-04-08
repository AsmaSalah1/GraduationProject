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

		[HttpPost("Add-newParticipant-To-Team/{teamId}")]
		public async Task<IActionResult> AddParticipantToTeam(int teamId, [FromBody] AddParticipantOnly request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// التأكد من وجود التوكن في الهيدر
			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			if (string.IsNullOrEmpty(token))
				return Unauthorized("Token is missing");

			// استخراج الـ userId من التوكن
			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
				return Unauthorized("Invalid user token");

			// إرسال الـ teamId فقط بينما الـ participantName و الـ year سيتم تحديدهم داخل الـ backend
			var result = await unitOfWork.iparticipantRepository.AddParticipantToTeamAsync(teamId, request.ParticipantName, request.Year);
			if (result)
				return Ok("Participant added to team successfully");

			return BadRequest("Error adding participant to team");
		}

		// حذف مشارك من الفريق
		[HttpDelete("Remove-Participant-From-Team/{teamId}/{participantId}")]
		public async Task<IActionResult> RemoveParticipantFromTeam(int teamId, int participantId)
		{
			// التأكد من وجود التوكن في الهيدر
			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			if (string.IsNullOrEmpty(token))
				return Unauthorized("Token is missing");

			// استخراج الـ userId من التوكن
			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
				return Unauthorized("Invalid user token");

			// محاولة حذف المشارك من الفريق
			var result = await unitOfWork.iparticipantRepository.RemoveParticipantFromTeamAsync(teamId, participantId);
			if (result)
				return Ok("Participant removed from team successfully");

			return BadRequest("Error removing participant from team");
		}

		// 1. إرجاع كل المشاركين
		[HttpGet("GetAllParticipants")]
		public async Task<IActionResult> GetAllParticipants()
		{
			var participants = await unitOfWork.iparticipantRepository.GetAllParticipantsAsync();
			return Ok(participants);
		}

		// 2. إرجاع المشارك بناء على الاسم
		[HttpGet("GetParticipantByName/{participantName}")]
		public async Task<IActionResult> GetParticipantByName(string participantName)
		{
			var participant = await unitOfWork.iparticipantRepository.GetParticipantByNameAsync(participantName);
			if (participant == null)
			{
				return NotFound("Participant not found");
			}
			return Ok(participant);
		}

		// إضافة مشارك إلى فريق
		// إضافة مشارك إلى الفريق
		[HttpPost("Add-oldParticipant-To-Team/{teamId}/{participantId}")]
		public async Task<IActionResult> AddoldParticipantToTeam(int teamId, int participantId, [FromBody] DateTime year)
		{
			// التأكد من صحة البيانات
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			// التأكد من وجود التوكن في الهيدر
			var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
			if (string.IsNullOrEmpty(token))
				return Unauthorized("Token is missing");

			var userId = ExtractClaims.ExtractUserId(token);
			if (!userId.HasValue)
				return Unauthorized("Invalid user token");

			// استدعاء الدالة لإضافة المشارك إلى الفريق
			var result = await unitOfWork.iparticipantRepository.AddoldParticipantToTeamAsync(teamId, participantId, year);
			if (result)
				return Ok("Participant added to team successfully");

			return BadRequest("Error adding participant to team");
		}
	}

}

