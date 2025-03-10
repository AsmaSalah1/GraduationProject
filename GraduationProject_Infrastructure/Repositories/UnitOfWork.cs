using GraduationProject_Core.Interfaces;
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
		}
	}
}
