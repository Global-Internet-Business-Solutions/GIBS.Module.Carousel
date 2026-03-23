using System.Collections.Generic;
using System.Threading.Tasks;

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
}
