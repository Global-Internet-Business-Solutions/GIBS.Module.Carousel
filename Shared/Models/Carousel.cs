using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Oqtane.Models;

namespace GIBS.Module.Carousel.Models
{
    [Table("GIBSCarousel")]
    public class Carousel : ModelBase
    {
        [Key]
        public int CarouselId { get; set; }
        public int ModuleId { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; } = true;
        public int OrderBy { get; set; } = 1;
    }
}
