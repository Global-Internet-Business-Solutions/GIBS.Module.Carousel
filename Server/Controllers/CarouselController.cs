using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Oqtane.Shared;
using Oqtane.Enums;
using Oqtane.Infrastructure;
using GIBS.Module.Carousel.Services;
using Oqtane.Controllers;
using System.Net;
using System.Threading.Tasks;

namespace GIBS.Module.Carousel.Controllers
{
    [Route(ControllerRoutes.ApiRoute)]
    public class CarouselController : ModuleControllerBase
    {
        private readonly ICarouselService _CarouselService;

        public CarouselController(ICarouselService CarouselService, ILogManager logger, IHttpContextAccessor accessor) : base(logger, accessor)
        {
            _CarouselService = CarouselService;
        }

        // GET: api/<controller>?moduleid=x
        [HttpGet]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<IEnumerable<Models.Carousel>> Get(string moduleid)
        {
            int ModuleId;
            if (int.TryParse(moduleid, out ModuleId) && IsAuthorizedEntityId(EntityNames.Module, ModuleId))
            {
                return await _CarouselService.GetCarouselsAsync(ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Carousel Get Attempt {ModuleId}", moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // GET api/<controller>/5
        [HttpGet("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.ViewModule)]
        public async Task<Models.Carousel> Get(int id, int moduleid)
        {
            Models.Carousel Carousel = await _CarouselService.GetCarouselAsync(id, moduleid);
            if (Carousel != null && IsAuthorizedEntityId(EntityNames.Module, Carousel.ModuleId))
            {
                return Carousel;
            }
            else
            { 
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Carousel Get Attempt {CarouselId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return null;
            }
        }

        // POST api/<controller>
        [HttpPost]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Carousel> Post([FromBody] Models.Carousel Carousel)
        {
            if (ModelState.IsValid && IsAuthorizedEntityId(EntityNames.Module, Carousel.ModuleId))
            {
                Carousel = await _CarouselService.AddCarouselAsync(Carousel);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Carousel Post Attempt {Carousel}", Carousel);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                Carousel = null;
            }
            return Carousel;
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task<Models.Carousel> Put(int id, [FromBody] Models.Carousel Carousel)
        {
            if (ModelState.IsValid && Carousel.CarouselId == id && IsAuthorizedEntityId(EntityNames.Module, Carousel.ModuleId))
            {
                Carousel = await _CarouselService.UpdateCarouselAsync(Carousel);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Carousel Put Attempt {Carousel}", Carousel);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                Carousel = null;
            }
            return Carousel;
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}/{moduleid}")]
        [Authorize(Policy = PolicyNames.EditModule)]
        public async Task Delete(int id, int moduleid)
        {
            Models.Carousel Carousel = await _CarouselService.GetCarouselAsync(id, moduleid);
            if (Carousel != null && IsAuthorizedEntityId(EntityNames.Module, Carousel.ModuleId))
            {
                await _CarouselService.DeleteCarouselAsync(id, Carousel.ModuleId);
            }
            else
            {
                _logger.Log(LogLevel.Error, this, LogFunction.Security, "Unauthorized Carousel Delete Attempt {CarouselId} {ModuleId}", id, moduleid);
                HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            }
        }
    }
}
