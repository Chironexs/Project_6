using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoomBooking.MVC.Context;
using RoomBooking.MVC.Services;
using RoomBooking.MVC.Services.Interface;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace RoomBooking.MVC
{
    public class Startup
    {
        protected IConfigurationRoot Configuration;

        public Startup()
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddXmlFile("appsettings.xml");
            Configuration = configurationBuilder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<RoomBookingContext>(builder =>
            {
                builder.UseSqlServer(Configuration["ConnectionStrings"]);
            });
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<RoomBookingContext>();
//            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IRoomService, RoomService>();
//            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IUserService, UserService>();
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
//           DbInitializer.CreateRole(roleManager);
//            DbInitializer.CreateFirstUser(userManager);
//            DbInitializer.CreateFirstAdmin(userManager);
        }
    }
}