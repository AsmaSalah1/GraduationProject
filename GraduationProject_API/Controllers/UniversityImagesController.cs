using GraduationProject_Core.Dtos.UniversityImage;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityImagesController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment env;
        public UniversityImagesController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            this.unitOfWork = unitOfWork;
            this.env = env;
        }

        // إضافة مجموعة من الصور للجامعة
        [HttpPost("universities/{universityId}/images")]
        public async Task<IActionResult> AddUniversityImages(int universityId, [FromForm] AddUniversityImageDto dto)
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

            var result = await unitOfWork.iUniversityImagesRepository.AddUniversityImagesAsync(universityId, dto);
            if (result)
                return Ok(new { Message = "Images added successfully" });

            return BadRequest("Error adding images");
        }

        // عرض كل الصور الخاصة بجامعة معينة
        /*
        [HttpGet("Get-UniversityImages/{universityId}")]
        public async Task<IActionResult> GetUniversityImages(int universityId)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            var images = await unitOfWork.iUniversityImagesRepository.GetImagesByUniversityIdAsync(universityId);
            if (images == null || !images.Any())
                return NotFound("No images found for this university");

            return Ok(images);
        }
        */
        [HttpGet("Get-UniversityImages/{universityId}")]
        public async Task<IActionResult> GetUniversityImages(int universityId)
        {
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            var images = await unitOfWork.iUniversityImagesRepository.GetImagesByUniversityIdAsync(universityId);

            if (images == null || !images.Any())
                return NotFound("No images found for this university");

            // تعديل مسار الصور
            foreach (var item in images)
            {
                //ImageUrl = $"{baseUrl}/Images/{item.ImageName}"
                // تحويل مسار الصورة إلى URL كامل
            //    string relativePath = item.ImageName.Replace(env.WebRootPath, "").Replace("\\", "/");
                item.ImageName = $"{baseUrl}/Images/{item.ImageName}";
            }

            return Ok(images);
        }

        // حذف صورة
        [HttpDelete("Delete-UniversityImage/{imageId}")]
        public async Task<IActionResult> DeleteUniversityImage(int imageId)
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

            var result = await unitOfWork.iUniversityImagesRepository.DeleteUniversityImageAsync(imageId);
            if (result)
                return Ok(new { Message = "Image deleted successfully" });

            return NotFound("Image not found");
        }


    }
}
