using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Oqtane.Services;
using Oqtane.Shared;

namespace GIBS.Module.Carousel.Services
{
    public interface ICarouselService 
    {
        Task<List<Models.Carousel>> GetCarouselsAsync(int ModuleId);

        Task<Models.Carousel> GetCarouselAsync(int CarouselId, int ModuleId);

        Task<Models.Carousel> AddCarouselAsync(Models.Carousel Carousel);

        Task<Models.Carousel> UpdateCarouselAsync(Models.Carousel Carousel);

        Task DeleteCarouselAsync(int CarouselId, int ModuleId);
    }

    public class CarouselService : ServiceBase, ICarouselService
    {
        public CarouselService(HttpClient http, SiteState siteState) : base(http, siteState) { }

        private string Apiurl => CreateApiUrl("Carousel");

        public async Task<List<Models.Carousel>> GetCarouselsAsync(int ModuleId)
        {
            List<Models.Carousel> Carousels = await GetJsonAsync<List<Models.Carousel>>(CreateAuthorizationPolicyUrl($"{Apiurl}?moduleid={ModuleId}", EntityNames.Module, ModuleId), Enumerable.Empty<Models.Carousel>().ToList());
            return Carousels.OrderBy(item => item.Name).ToList();
        }

        public async Task<Models.Carousel> GetCarouselAsync(int CarouselId, int ModuleId)
        {
            return await GetJsonAsync<Models.Carousel>(CreateAuthorizationPolicyUrl($"{Apiurl}/{CarouselId}/{ModuleId}", EntityNames.Module, ModuleId));
        }

        public async Task<Models.Carousel> AddCarouselAsync(Models.Carousel Carousel)
        {
            return await PostJsonAsync<Models.Carousel>(CreateAuthorizationPolicyUrl($"{Apiurl}", EntityNames.Module, Carousel.ModuleId), Carousel);
        }

        public async Task<Models.Carousel> UpdateCarouselAsync(Models.Carousel Carousel)
        {
            return await PutJsonAsync<Models.Carousel>(CreateAuthorizationPolicyUrl($"{Apiurl}/{Carousel.CarouselId}", EntityNames.Module, Carousel.ModuleId), Carousel);
        }

        public async Task DeleteCarouselAsync(int CarouselId, int ModuleId)
        {
            await DeleteAsync(CreateAuthorizationPolicyUrl($"{Apiurl}/{CarouselId}/{ModuleId}", EntityNames.Module, ModuleId));
        }
    }
}
