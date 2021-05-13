using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
 using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;

namespace HomeBudgetManagementMVC.Core
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //The templates place UseDeveloperExceptionPage or UseExceptionHandler early in the 
            //middleware pipeline so that it can catch unhandled exceptions
            //thrown in middleware that follows.
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //Static files, such as HTML, CSS, images, and
            //JavaScript, are assets an ASP.NET Core app serves directly to clients by default.
            //Static files are stored within the project's web root directory. The default directory is {content root}/wwwroot
            //Static files are accessible via a path relative to the web root. For example, the Web Application project templates contain several folders within the wwwroot folder:
            //wwwroot
            //css
            //js
            //lib
            //Consider creating the wwwroot/ images folder and adding the wwwroot / images / MyImage.jpg file.The URI format to access a file in the images folder is https://<hostname>/images/<image_file_name>. For example, https://localhost:5001/images/MyImage.jpg
            app.UseStaticFiles();

            //adds route matching to the middleware pipeline.
            //This middleware looks at the set of endpoints defined in the app,
            //and selects the best match based on the request.
            app.UseRouting();

            //adds route matching to the middleware pipeline. This middleware looks at the set of endpoints defined in the app,
            //and selects the best match based on the request.
            app.UseEndpoints(option => option.MapDefaultControllerRoute());
        }
    }
}
