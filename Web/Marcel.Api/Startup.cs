using Marcel.Access;
using Marcel.Api.Filter;
using Marcel.DbModels.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Marcel.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(builder =>
             {
                 //  builder.AddSerilog <= can be used instead
                 builder.AddConsole();
                 builder.SetMinimumLevel(LogLevel.Information);
             });
            services.AddControllers();
            services.AddTransient<IDishAccess, DishAccess>();
            services.AddDbContext<MyDbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("MyDatabase")));

            services.AddMvc(options =>
            {
                var logger = services.BuildServiceProvider().GetService<ILoggerFactory>();
                options.Filters.Add(new ResposeExceptionFilterAttribute(logger.CreateLogger<ResposeExceptionFilterAttribute>()));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My Dish Sniff API", Version = "v1" });
            });

            var context = services.BuildServiceProvider().GetService<MyDbContext>();
            // migrate any database changes on startup (includes initial db creation)
            context.Database.Migrate();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Dish Sniff API");
                c.DefaultModelExpandDepth(2);
                c.DefaultModelsExpandDepth(-1);
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.EnableDeepLinking();
                c.EnableFilter();
                c.ShowExtensions();
                c.ShowCommonExtensions();
                c.EnableValidator();
            });
        }
    }
}