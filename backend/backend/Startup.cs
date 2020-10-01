using backend.DatabaseContext;
using backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // configuring entityFrameworkCore with sql server
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            // registering identity package, which work with database and authentication
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
            }).AddEntityFrameworkStores<ApplicationDbContext>() // configuring identity which database to use
              .AddErrorDescriber<RussianIdentityErrorDescriber>();

            services.AddScoped<IUserService, UserService>(); // DI, creates single service object for each new request

            // authentication service config, registering JWT authentication
            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["AuthSettings:Issuer"],
                    ValidAudience = Configuration["Clients:SpaAddress"],
                    RequireExpirationTime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["AuthSettings:Key"])),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            // enabling and configuring cross origin resource sharing for cookies, because server and SPA are running on different ports
            services.AddCors(options => {
                options.AddPolicy("AllowOrigin", corsPolicyBuilder => 
                {
                    corsPolicyBuilder.WithOrigins(Configuration["Clients:SpaAddress"])
                                     .AllowCredentials() // allowing XMLHttpRequest.withCredentials requests
                                     .AllowAnyMethod()
                                     .AllowAnyHeader();
                });
            });

            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("AllowOrigin");

            app.UseCookiePolicy(new CookiePolicyOptions 
            { 
                HttpOnly = HttpOnlyPolicy.Always, // XSS protection, only server has access to cookies
                //Secure = CookieSecurePolicy.Always // send cookies only through https
            });

            // check if cookies has token, if so, add it to header and proceed the pipeline
            app.Use(async (context, next) =>
            {
                var token = context.Request.Cookies["Token"];
                if (!string.IsNullOrEmpty(token))
                    context.Request.Headers.Add("Authorization", "Bearer " + token);

                await next();
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
