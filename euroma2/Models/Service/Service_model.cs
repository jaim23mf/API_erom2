using euroma2.Models.Promo;

namespace euroma2.Models.Service
{
    public class Service_model
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int? order { get; set; }
        public string icon { get; set; }
    }
    public class Service_model_it
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Service_model sm { get; set; }

    }
    public class Service_modelCMS
    {
        public Service_modelCMS(Service_model s) { 
            this.id = s.id;
            this.title = s.title;
            this.description = s.description;
            this.order = s.order;
            this.icon = s.icon;
        }
        public Service_modelCMS() { }
        public int id { get; set; }
        public string title { get; set; }
        public string title_it { get; set; }
        public string description { get; set; }
        public string description_it { get; set; }
        public int? order { get; set; }
        public string icon { get; set; }
    }
}
