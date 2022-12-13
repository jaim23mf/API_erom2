using euroma2.Models.Events;
using euroma2.Models.Hours;
using euroma2.Models.Interest;
using euroma2.Models.Map;
using euroma2.Models.Promo;
using euroma2.Models.Reach;
using euroma2.Models.Service;
using Microsoft.EntityFrameworkCore;

namespace euroma2.Models
{
    public class DataContext: DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { 
        
        }

        public DbSet<Shop> shop { get; set; } = null!;
        public DbSet<ShopCategory> shopCategory { get; set; } = null!;
        public DbSet<ShopSubCategory> shopSubCategory { get; set; } = null!;
        public DbSet<Mall_Event> events { get; set; } = null!;
        public DbSet<Opening> opening_hours { get; set; } = null!;
        public DbSet<General> opening_general { get; set; } = null!;
        public DbSet<Exception_Rules> opening_exceptions { get; set; } = null!;

        public DbSet<Interest_model> interests { get; set; } = null!;
        public DbSet<Promotion> promotion { get; set; } = null!;
        public DbSet<Reach_Us> reach { get; set; } = null!;
        public DbSet<Service_model> service { get; set; } = null!;
        public DbSet<FloorInfo> floorInfo { get; set; } = null!;


       

    }
}
