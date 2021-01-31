using EphIt.BL.Audit;
using EphIt.BL.Authorization;
using EphIt.BL.JobManager;
using EphIt.BL.Script;
using EphIt.BL.User;
using EphIt.Db.Models;
using Microsoft.AspNet.OData.Builder;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OData.Edm;
using OMyEF;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var configSection = Configuration.GetSection("EphItSettings");
            if(!String.IsNullOrEmpty(configSection["AzureADAuthentication"]))
            {
                if(configSection["AzureADAuthentication"] == "true")
                {
                    services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                        .AddMicrosoftIdentityWebApp(Configuration.GetSection("AzureAdApp"));
                }
            }
            AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
            services.AddSingleton(Log.Logger);

            services.AddControllersWithViews();
            services.AddRazorPages();
            services.AddOData();
            services.AddDbContext<EphItContext>(
                    options => 
                        options.UseSqlServer(Configuration.GetConnectionString("EphItDb"))
                    );
            services.AddHttpContextAccessor();
            services.AddScoped<IEphItUser, EphItUser>();
            services.AddScoped<IUserAuthorization, UserAuthorization>();
            services.AddScoped<IScriptManager, ScriptManager>();
            services.AddScoped<IAuditLogger, AuditLogger>();
            services.AddScoped<IJobManager, JobManager>();
            services.AddAuthorization(options => 
            {
                RBACObjectEnum[] objEnums = (RBACObjectEnum[]) Enum.GetValues(typeof(RBACObjectEnum));
                foreach (RBACActionEnum actionEnum in (RBACActionEnum[])Enum.GetValues(typeof(RBACActionEnum)))
                {
                    foreach(var objEnum in objEnums)
                    {
                        string name = $"{objEnum}{actionEnum}";
                        options.AddPolicy(name, policy => policy.Requirements.Add(new EphItAuthRequirement(actionEnum, objEnum)));
                    }
                }
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
#if DEBUG
            // this will drop the DB - don't deploy Debug
            
            var dropDb = Environment.GetEnvironmentVariable("ASPNETCORE_DROPDB");
            if(dropDb == "True")
            {
                db.Database.EnsureDeleted();
            }
#endif
            ConfigureDb(user, db);

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseSerilogRequestLogging();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                
                endpoints.MapRazorPages();
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
                endpoints.AddOMyEFRoute<EphItContext>();
            });
        }
        private IEdmModel GetEdmModel()
        {
            var odataBuilder = new ODataConventionModelBuilder();
            odataBuilder.EntitySet<Script>("Script");
            odataBuilder.EntitySet<Job>("Job");
            odataBuilder.EntitySet<ScriptVersion>("ScriptVersion");
            odataBuilder.EntitySet<Authentication>("Authentication");
            return odataBuilder.GetEdmModel();
        }
        public void ConfigureDb(IEphItUser user, EphItContext _context)
        {
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
                var internalUser = _context.User.Where(p => p.AuthenticationId.Equals((short)AuthenticationEnum.EphItInternal)).First();
                var admin = _context.Role.Where(p => p.Name.Equals("Administrators")).FirstOrDefault();
                if (admin == null)
                {
                    admin = new Role();
                    admin.CreatedByUserId = internalUser.UserId;
                    admin.Created = DateTime.UtcNow;
                    admin.Description = "Full administrator of all objects";
                    admin.Name = "Administrators";
                    admin.IsGlobal = true;
                    admin.Modified = DateTime.UtcNow;
                    admin.ModifiedByUserId = internalUser.UserId;
                    _context.Add(admin);
                    _context.SaveChanges();
                }
                foreach (RBACActionEnum a in (RBACActionEnum[])Enum.GetValues(typeof(RBACActionEnum)))
                {
                    foreach (RBACObjectEnum b in (RBACObjectEnum[])Enum.GetValues(typeof(RBACObjectEnum)))
                    {
                        if (!_context.RoleObjectAction.Where(p =>
                                p.RoleId.Equals(admin.RoleId)
                                && p.RbacObjectId.Equals((short)b)
                                && p.RbacActionId.Equals((short)a)
                            )
                            .Any()
                        )
                        {
                            var tempObject = new RoleObjectAction();
                            tempObject.RoleId = admin.RoleId;
                            tempObject.RbacObjectId = (short)b;
                            tempObject.RbacActionId = (short)a;
                            _context.Add(tempObject);
                        }
                    }
                }
                _context.SaveChanges();
            }
            // Add current user to full admin role
            var vUser = user.RegisterCurrent();
            if (!_context.RoleMembershipUser.Where(p => p.UserId == vUser.UserId && p.Role.Name.Equals("Administrators")).Any())
            {
                var admin = _context.Role.Where(p => p.Name.Equals("Administrators")).FirstOrDefault();
                var newRoleMembership = new RoleMembershipUser();
                newRoleMembership.RoleId = admin.RoleId;
                newRoleMembership.UserId = vUser.UserId;
                _context.Add(newRoleMembership);
            }
            _context.SaveChanges();
            IConfigurationSection configSection = Configuration.GetSection("AdminUsers");
            if(configSection != null)
            {
                foreach(var section in configSection.GetChildren())
                {
                    var paramDictionary = new Dictionary<string, string>();
                    string authType = "";
                    switch (section["AuthenticationType"])
                    {
                        case "AzureActiveDirectory":
                            authType = "AzureActiveDirectory";
                            paramDictionary = new Dictionary<string, string>()
                            {
                                { "TenantId", section["TenantId"] },
                                { "ObjectId", section["ObjectId"] },
                                { "UserName", section["UserName"] },
                                { "Name", section["Name"] },
                                { "Email", section["Email"] }
                            };
                            break;
                    }
                    if (!String.IsNullOrEmpty(authType))
                    {
                        var aUser = user.Register(authType, paramDictionary);
                        if (!_context.RoleMembershipUser.Where(p => p.UserId == aUser.UserId && p.Role.Name.Equals("Administrators")).Any())
                        {
                            var admin = _context.Role.Where(p => p.Name.Equals("Administrators")).FirstOrDefault();
                            var newRoleMembership = new RoleMembershipUser();
                            newRoleMembership.RoleId = admin.RoleId;
                            newRoleMembership.UserId = aUser.UserId;
                            _context.Add(newRoleMembership);
                        }
                        _context.SaveChanges();
                    }
                }
            }
        }
    }
}
