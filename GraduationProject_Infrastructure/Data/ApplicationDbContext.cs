using GraduationProject_Core.Dtos.Auth;
using GraduationProject_Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace GraduationProject_Infrastructure.Data
{
	public class ApplicationDbContext: IdentityDbContext<User, IdentityRole<int>, int>//لانه كمان الايدي للرول بدي اقله انه انتجر مش سترنج
	{
		public ApplicationDbContext(DbContextOptions options ):base(options) {
			
		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			

			builder.Entity<IdentityRole<int>>().HasData(
	           new IdentityRole<int> { Id = 1, Name = "Admin", NormalizedName = "ADMIN" },
	           new IdentityRole<int> { Id = 2, Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
	           new IdentityRole<int> { Id = 3, Name = "User", NormalizedName = "USER" }
             );

			var hasher = new PasswordHasher<User>();
			var adminUser = new User
			{
				Id=-1,
				UserName = "mustafaalrifaya",
				NormalizedUserName = "MUSTAFAALRIFAYA",
				Email = "mustafaalrifaya3@gmail.com",
				NormalizedEmail = "MUSTAFAALRIFAYA3@GMAIL.COM",
				EmailConfirmed = true,
				Gender=Gender.Male
			};
			        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin!!123");
			//         var superAdmin = new User
			//         {
			//             UserName = "superAdmin@comp.com",
			//             NormalizedUserName = "SUPERADMIN@COMP.COM",
			//             Email = "superAdmin@comp.com",
			//             NormalizedEmail = "SUPERADMIN@COMP.COM",
			//             EmailConfirmed = true,
			//         };
			//         superAdmin.PasswordHash = hasher.HashPassword(adminUser, "SuperAdmin@1212");
			//         var user = new User
			//         {
			//             UserName = "user@comp.com",
			//             NormalizedUserName = "User@COMP.COM",
			//             Email = "user@comp.com",
			//             NormalizedEmail = "USER@COMP.COM",
			//             EmailConfirmed = true,
			//         };
			//         user.PasswordHash = hasher.HashPassword(adminUser, "User@1212");
			builder.Entity<User>().HasData( adminUser);
			builder.Entity<IdentityUserRole<int>>().HasData(
				new IdentityUserRole<int> { RoleId = 1, UserId = adminUser.Id }
				//new IdentityUserRole<int> { RoleId = 2, UserId = user.Id },
				//new IdentityUserRole<int> { RoleId = 3, UserId = superAdmin.Id }
				);
			base.OnModelCreating(builder);
			builder.Entity<TeamParticipant>().HasKey(x => new { x.TeamId, x.ParticipantId });
			builder.Entity<UniversityCompetition>().HasKey(r => new { r.UniversityId, r.CompetitionID });
			builder.Entity<SponsorComptiition>().HasKey(r => new { r.SponsorID, r.CompetitionID });
			builder.Entity<TeamCompetition>().HasKey(r => new { r.TeamId, r.CompetitionID });
			//builder.Entity<User>(u =>
			//u.Property(i => i.Image).HasDefaultValue("C:\\Users\\user\\Desktop\\Man defult image.png"));
			//builder.Entity<User>()
			//     .HasOne(u => u.University).WithMany(u => u.Users).HasForeignKey(u => u.UniversityId)
			//     .OnDelete(DeleteBehavior.SetNull);

			builder.Entity<User>().HasOne(u => u.PersonalExperience).WithOne(p => p.User)
		   .HasForeignKey<PersonalExperience>(p => p.UserId)
		  .OnDelete(DeleteBehavior.Cascade); // عند حذف المستخدم، يتم حذف تجربته الشخصية أيضًا
          
		}
		public DbSet<User> User { get; set; }
		public DbSet<Team> Teams { get; set; }
		public DbSet<Competition> Competitions { get; set; }
		public DbSet<Participant> Participants { get; set; }
		//public DbSet<Role> Roles { get; set; }
		public DbSet<Rule> Rules { get; set; }
		public DbSet<Sponsor> Sponsors { get; set; }
		public DbSet<University> Universities { get; set; }
		public DbSet<UniversityCompetition> UniversityCompetitions { get; set; }
		public DbSet<QAA> QAAs { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<PersonalExperience> PersonalExperiences { get; set; }
		public DbSet<TeamCompetition> TeamsCompetitions { get; set; }
		public DbSet<TeamParticipant> TeamsParticipants { get; set; }
		public DbSet<SponsorComptiition> SponsorComptiitions { get; set; }
		public DbSet<CompetitionImages> CompetitionImages { get; set; }
		public DbSet<UniversityImages> UniversityImages { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DbSet<Comment> Comments { get; set; }
	}
}
