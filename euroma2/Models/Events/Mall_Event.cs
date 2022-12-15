using static System.Net.Mime.MediaTypeNames;
using System;
using euroma2.Models.Promo;

namespace euroma2.Models.Events
{
    public class Mall_Event
    {
        public int id { get; set; }

        public Date_Range dateRange { get; set; }

        public string image { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public List<LineaInterest_event> interestIds { get; set; }

    }

    public class LineaInterest_event { 
        public int id { get; set; }
        public int id_event { get; set; }
        public int id_interest { get; set; }
    }

    public class EventView
    {

        public EventView() { }

        public EventView(Mall_Event p)
        {
            this.id = p.id;
            this.dateRange = p.dateRange;
            this.image = p.image;
            this.title = p.title;
            this.description = p.description;
        }

        public int id { get; set; }

        public Date_Range dateRange { get; set; }

        public string image { get; set; }

        public string title { get; set; }

        public string description { get; set; }

        public List<int> interestIds { get; set; }
    }

}
