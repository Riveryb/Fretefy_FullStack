using System.Linq;
using Fretefy.Test.Domain.Entities;
using Fretefy.Test.Domain.Interfaces;
using Fretefy.Test.Domain.Interfaces.Repositories;
using Fretefy.Test.Domain.Services;
using Fretefy.Test.Infra.EntityFramework;
using Fretefy.Test.Infra.EntityFramework.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;

namespace Fretefy.Test.WebApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // DbContext (não precisa registrar DbContext base separadamente)
            services.AddDbContext<TestDbContext>(options =>
            {
                options.UseSqlite("Data Source=Data\\Test.db");
            });

            // Repositórios
            services.AddScoped<ICidadeRepository, CidadeRepository>();
            services.AddScoped<IRegiaoRepository, RegiaoRepository>();

            // Serviços de domínio
            services.AddScoped<ICidadeService, CidadeService>();
            services.AddScoped<IRegiaoService, RegiaoService>();

            // Controllers + Newtonsoft (ignora ciclos de navegação)
            services.AddControllers()
                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            // (opcional) CORS pro Angular
            services.AddCors(opt =>
                opt.AddPolicy("frontend", p => p
                    .AllowAnyHeader().AllowAnyMethod()
                    .WithOrigins("http://localhost:4200")));

            services.AddSwaggerGen();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();

            app.UseCors("frontend"); // opcional

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI();

            // -------- SEED RÁPIDO (roda só se vazio) --------
            using var scope = app.ApplicationServices.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<TestDbContext>();

            if (!ctx.Cidades.Any())
            {
                var sp  = new Cidade("São Paulo", "SP");
                var ctba= new Cidade("Curitiba", "PR");
                var rj  = new Cidade("Rio de Janeiro", "RJ");
                var bh  = new Cidade("Belo Horizonte", "MG");
                var bsb = new Cidade("Brasília", "DF");

                ctx.Cidades.AddRange(sp, ctba, rj, bh, bsb);
                ctx.SaveChanges();
            }

            if (!ctx.Regioes.Any())
            {
                // pega por nome/UF pra garantir IDs corretos
                var sp  = ctx.Cidades.FirstOrDefault(c => c.Nome == "São Paulo" && c.UF == "SP");
                var rj  = ctx.Cidades.FirstOrDefault(c => c.Nome == "Rio de Janeiro" && c.UF == "RJ");
                var bh  = ctx.Cidades.FirstOrDefault(c => c.Nome == "Belo Horizonte" && c.UF == "MG");
                var ctba= ctx.Cidades.FirstOrDefault(c => c.Nome == "Curitiba" && c.UF == "PR");

                if (sp != null && rj != null && bh != null)
                {
                    var sudeste = new Regiao("Sudeste Teste");
                    sudeste.RegiaoCidades.Add(new RegiaoCidade(sudeste.Id, sp.Id));
                    sudeste.RegiaoCidades.Add(new RegiaoCidade(sudeste.Id, rj.Id));
                    sudeste.RegiaoCidades.Add(new RegiaoCidade(sudeste.Id, bh.Id));
                    ctx.Regioes.Add(sudeste);
                }

                if (sp != null && ctba != null)
                {
                    var centroSul = new Regiao("Centro-Sul Teste");
                    centroSul.RegiaoCidades.Add(new RegiaoCidade(centroSul.Id, sp.Id));
                    centroSul.RegiaoCidades.Add(new RegiaoCidade(centroSul.Id, ctba.Id));
                    ctx.Regioes.Add(centroSul);
                }

                ctx.SaveChanges();
            }
            // -------- FIM SEED --------
        }
    }
}
