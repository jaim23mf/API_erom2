// Add-migration
// Remove-Migration
// Scaffold-DbContext
// Script-Migration
// Update-Database

using euroma2.Models.Events;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace euroma2.Models
{
    public class Shop
    {
        public int id { get; set; }
        public string title { get; set; }
        public ShopType type { get; set; }
        public int categoryId { get; set; }
        public int subcategoryId { get; set; }
        public string logo { get; set; }
        public string photo { get; set; }
        public ICollection<oDay> openingHours { get; set; }
        public string phoneNumber { get; set; }
        public string description { get; set; }
        public string firstOpeningDay { get; set; }
        public List<LineaInterest_shop> interestIds { get; set; }

    }

    public class LineaInterest_shop
    {
        [Key]
        public int id { get; set; }
        public int id_interest { get; set; }
        public int id_shop { get; set; }
    }

    public class ShopInfo {

        public ShopInfo(Shop t)
        {
            this.id = t.id ;
            this.title = t.title ;
        }

        public ShopInfo() {
            this.title = "";
        }

        public int id { get; set; }
        public string title { get; set; }
    }

    public class ShopView
    {
        public ShopView(Shop t)
        {
            this.id = t.id;
            this.photo = t.photo;
            this.title = t.title;
            this.type = t.type;
            this.logo = t.logo;
            this.openingHours = setOH(t.openingHours);
            this.phoneNumber = t.phoneNumber;
            this.description = t.description;
            this.firstOpeningDay = t.firstOpeningDay;

        }

        private ShopOpH setOH(ICollection<oDay> horas) {
            ShopOpH op = new ShopOpH();

            op.monday = new TimeRange();
            op.monday.from = horas.ElementAt(0).from;
            op.monday.to = horas.ElementAt(0).to;

            op.tuesday = new TimeRange();
            op.tuesday.from = horas.ElementAt(1).from;
            op.tuesday.to = horas.ElementAt(1).to;

            op.wednesday = new TimeRange();
            op.wednesday.from = horas.ElementAt(2).from;
            op.wednesday.to = horas.ElementAt(2).to;

            op.thursday = new TimeRange();
            op.thursday.from = horas.ElementAt(3).from;
            op.thursday.to = horas.ElementAt(3).to;

            op.friday = new TimeRange();
            op.friday.from = horas.ElementAt(4).from;
            op.friday.to = horas.ElementAt(4).to;

            op.saturday = new TimeRange();
            op.saturday.from = horas.ElementAt(5).from;
            op.saturday.to = horas.ElementAt(5).to;

            op.sunday = new TimeRange();
            op.sunday.from = horas.ElementAt(6).from;
            op.sunday.to = horas.ElementAt(6).to;

            return op;
        }
        public int id { get; set; }
        public string title { get; set; }
        public ShopType type { get; set; }
        public ShopCategory categoryId { get; set; }
        public ShopSubCategory subcategoryId { get; set; }
        public string logo { get; set; }
        public string photo { get; set; }
        public ShopOpH openingHours { get; set; }
        public string phoneNumber { get; set; }
        public string description { get; set; }
        public string firstOpeningDay { get; set; }
        public List<int> interestIds { get; set; }

    }

    public enum ShopType {
        store=0, 
        foodAndBeverage=1
    }

    public enum DayName 
    {
        Monday ,
        Tuesday ,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

    public class ShopOpH { 
    
         public TimeRange monday { get; set; }
         public TimeRange tuesday { get; set; }
         public TimeRange wednesday { get; set; }
         public TimeRange thursday { get; set; }
         public TimeRange friday { get; set; }
         public TimeRange saturday { get; set; }
         public TimeRange sunday { get; set; }
    }

    public class TimeRange {
        public string from { get; set; }
        public string to { get; set; }
    }

    public class LineaShopCategory { 
        public int id { get; set; }
        public int id_shop { get; set; }
        public int id_category { get; set; }
    }

    public class LineaShopSubCategory
    {
        public int id { get; set; }
        public int id_shop { get; set; }
        public int id_subcategory { get; set; }
    }

    public class oDay {
        public int id { get; set; }
        public DayName description { get; set; }
        public string from { get; set; }
        public string to { get; set; }
        public int id_shop { get; set; }
    }

   
}
