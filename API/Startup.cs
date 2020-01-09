﻿using API.Utils;
using Logica.Alunos;
using Logica.Decorators;
using Logica.Models;
using Logica.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            var config = new Config(3);
            services.AddSingleton(config);

            services.AddSingleton(new CommandsConnectionString(Configuration["ConnectionString"]));
            services.AddSingleton(new QueriesConnectionString(Configuration["QueriesConnectionString"]));

            services.AddSingleton<SessionFactory>();

            services.AddHandlers();

            services.AddSingleton<Messages>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseMvc();
        }
    }
}
