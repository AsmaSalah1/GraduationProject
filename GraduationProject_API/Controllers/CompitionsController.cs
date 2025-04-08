using GraduationProject_Core.Dtos.Competition;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GraduationProject_Core.Models;
using static GraduationProject_Core.Models.Competition;
using GraduationProject_Core.Dtos.CompetitionImage;
using GraduationProject_Core.Dtos.Sponsor;
using GraduationProject_Core.Dtos.Team;
using static System.Net.Mime.MediaTypeNames;

namespace GraduationProject_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CompitionsController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;
		private readonly IWebHostEnvironment env;

		// حقن الـ unitOfWork عبر الـ constructor
		public CompitionsController(IUnitOfWork unitOfWork,IWebHostEnvironment env)
		{
			this.unitOfWork = unitOfWork;
			this.env = env;
		}
	
		[HttpPost("Add-Competition")]
		public async Task<IActionResult> AddCompetition([FromForm] CreateCompetitionDto dto)
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

			var result = await unitOfWork.iCompetitionRepository.AddCompetitionAsync(dto, userId.Value);

			if (result)
				return Ok(new { Message = "Competition added successfully" });

			return BadRequest("Error adding competition");
		}

		[HttpGet("GetCompetitionsByType/{type}")]
		public async Task<IActionResult> GetCompetitionsByType(CompetitionType type)
		{
			// استرجاع المسابقات بناءً على النوع
			var competitions = await unitOfWork.iCompetitionRepository.GetCompetitionsByTypeAsync(type);

			if (competitions == null || competitions.Count == 0)
			{
				return NotFound("No competitions found for the specified type");
			}
			// عنوان الموقع المحلي (يمكنك تعديله لاحقًا إذا نشرته)
			var baseUrl = $"{Request.Scheme}://{Request.Host}";
			// تحويل النتائج إلى DTO المناسب
			var competitionsDto = competitions.Select(c => new CompetitionDto
			{
				Name = c.Name,
				Image = !string.IsNullOrEmpty(c.Image)
			? $"{baseUrl}{c.Image}"
			: null
			}).ToList();

			return Ok(competitionsDto);
		}

		[HttpDelete("DeleteCompetition/{competitionId}")]
		public async Task<IActionResult> DeleteCompetition(int competitionId)
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
			// استدعاء دالة الحذف من الـ Repository
			var result = await unitOfWork.iCompetitionRepository.DeleteCompetitionAsync(competitionId);

			if (result)
			{
				return Ok(new { Message = "Competition deleted successfully" });
			}
			else
			{
				return NotFound(new { Message = "Competition not found" });
			}
		}

		//[HttpPut("UpdateCompetition/{competitionId}")]
		//public async Task<IActionResult> UpdateCompetition(int competitionId, [FromForm] UpdateCompetitionDto dto)
		//{
		//	if (!ModelState.IsValid)
		//	{
		//		return BadRequest(ModelState);
		//	}

		//	var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
		//	if (string.IsNullOrEmpty(token))
		//	{
		//		return Unauthorized("Token is missing");
		//	}

		//	var userId = ExtractClaims.ExtractUserId(token);
		//	if (!userId.HasValue)
		//	{
		//		return Unauthorized("Invalid user token");
		//	}
		//	// استدعاء دالة التعديل من الـ Repository
		//	var result = await unitOfWork.iCompetitionRepository.UpdateCompetitionAsync(competitionId, dto);

		//	if (result)
		//	{
		//		return Ok(new { Message = "Competition updated successfully" });
		//	}
		//	else
		//	{
		//		return NotFound(new { Message = "Competition not found" });
		//	}
		//}
		[HttpPut("UpdateCompetition/{competitionId}")]
		public async Task<IActionResult> UpdateCompetition(int competitionId, [FromForm] UpdateCompetitionDto dto)
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
			// استدعاء دالة التعديل من الـ Repository
			var result = await unitOfWork.iCompetitionRepository.UpdateCompetitionAsync(competitionId, dto);

			if (result)
			{
				return Ok(new { Message = "Competition updated successfully" });
			}
			else
			{
				return NotFound(new { Message = "Competition not found" });
			}
		}
		[HttpGet("GetCompetitionByName/{competitionName}")]
		public async Task<IActionResult> GetCompetitionByName(string competitionName)
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
			// استدعاء الدالة في الـ Repository لاسترجاع المسابقة
			var competition = await unitOfWork.iCompetitionRepository.GetCompetitionByNameAsync(competitionName);

			if (competition == null)
			{
				// إذا كانت المسابقة غير موجودة
				return NotFound(new { Message = "Competition not found" });
			}

			// إذا كانت المسابقة موجودة، نرجع الصورة والاسم فقط
			var result = new
			{
				Name = competition.Name,
				Image = competition.Image
			};

			return Ok(result);  // إعادة المسابقة بالاسم والصورة
		}
		/*''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''*/


		[HttpGet("GetDescription/{competitionId}")]
		public async Task<IActionResult> GetDescription(int competitionId)
		{
			var competition = await unitOfWork.iCompetitionRepository.GetCompetitionByIdAsync(competitionId);

			if (competition == null)
			{
				return NotFound(new { Message = "Competition not found" });
			}

			return Ok(new { Description = competition.Description });
		}

		[HttpPut("UpdateDescription/{competitionId}")]
		public async Task<IActionResult> UpdateDescription(int competitionId, [FromBody] string description)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
			if (string.IsNullOrEmpty(description))
			{
				return BadRequest("Description cannot be empty");
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
			
			var result = await unitOfWork.iCompetitionRepository.UpdateDescriptionAsync(competitionId, description);

			if (result)
			{
				return Ok(new { Message = "Description updated successfully" });
			}
			else
			{
				return NotFound(new { Message = "Competition not found" });
			}
		}
		[HttpGet("GetLocation/{competitionId}")]
		public async Task<IActionResult> GetLocation(int competitionId)
		{
			var competition = await unitOfWork.iCompetitionRepository.GetCompetitionByIdAsync(competitionId);

			if (competition == null)
			{
				return NotFound(new { Message = "Competition not found" });
			}

			return Ok(new { Locations = competition.Location });
		}
		[HttpPut("UpdateLocation/{competitionId}")]
		public async Task<IActionResult> UpdateLocation(int competitionId, [FromBody] List<string> locations)
		{

			if (locations == null || locations.Count == 0)
			{
				return BadRequest("Locations cannot be empty");
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

			var result = await unitOfWork.iCompetitionRepository.UpdateLocationAsync(competitionId, locations);

			if (result)
			{
				return Ok(new { Message = "Location updated successfully" });
			}
			else
			{
				return NotFound(new { Message = "Competition not found" });
			}
		}
		[HttpGet("GetImage2/{competitionId}")]
		public async Task<IActionResult> GetImage2(int competitionId)
		{
			var competition = await unitOfWork.iCompetitionRepository.GetCompetitionByIdAsync(competitionId);

			if (competition == null)
			{
				return NotFound(new { Message = "Competition not found" });
			}
			var baseUrl = $"{Request.Scheme}://{Request.Host}";

			// التأكد من أن الصورة موجودة
			if (string.IsNullOrEmpty(competition.Image2))
			{
				return NotFound(new { Message = "Image2 not found" });
			}
			var InvetationImage = $"{baseUrl}{competition.Image2}";
			return Ok(new { InvetationImage });
		}



		[HttpGet("GetCompetitionDetails/{competitionId}")]
		public async Task<IActionResult> GetCompetitionDetails(int competitionId)
		{
			// استرجاع جميع التفاصيل المطلوبة من الدوال السابقة
			var competition = await unitOfWork.iCompetitionRepository.GetCompetitionByIdAsync(competitionId);
			if (competition == null)
			{
				return NotFound(new { Message = "Competition not found" });
			}
			var baseUrl = $"{Request.Scheme}://{Request.Host}";

		

			// الحصول على الصورة الثانية
			var image2Result = string.IsNullOrEmpty(competition.Image2) ? null : $"{baseUrl}{competition.Image2}";
		//	competition.Image2 = !string.IsNullOrEmpty(competition.Image2)
		//? $"{baseUrl}{competition.Image2}"
		//: null;
			// الحصول على الداعمين
			var sponsors = await unitOfWork.iSponsorRepository.GetSponsorsByCompetitionIdAsync(competitionId);
			if (sponsors == null || !sponsors.Any())
			{
				sponsors = new List<Sponsorl>();  // إذا لم يتم العثور على داعمين، نقوم بإرجاع قائمة فارغة
			}
			foreach (var s in sponsors) {
				string relativePath1 = s.Logo.Replace(env.WebRootPath, "").Replace("\\", "/");
				s.Logo = $"{baseUrl}{relativePath1}";		
			}
				// الحصول على التيمز
				var teams = await unitOfWork.teamRepository.GetTeamsByCompetitionIdAsync(competitionId);
			if (teams == null || !teams.Any())
			{
				teams = new List<TeamsDetailsDto>();  // إذا لم يتم العثور على فرق، نقوم بإرجاع قائمة فارغة
			}

			// الحصول على الموقع
			var location = competition.Location;
			var discribtion = competition.Description;
			// الحصول على صور المسابقة
			var images = await unitOfWork.iCompetitionImagesRepository.GetImagesByCompetitionIdAsync(competitionId);
			if (images == null || !images.Any())
			{
				images = new List<GetImagesByCompetitionDto>();  // إذا لم يتم العثور على صور، نقوم بإرجاع قائمة فارغة
			}
			foreach (var i in images)
			{
				string relativePath1 = i.ImageName.Replace(env.WebRootPath, "").Replace("\\", "/");
				i.ImageName = $"{baseUrl}{relativePath1}";
			}
			// تجهيز البيانات للـ response
			var competitionDetails = new
			{
				Image2 = image2Result,

				Sponsors = sponsors,
				Description = discribtion,
				Location = location,
				Teams = teams,
				Images = images  // إضافة الصور إلى التفاصيل
			};

			// إرجاع البيانات في response
			return Ok(competitionDetails);
		}



	}

}
