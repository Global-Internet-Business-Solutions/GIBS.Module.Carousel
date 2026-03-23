using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Migrations.Operations.Builders;
using Oqtane.Databases.Interfaces;
using Oqtane.Migrations;
using Oqtane.Migrations.EntityBuilders;

namespace GIBS.Module.Carousel.Migrations.EntityBuilders
{
    public class CarouselEntityBuilder : AuditableBaseEntityBuilder<CarouselEntityBuilder>
    {
        private const string _entityTableName = "GIBSCarousel";
        private readonly PrimaryKey<CarouselEntityBuilder> _primaryKey = new("PK_GIBSCarousel", x => x.CarouselId);
        private readonly ForeignKey<CarouselEntityBuilder> _moduleForeignKey = new("FK_GIBSCarousel_Module", x => x.ModuleId, "Module", "ModuleId", ReferentialAction.Cascade);

        public CarouselEntityBuilder(MigrationBuilder migrationBuilder, IDatabase database) : base(migrationBuilder, database)
        {
            EntityTableName = _entityTableName;
            PrimaryKey = _primaryKey;
            ForeignKeys.Add(_moduleForeignKey);
        }

        protected override CarouselEntityBuilder BuildTable(ColumnsBuilder table)
        {
            CarouselId = AddAutoIncrementColumn(table,"CarouselId");
            ModuleId = AddIntegerColumn(table,"ModuleId");
            Title = AddStringColumn(table, "Title", 255);
            ImageUrl = AddStringColumn(table, "ImageUrl", 255);
            Description = AddStringColumn(table, "Description", 500, true);
            IsActive = AddBooleanColumn(table, "IsActive");
            AddAuditableColumns(table);
            return this;
        }

        public OperationBuilder<AddColumnOperation> CarouselId { get; set; }
        public OperationBuilder<AddColumnOperation> ModuleId { get; set; }
        public OperationBuilder<AddColumnOperation> Title { get; set; }
        public OperationBuilder<AddColumnOperation> ImageUrl { get; set; }
        public OperationBuilder<AddColumnOperation> Description { get; set; }
        public OperationBuilder<AddColumnOperation> IsActive { get; set; }

    }
}
