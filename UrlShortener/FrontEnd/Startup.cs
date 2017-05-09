using System;
using Amazon.S3;
using LinkShrink;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.Semantics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FrontEnd
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();

            RandomShrinkStrategy StrategySupplier(IServiceProvider serviceProvider) =>
                new RandomShrinkStrategy(Configuration["AcceptableCharacters"].ToCharArray());

            services.AddSingleton<IShrinkStrategy, RandomShrinkStrategy>(StrategySupplier);

            S3LinkShrink LinkShrinkSupplier(IServiceProvider serviceProvider) =>
                new S3LinkShrink(serviceProvider.GetService<IAmazonS3>(),
                    serviceProvider.GetService<IShrinkStrategy>(),
                    Configuration["S3Bucket"],
                    Configuration["BaseUrl"]);

            services.AddSingleton<ILinkShrink, S3LinkShrink>(LinkShrinkSupplier);   
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Entry}/{action=Index}/");
            });
        }
    }
}
