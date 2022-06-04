using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using CourseDesign.API.Context;
using CourseDesign.API.Context.Repository;
using CourseDesign.API.Extensions;
using CourseDesign.API.Services;
using CourseDesign.API.Services.Interfaces;
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
using System.Linq;
using System.Threading.Tasks;

namespace CourseDesign.API
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
            // 初始化 - 生成数据库迁移文件、添加工作单元和仓储
            services.AddDbContext<CourseDesignContext>(option =>
            {
                var connectionString = Configuration.GetConnectionString("CourseDesignConnection");
                option.UseSqlite(connectionString);
            })
            .AddUnitOfWork<CourseDesignContext>()                  // 添加工作单元
            .AddCustomRepository<TextTask, TextTaskRepository>()   // 添加文本类计划的仓储
            .AddCustomRepository<ImageTask, ImageTaskRepository>() // 添加图片类计划的仓储
            .AddCustomRepository<User, UserRepository>()           // 添加用户的仓储
            .AddCustomRepository<TDoll, TDollRepository>();        // 添加战术人形的仓储
            // 依赖注入 - 添加服务
            services.AddTransient<IImageTaskService, ImageTaskService>();
            services.AddTransient<ITextTaskService, TextTaskService>();
            services.AddTransient<ILoginService, LoginService>();
            // 注册AutoMapper映射服务，并添加AutoMapper配置
            var automapperConfig = new MapperConfiguration(config =>
            {
                config.AddProfile(new AutoMapperProfile());
            });
            services.AddSingleton(automapperConfig.CreateMapper());

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CourseDesign.API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CourseDesign.API v1"));
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
