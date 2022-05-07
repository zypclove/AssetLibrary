using AssetLibrary.Api.Infrastructure;
using AssetLibrary.Api.Models.AutoMapperProfile;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AssetLibrary.Api
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
            #region ����swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "AssetLibrary.Api"
                });
                // ��ȡdll
                var path = typeof(Program).Assembly.Location;
                var basePath = Path.GetDirectoryName(path);
                // ΪSwagger�ӿ�����ע��·��
                var xmlPath = Path.Combine(basePath, "AssetLibrary.Api.xml");
                c.IncludeXmlComments(xmlPath);
            });
            #endregion


            #region �������ݿ�����
            string sqlServerConn = Configuration.GetValue<string>("SqlConnections:SqlServer");
            services.AddDbContext<AssetLibraryDbContext>(opt =>
            {
                opt.UseSqlServer(sqlServerConn);
            });
            #endregion

            services.AddAutoMapper(typeof(AssetLibraryProfile));


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "AssetLibrary.Api v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}