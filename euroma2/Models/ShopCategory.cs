using Microsoft.EntityFrameworkCore.Storage;

namespace euroma2.Models
{
    public class ShopCategory
    {
        public int id { get; set; }
        public string title { get; set; }

        public StoreType shopType { get; set; }

    }

    public class ShopCategoryCMS
    {
        public ShopCategoryCMS(ShopCategory s) {
            this.id = s.id;
            this.title = s.title;
            this.title_it = "";
            this.shopType = s.shopType;
        }

        public ShopCategoryCMS() { }
        public int id { get; set; }
        public string title { get; set; }
        public string? title_it { get; set; }

        public StoreType shopType { get; set; }

    }

    public class ShopCategory_it { 
        public int id { get; set; }
        public string? title { get; set; } 
        public int cat_id { get; set; }
        public ShopCategory shopCategory { get; set; }
    }
    public class ShopCategoryM
    {
        public int id { get; set; }
        public string title { get; set; }
        public string shopType { get; set; }

    }

    public enum StoreType {
        foodAndBeverage = 1,
        store=2
    }
}
