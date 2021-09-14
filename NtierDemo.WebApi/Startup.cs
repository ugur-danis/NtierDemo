using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using NtierDemo.Business.Abstract;
using NtierDemo.Business.Concrete;
using NtierDemo.DataAccess.Abstract;
using NtierDemo.DataAccess.Concrete.AdoNet;

namespace NtierDemo.WebApi
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NtierDemo.WebApi", Version = "v1" });
            });

            services.AddSingleton<IAuthorDal, AdoAuthorDal>(_ => new AdoAuthorDal(Configuration["ConnectionStrings:MySql"]));
            services.AddSingleton<IBookDal, AdoBookDal>(_ => new AdoBookDal(Configuration["ConnectionStrings:MySql"]));
            services.AddSingleton<IAuthorService, AuthorManager>();
            services.AddSingleton<IBookService, BookManager>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NtierDemo.WebApi v1"));
            }

            app.UseCors(x => x
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowed(origin => true)
                    .AllowCredentials());

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
