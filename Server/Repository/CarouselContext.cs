using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Oqtane.Modules;
using Oqtane.Repository;
using Oqtane.Infrastructure;
using Oqtane.Repository.Databases.Interfaces;

namespace GIBS.Module.Carousel.Repository
{
    public class CarouselContext : DBContextBase, ITransientService, IMultiDatabase
    {
        public virtual DbSet<Models.Carousel> Carousel { get; set; }

        public CarouselContext(IDBContextDependencies DBContextDependencies) : base(DBContextDependencies)
        {
            // ContextBase handles multi-tenant database connections
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.Carousel>().ToTable(ActiveDatabase.RewriteName("GIBSCarousel"));
        }
    }
}
