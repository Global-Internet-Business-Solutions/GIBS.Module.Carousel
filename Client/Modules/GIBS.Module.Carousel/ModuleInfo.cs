using Oqtane.Models;
using Oqtane.Modules;

namespace GIBS.Module.Carousel
{
    public class ModuleInfo : IModule
    {
        public ModuleDefinition ModuleDefinition => new ModuleDefinition
        {
            Name = "Carousel",
            Description = "Bootstrap 5 Image Carousel",
            Version = "1.0.1",
            ServerManagerType = "GIBS.Module.Carousel.Manager.CarouselManager, GIBS.Module.Carousel.Server.Oqtane",
            ReleaseVersions = "1.0.0,1.0.1",
            Dependencies = "GIBS.Module.Carousel.Shared.Oqtane",
            PackageName = "GIBS.Module.Carousel" 
        };
    }
}
