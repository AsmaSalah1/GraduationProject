using GraduationProject_Core.Dtos.Sponsor;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class SponsorController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;

		public SponsorController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		// إضافة Sponsor وربطه بالمسابقة
		[HttpPost("AddSponsorToCompetition/{competitionId}")]
		public async Task<IActionResult> AddSponsorToCompetition(int competitionId, [FromForm] AddSponsorToCompetitionDto dto)

		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // التحقق من صحة البيانات المدخلة
			}

			// التحقق من التوكن
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

			// استدعاء الـ Repository لإضافة الداعم وربطه بالمسابقة
			var result = await unitOfWork.iSponsorRepository.AddSponsorToCompetitionAsync(competitionId, dto);

			if (result)
				return Ok(new { Message = "Sponsor added and linked to competition successfully" });

			return BadRequest("Error adding sponsor or competition not found");
		}
		[HttpGet("Get-Sponsors-By-Competition/{competitionId}")]
		public async Task<IActionResult> GetSponsorsByCompetition(int competitionId)
		{
			// استدعاء الـ Repository لجلب الداعمين للمسابقة
			var sponsors = await unitOfWork.iSponsorRepository.GetSponsorsByCompetitionIdAsync(competitionId);

			if (sponsors == null || !sponsors.Any())
			{
				return NotFound("No sponsors found for this competition");
			}

			return Ok(sponsors); // إرجاع الداعمين
		}


		[HttpPost("AddSponsorToCompetition/{competitionId}/{sponsorId}")]
		public async Task<IActionResult> AddSponsorToCompetitionAsync(int competitionId, int sponsorId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // التحقق من صحة البيانات المدخلة
			}

			// التحقق من التوكن
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
			// هنا ممكن تستخدم الـ competitionId و sponsorId مباشرة من الرابط
			var result = await unitOfWork.iSponsorRepository.RAddSponsorToCompetitionAsync(competitionId, sponsorId);

			if (!result)
			{
				return BadRequest("Error adding sponsor or competition not found");
			}

			return Ok("Sponsor added and linked to competition successfully");
		}



		[HttpGet("getAllSponsors")]
		public async Task<ActionResult<List<SponsorDto>>> GetAllSponsors()
		{
			var sponsors = await unitOfWork.iSponsorRepository.GetAllSponsorsAsync();
			return Ok(sponsors);
		}// [HttpGet("Get-Sponsors-By-Competition/{competitionId}")]
		[HttpPut("update/{sponsorId}")]
		public async Task<IActionResult> UpdateSponsorAsync(int sponsorId, [FromForm] UpdateSponsorDto dto)

		{

			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // التحقق من صحة البيانات المدخلة
			}

			// التحقق من التوكن
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

			var result = await unitOfWork.iSponsorRepository.UpdateSponsorAsync(sponsorId, dto);

			if (!result)
			{
				return BadRequest("لم يتم تعديل السبونسور.");
			}

			return Ok("تم تعديل السبونسور بنجاح");
		}

		[HttpDelete("competition/{competitionId}/remove/{sponsorId}")]
		public async Task<IActionResult> RemoveSponsorFromCompetitionAsync(int sponsorId, int competitionId)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState); // التحقق من صحة البيانات المدخلة
			}

			// التحقق من التوكن
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

			// استدعاء الدالة لإزالة السبونسور من المسابقة المحددة
			var result = await unitOfWork.iSponsorRepository.RemoveSponsorFromCompetitionAsync(sponsorId, competitionId);

			if (!result)
			{
				return BadRequest("لم يتم إزالة السبونسور من المسابقة.");
			}

			return Ok("تم إزالة السبونسور من المسابقة بنجاح");
		}

		[HttpGet("GetSponsorsByName/{name}")]
		public async Task<IActionResult> GetSponsorsByName(string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return BadRequest("Name parameter is required.");
			}

			// استدعاء الـ Repository للبحث عن السبونسرات باستخدام الاسم
			var sponsors = await unitOfWork.iSponsorRepository.GetSponsorsByNameAsync(name);

			if (sponsors == null || !sponsors.Any())
			{
				return NotFound("No sponsors found with this name.");
			}

			return Ok(sponsors);  // إرجاع قائمة السبونسرات التي تم العثور عليها
		}

	}
}
