using Microsoft.AspNetCore.Builder; 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Oqtane.Infrastructure;
using GIBS.Module.Carousel.Repository;
using GIBS.Module.Carousel.Services;

namespace GIBS.Module.Carousel.Startup
{
    public class ServerStartup : IServerStartup
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // not implemented
        }

        public void ConfigureMvc(IMvcBuilder mvcBuilder)
        {
            // not implemented
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ICarouselService, ServerCarouselService>();
            services.AddDbContextFactory<CarouselContext>(opt => { }, ServiceLifetime.Transient);
        }
    }
}
