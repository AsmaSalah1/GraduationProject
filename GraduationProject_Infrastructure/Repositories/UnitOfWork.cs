﻿using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Infrastructure.Repositories
{
	public class UnitOfWork : IUnitOfWork, IDisposable
	{
		private readonly ApplicationDbContext dbContext;
		private readonly SignInManager<User> signInManager;
		private readonly IConfiguration configuration;

		public IAuthRepositry authRepositry { get; }

		public IUserProfileRepositry userProfileRepositry { get; }
		public UserManager<User> UserManager { get; }

		public IQAARepositry iQAARepositry { get; }

		public IRuleRepositry iRuleRepositry { get; }

		public IPersonalExperianceRepositry personalExperianceRepositry { get; }

		public ITeamRepository teamRepository { get; }

		public IParticipantRepository iparticipantRepository { get; }

		public IPostRepositry iPostRepositry { get; }

		public ICommentRepositry iCommentRepositry { get; }

		public ILikeRepository iLikeRepository { get; }
		public ICompetitionRepository iCompetitionRepository { get; }
		public ICompetitionImagesRepository iCompetitionImagesRepository { get; }
		public ISponsorRepository iSponsorRepository { get; }

		public IUniversityRepositry iUniversityRepositry { get; }

		public IUniversityImagesRepository iUniversityImagesRepository { get; }

		public async Task<int> SaveAsync()
	=> await dbContext.SaveChangesAsync();


		//نفسسسسسسها 
		//public async Task<int> SaveAsync()
		//{
		//	return await dbContext.SaveChangesAsync();
		//}
		public void Dispose()
		{
			//عشان الاوبجيكت ما يضل موجود دايما للابد
			dbContext.Dispose();
		}
		public UnitOfWork(ApplicationDbContext dbContext
			, UserManager<User> userManager,
			SignInManager<User> signInManager,
			IConfiguration configuration)
		{
			this.dbContext = dbContext;
			UserManager = userManager;
			this.signInManager = signInManager;
			this.configuration = configuration;
			authRepositry = new AuthRepositry(userManager, configuration, signInManager,dbContext);
		    userProfileRepositry=new UserProfileRepositry(userManager,dbContext);
			iQAARepositry=new QAARepositry(dbContext);
			iRuleRepositry=new RuleRepositry(dbContext);
			teamRepository = new TeamRepositry(dbContext);
			personalExperianceRepositry = new PersonalExperianceRepositry(dbContext);
			iparticipantRepository =new ParticipantRepository(dbContext);
			iPostRepositry =new PostRepositry(dbContext,userManager);
			iCommentRepositry = new CommentRepositry(dbContext);
			iLikeRepository = new LikeRepository(dbContext);
			iCompetitionRepository = new CompetitionRepository(dbContext);
			iCompetitionImagesRepository = new CompetitionImagesRepository(dbContext);
			iSponsorRepository = new SponsorRepository(dbContext);
			iUniversityRepositry = new UniversityRepositry(dbContext);
			iUniversityImagesRepository = new UniversityImagesRepository(dbContext);
		}
	}
}
