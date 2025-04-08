
using GraduationProject_Core.Helper;
using GraduationProject_Core.Interfaces;
using GraduationProject_Core.Models;
using GraduationProject_Infrastructure.Data;
using GraduationProject_Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

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
              .AddDefaultTokenProviders().AddUserManager<UserManager<User>>();
			builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
			{
				options.TokenLifespan = TimeSpan.FromHours(3); // صلاحية 3 ساعات
			});

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
			///////////////////////////////////
			// إعداد Swagger مع دعم التوكن (Bearer Token)
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(options =>
			{
				options.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = "GraduationProject API",
					Version = "v1",
					Description = "API documentation for the Graduation Project system."
				});

				// إضافة تعريف التوكن (Bearer Authentication)
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					Scheme = "Bearer",
					BearerFormat = "JWT",
					In = ParameterLocation.Header,
					Description = "أدخل التوكن بصيغة: Bearer YOUR_ACCESS_TOKEN"
				});

				// إجبار جميع الـ Endpoints على استخدام التوكن
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new List<string>()
					}
				});
			});
///////////////////////////////////////////////////
			//من شات عشان الايكسيبشين هاندلر يزبط
			builder.Services.AddProblemDetails(); // لدعم ProblemDetails
			builder.Services.AddExceptionHandler<GlobalExceptionHandler>(); // تسجيل المعالج
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

			app.UseAuthentication();//new
            app.UseAuthorization();


			app.MapControllers();
			//عشان ال global exeption handler
			app.UseExceptionHandler();

			//app.UseExceptionHandler(opt => { });
			app.Run();
        }
    }
}
