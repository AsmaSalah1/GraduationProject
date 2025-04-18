﻿using GraduationProject_Core.Dtos.Likes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Interfaces
{
	public interface ILikeRepository
	{
		Task<string> ToggleLikeAsync(int postId, int userId);
		Task<IEnumerable<GetLikesDto>> GetLikesByPostId(int postId);

	}
}
