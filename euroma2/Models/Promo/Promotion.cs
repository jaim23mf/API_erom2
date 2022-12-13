
using euroma2.Models.Events;

namespace euroma2.Models.Promo
{
    public class Promotion
    {
        public int id { get; set; }

        public int shopId { get; set; }

        public Date_Range dateRange { get; set; }

        public string image { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public List<LineaInterest> interestIds { get; set; }

    }

    public class PromoView {

        public PromoView() { }

        public PromoView(Promotion p) { 
            this.id = p.id;
            this.shopId = p.shopId;
            this.dateRange = p.dateRange;
            this.image = p.image;
            this.title = p.title;
            this.description = p.description;
        }

        public int id { get; set; }

        public int shopId { get; set; }

        public Date_Range dateRange { get; set; }

        public string image { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public List<int> interestIds { get; set; }
    }
}
