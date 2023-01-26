namespace euroma2.Models.Interest
{
    public class Interest_model
    {
        public int id { get; set; }

        public string name { get; set; }

        public Interest_Group group { get; set; }

    }

    public class Interest_modelCMS
    {

        public Interest_modelCMS(Interest_model i) {
            this.id = i.id;
            this.name = i.name;
            this.group = i.group;
        }
        public Interest_modelCMS() { }

        public int id { get; set; }

        public string name { get; set; }
        public string? name_it { get; set; }

        public Interest_Group group { get; set; }

    }

    public class Interest_model_it { 
        public int id { get; set; }
        public string name { get; set; }
        public Interest_model interest_model { get; set; }
    }
    public class Interest_View
    {
        public int id { get; set; }

        public string name { get; set; }

        public string group { get; set; }

    }

    public enum Interest_Group {
        shops = 1,
        hobbies=2,
        events=3
    }
}
