using Microsoft.EntityFrameworkCore;
using Serilog.Debugging;

namespace BookingKTX.Models
{
    public class DataContext : DbContext
    {
        public static Random random = new Random();
        public DbSet<SqlUser>? users { get; set; }
        public DbSet<SqlRole>? roles { get; set; }
        public DbSet<SqlAction>? actions { get; set; }
        public DbSet<SqlCustomer>? customers { get; set; }
        public DbSet<SqlFile>? files { get; set; }
        public DbSet<SqlLogOrder>? logOrders { get; set; }
        public DbSet<SqlOrder>? orders { get; set; }
        public DbSet<SqlProduct>? products { get; set; }
        public DbSet<SqlShop>? shops { get; set; }
        public DbSet<SqlState>? states { get; set; }
        public DbSet<SqlType>? types { get; set; }
        public DbSet<SqlCart>? carts { get; set; }
        public DbSet<SqlCartOrder>? cartOrders { get; set; }
        public DbSet<SqlCartProduct>? cartProducts { get; set; }




        public static string randomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }
        //public static string configSql = "";
        public static string configSql = "Host=localhost:5432;Database=db_booking_ktx;Username=postgres;Password=postgres";
       // public static string configSql = "Host=HAIBROTHER\\MSSQLSERVER01;Database=db_booking_ktx;Username=sa;Password=123456";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(configSql);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SqlProduct>().HasOne<SqlShop>(s => s.shop).WithMany(s => s.products);
            modelBuilder.Entity<SqlOrder>().HasOne<SqlCustomer>(s => s.customer).WithMany(s => s.orders);
            modelBuilder.Entity<SqlOrder>().HasOne<SqlUser>(s => s.shipper).WithMany(s => s.orderShippers);
            //modelBuilder.Entity<SqlCart>().HasOne<SqlCustomer>(s => s.customer).WithOne(s => s.cart);
        }

    }
}
