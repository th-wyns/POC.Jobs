﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POC.Jobs.Manager;
using POC.Jobs.Manager.Hangfire;
using POC.Jobs.State.Communication;
using POC.Jobs.State.Communication.SignalR;
using POC.Jobs.Storage;
using POC.Jobs.Storage.EntityFrameworkCore;

namespace POC.Jobs.Samples.ServerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => 
            { 
                options.AddPolicy("CorsPolicy", builder => builder.WithOrigins("http://localhost:4200")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()); 
            });

            services.AddSignalR();

            services.AddControllers();

            // DI
            services
                .AddTransient(typeof(IJobStore), typeof(JobStore))
                .AddTransient(typeof(IJobStateStore), typeof(JobStateStore))
                .AddTransient(typeof(JobStateManager), typeof(JobStateManager))
                .AddTransient(typeof(JobManager), typeof(JobManager))
                .AddTransient(typeof(IJobStateCommunication), typeof(SignalrClient));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<JobStateHub>("/jobstate");
            });
        }
    }
}
