using GraduationProject_Core.Dtos.Rule;
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
	public class RuleController : ControllerBase
	{
		private readonly IUnitOfWork unitOfWork;

		public RuleController(IUnitOfWork unitOfWork )
		{
			this.unitOfWork = unitOfWork;
		}

		[Authorize(Roles ="Admin")]
		[HttpPost("Add-Rule")]
		public async Task<IActionResult> AddRule(AddRuleDto dto)
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

			var result = await unitOfWork.iRuleRepositry.AddRuleAsync(userId.Value,dto);
			if (result== "Rule Added Successfully")
			{
				return Ok("Rule added successfully");
			}
			return BadRequest(result);
		}

		[HttpGet("Get-All-Rules")]
		public async Task<IActionResult> GetAllRules()
		{
			var rules = await unitOfWork.iRuleRepositry.GetAllRulesAsync();
			if (rules == null || !rules.Any())
			{
				return NotFound("No rules found");
			}
			return Ok(rules);
		}


		[Authorize(Roles ="Admin")]
		[HttpPut("Update-Rule/{ruleId}")]
		public async Task<IActionResult> UpdateRule(int ruleId, EditRuleDto dto)
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

			var result = await unitOfWork.iRuleRepositry.EditRuleAsync(userId.Value, ruleId, dto);
			if (result == "Rule updated successfully")
			{
				return Ok("Rule updated successfully");
			}

			return BadRequest(result);
		}

		[Authorize(Roles ="Admin")]
		[HttpDelete("Delete-Rule/{ruleId}")]
		public async Task<IActionResult> DeleteRule(int ruleId)
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

			var result = await unitOfWork.iRuleRepositry.DeleteRuleAsync(userId.Value, ruleId);
			if (result == "Rule deleted successfully")
			{
				return Ok("Rule deleted successfully");
			}

			return BadRequest(result);
		}

	}
}
