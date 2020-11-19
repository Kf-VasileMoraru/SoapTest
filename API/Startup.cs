using API.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SoapTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace API
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
            services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });


            services.AddControllers();
            services.AddSingleton<CalculatorService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> loggerFactory)
        {
            /*           if (env.IsDevelopment())
                       {
                           app.UseDeveloperExceptionPage();
                       }

                       app.UseRouting();

                       app.UseAuthorization();

                       app.UseEndpoints(endpoints =>
                       {
                           endpoints.MapControllers();
                       });*/

        

            app.Use(async (context, next) =>
            {
                loggerFactory.LogInformation("LogInformation {0} {1} {2}", context.Request.Path, context.Request.QueryString, context.Request.HttpContext);
                await next.Invoke();
            });

            //loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            //loggerFactory.AddDebug();

            //app.UseMiddleware<SOAPEndpointMiddleware>();
            app.UseSOAPEndpoint<CalculatorService>("/CalculatorService.svc", new BasicHttpBinding());

            // This call to use MVC middleware from the project template is not necessary, but 
            // doesn't hurt anything so long as it comes after our UseMiddleware call. 
            //app.UseMvc();

        }
    }
}
