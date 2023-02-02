using euroma2.Models.Interest;
using euroma2.Models.Promo;

namespace euroma2.Models.Blog_model
{
    public class Blog
    {
        public Blog() { }
            public int id { get; set; }
            public string title { get; set; }
            public string date { get; set; }
            public string description { get; set; }
            public string shortDescription { get; set; }
            public string image { get; set; }
            public string thumb { get; set; }
            public bool highlight { get; set; }
    }

    public class BlogCMS
    {

        public BlogCMS(Blog i)
        {
            this.id = i.id;
            this.title = i.title;
            this.date = i.date;
            this.description = i.description;
            this.shortDescription = i.shortDescription;
            this.image = i.image;
            this.thumb = i.thumb;
            this.highlight = i.highlight;
        }
        public BlogCMS() { }

        public int id { get; set; }
        public string title { get; set; }
        public string title_it { get; set; }
        public string date { get; set; }
        public string description { get; set; }
        public string description_it { get; set; }
        public string shortDescription { get; set; }
        public string shortDescription_it { get; set; }
        public string image { get; set; }
        public string thumb { get; set; }
        public bool highlight { get; set; }

    }

    public class Blog_it
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string shortDescription { get; set; }
        public Blog blog { get; set; }
    }

}



