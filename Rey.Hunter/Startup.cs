﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rey.Hunter.Models.Identity;
using Rey.Identity.Services;
using Rey.Authority.Models;
using Rey.Hunter.Models.Business;
using Rey.Hunter.Models.Basic;

namespace Rey.Hunter {
    public partial class Startup {
        public void ConfigureServices(IServiceCollection services) {
            services.AddMvc();

            services.AddMvcJsonModelRef(options => {
                options.AddRef<Role>();
                options.AddRef<User>();
                options.AddRef<Talent>();
                options.AddRef<Company>();
                options.AddRef<Project>();
                options.AddNodeRef<LocationNode>();
                options.AddNodeRef<CategoryNode>();
                options.AddNodeRef<ChannelNode>();
                options.AddNodeRef<FunctionNode>();
                options.AddNodeRef<IndustryNode>();
            });

            services.AddMon(this.Configuration.GetSection("Mon"));
            services.AddModelLogging();

            services.AddReyAuthority(authority => {
                authority.AddMemoryAuthStore(options => {
                    options.Add(AuthTarget.Talent, AuthActivity.DataActivities);
                    options.Add(AuthTarget.Company, AuthActivity.DataActivities);
                    options.Add(AuthTarget.Project, AuthActivity.DataActivities);
                    options.Add(AuthTarget.Role, AuthActivity.DataActivities);
                    options.Add(AuthTarget.User, AuthActivity.DataActivities);
                });
            });

            services.AddReyIdentity<User, Role, Account>(identity => {
                identity.Scheme = "ReyHunterWeb";
                identity.AddUserStore<MongoUserStore<User>>();
                identity.AddRoleStore<MongoRoleStore<Role>>();
                identity.AddAccountStore<MongoAccountStore<Account>>();
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
            loggerFactory.AddConsole();

            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseCookieAuthentication(new CookieAuthenticationOptions {
                AuthenticationScheme = "ReyHunterWeb",
                AutomaticChallenge = true,
                LoginPath = "/Account/Login",
            });

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
