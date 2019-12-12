using API.Utils;
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

            services.AddSingleton(new SessionFactory(Configuration["ConnectionString"]));

            services.AddTransient<UnitOfWork>();

            // Queries:

            services.AddTransient<IQueryHandler<RecuperarAlunosQuery, List<AlunoDto>>, RecuperarAlunosQueryHandler>();

            // Commands:

            services.AddTransient<ICommandHandler<EditarInformacoesPessoaisCommand>>(sp =>
            {
                return new AuditLoggingDecorator<EditarInformacoesPessoaisCommand>(new DatabaseRetryDecorator<EditarInformacoesPessoaisCommand>(new EditarInformacoesPessoaisCommandHandler(sp.GetService<SessionFactory>()), sp.GetService<Config>()));
            });

            services.AddTransient<ICommandHandler<DesinscreverCursoCommand>, DesinscreverCursoCommandHandler>();
            services.AddTransient<ICommandHandler<TransferirCursoCommand>, TransferirCursoCommandHandler>();
            services.AddTransient<ICommandHandler<InscreverCursoCommand>, InscreverCursoCommandHandler>();
            services.AddTransient<ICommandHandler<DesregistrarAlunoCommand>, DesregistrarAlunoCommandHandler>();
            services.AddTransient<ICommandHandler<RegistrarAlunoCommand>, RegistrarAlunoCommandHandler>();

            services.AddSingleton<Messages>();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandler>();
            app.UseMvc();
        }
    }
}
