using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using EphIt.Db.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using EphIt.Db.Enums;
using System;
using Serilog;
using EphIt.BL.Authorization;
using EphIt.BL.User;

namespace EphIt.Blazor.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {

            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
            services.AddSingleton(Log.Logger);

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddDbContext<EphItContext>(
                    options => 
                        options.UseSqlServer(Configuration.GetConnectionString("EphItDb"))
                    );
            services.AddHttpContextAccessor();
            services.AddScoped<IEphItUser, EphItUser>();
            services.AddScoped<IUserAuthorization, UserAuthorization>();

            services.AddAuthorization(options => 
            {
                options.AddPolicy("ScriptEdit", policy => policy.Requirements.Add(new EphItAuthRequirement(RBACActionsEnum.Modify, RBACObjectsId.Scripts)));
                options.AddPolicy("Script", policy => policy.Requirements.Add(new EphItAuthRequirement(null, RBACObjectsId.Scripts)));
                options.AddPolicy("ScriptRead", policy => policy.Requirements.Add(new EphItAuthRequirement(RBACActionsEnum.Read, RBACObjectsId.Scripts)));
                options.AddPolicy("ScriptDelete", policy => policy.Requirements.Add(new EphItAuthRequirement(RBACActionsEnum.Delete, RBACObjectsId.Scripts)));
            });
            services.AddTransient<IAuthorizationHandler, EphItAuthRequirementHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IEphItUser user, EphItContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            ConfigureAdministratorRole(user, db);

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
            });
        }
        public void ConfigureAdministratorRole(IEphItUser user, EphItContext db)
        {
            foreach(RBACActionsEnum action in Enum.GetValues(typeof(RBACActionsEnum)))
            {
                foreach(RBACObjectsId obj in Enum.GetValues(typeof(RBACObjectsId)))
                {
                    if(!db.RoleObjectAction
                        .Where(p => 
                            p.RbacActionId.Equals((short)action)
                            && p.RbacObjectId.Equals((short)obj)
                            && p.RoleId.Equals(1)
                        )
                        .Any())
                    {
                        var newRoleObjAction = new RoleObjectAction();
                        newRoleObjAction.RbacActionId = (short)action;
                        newRoleObjAction.RbacObjectId = (short)obj;
                        newRoleObjAction.RoleId = 1;
                        db.Add(newRoleObjAction);
                    }
                }
            }

            // Add current user to full admin role
            var vUser = user.RegisterCurrent();
            if (!db.RoleMembershipUser.Where(p => p.UserId == vUser.UserId && p.RoleId == 1).Any())
            {
                var newRoleMembership = new RoleMembershipUser();
                newRoleMembership.RoleId = 1;
                newRoleMembership.UserId = vUser.UserId;
                db.Add(newRoleMembership);
            }
            db.SaveChangesAsync();
        }
    }
}
