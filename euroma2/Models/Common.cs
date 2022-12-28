namespace euroma2.Models
{
    public class Common
    {
    }

    public class Date_Range
    {
        public int id { get; set; }
        public string from;
        public string From { get { return from.Split('T')[0]; } set { from = value; } }
        public string to;
        public string To { get { return to.Split('T')[0]; } set { to = value; } }
    }
}
