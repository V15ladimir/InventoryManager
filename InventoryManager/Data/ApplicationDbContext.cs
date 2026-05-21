using InventoryManager.Models.Entitites;
using InventoryManager.Models.Entitites.Custom;
using InventoryManager.Models.Entitites.Inventories;
using InventoryManager.Models.Entitites.Items;
using InventoryManager.Models.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InventoryManager.Data {

    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options) {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<ItemValue> ItemValues { get; set; }
        public DbSet<IdPart> IdParts { get; set; }
        public DbSet<Field> Fields { get; set; }
        public DbSet<InventoryAccess> InventoryAccess { get; set; }
        public DbSet<InventoryDiscussion> InventoryDiscussions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(x => {
                x.HasKey(x => x.Id);
                x.Property(x => x.Name).IsRequired();
            });

            modelBuilder.Entity<Inventory>(x => {
                x.HasKey(x => x.Id);
                x.Property(x => x.Name).IsRequired();
                x.HasGeneratedTsVectorColumn(
                    x => x.SearchVectorEn, 
                    "english", 
                    x => new { x.Name, x.Description });
                x.HasIndex(x => x.SearchVectorEn)
                    .HasMethod("GIN");
                x.HasGeneratedTsVectorColumn(
                    x => x.SearchVectorRu,
                    "russian",
                    x => new { x.Name, x.Description });
                x.HasIndex(x => x.SearchVectorRu)
                    .HasMethod("GIN");
                x.HasOne(x => x.Category)
                    .WithMany(x => x.Inventories)
                    .HasForeignKey(x => x.CategoryId);
                x.HasOne(x => x.CreatedBy)
                    .WithMany(x => x.CreatedInventories)
                    .HasForeignKey(x => x.CreatedById);
            });

            modelBuilder.Entity<Item>(x => {
                x.HasKey(x => x.Id);
                x.HasIndex(x => new { x.InventoryId, x.CustomId })
                    .IsUnique();
                x.HasGeneratedTsVectorColumn(
                    x => x.SearchVectorEn,
                    "english",
                    x => new { x.SearchText });
                x.HasIndex(x => x.SearchVectorEn)
                    .HasMethod("GIN");
                x.HasGeneratedTsVectorColumn(
                    x => x.SearchVectorRu,
                    "russian",
                    x => new { x.SearchText });
                x.HasIndex(x => x.SearchVectorRu)
                    .HasMethod("GIN");
                x.Property(x => x.CustomId).IsRequired();
                x.HasOne(x => x.Inventory)
                    .WithMany(x => x.Items)
                    .HasForeignKey(x => x.InventoryId);
                x.HasOne(x => x.CreatedBy)
                    .WithMany(x => x.CreatedItems)
                    .HasForeignKey(x => x.CreatedById);
            });

            modelBuilder.Entity<ItemValue>(x => {
                x.HasKey(x => x.Id);
                x.HasOne(x => x.Item)
                    .WithMany(x => x.ItemValues)
                    .HasForeignKey(x => x.ItemId);
                x.HasOne(x => x.Field)
                    .WithMany(x => x.ItemValues)
                    .HasForeignKey(x => x.FieldId);
            });

            modelBuilder.Entity<IdPart>(x => {
                x.HasKey(x => x.Id);
                x.HasDiscriminator<IdType>("IdType")
                    .HasValue<FixedTextPart>(IdType.FixedText)
                    .HasValue<Random20bitPart>(IdType.Random20bit)
                    .HasValue<Random32bitPart>(IdType.Random32bit)
                    .HasValue<Random6Part>(IdType.Random6)
                    .HasValue<Random9Part>(IdType.Random9)
                    .HasValue<GuidPart>(IdType.Guid)
                    .HasValue<DateTimePart>(IdType.DateTime)
                    .HasValue<SequencePart>(IdType.Sequence);
                x.HasOne(x => x.Inventory)
                    .WithMany(x => x.Elements)
                    .HasForeignKey(x => x.InventoryId);
            });

            modelBuilder.Entity<Field>(x => {
                x.HasKey(x => x.Id);
                x.HasDiscriminator<FieldType>("FieldType")
                    .HasValue<SinglelineField>(FieldType.SingleLine)
                    .HasValue<MultilineField>(FieldType.MultiLine)
                    .HasValue<NumberField>(FieldType.Number)
                    .HasValue<LinkField>(FieldType.Link)
                    .HasValue<BooleanField>(FieldType.Boolean);
                x.HasOne(x => x.Inventory)
                    .WithMany(x => x.Fields)
                    .HasForeignKey(x => x.InventoryId);
            });

            modelBuilder.Entity<InventoryAccess>(x => {
                x.HasKey(x => x.Id);
                x.HasIndex(x => new { x.InventoryId, x.UserId }).IsUnique();
                x.HasOne(x => x.Inventory)
                    .WithMany(x => x.AccessList)
                    .HasForeignKey(x => x.InventoryId);
                x.HasOne(x => x.ApplicationUser)
                    .WithMany(x => x.AccessList)
                    .HasForeignKey(x => x.UserId);
            });

            modelBuilder.Entity<InventoryDiscussion>(x => {
                x.HasKey(x => x.Id);
                x.Property(x => x.Content).IsRequired()
                    .HasMaxLength(2000);
                x.HasOne(x => x.Inventory)
                    .WithMany(x => x.Discussions)
                    .HasForeignKey(x => x.InventoryId);
                x.HasOne(x => x.CreatedBy)
                    .WithMany()
                    .HasForeignKey(x => x.CreatedById);
            });

            modelBuilder.Entity<ApplicationUser>(x => {
                x.Property(x => x.CreatedAt)
                    .HasDefaultValueSql("current_timestamp")
                    .ValueGeneratedOnAdd();
            });
        }
    }
}
