using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Oqtane.Modules;
using Oqtane.Models;
using Oqtane.Infrastructure;
using Oqtane.Interfaces;
using Oqtane.Enums;
using Oqtane.Repository;
using GIBS.Module.Carousel.Repository;
using System.Threading.Tasks;

namespace GIBS.Module.Carousel.Manager
{
    public class CarouselManager : MigratableModuleBase, IInstallable, IPortable, ISearchable
    {
        private readonly ICarouselRepository _CarouselRepository;
        private readonly IDBContextDependencies _DBContextDependencies;

        public CarouselManager(ICarouselRepository CarouselRepository, IDBContextDependencies DBContextDependencies)
        {
            _CarouselRepository = CarouselRepository;
            _DBContextDependencies = DBContextDependencies;
        }

        public bool Install(Tenant tenant, string version)
        {
            return Migrate(new CarouselContext(_DBContextDependencies), tenant, MigrationType.Up);
        }

        public bool Uninstall(Tenant tenant)
        {
            return Migrate(new CarouselContext(_DBContextDependencies), tenant, MigrationType.Down);
        }

        public string ExportModule(Oqtane.Models.Module module)
        {
            string content = "";
            List<Models.Carousel> Carousels = _CarouselRepository.GetCarousels(module.ModuleId).ToList();
            if (Carousels != null)
            {
                content = JsonSerializer.Serialize(Carousels);
            }
            return content;
        }

        public void ImportModule(Oqtane.Models.Module module, string content, string version)
        {
            List<Models.Carousel> Carousels = null;
            if (!string.IsNullOrEmpty(content))
            {
                Carousels = JsonSerializer.Deserialize<List<Models.Carousel>>(content);
            }
            if (Carousels != null)
            {
                foreach(var Carousel in Carousels)
                {
                    _CarouselRepository.AddCarousel(new Models.Carousel { ModuleId = module.ModuleId, Title = Carousel.Title, ImageUrl = Carousel.ImageUrl, Description = Carousel.Description });
                }
            }
        }

        public Task<List<SearchContent>> GetSearchContentsAsync(PageModule pageModule, DateTime lastIndexedOn)
        {
           var searchContentList = new List<SearchContent>();

           foreach (var Carousel in _CarouselRepository.GetCarousels(pageModule.ModuleId))
           {
               if (Carousel.ModifiedOn >= lastIndexedOn)
               {
                   searchContentList.Add(new SearchContent
                   {
                       EntityName = "GIBSCarousel",
                       EntityId = Carousel.CarouselId.ToString(),
                       Title = Carousel.Title,
                       Body = Carousel.Description,
                       ContentModifiedBy = Carousel.ModifiedBy,
                       ContentModifiedOn = Carousel.ModifiedOn
                   });
               }
           }

           return Task.FromResult(searchContentList);
        }
    }
}
