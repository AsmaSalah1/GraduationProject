using GraduationProject_Core.Dtos.QAA;
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GraduationProject_API.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class QAAController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;

		public QAAController(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}
		[Authorize(Roles = "Admin")]
		[HttpPost("AddQAA")]
		public async Task<IActionResult> AddQAA(CreateQAADto dto)
		{
			if (!ModelState.IsValid) { 
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
			var result = await unitOfWork.iQAARepositry.Add_QAA_Async(userId.Value, dto);
			if (result)
			{
				return Ok("Question and Answer added successfully");
			}
			return BadRequest("There is an error");
		}
		
		[Authorize(Roles ="Admin")]
		[HttpPut("UpdateQAA/{qaaId}")]
		public async Task<IActionResult> UpdateQAA(int qaaId, UpdateQAADto dto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest("Invalid input data");
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

			var result = await unitOfWork.iQAARepositry.Update_QAA_Async(qaaId, userId.Value, dto);
			if (result)
			{
				return Ok("Question and Answer updated successfully");
			}
		
			return BadRequest("Error updating the QAA");
		}
	
		[Authorize(Roles ="Admin")]
		[HttpDelete("DeleteQAA/{qaaId}")]
		public async Task<IActionResult> DeleteQAA(int qaaId)
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

			var result = await unitOfWork.iQAARepositry.Delete_QAA_Async(userId.Value, qaaId);
			if (result)
			{
				return Ok("Question and Answer deleted successfully");
			}

			return NotFound("Question and Answer not found or unauthorized");
		}
		
		[HttpGet("GetAllQAA")]
		public async Task<IActionResult> GetAllQAA()
		{
			var qaas = await unitOfWork.iQAARepositry.GetAll_QAA_Async();
			if (qaas == null || !qaas.Any())
			{
				return NotFound("No Questions and Answers found.");
			}
			return Ok(qaas);
		}

	}
}
