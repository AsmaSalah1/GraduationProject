﻿using GraduationProject_Core.Dtos.Comment;
using GraduationProject_Core.Dtos.Likes;
using GraduationProject_Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Core.Dtos.Post
{
	public class GetPostDto
	{
		public int PostId { get; set; }
		public string ProfileImage { get; set; }
		public string PosterName { get; set; }
		public string university { get;set; }
		public DateTime CreatedAt { get; set; }
		public string PostImage { get;set;}
		public string Title { get; set; }
		public string Description { get; set; }
	  	public double TotalLikes { get; set; }
      	public double TotalComments { get; set; }
		//public List<Comment> Comments { get; set; }
		public IEnumerable<GetCommentDto> Comments { get; set; }
		public IEnumerable<GetLikesDto> Likes { get; set; }
	}
}
