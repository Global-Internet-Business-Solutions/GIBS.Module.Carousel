using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using GIBS.Module.Carousel.Migrations.EntityBuilders;
using GIBS.Module.Carousel.Repository;

namespace GIBS.Module.Carousel.Migrations
{

    [DbContext(typeof(CarouselContext))]
    [Migration("GIBS.Module.Carousel.01.00.01.00")]
    public class AddOrderBy : MultiDatabaseMigration
    {
        public AddOrderBy(IDatabase database) : base(database)
        {
        }

        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                 name: "OrderBy",
                 table: "GIBSCarousel", // Replace with your table name
                 type: "int", // EF Core will typically map this correctly
                 nullable: false, // Set nullability as needed
                 defaultValue: 1); // Essential for non-nullable columns on existing data

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderBy",
                table: "GIBSCarousel");
        }
    }
}
