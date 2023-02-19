using Microsoft.EntityFrameworkCore;
using GraphicsForYouShopApi.Models;

namespace GraphicsForYouShopApi.Models
{
    public class GraphicsDbContext : DbContext
    {
        public GraphicsDbContext(DbContextOptions<GraphicsDbContext> options) : base(options)
        {

        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderItem>().HasKey(op => new
            {
                op.OrderId,
                op.GraphicId
            });

            modelBuilder.Entity<OrderItem>().HasOne(o => o.Order).WithMany(op => op.OrderItems).HasForeignKey(o => o.OrderId);


            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
            modelBuilder.Entity<Message>()
    .HasOne(e => e.Sender)
    .WithMany();

            modelBuilder.Entity<Message>()
                .HasOne(e => e.Receiver)
                .WithMany();

            modelBuilder.Entity<Message>()
    .HasOne(e => e.Sender)
    .WithMany()
    .OnDelete(DeleteBehavior.Restrict); // <--

            modelBuilder.Entity<Message>()
                .HasOne(e => e.Receiver)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }



        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> Roles { get; set; }
        public DbSet<Graphic> Graphics { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<GraphicPictures> GraphicPictures { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<OrderFile> OrderFiles { get; set; }
        public DbSet<Promotion> Promotions { get; set; }
    }
}
