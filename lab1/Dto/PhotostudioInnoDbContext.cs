using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace lab1.Dto
{
    public partial class PhotostudioInnoDbContext : DbContext
    {
        public PhotostudioInnoDbContext()
        {
        }

        public PhotostudioInnoDbContext(DbContextOptions<PhotostudioInnoDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Equipment> Equipment { get; set; }
        public virtual DbSet<EquipmentCategory> EquipmentCategories { get; set; }
        public virtual DbSet<Hall> Halls { get; set; }
        public virtual DbSet<HallEquipment> HallEquipments { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderedEquipment> OrderedEquipments { get; set; }
        public virtual DbSet<OrderedService> OrderedServices { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<User> Users { get; set; }
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("server=localhost;user=root;password=150200;database=photostudio_innodb", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.28-mysql"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasCharSet("utf8");

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.HasKey(e => e.IdEquipment)
                    .HasName("PRIMARY");

                entity.ToTable("equipment");

                entity.HasIndex(e => e.FkCategory, "fk_Equipment_Category_id");

                entity.Property(e => e.IdEquipment).HasColumnName("id_equipment");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.FkCategory).HasColumnName("fk_category");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.HasOne(d => d.FkCategoryNavigation)
                    .WithMany(p => p.Equipment)
                    .HasForeignKey(d => d.FkCategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Equipment_Category_name");
            });

            modelBuilder.Entity<EquipmentCategory>(entity =>
            {
                entity.HasKey(e => e.IdEquipmentCategory)
                    .HasName("PRIMARY");

                entity.ToTable("equipment_category");

                entity.Property(e => e.IdEquipmentCategory).HasColumnName("id_equipment_category");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Hall>(entity =>
            {
                entity.HasKey(e => e.IdHall)
                    .HasName("PRIMARY");

                entity.ToTable("hall");

                entity.Property(e => e.IdHall).HasColumnName("id_hall");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Image)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("image");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<HallEquipment>(entity =>
            {
                entity.HasKey(e => new { e.FkHall, e.FkEquipment })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("hall_equipment");

                entity.HasIndex(e => e.FkEquipment, "fk_HallEquipment_Equipment_name_idx");

                entity.Property(e => e.FkHall).HasColumnName("fk_hall");

                entity.Property(e => e.FkEquipment).HasColumnName("fk_equipment");

                entity.Property(e => e.Count).HasColumnName("count");

                entity.HasOne(d => d.FkEquipmentNavigation)
                    .WithMany(p => p.HallEquipments)
                    .HasForeignKey(d => d.FkEquipment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_HallEquipment_Equipment_name");

                entity.HasOne(d => d.FkHallNavigation)
                    .WithMany(p => p.HallEquipments)
                    .HasForeignKey(d => d.FkHall)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_HallEquipment_Hall_name");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.IdOrder)
                    .HasName("PRIMARY");

                entity.ToTable("order");

                entity.HasIndex(e => e.FkHall, "fk_Order_Hall_name_id");

                entity.HasIndex(e => e.FkStatus, "fk_Order_Status_id");

                entity.HasIndex(e => e.FkRenter, "fk_Order_User_id");

                entity.Property(e => e.IdOrder).HasColumnName("id_order");

                entity.Property(e => e.CreatingDate)
                    .HasColumnType("datetime")
                    .HasColumnName("creating_date");

                entity.Property(e => e.EndHallReserving)
                    .HasColumnType("datetime")
                    .HasColumnName("end_hall_reserving");

                entity.Property(e => e.FkHall).HasColumnName("fk_hall");

                entity.Property(e => e.FkRenter).HasColumnName("fk_renter");

                entity.Property(e => e.FkStatus).HasColumnName("fk_status").HasDefaultValue();

                entity.Property(e => e.StartHallReserving)
                    .HasColumnType("datetime")
                    .HasColumnName("start_hall_reserving");

                entity.Property(e => e.TotalCost).HasColumnName("total_cost").HasDefaultValueSql();

                entity.HasOne(d => d.FkHallNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.FkHall)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Order_Hall_name");

                entity.HasOne(d => d.FkRenterNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.FkRenter)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Order_User_name");

                entity.HasOne(d => d.FkStatusNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.FkStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_Order_Status_name");
            });

            modelBuilder.Entity<OrderedEquipment>(entity =>
            {
                entity.HasKey(e => new { e.FkOrder, e.FkEquipment })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("ordered_equipment");

                entity.HasIndex(e => e.FkEquipment, "fk_OrderedEquipment_Equipment_id");

                entity.Property(e => e.FkOrder).HasColumnName("fk_order");

                entity.Property(e => e.FkEquipment).HasColumnName("fk_equipment");

                entity.Property(e => e.Amount).HasColumnName("amount");

                entity.HasOne(d => d.FkEquipmentNavigation)
                    .WithMany(p => p.OrderedEquipments)
                    .HasForeignKey(d => d.FkEquipment)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_OrderedEquipment_Equipment_name");

                entity.HasOne(d => d.FkOrderNavigation)
                    .WithMany(p => p.OrderedEquipments)
                    .HasForeignKey(d => d.FkOrder)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_OrderedEquipment_Order_name");
            });

            modelBuilder.Entity<OrderedService>(entity =>
            {
                entity.HasKey(e => new { e.FkOrder, e.FkServices })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("ordered_services");

                entity.HasIndex(e => e.FkServices, "fk_OrderedServices_Services_name_idx");

                entity.Property(e => e.FkOrder).HasColumnName("fk_order");

                entity.Property(e => e.FkServices).HasColumnName("fk_services");

                entity.HasOne(d => d.FkOrderNavigation)
                    .WithMany(p => p.OrderedServices)
                    .HasForeignKey(d => d.FkOrder)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_OrderedServices_Order_name");

                entity.HasOne(d => d.FkServicesNavigation)
                    .WithMany(p => p.OrderedServices)
                    .HasForeignKey(d => d.FkServices)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_OrderedServices_Services_name");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.IdRole)
                    .HasName("PRIMARY");

                entity.ToTable("role");

                entity.Property(e => e.IdRole).HasColumnName("id_role");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.HasKey(e => e.IdService)
                    .HasName("PRIMARY");

                entity.ToTable("services");

                entity.Property(e => e.IdService).HasColumnName("id_service");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("description");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Price).HasColumnName("price");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.HasKey(e => e.IdStatus)
                    .HasName("PRIMARY");

                entity.ToTable("status");

                entity.Property(e => e.IdStatus).HasColumnName("id_status");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.IdUser)
                    .HasName("PRIMARY");

                entity.ToTable("user");

                entity.HasIndex(e => e.Email, "Email_UNIQUE")
                    .IsUnique();

                entity.HasIndex(e => e.FkRole, "fk_User_Role_id");

                entity.Property(e => e.IdUser).HasColumnName("id_user");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email");

                entity.Property(e => e.FkRole).HasColumnName("fk_role");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("password");

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(12)
                    .HasColumnName("phone_number")
                    .IsFixedLength(true);

                entity.Property(e => e.Surname)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("surname");

                entity.HasOne(d => d.FkRoleNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.FkRole)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_User_Role_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
