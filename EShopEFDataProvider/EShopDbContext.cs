using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using EShop.Entity;

namespace EShopEFDataProvider.FluentDbContext
{
    public class EShopDbContext : DbContext
    {
        //свойства - коллекции сущностей
        //в принципе это отражения таблиц БД в DbContext в нашем коде
        public DbSet<Category> Categories { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Availability> Availabilities { get; set; }
        public DbSet<Delivery> Delivries { get; set; }

        //мы сделаем так, что нужно обязательно передавать ConnectionString через конструктор,
        //ConnectionString по умолчанию, как в EMDX моделях не используется
        public EShopDbContext(string strConn)
            : base(string.Format(strConn))
        {
        }

        //перегруженный метод, который будет автоматически вызываться при создании экземпляря класса EShopDbContext
        //в этом методе происходит настройка привязки сущностоей к таблицам базы данных и определение связей между ними
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<ForeignKeyAssociationMultiplicityConvention>();
            //можно использовать класс настроек для сущности
            modelBuilder.Configurations.Add(new CategoryConfig());
            modelBuilder.Configurations.Add(new ModelConfig());
            //или можно настраивать привязку сущностей напрямую
            //можно использовать прямую форму записи
            modelBuilder.Entity<Availability>().ToTable("Availability");
            modelBuilder.Entity<Availability>().HasEntitySetName("Availabilities");
            modelBuilder.Entity<Availability>().HasKey(k => k.Id);
            modelBuilder.Entity<Availability>().HasMany<Model>(a => a.Models).WithRequired(model => model.Availability);
            //можно использовать итеративную
            modelBuilder.Entity<Delivery>().ToTable("Delivery");
            modelBuilder.Entity<Delivery>().HasEntitySetName("Deliveries").HasKey(k => k.Id).HasMany<Model>(a => a.Models).WithRequired(model => model.Delivery);

            base.OnModelCreating(modelBuilder);
        }
    }

    class CategoryConfig : EntityTypeConfiguration<Category>
    {
        public CategoryConfig()
        {
            ToTable("Categories");
            Property(p => p.Id).IsRequired().HasColumnType("int").HasColumnName("ID");
            HasKey(c => c.Id);
            Property(p => p.ParentId).IsOptional().HasColumnName("ParentID");
            Property(p => p.ImageId).IsOptional().HasColumnName("ImageID");
            Property(p => p.Name).HasMaxLength(150).HasColumnName("Name").IsRequired();
            HasEntitySetName("Categories");
            HasMany(m => m.Models).WithRequired(p => p.Category);
        }
    }

    class ModelConfig : EntityTypeConfiguration<Model>
    {
        public ModelConfig()
        {
            //map DbSet<Model> Models to table
            ToTable("Models");
            //название свойства коллекции сущностей типа Model в классе DbContext
            HasEntitySetName("Models");
            //Key field
            HasKey(m => m.Id);
            
            //map property to table column's
            Property(p => p.Id).HasColumnName("ID").HasColumnType("int").IsRequired();
            Property(p => p.Title).HasMaxLength(512).IsRequired();
            Property(p => p.Description).HasMaxLength(2048);
            Property(p => p.ImageId).IsOptional();
            
            //Foreign key's -------------------------------------
            //можно использовать такую форму записи
            Property(p => p.CategoryId).IsRequired();
            Property(p => p.CategoryId).HasColumnName("CategoryId");
            Property(p => p.CategoryId).HasColumnType("int");
            //можно использовать итеративную форму записи
            Property(p => p.DeliveryId).IsRequired().HasColumnName("Delivery").HasColumnType("smallint");
            Property(p => p.AvailabilityId).IsRequired().HasColumnName("Availability").HasColumnType("smallint");
            //--------------------------------------------------

            //navigation property
            HasRequired(model => model.Availability).WithMany(av => av.Models).HasForeignKey(k => k.AvailabilityId);
            HasRequired(model => model.Delivery).WithMany(dev => dev.Models).HasForeignKey(k => k.DeliveryId);
            HasRequired(model => model.Category).WithMany(category => category.Models).HasForeignKey(k => k.CategoryId);
        }
    }
}
