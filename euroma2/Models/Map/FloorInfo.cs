using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Xml.Linq;

namespace euroma2.Models.Map
{
    public class FloorInfo
    {
        public int id { get; set; }
        public string modelUrl { get; set; }
        public string modelBinUrl { get; set; }
        public string name { get; set; }
        public int floor { get; set; }

    }

    public class FloorInfoView {
        public int id { get; set; }
        public string modelUrl { get; set; }
        public string name { get; set; }
        public List<Map_Graph_Node> navPoints { get; set; }
        public List<ShopNode> shopsNodes { get; set; }
    }


    public class ShopNode {
       public ShopInfo attachedShop { get; set; }
       public string nodeName { get; set; }
    }

    public class NavPointInfo {
        public int id { get; set; }

        public string name { get; set; }
        public List<string> navPoints { get; set; }
    }

    public class FloorSaveData
    {
        public List<Map_Graph_Node> navPoints { get; set; }
        public List<ShopNode> shopsNodes { get; set; }
    }

}
