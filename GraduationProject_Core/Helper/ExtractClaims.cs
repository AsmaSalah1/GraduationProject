using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Helper
{
	public class ExtractClaims
	{
		//بحط ؟ عند الانتجر انه ممكن ترجع نال
		public static int? ExtractUserId(string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var jwtToken = tokenHandler.ReadJwtToken(token);
				var userIdClaims = jwtToken.Claims.FirstOrDefault(t =>
				t.Type == ClaimTypes.NameIdentifier);
				if (userIdClaims != null && int.TryParse(userIdClaims.Value, out int userId))
				{
					return userId;
				}
				return null;

			}
			catch (Exception ex)
			{
				return null;
			}
		}
		public static List<string> ExtractRoles(string token)
		{
			try
			{
				var tokenHandler = new JwtSecurityTokenHandler();
				var jwtToken = tokenHandler.ReadJwtToken(token);

				// استخراج جميع الـ roles من الـ claims
				var rolesClaims = jwtToken.Claims.Where(t => t.Type == ClaimTypes.Role)
					.Select(t => t.Value).ToList();

				return rolesClaims;
			}
			catch (Exception ex)
			{
				return new List<string>(); // إرجاع قائمة فارغة في حال حدوث خطأ
			}
		}

	}
}
