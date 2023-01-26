namespace euroma2.Models.Reach
{
    public class Reach_Us
    {

        public int id { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public int? order { get; set; }

        public string icon { get; set; }

    }

    public class Reach_UsCMS
    {

        public Reach_UsCMS(Reach_Us s) { 
            this.id= s.id;
            this.title = s.title;
            this.description = s.description;
            this.order = s.order;
            this.description = s.description;
            this.icon = s.icon;
        }

        public Reach_UsCMS() { }
        public int id { get; set; }

        public string title { get; set; }
        public string? title_it { get; set; }

        public string description { get; set; }
        public string? description_it { get; set; }

        public int? order { get; set; }

        public string icon { get; set; }

    }

    public class Reach_Us_it
    {

        public int id { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public Reach_Us reach { get; set; }

    }
}
