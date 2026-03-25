using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using Oqtane.Models;
using Oqtane.Security;
using Oqtane.Shared;
using GIBS.Module.Carousel.Repository;

namespace GIBS.Module.Carousel.Services
{
    public class ServerCarouselService : ICarouselService
    {
        private readonly ICarouselRepository _CarouselRepository;
        private readonly IUserPermissions _userPermissions;
        private readonly ILogManager _logger;
        private readonly IHttpContextAccessor _accessor;
        private readonly Alias _alias;

        public ServerCarouselService(ICarouselRepository CarouselRepository, IUserPermissions userPermissions, ITenantManager tenantManager, ILogManager logger, IHttpContextAccessor accessor)
        {
            _CarouselRepository = CarouselRepository;
            _userPermissions = userPermissions;
            _logger = logger;
            _accessor = accessor;
            _alias = tenantManager.GetAlias();
        }

        public Task<List<Models.Carousel>> GetCarouselsAsync(int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_CarouselRepository.GetCarousels(ModuleId).OrderByDescending(item => item.OrderBy).ToList());
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Carousel Get Attempt {ModuleId}", ModuleId);
                return null;
            }
        }

        public Task<Models.Carousel> GetCarouselAsync(int CarouselId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.View))
            {
                return Task.FromResult(_CarouselRepository.GetCarousel(CarouselId));
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Carousel Get Attempt {CarouselId} {ModuleId}", CarouselId, ModuleId);
                return null;
            }
        }

        public Task<Models.Carousel> AddCarouselAsync(Models.Carousel Carousel)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Carousel.ModuleId, PermissionNames.Edit))
            {
                Carousel = _CarouselRepository.AddCarousel(Carousel);
                _logger.Log(LogLevel.Information, this, LogFunction.Create, "Carousel Added {Carousel}", Carousel);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Carousel Add Attempt {Carousel}", Carousel);
                Carousel = null;
            }
            return Task.FromResult(Carousel);
        }

        public Task<Models.Carousel> UpdateCarouselAsync(Models.Carousel Carousel)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, Carousel.ModuleId, PermissionNames.Edit))
            {
                Carousel = _CarouselRepository.UpdateCarousel(Carousel);
                _logger.Log(LogLevel.Information, this, LogFunction.Update, "Carousel Updated {Carousel}", Carousel);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Carousel Update Attempt {Carousel}", Carousel);
                Carousel = null;
            }
            return Task.FromResult(Carousel);
        }

        public Task DeleteCarouselAsync(int CarouselId, int ModuleId)
        {
            if (_userPermissions.IsAuthorized(_accessor.HttpContext.User, _alias.SiteId, EntityNames.Module, ModuleId, PermissionNames.Edit))
            {
                _CarouselRepository.DeleteCarousel(CarouselId);
                _logger.Log(LogLevel.Information, this, LogFunction.Delete, "Carousel Deleted {CarouselId}", CarouselId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Carousel Delete Attempt {CarouselId} {ModuleId}", CarouselId, ModuleId);
            }
            return Task.CompletedTask;
        }
    }
}
