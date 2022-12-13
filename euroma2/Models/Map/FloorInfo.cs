using System.Drawing;

namespace euroma2.Models.Map
{
    public class FloorInfo
    {
        public int id { get; set; }
        public string modelUrl { get; set; }

        public string name { get; set; }

        public List<Floor_Shop> shopNodes { get; set; }

        public List<Floor_Nav_Point> navPoints { get; set; }


    }

    public class Floor_Shop {
        public int id { get; set; }
        public string nodeName { get; set; }
        public ShopInfo attachedShop { get; set; }
    }

    public class Floor_Nav_Point {
        public int id { get; set; }
        public string nodeName { get; set; }

        public string attachedShopNode { get; set; }

        private List<Floor_Nav_Point_Relation> relations { get; set; }
        //relations
    }

    public class Floor_Nav_Point_Relation {
        public int id { get; set; }
        public string targetNode { get; set; }
        public int targetFloorId { get; set; }
        public Boolean accessibility { get; set; }
        public float linkWeight { get; set; }

        public PoinRelationType type { get; set; }
    }

    public enum PoinRelationType
    {
        defaultfloor = 0,
        floorSwitcher = 1
    }

}
