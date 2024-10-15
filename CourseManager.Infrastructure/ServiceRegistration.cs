using CourseManager.Application.Repository;
using CourseManager.Domain.Entities;
using CourseManager.Infrastructure.Context;
using CourseManager.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text.RegularExpressions;

namespace CourseManager.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            //builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme).AddIdentityCookies();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("AppDb"));

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
            RegisterGenericRepositories(builder.Services);
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


        }

        private static void RegisterGenericRepositories(IServiceCollection services)
        {
            var entities = new[]
            {
        typeof(User),
        typeof(Enrollment),
        typeof(Course),
            };

            // Register the generic repository for each entity type
            foreach (var entityType in entities)
            {
                var repositoryInterface = typeof(IBaseRepository<>).MakeGenericType(entityType);
                var repositoryImplementation = typeof(BaseRepository<>).MakeGenericType(entityType);
                services.AddTransient(repositoryInterface, repositoryImplementation);
            }
        }
    }
}
