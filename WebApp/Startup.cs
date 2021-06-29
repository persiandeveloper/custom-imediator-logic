using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using ApplicationLogic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace WebApp
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

            services.AddMvc(options =>
            {
                options.ModelBinderProviders.RemoveType<CancellationTokenModelBinderProvider>();
                options.ModelBinderProviders.Insert(0, new CustomCancellationTokenModelBinderProvider());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "template", Version = "v1" });
            });

            services.AddMediator();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "template v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public class CustomCancellationTokenModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context?.Metadata.ModelType != typeof(CancellationToken))
            {
                return null;
            }

            return new CustomCancellationTokenModelBinder();
        }

        private class CustomCancellationTokenModelBinder : CancellationTokenModelBinder, IModelBinder
        {

            public CustomCancellationTokenModelBinder()
            {
            }

            public new async Task BindModelAsync(ModelBindingContext bindingContext)
            {
                await base.BindModelAsync(bindingContext);
                // combine the default token with a timeout
                if (bindingContext.Result.Model is CancellationToken cancellationToken)
                {
                    var timeoutCts = new CancellationTokenSource();
                    timeoutCts.CancelAfter(TimeSpan.FromSeconds(3));
                    var combinedCts =
                        CancellationTokenSource.CreateLinkedTokenSource(timeoutCts.Token, cancellationToken);
                    bindingContext.Result = ModelBindingResult.Success(combinedCts.Token);
                }
            }
        }
    }

    class TimeoutOptions
    {
        public int TimeoutSeconds { get; set; }
        public TimeSpan Timeout => TimeSpan.FromSeconds(TimeoutSeconds);
    }
}
