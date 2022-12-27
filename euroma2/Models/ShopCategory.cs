using Microsoft.EntityFrameworkCore.Storage;

namespace euroma2.Models
{
    public class ShopCategory
    {
        public int id { get; set; }
        public string title { get; set; }
        public StoreType shopType { get; set; }

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
