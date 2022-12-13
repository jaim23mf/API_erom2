using System.ComponentModel.DataAnnotations.Schema;

namespace euroma2.Models.Hours
{
    public class Opening
    {
        public int id { get; set; }
        public General general { get; set; }
        public List<Exception_Rules> exceptions { get; set; }

    }

    public class General {
        public int id { get; set; }
        public Time_Range global { get; set; }
        public Day_Opening_Hours_Food food { get; set; }
        public Day_Opening_Hours_Hipermarket hypermarket { get; set; }
        public Day_Opening_Hours_Stores ourStores { get; set; }
    }

    public class Exception_Rules {
        public int id { get; set; }
        public Date_Range dateRange { get; set; }
        public Time_Range global { get; set; }
        public Day_Opening_Hours_Food food { get; set; }
        public Day_Opening_Hours_Hipermarket hypermarket { get; set; }
        public Day_Opening_Hours_Stores ourStores { get; set; }
    }

    public class Time_Range {
        public int id { get; set; }
        public string from { get; set; }
        public string to { get; set; }
    }
    public class Day_Opening_Hours_Food {
        public int id { get; set; }
        public DayOfWeek fromWeekDay { get; set; }
        public DayOfWeek toWeekDay { get; set; }
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Day_Opening_Hours_Hipermarket
    {
        public int id { get; set; }
        public DayOfWeek fromWeekDay { get; set; }
        public DayOfWeek toWeekDay { get; set; }
        public string from { get; set; }
        public string to { get; set; }
    }

    public class Day_Opening_Hours_Stores
    {
        public int id { get; set; }
        public DayOfWeek fromWeekDay { get; set; }
        public DayOfWeek toWeekDay { get; set; }
        public string from { get; set; }
        public string to { get; set; }
    }
}
