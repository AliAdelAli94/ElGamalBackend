namespace ElGamal.DAL.Context
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using ElGamal.DAL.Entities;

    public partial class ElGamalContext : DbContext
    {
        public ElGamalContext()
            : base("name=ElGamalContext")
        {
        }

        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Favourite> Favourites { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<ProductOption> ProductOptions { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>()
                .HasMany(e => e.Categories1)
                .WithOptional(e => e.Category1)
                .HasForeignKey(e => e.parentCategoryID)
                .WillCascadeOnDelete();

            modelBuilder.Entity<Order>()
                .Property(e => e.total)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Order>()
                .Property(e => e.shippingAmount)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Product>()
                .Property(e => e.priceBefore)
                .HasPrecision(18, 4);

            modelBuilder.Entity<Product>()
                .Property(e => e.priceAfter)
                .HasPrecision(18, 4);
        }
    }
}
