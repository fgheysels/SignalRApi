using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SignalRAlerting.Hubs;
using Swashbuckle.AspNetCore.Swagger;

namespace SignalRAlerting
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
            services.AddSignalR();
            services.AddCors();

            services.AddSwaggerGen(c => c.SwaggerDoc("v1", new Info { Title = "SignalR Test" }));

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            app.UseSignalR(options =>
            {
                options.MapHub<ActionHub>("/hubs/actions");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SignalR Alerting"));

            app.UseHttpsRedirection();

            app.UseMvc();
        }
    }
}
