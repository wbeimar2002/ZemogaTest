using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using ZemogaTest.Repositories;
using ZemogaTest.Repositories.Repositories;
using ZemogaTest.Services.Interfaces;
using ZemogaTest.Sevices.Blogs;

using ZemogaTest.Sevices.Users;
using ZemogaTest.Utilities.Dtos;
using ZemogaTest.Utilities.Entities;

namespace ZemogaTest.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IBlogEngineRepository<User>, BlogEngineRepository<User>>();
            services.AddTransient<IBlogService, BlogService>();
            services.AddTransient<IBlogEngineRepository<Blog>, BlogEngineRepository<Blog>>();
            

            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettingsDto>(appSettingsSection);

            var appSettings = appSettingsSection.Get<AppSettingsDto>();
            var key = Encoding.ASCII.GetBytes(appSettings.SecureKey);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false

                };
            });
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });

            services.AddDbContext<ZemogaTestContext>(options =>
                options.UseSqlServer("Server=.;Database=dbZemogaTest;Trusted_Connection=True;"));

            services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            using (var scope = app.ApplicationServices.CreateScope())
            using (var context = scope.ServiceProvider.GetService<ZemogaTestContext>())
            {
                context.Database.EnsureCreated();
            }

        }
    }
}
