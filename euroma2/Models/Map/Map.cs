namespace euroma2.Models.Map
{
    public class Map_Map
    {
        public int id { get; set; }
        public List<FloorInfo> floors {get;set;}

        public List<Map_Graph_Node> completeGraph { get; set; }

        //public List<Map_Graph_Node> accessibilityGraph { get; set; }

        public List<Map_Shop> shops { get; set; }

    }

    public class Map_View
    {
        public int id { get; set; }
        public List<FloorInfo> floors { get; set; }

        public List<Map_Graph_Node> completeGraph { get; set; }

        public List<Map_Graph_Node> accessibilityGraph { get; set; }

        public List<Map_Shop> shops { get; set; }

    }

    public class Map_Graph_Node {
        public int id { get; set; }
        public string nodeName { get; set; }

        public int floorId { get; set; }
        public List<Map_Graph_Node_Relations> relations {get;set;}
        public string attachedShopNodeName { get; set; }

        public int accessibility { get; set; }
    }

    public class Map_Graph_Node_Relations {
        public int id { get; set; }
        public int targetFloorId { get; set; }
        public string targetNavPointNodeName { get; set; }
        public float weight{ get; set; }

    }

    public class Map_Shop { 
        public int id { get; set; }
        public string nodeName { get; set; }
        public int shopId { get; set; }
        public string attachedNavPoint { get; set; }
        public int floorId { get; set; }

    }

}
