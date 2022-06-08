using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using CourseDesign.API.Context;
using CourseDesign.API.Context.Repository;
using CourseDesign.API.Extensions;
using CourseDesign.API.Services;
using CourseDesign.API.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

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
            // ��ʼ�� - �������ݿ�Ǩ���ļ�����ӹ�����Ԫ�Ͳִ�
            services.AddDbContext<CourseDesignContext>(option =>
            {
                var connectionString = Configuration.GetConnectionString("CourseDesignConnection");
                option.UseSqlite(connectionString);
            })
            .AddUnitOfWork<CourseDesignContext>()                       // ��ӹ�����Ԫ
            .AddCustomRepository<TextPlan, TextPlanRepository>()        // ����ı���ƻ��Ĳִ�
            .AddCustomRepository<ImagePlan, ImagePlanRepository>()      // ���ͼƬ��ƻ��Ĳִ�
            .AddCustomRepository<User, UserRepository>()                // ����û��Ĳִ�
            .AddCustomRepository<TDoll, TDollRepository>()              // ���ս�����εĲִ�
            .AddCustomRepository<TDollObtain, TDollObtainRepository>(); // ����û�ӵ��ս�����εĲִ�
            // ����ע�� - ��ӷ���
            services.AddTransient<IImagePlanService, ImagePlanService>(); // ͼ����ƻ��ķ���
            services.AddTransient<ITextPlanService, TextPlanService>();   // ������ƻ��ķ���
            services.AddTransient<ILoginService, LoginService>();         // ��¼�ķ���
            services.AddTransient<ITDollService, TDollService>();         // ս�����εķ���
            // ����ע�� - ��Ӽ��
            //services.AddTransient<ITDollObtainCheck, TDollObtainCheck>();
            // TODO: 2 - API��ûʵ������ͼ����
            // ע��AutoMapperӳ����񣬲����AutoMapper����
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
