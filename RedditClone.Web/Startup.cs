using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RedditClone.Data;
using RedditClone.Data.Interfaces;
using AutoMapper;
using RedditClone.Models;
using RedditClone.Services.UserServices.Interfaces;
using RedditClone.Services.UserServices;
using RedditClone.Data.Repositories.Interfaces;
using RedditClone.Data.Repositories;
using RedditClone.Services.QuestServices.Interfaces;
using RedditClone.Services.QuestServices;

namespace RedditClone.Web
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddResponseCompression();

            RegisterServiceLayer(services);

            services.AddAutoMapper();

            services.AddAntiforgery();

            services.AddDbContext<RedditCloneDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("RedditCloneConnection")));
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password = new PasswordOptions()
                {
                    RequireUppercase = false,
                    RequireNonAlphanumeric = false,
                    RequireLowercase = false
                };
                options.User = new UserOptions()
                {
                    RequireUniqueEmail = true
                };
            })
                .AddDefaultUI(UIFramework.Bootstrap4)
                .AddEntityFrameworkStores<RedditCloneDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication()
                .AddFacebook(options =>
                {
                    options.AppId = this.Configuration.GetSection("ExternalAuthentication:Facebook:AppId").Value;
                    options.AppSecret = this.Configuration.GetSection("ExternalAuthentication:Facebook:AppSecret").Value;
                });

            services.AddMvc(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseResponseCompression();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "Identity",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }

        private void RegisterServiceLayer(IServiceCollection services)
        {
            services.AddScoped<IRedditCloneUnitOfWork, RedditCloneUnitOfWork>();

            services.AddScoped<IPostRepository, PostRepository>();

            services.AddScoped<IUserAccountService, UserAccountService>();

            services.AddScoped<IUserPostService, UserPostService>();

            services.AddScoped<IUserSubredditService, UserSubredditService>();

            services.AddScoped<IQuestPostService, QuestPostService>();

            services.AddScoped<IUserCommentService, UserCommentService>();
        }
    }
}
