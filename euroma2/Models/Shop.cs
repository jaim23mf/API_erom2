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

    public class ShopCMS
    {
        public ShopCMS() { }
        public ShopCMS(Shop s) {
            this.id = s.id;
            this.photo = s.photo;
            this.title = s.title;
            this.categoryId = s.categoryId;
            this.type = s.type;
            this.subcategoryId = s.subcategoryId;
            this.logo = s.logo;
            this.openingHours = s.openingHours;
            this.phoneNumber = s.phoneNumber;
            this.description = s.description;
            this.firstOpeningDay = s.firstOpeningDay;
            this.interestIds = s.interestIds;
        }
        public int id { get; set; }
        public string title { get; set; }
        public string? title_it { get; set; }
        public ShopType type { get; set; }
        public int categoryId { get; set; }
        public int subcategoryId { get; set; }
        public string logo { get; set; }
        public string photo { get; set; }
        public ICollection<oDay> openingHours { get; set; }
        public string phoneNumber { get; set; }
        public string description { get; set; }
        public string? description_it { get; set; }
        public string firstOpeningDay { get; set; }
        public List<LineaInterest_shop> interestIds { get; set; }

    }

    public class Shop_it { 
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set;}
        public Shop shop { get; set; }
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
            this.categoryId= t.categoryId;
            this.subcategoryId= t.subcategoryId;
        }

        private ShopOpH setOH(ICollection<oDay> horas) {
            ShopOpH op = new ShopOpH();

            op.monday = new TimeRange();
            op.monday.from = horas.ElementAt(0).from.Split('T')[0];
            op.monday.to = horas.ElementAt(0).to.Split('T')[0];

            op.tuesday = new TimeRange();
            op.tuesday.from = horas.ElementAt(1).from.Split('T')[0];
            op.tuesday.to = horas.ElementAt(1).to.Split('T')[0];

            op.wednesday = new TimeRange();
            op.wednesday.from = horas.ElementAt(2).from.Split('T')[0];
            op.wednesday.to = horas.ElementAt(2).to.Split('T')[0];

            op.thursday = new TimeRange();
            op.thursday.from = horas.ElementAt(3).from.Split('T')[0];
            op.thursday.to = horas.ElementAt(3).to.Split('T')[0];

            op.friday = new TimeRange();
            op.friday.from = horas.ElementAt(4).from.Split('T')[0];
            op.friday.to = horas.ElementAt(4).to.Split('T')[0];

            op.saturday = new TimeRange();
            op.saturday.from = horas.ElementAt(5).from.Split('T')[0];
            op.saturday.to = horas.ElementAt(5).to.Split('T')[0];

            op.sunday = new TimeRange();
            op.sunday.from = horas.ElementAt(6).from.Split('T')[0];
            op.sunday.to = horas.ElementAt(6).to.Split('T')[0];

            return op;
        }
        public int id { get; set; }
        public string title { get; set; }
        public ShopType type { get; set; }
        public int categoryId { get; set; }
        public int subcategoryId { get; set; }
        public string logo { get; set; }
        public string photo { get; set; }
        public ShopOpH openingHours { get; set; }
        public string phoneNumber { get; set; }
        public string description { get; set; }
        public string firstOpeningDay;
        public string FirstOpeningDay { 
            get{ return firstOpeningDay.Split('T')[0]; }
            set { firstOpeningDay = value; }
        }
        public List<int> interestIds { get; set; }

    }

    public enum ShopType {
        foodAndBeverage=1,
        store = 2
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
