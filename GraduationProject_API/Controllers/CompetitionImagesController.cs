using GraduationProject_Core.Dtos.Competition;
using GraduationProject_Core.Dtos.CompetitionImage;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class CompetitionImagesController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;

		public CompetitionImagesController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

		// إضافة مجموعة من الصور
		[HttpPost("competitions/{competitionId}/images")]
		public async Task<IActionResult> AddCompetitionImages(int competitionId, [FromForm] AddCompetitionImageDto dto)
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

			var result = await unitOfWork.iCompetitionImagesRepository.AddCompetitionImagesAsync(competitionId, dto);
			if (result)
				return Ok(new { Message = "Images added successfully" });

			return BadRequest("Error adding images");
		}


		/*

         // عرض كل الصور الخاصة بمسابقة معينة
         [HttpGet("Get-CompetitionImages/{competitionId}")]
         public async Task<IActionResult> GetCompetitionImages(int competitionId)
         {
             var images = await unitOfWork.iCompetitionImagesRepository.GetImagesByCompetitionIdAsync(competitionId);
             if (images == null || !images.Any())
                 return NotFound("No images found for this competition");

             return Ok(images);
         }

         */


		// عرض كل الصور الخاصة بمسابقة معينة
		[HttpGet("Get-CompetitionImages/{competitionId}")]
		public async Task<IActionResult> GetCompetitionImages(int competitionId)
		{
			var images = await unitOfWork.iCompetitionImagesRepository.GetImagesByCompetitionIdAsync(competitionId);
			if (images == null || !images.Any())
				return NotFound("No images found for this competition");

			return Ok(images); // سترجع الآن قائمة تحتوي على الـ ID واسم الصورة
		}

		// حذف صورة
		[HttpDelete("Delete-CompetitionImage/{imageId}")]
		public async Task<IActionResult> DeleteCompetitionImage(int imageId)
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
			var result = await unitOfWork.iCompetitionImagesRepository.DeleteCompetitionImageAsync(imageId);
			if (result)
				return Ok(new { Message = "Image deleted successfully" });

			return NotFound("Image not found");
		}



	}
}
