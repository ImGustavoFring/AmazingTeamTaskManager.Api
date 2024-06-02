using AmazingTeamTaskManager.Core.Contexts;
using AmazingTeamTaskManager.Core.Infrastructure;
using AmazingTeamTaskManager.Core.Repositories.UserDbRepositories;
using AmazingTeamTaskManager.Core.Repositories.TaskManagerRepositories;
using AmazingTeamTaskManager.Core.Services;
using AmazingTeamTaskManager.Core.Services.UserServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace AmazingTeamTaskManager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var configuration = builder.Configuration;

            builder.Services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "AmazingTeamTaskManager.Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                        },
                        new string[] {}
                    }
                });
            });

            var jwtConfig = configuration.GetSection("JwtConfig");
            var key = Encoding.ASCII.GetBytes(jwtConfig["Secret"]);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig["Issuer"],
                    ValidAudience = jwtConfig["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddDbContext<UserDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("UserDbConnection")));

            builder.Services.AddDbContext<TaskManagerDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("TaskManagerDbConnection")));

            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<ProfileRepository>();
            builder.Services.AddScoped<MemberRepository>();
            builder.Services.AddScoped<TeamRepository>();
            builder.Services.AddScoped<ProjectRepository>();
            builder.Services.AddScoped<PlanRepository>();
            builder.Services.AddScoped<TaskFromPlanRepository>();
            builder.Services.AddScoped<NotificationRepository>();
            builder.Services.AddScoped<AttachmentRepository>();

            builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<UserManagementService>();
            builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AmazingTeamTaskManager API V1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
