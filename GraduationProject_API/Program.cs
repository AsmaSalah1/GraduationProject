
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using GraduationProject_Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GraduationProject_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;//ما يحتاج رموز فريدة
                options.Password.RequireDigit = true;
                options.Password.RequiredUniqueChars = 0;
                options.SignIn.RequireConfirmedAccount = true;

			}).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAuthRepositry, AuthRepositry>();
            //builder.Services.AddScoped<IUserProfileRepositry, UserProfileRepositry>();
			builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Allow",
                    builder =>
                    {
                        builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
                    });
            });
			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //عشان الصور
			app.UseStaticFiles();
			app.UseHttpsRedirection();
            app.UseCors("Allow");

            app.UseAuthorization();


            app.MapControllers();
			//عشان ال global exeption handler
			app.UseExceptionHandler(opt => { });
			app.Run();
        }
    }
}
