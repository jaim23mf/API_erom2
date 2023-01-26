namespace euroma2.Models
{
    public class ShopSubCategory
    {
        public int id { get; set; }

        public int categoryId { get; set; }
        public string title { get; set; }

    }

    public class ShopSubCategoryCMS
    {
        public ShopSubCategoryCMS(ShopSubCategory s)
        {
            this.id = s.id;
            this.title = s.title;
            this.title_it = "";

        }

        public ShopSubCategoryCMS() { }
        public int id { get; set; }
        public int categoryId { get; set; }
        public string title { get; set; }
        public string? title_it { get; set; }

    }

    public class ShopSubCategory_it
    {
        public int id { get; set; }
        public string title { get; set; }
        public ShopSubCategory shopSubCategory { get; set; }
    }
}
