using Microsoft.Extensions.Diagnostics.HealthChecks;
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

        public virtual ICollection<FloorNavPoint> navPoints { get; set; }
        public virtual ICollection<FloorShop> shopsNodes { get; set; }

    }


    public class FloorInfo_it { 
        
        public int id { get; set; }
        public string name { get; set; }
        public FloorInfo fi { get; set; }
    }

    public class FloorInfoUpdate
    {
        public int id { get; set; }
        public string modelUrl { get; set; }
        public string modelBinUrl { get; set; }
        public string name { get; set; }
        public string? name_it { get; set; }
        public int floor { get; set; }
    }



    public class FloorNavPoint
    {
        public int id { get; set; }
        public string nodeName { get; set; } = string.Empty;
        public virtual ICollection<FloorNavPointRelation> relations { get; set; }
        public string? attachedShopNode { get; set; }
    }

    public class FloorNavPointRelation
    {
        public int id { get; set; }
        public string targetNode { get; set; } = string.Empty;
        public int targetFloorId { get; set; }
        public string type { get; set; } = string.Empty;
        public bool accessibility { get; set; }
        public float linkWeight { get; set; }
    }

    public class FloorShop
    {
        public int id { get; set; }
        public string nodeName { get; set; }
        public int? attachedShopId { get; set; }
    }


    public class FloorInfoView
    {
        public int id { get; set; }
        public string modelUrl { get; set; }
        public string name { get; set; }
        public ICollection<FloorNavPoint> navPoints { get; set; }
        public ICollection<FloorShopView> shopsNodes { get; set; }
    }

    public class FloorShopView
    {
        public int id { get; set; }
        public string nodeName { get; set; }
        public FloorShopInfo? attachedShop { get; set; }
    }

    public class FloorNavPointsInfo
    {
        public int id  { get; set; }
        public string name { get; set; } = string.Empty;
        public IReadOnlyCollection<string> navPoints { get; set; }
    }

    public class FloorSaveData
    {
        public List<FloorNavPoint> navPoints { get; set; }
        public List<FloorShopView> shopsNodes { get; set; }
    }

    public class FloorShopInfo
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    //public class ShopNode {
    //   public ShopInfo? attachedShop { get; set; }
    //   public string? nodeName { get; set; }
    //}

    //public class NavPointInfo {
    //    public int id { get; set; }

    //    public string name { get; set; }
    //    public List<string> navPoints { get; set; }
    //}

    //public class FloorSaveData
    //{
    //    public List<Map_Graph_Node> navPoints { get; set; }
    //    public List<ShopNode> shopsNodes { get; set; }
    //}

}
