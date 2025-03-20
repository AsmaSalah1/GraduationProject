using GraduationProject_Core.Dtos.PersonalExperiance;
using GraduationProject_Core.Dtos.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
	public interface IPostRepositry
	{
		Task<string> AddPost(int userId, CreatePostDto dto);
		Task<PostPagedResponseDto<GetPostDto>> GetPostDtos(int PageIndex,int PageSize);
		Task<PostPagedResponseDto<GetPostDto>> PaginationAsync(IQueryable<GetPostDto> query, int PageIndex, int PageSize);
	    Task<string> DeletePost(int userId,int PostId);
	
	}
}
