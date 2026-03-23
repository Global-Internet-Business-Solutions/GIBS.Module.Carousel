using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Oqtane.Services;
using GIBS.Module.Carousel.Services;

namespace GIBS.Module.Carousel.Startup
{
    public class ClientStartup : IClientStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            if (!services.Any(s => s.ServiceType == typeof(ICarouselService)))
            {
                services.AddScoped<ICarouselService, ClientCarouselService>();
            }
        }
    }
}
