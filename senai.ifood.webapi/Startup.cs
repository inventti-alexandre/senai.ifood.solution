﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using senai.ifood.repository.Context;
using senai.ifood.domain.Contracts;
using senai.ifood.repository.Repositories;

namespace senai.ifood.webapi
{
    public class Startup
    {
        //############## CRIAR CONSTRUTOR DA CLASSE CONTEXT PARA CRIACAO DO BANCO DE DADOS
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration){
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //############# ADICIONE O SERVICES PARA CRIAR A CONEXAO DO BANCO DE DADOS PARA O CONTEXTO
            //############# O DefaultConnection PRECISA SER CRIADO NO appsettings.json
            //############# DEPOIS DE CRIAR O DBCONTEXT E O appsettings.json COM A CONEXAO COM O BANCO EXECUTAR O COMANDO ABAIXO:
            //############# (dotnet ef migrations add BancoInicial --startup-project ../senai.ifood.webapi/senai.ifood.webapi.csproj)
            //############# ESSE COMANDO CRIA O BANCO DE DADOS UTILIZANDO AS REGRAS DO CONTEXT E A CONEXAO DO json.
            services.AddDbContext<IFoodContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            //DEFINICAO DE DEPENDENCIA DO Repository PARA O Contracts
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            //INCLUSAO DO SERVICO DO MVC QUE EH RESPONSAVEL PELAS RESPOSTAS HTTP
            //O "AddJson" É PARA QUE SEJA POSSIVEL UTILIZAR OS RELACIONAMENTOS (ICollections) DAS ENTIDADE
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //UTILIZADO PARA DAR O START DO MVC NO PROJETO
            app.UseMvc();
        }
    }
}
