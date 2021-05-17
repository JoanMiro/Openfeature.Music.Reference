namespace Openfeature.Music.Reference
{
    using System;
    using System.IO;
    using System.Reflection;
    using Hellang.Middleware.ProblemDetails;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;
    using Models;
    using static Microsoft.AspNetCore.Mvc.CompatibilityVersion;

    /// <summary>
    /// Startup class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        /// <summary>
        /// Config for application
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IChordData, ChordData>();
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
            })
            .SetCompatibilityVersion(Version_3_0);

            services.AddProblemDetails();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc(
                    "v1",
                    new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "Openfeature Chord Data API",
                        Description = "A musical chord and scale reference API",
                        Contact = new OpenApiContact
                        {
                            Name = "Gordon Mackie",
                            Email = "gmm@msn.com",
                            Url = new Uri("https://www.openfeature.com"),
                        }
                    }
                );

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseProblemDetails();
            app.UseSwagger();
            app.UseSwaggerUI(
                c =>
                {
                    c.SwaggerEndpoint("./swagger/v1/swagger.json", "Openfeature Music Reference Data V1");
                    c.RoutePrefix = string.Empty;
                });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}