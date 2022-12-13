namespace euroma2.Models.Interest
{
    public class Interest_model
    {
        public int id { get; set; }

        public string name { get; set; }

        public Interest_Group group { get; set; }

    }

    public enum Interest_Group {
        shops = 0,
        hobbies=1,
        events=2
    }
}
