using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Helper
{
	public class GlobalExceptionHandler : IExceptionHandler
	{
		private readonly ILogger<GlobalExceptionHandler> logger;

		public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
		{
			this.logger = logger;
		}
		public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
		{
			logger.LogError(exception, "Catch Error", exception.Message);
			var problemDetails = new ProblemDetails
			{
				Status = StatusCodes.Status500InternalServerError,
				Title = "Server error",
				Detail = exception.Message
			};
			httpContext.Response.StatusCode = problemDetails.Status.Value;

			await httpContext.Response
				.WriteAsJsonAsync(problemDetails);
			return true;
		}
	}
}
