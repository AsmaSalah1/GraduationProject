using GraduationProject_Core.Dtos.University;
using GraduationProject_Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment env;
        public UniversityController(IUnitOfWork unitOfWork, IWebHostEnvironment env)
        {
            this.unitOfWork = unitOfWork;
            this.env = env;
        }
        // إضافة جامعة جديدة
        [HttpPost("Add-University")]
        public async Task<IActionResult> AddUniversity([FromForm] CreateUniversityDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // استدعاء الـ Repository لإضافة الجامعة
            var result = await unitOfWork.iUniversityRepositry.AddUniversityAsync(dto);

            if (result)
                return Ok(new { Message = "University added successfully" });

            return BadRequest("Error adding university");
        }
        [HttpGet("GetAllUniversities")]
        public async Task<IActionResult> GetAllUniversities()
        {
            // استدعاء الـ Repository لاسترجاع جميع الجامعات
            var universities = await unitOfWork.iUniversityRepositry.GetAllUniversitiesAsync();

            if (universities == null || universities.Count == 0)
            {
                return NotFound(new { Message = "No universities found" });
            }

            // تحديد الـ baseUrl
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            // تحويل البيانات لإرجاع الاسم، الصورة، وID فقط مع تعديل مسار الصورة
            var universitiesDto = universities.Select(u => new
            {
                u.UniversityId,
                u.Name,
                // تعديل مسار الصورة إلى URL كامل
                ImageUrl = $"{baseUrl}/Images/{u.ImageName}" // تأكد من مسار الصور الصحيح على السيرفر
            }).ToList();

            return Ok(universitiesDto);  // إرجاع الجامعات مع مسارات الصور المعدلة
        }
        [HttpGet("GetAllUniversitiesforRigister")]
        public async Task<IActionResult> GetAllUniversitiesforRigister()
        {
            // استدعاء الـ Repository لاسترجاع جميع الجامعات
            var universities = await unitOfWork.iUniversityRepositry.GetAllUniversitiesAsync();

            if (universities == null || universities.Count == 0)
            {
                return NotFound(new { Message = "No universities found" });
            }

            // تحديد الـ baseUrl
            string baseUrl = $"{Request.Scheme}://{Request.Host}";

            // تحويل البيانات لإرجاع الاسم، الصورة، وID فقط مع تعديل مسار الصورة
            var universitiesDto = universities.Select(u => new
            {
                u.UniversityId,
                u.Name,
                // تعديل مسار الصورة إلى URL كامل
                //  ImageUrl = $"{baseUrl}/Images/{u.ImageName}" // تأكد من مسار الصور الصحيح على السيرفر
            }).ToList();

            return Ok(universitiesDto);  // إرجاع الجامعات مع مسارات الصور المعدلة
        }

        [HttpPut("UpdateUniversity/{universityId}")]
        public async Task<IActionResult> UpdateUniversity(int universityId, [FromForm] UpdateUniversityDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  // تأكد من صحة البيانات المدخلة
            }

            // تحقق من أن الـ DTO لا يحتوي على قيم فارغة
            if (dto == null)
            {
                return BadRequest("Invalid data");
            }

            // استدعاء الدالة في الـ Repository لتحديث الجامعة
            var result = await unitOfWork.iUniversityRepositry.UpdateUniversityAsync(universityId, dto);

            if (result)
            {
                return Ok(new { Message = "University updated successfully" });
            }
            else
            {
                return NotFound(new { Message = "University not found" });
            }
        } // دالة لحذف الجامعة
        [HttpDelete("DeleteUniversity/{universityId}")]
        public async Task<IActionResult> DeleteUniversity(int universityId)
        {
            // استدعاء الدالة في الـ Repository لحذف الجامعة
            var result = await unitOfWork.iUniversityRepositry.DeleteUniversityAsync(universityId);

            if (result)
            {
                return Ok(new { Message = "University deleted successfully" });
            }
            else
            {
                return NotFound(new { Message = "University not found" });
            }
        }
		/*
                [HttpGet("GetUniversityDetails/{universityId}")]
                public async Task<IActionResult> GetUniversityDetails(int universityId)
                {
                    // جلب تفاصيل الجامعة باستخدام الـ universityId
                    var university = await unitOfWork.iUniversityRepositry.GetUniversityByIdAsync(universityId);

                    if (university == null)
                    {
                        return NotFound(new { Message = "University not found" });
                    }

                    // جلب الصور الخاصة بالجامعة
                    string baseUrl = $"{Request.Scheme}://{Request.Host}";
                    var images = await unitOfWork.iUniversityImagesRepository.GetImagesByUniversityIdAsync(universityId);
                    var imageUrls = images.Select(img => $"{baseUrl}/Images/{img.ImageName}").ToList();

                    // مسار الشعار
                    var logoUrl = university.Logo != null ? $"{baseUrl}/Images/{university.Logo}" : null;

                    // جلب الـ subadmins للجامعة
                    var subAdmins = await unitOfWork.userProfileRepositry.GetSubAdminsByUniversityIdAsync(universityId);

                    // جلب آخر بوست من الـ subadmins
                    var latestPost = await unitOfWork.iPostRepositry.GetLatestPostBySubAdmin(subAdmins.ToList());

                    if (latestPost == null)
                    {
                        return NotFound(new { Message = "No posts found for the SubAdmins of this university." });
                    }

                    // إعداد الاستجابة
                    var universityDetails = new
                    {
                        university.Name,
                        LogoUrl = logoUrl,  // استخدام مسار الشعار الكامل
                        university.Description,
                        UniversityImages = imageUrls,
                        university.Gmail,
                        university.Location,
                        university.PhoneNumber,
                        SubAdmins = subAdmins.Select(sa => new
                        {
                            sa.UserName,
                            sa.Email,
                            ImageUrl =  $"{baseUrl}/Images/{sa.Image}" // مسار الصورة
                        }), // عرض البيانات الأساسية للـ SubAdmins
                        LatestPost = new
                        {
                            latestPost.Title,
                            latestPost.Description,
                            PostImageUrl = latestPost.PostImage != null ? $"{baseUrl}/Images/{latestPost.PostImage}" : null

                        }
                    };

                    return Ok(universityDetails);
                }

                */

		[HttpGet("GetUniversityDetails/{universityId}")]
		public async Task<IActionResult> GetUniversityDetails(int universityId)
		{
			// جلب تفاصيل الجامعة
			var university = await unitOfWork.iUniversityRepositry.GetUniversityByIdAsync(universityId);

			if (university == null)
			{
				return NotFound(new { Message = "University not found" });
			}

			string baseUrl = $"{Request.Scheme}://{Request.Host}";

			// جلب الصور الخاصة بالجامعة
			var images = await unitOfWork.iUniversityImagesRepository.GetImagesByUniversityIdAsync(universityId);
			var imageUrls = images?.Select(img => $"{baseUrl}/Images/{img.ImageName}").ToList() ?? new List<string>();

			// مسار الشعار
			var logoUrl = university.Logo != null ? $"{baseUrl}/Images/{university.Logo}" : null;

			// جلب الـ subadmins للجامعة
			var subAdmins = await unitOfWork.userProfileRepositry.GetSubAdminsByUniversityIdAsync(universityId);

			// جلب آخر بوست من الـ subadmins
			var latestPost = subAdmins != null ? await unitOfWork.iPostRepositry.GetLatestPostBySubAdmin(subAdmins.ToList()) : null;

			// جلب آخر مسابقة للجامعة
			var competition = await unitOfWork.iCompetitionRepository.GetLastCompetitionByUniversityIdAsync(universityId);

			// إعداد الاستجابة
			var universityDetails = new
			{
				university.UniversityId,
				university.Name,
				LogoUrl = logoUrl,
				university.Description,
				UniversityImages = imageUrls,
				university.Gmail,
				university.Location,
				university.PhoneNumber,
				SubAdmins = subAdmins?.Select(sa => new
				{
					sa.UserName,
					sa.Email,
					sa.Description,
					ImageUrl = sa.Image != null ? $"{baseUrl}/Images/{sa.Image}" : null
				}),
				LatestPost = latestPost != null ? new
				{
					latestPost.Title,
					latestPost.Description,
					PostImageUrl = latestPost.PostImage != null ? $"{baseUrl}/Images/{latestPost.PostImage}" : null
				} : null,
				LastCompetition = competition != null ? new
				{
					competition.CompetitionId,
					competition.Name,
					CompetitionImageUrl = competition.Image != null ? $"{baseUrl}/Images/{competition.Image}" : null,
					competition.Description,


				} : null
			};

			return Ok(universityDetails);
		}
	}
}
