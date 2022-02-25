using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using MediatR;
using Microsoft.EntityFrameworkCore;
using N5.Exam.Domain.CQRS.Handlers;
using N5.Exam.Domain.Filters;
using N5.Exam.Domain.Options;
using N5.Exam.Domain.Profiles;
using N5.Exam.Domain.Repositories;
using N5.Exam.Infrastructure;
using N5.Exam.Infrastructure.Models;
using Nest;

namespace N5.Exam.API
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
            services.AddDbContext<N5ExamContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions =>
                    {
                        sqlOptions.EnableRetryOnFailure(15, TimeSpan.FromSeconds(30), null);
                    }
                ));

            services.AddControllers(config => config.Filters.Add<RequestLoggerFilter>());
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "N5.Exam.API", Version = "v1" });
            });

            var url = Configuration["Elasticsearch:Url"];
            var defaultIndex = Configuration["Elasticsearch:Index"];

            var settings = new ConnectionSettings(new Uri(url))
                .DefaultIndex(defaultIndex)
                .DefaultMappingFor<Permission>(m => m
                    .PropertyName(p => p.Id, "id")
                );

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            services.Configure<KafkaSettings>(Configuration.GetSection(nameof(KafkaSettings)));

            services.AddMediatR(typeof(GetPermissionByIdQueryHandler));
            services.AddAutoMapper(typeof(PermissionProfile));

            services.AddScoped<IPermissionRepository, PermissionRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "N5.Exam.API v1"));
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
