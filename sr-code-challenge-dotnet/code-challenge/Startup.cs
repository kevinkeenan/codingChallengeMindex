using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using challenge.Data;
using Microsoft.EntityFrameworkCore;
using challenge.Repositories;
using challenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace code_challenge
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        // This method gets called by the runtime. Use this method to add services to the container.
        /// Added swagger and new comp repo/ comp service
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseInMemoryDatabase("EmployeeDB");
            });
            services.AddSwaggerGen();

            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddTransient<EmployeeDataSeeder>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ICompensationRepository, CompensationRepository>();
            services.AddScoped<ICompensationService, CompensationService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, EmployeeDataSeeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                seeder.Seed().Wait();
                seeder.CompSeed().Wait();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Test1 Api v1");
            });
            app.UseMvc();
        }
    }
}
