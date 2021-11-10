using EducateApp.Models;
using EducateApp.Models.Validators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EducateApp
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
            services.AddDbContext<AppCtx>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // ���������� ������� ���������� ������
            services.AddTransient<IPasswordValidator<User>, CustomPasswordValidator>(
                serv => new CustomPasswordValidator(8));

            // ���������� ������� ���������� ������������
            services.AddTransient<IUserValidator<User>, CustomUserValidator>();

            services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppCtx>()
                .AddDefaultTokenProviders();    // ��������� �������, ������� ���������� � ������ ��� �������������

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();    // ����������� �������������� ����������� ����� UseAuthorization()
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Account}/{action=Login}/{id?}");
            });
        }
    }
}
