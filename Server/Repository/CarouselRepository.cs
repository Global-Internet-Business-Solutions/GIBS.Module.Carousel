using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using Oqtane.Modules;

namespace GIBS.Module.Carousel.Repository
{
    public interface ICarouselRepository
    {
        IEnumerable<Models.Carousel> GetCarousels(int ModuleId);
        Models.Carousel GetCarousel(int CarouselId);
        Models.Carousel GetCarousel(int CarouselId, bool tracking);
        Models.Carousel AddCarousel(Models.Carousel Carousel);
        Models.Carousel UpdateCarousel(Models.Carousel Carousel);
        void DeleteCarousel(int CarouselId);
    }

    public class CarouselRepository : ICarouselRepository, ITransientService
    {
        private readonly IDbContextFactory<CarouselContext> _factory;

        public CarouselRepository(IDbContextFactory<CarouselContext> factory)
        {
            _factory = factory;
        }

        public IEnumerable<Models.Carousel> GetCarousels(int ModuleId)
        {
            using var db = _factory.CreateDbContext();
            return db.Carousel.Where(item => item.ModuleId == ModuleId).ToList();
        }

        public Models.Carousel GetCarousel(int CarouselId)
        {
            return GetCarousel(CarouselId, true);
        }

        public Models.Carousel GetCarousel(int CarouselId, bool tracking)
        {
            using var db = _factory.CreateDbContext();
            if (tracking)
            {
                return db.Carousel.Find(CarouselId);
            }
            else
            {
                return db.Carousel.AsNoTracking().FirstOrDefault(item => item.CarouselId == CarouselId);
            }
        }

        public Models.Carousel AddCarousel(Models.Carousel Carousel)
        {
            using var db = _factory.CreateDbContext();
            db.Carousel.Add(Carousel);
            db.SaveChanges();
            return Carousel;
        }

        public Models.Carousel UpdateCarousel(Models.Carousel Carousel)
        {
            using var db = _factory.CreateDbContext();
            db.Entry(Carousel).State = EntityState.Modified;
            db.SaveChanges();
            return Carousel;
        }

        public void DeleteCarousel(int CarouselId)
        {
            using var db = _factory.CreateDbContext();
            Models.Carousel Carousel = db.Carousel.Find(CarouselId);
            db.Carousel.Remove(Carousel);
            db.SaveChanges();
        }
    }
}
