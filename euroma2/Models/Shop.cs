// Add-migration
// Remove-Migration
// Scaffold-DbContext
// Script-Migration
// Update-Database

using System.Runtime.CompilerServices;

namespace euroma2.Models
{
    public class Shop
    {
        public int id { get; set; }
        public string title { get; set; }
        public ShopType type { get; set; }
        public List<LineaShopCategory> category { get; set; }
        public List<LineaShopSubCategory> subcategory { get; set; }
        public string logo { get; set; }
        public string photo { get; set; }
        public ICollection<oDay> openingHours { get; set; }
        public string phoneNumber { get; set; }
        public string description { get; set; }


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
            this.openingHours = t.openingHours;
            this.phoneNumber = t.phoneNumber;
            this.description = t.description;
        }

        public int id { get; set; }
        public string title { get; set; }
        public ShopType type { get; set; }
        public List<ShopCategory> category { get; set; }
        public List<ShopSubCategory> subcategory { get; set; }
        public string logo { get; set; }
        public string photo { get; set; }
        public ICollection<oDay> openingHours { get; set; }
        public string phoneNumber { get; set; }
        public string description { get; set; }

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
        public string hi { get; set; }
        public string hf { get; set; }
    }

   
}
