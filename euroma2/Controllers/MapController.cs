using euroma2.Models.Hours;
using euroma2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using euroma2.Models.Map;
using Microsoft.AspNetCore.Authorization;
using euroma2.Services;
using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using System.IO;

namespace euroma2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly PtaInfo _options;

        public MapController(DataContext dbContext, IOptions<PtaInfo> options)
        {
            _dbContext = dbContext;
            _options = options.Value;
        }

        #region CMS

        // GET: api/<InterestController>
        [HttpGet("Floor")]
        public async Task<ActionResult<IEnumerable<FloorInfo>>> Get()
        {
            if (_dbContext.floorInfo == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .floorInfo
                .ToListAsync();

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        // GET api/<InterestController>/5
        [HttpGet("Floor/{id}")]
        public async Task<ActionResult<FloorInfo>> GetFloor(int id)
        {
            if (_dbContext.floorInfo == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .floorInfo
                .FirstOrDefaultAsync(p => p.id == id); ;

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        // POST api/<InterestController>
        [HttpPost("Floor")]
        [Authorize]
        public async Task<ActionResult<FloorInfo>> Post(FloorInfo serv)
        {
            Map_Map mp = await GetMapPriv();
            if (mp!= null) { 
                mp.floors.Add(serv);
            }
            _dbContext.floorInfo.Add(serv);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFloor), new { id = serv.id }, serv);
        }

        // PUT api/<InterestController>/5
        [HttpPut("Floor/{id}")]
        [Authorize]
        public async Task<IActionResult> PutFloor(int id, FloorInfo serv)
        {
            if (id != serv.id)
            {
                return BadRequest();
            }

            _dbContext.Entry(serv).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FloorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return Ok(new PutResult { result="Ok"});
        }

        private bool FloorExists(long id)
        {
            return (_dbContext.floorInfo?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpDelete("Floor/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFloor(int id)
        {
            if (_dbContext.floorInfo == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.floorInfo.FindAsync(id);
            if (ss == null)
            {
                return NotFound();
            }
            _dbContext.floorInfo.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }


        [HttpPost("Floor/GltfFile/{id}")]
        [HttpPost("Floor/BinFile/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles(this._options);
            uf = await uf.UploadFileToAsync(Consts.FloorGltf, file);
            return Ok(uf);
        }

        #endregion

        #region MOBILE

        #region MAP

        [HttpGet]
        public async Task<ActionResult<Map_View>> GetMap()
        {
            if (_dbContext.map == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .map
                .Include(a => a.floors)
                .Include(a => a.shops)
                .Include(a => a.completeGraph).ThenInclude(b => b.relations)
                .FirstAsync();

            if (t == null)
            {
                return NotFound();
            }

            Map_View mv = new Map_View();
            mv.floors = t.floors;
            mv.shops = t.shops;
            mv.completeGraph = t.completeGraph;

            mv.accessibilityGraph = GetCompletGrap(t.completeGraph);

            return mv;
        }

        private async Task<Map_Map> GetMapPriv()
        {
            if (_dbContext.map == null)
            {
                return null;
            }
            var t = await _dbContext
                .map
                .Include(a => a.floors)
                .Include(a => a.shops)
                .Include(a => a.completeGraph)
                .FirstAsync();

            if (t == null)
            {
                return null;
            }


            return t;
        }

        private List<Map_Graph_Node> GetCompletGrap(List<Map_Graph_Node> lt) { 
            List<Map_Graph_Node> result = new List<Map_Graph_Node>();
            if (lt != null)
            {
                foreach (Map_Graph_Node l in lt)
                {
                    if (l.accessibility == 1)
                    {
                        result.Add(l);
                    }

                }
            }
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Map_Map>> GetMapId(int id)
        {
            if (_dbContext.map == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .map
                .FirstOrDefaultAsync(p => p.id == id); ;

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Map_Map>> PostMap(Map_Map m)
        {
            _dbContext.map.Add(m);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetMapId), new { id = m.id }, m);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutMap(int id, Map_Map s)
        {
            if (id != s.id)
            {
                return BadRequest();
            }

            _dbContext.Entry(s).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return Ok(new PutResult { result = "Ok" });
        }

        private bool MapExists(long id)
        {
            return (_dbContext.map?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMap(int id)
        {
            if (_dbContext.map == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.map.FindAsync(id);
            if (ss == null)
            {
                return NotFound();
            }
            _dbContext.map.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }
        #endregion

        #region Map_Shop
        
        [HttpPost("MapShop")]
        [Authorize]
        public async Task<ActionResult<Map_Shop>> PostMapShop(Map_Shop s)
        {
         Map_Map mp = await GetMapPriv();
            if (mp!= null) { 
                mp.shops.Add(s);
            }
            _dbContext.map_shop.Add(s);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMapShop), new { id = s.id }, s);
        }


        [HttpGet("MapShop/{id}")]
        public async Task<ActionResult<Map_Shop>> GetMapShop(int id)
        {
            if (_dbContext.map_shop == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .map_shop
                .FirstOrDefaultAsync(p => p.id == id); ;

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        [HttpPut("MapShop/{id}")]
        [Authorize]
        public async Task<IActionResult> PutMapShop(int id, Map_Shop s)
        {
            if (id != s.id)
            {
                return BadRequest();
            }

            _dbContext.Entry(s).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapShopExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return Ok(new PutResult { result="Ok"});
        }

        private bool MapShopExists(long id)
        {
            return (_dbContext.map_shop?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpDelete("MapShop/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMapShop(int id)
        {
            if (_dbContext.map_shop == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.map_shop.FindAsync(id);
            if (ss == null)
            {
                return NotFound();
            }
            _dbContext.map_shop.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result="Ok"});
        }

        #endregion

        #region MAP GRAPH NODE
        [HttpPost("MapNode")]
        [Authorize]
        public async Task<ActionResult<Map_Graph_Node>> PostMapNode(Map_Graph_Node s)
        {
            Map_Map mp = await GetMapPriv();
            if (mp != null)
            {
                mp.completeGraph.Add(s);
            }
            _dbContext.map_graph_node.Add(s);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMapNode), new { id = s.id }, s);
        }


        [HttpGet("MapNode/{id}")]
        public async Task<ActionResult<Map_Graph_Node>> GetMapNode(int id)
        {
            if (_dbContext.map_graph_node == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .map_graph_node
                .FirstOrDefaultAsync(p => p.id == id); ;

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        [HttpPut("MapNode/{id}")]
        [Authorize]
        public async Task<IActionResult> PutMapNode(int id, Map_Graph_Node s)
        {
            if (id != s.id)
            {
                return BadRequest();
            }

            _dbContext.Entry(s).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapNodeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return Ok(new PutResult { result = "Ok" });
        }

        private bool MapNodeExists(long id)
        {
            return (_dbContext.map_graph_node?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpDelete("MapNode/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMapNode(int id)
        {
            if (_dbContext.map_graph_node == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.map_graph_node.FindAsync(id);
            if (ss == null)
            {
                return NotFound();
            }
            _dbContext.map_graph_node.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }
        #endregion

        #region Map Graph Node RELATIONS

        [HttpPost("NodeRelation")]
        [Authorize]
        public async Task<ActionResult<Map_Graph_Node_Relations>> PostMapRelation(Map_Graph_Node_Relations s)
        {
            Map_Graph_Node mp = await GetMapRelationPriv(s.targetNavPointNodeName);
            if (mp != null)
            {
                mp.relations.Add(s);
            }
            _dbContext.map_graph_node_relations.Add(s);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetMapRelation), new { id = s.id }, s);
        }


        private async Task<Map_Graph_Node> GetMapRelationPriv(string nodeName)
        {
            if (_dbContext.map == null)
            {
                return null;
            }
            var t = await _dbContext
                .map_graph_node
                .Include(a => a.relations).Where(a=>a.nodeName == nodeName)
                .FirstAsync();

            if (t == null)
            {
                return null;
            }


            return t;
        }

        [HttpGet("NodeRelation/{id}")]
        public async Task<ActionResult<Map_Graph_Node_Relations>> GetMapRelation(int id)
        {
            if (_dbContext.map_graph_node_relations == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .map_graph_node_relations
                .FirstOrDefaultAsync(p => p.id == id); ;

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        [HttpPut("NodeRelation/{id}")]
        [Authorize]
        public async Task<IActionResult> PutMapRelation(int id, Map_Graph_Node_Relations s)
        {
            if (id != s.id)
            {
                return BadRequest();
            }

            _dbContext.Entry(s).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MapRelationExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }
            return Ok(new PutResult { result = "Ok" });
        }

        private bool MapRelationExists(long id)
        {
            return (_dbContext.map_graph_node_relations?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpDelete("NodeRelation/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteMapRelation(int id)
        {
            if (_dbContext.map_graph_node_relations == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.map_graph_node_relations.FindAsync(id);
            if (ss == null)
            {
                return NotFound();
            }
            _dbContext.map_graph_node_relations.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }

        #endregion


        #region BABYLON

        [HttpGet("FloorView/{id}")]
        public async Task<ActionResult<FloorInfoView>> GetFloorView(int id)
        {
            if (_dbContext.map == null)
            {
                return NotFound();
            }
            var t = await _dbContext.map_graph_node.Where(s => s.floorId == id).ToListAsync();
            var s = await _dbContext.map_shop.Where(s => s.floorId == id).ToListAsync();

            var x = await _dbContext
              .floorInfo
              .FirstOrDefaultAsync(p => p.id == id);

            if (x == null) {
                return NotFound();
            }


            if (t == null)
            {
                return NotFound();
            }

            FloorInfoView fw = new FloorInfoView();
            fw.id = x.id;
            fw.name= x.name;
            fw.modelUrl= x.modelUrl;
            fw.navPoints = new List<Map_Graph_Node>();
            fw.shopsNodes = new List<ShopNode>();

            if (t.Count > 0) {
                foreach (Map_Graph_Node e in t) {
                    if (e.floorId == x.id) {
                        fw.navPoints.Add(e);
                    }
                }
            }


            if (s.Count > 0)
            {
                foreach (Map_Shop e in s)
                {
                    if (e.floorId == x.id)
                    {
                        ShopNode sn = new ShopNode();
                        sn.nodeName = e.nodeName;
                        sn.attachedShop = new ShopInfo();
                        sn.attachedShop.id = e.shopId;

                        var y = await _dbContext.shop.FirstOrDefaultAsync(p => p.id == e.shopId);
                        if (y != null)
                        {
                            sn.attachedShop.title = y.title;
                        }
                        fw.shopsNodes.Add(sn);
                    }
                }
            }

            return fw;
        }

        [HttpGet("NavInfoPoint")]
        public async Task<ActionResult<List<NavPointInfo>>> GetNavInfoPoint()
        {

            if (_dbContext.map == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .floorInfo
                .ToListAsync();

            var m = await _dbContext
            .map
            .Include(a => a.shops)
            .Include(a => a.completeGraph).ThenInclude(b => b.relations)
            .FirstAsync();

            if (t == null)
            {
                return NotFound();
            }

            List<NavPointInfo> lnp = new List<NavPointInfo>();

            foreach (FloorInfo e in t)
            {
                NavPointInfo fw = new NavPointInfo();
                fw.id = e.id;
                fw.name = e.name;
                fw.navPoints = new List<string>();
                if(m.completeGraph != null)
                foreach (Map_Graph_Node mg in m.completeGraph) {
                        if (mg.floorId == e.id) {
                            fw.navPoints.Add(mg.nodeName);
                        }    
                }

                lnp.Add(fw);
            }


            return lnp;
        }


        [HttpGet("ShopInfo")]
        public async Task<ActionResult<List<ShopInfo>>> GetShopInfo()
        {
            if (_dbContext.shop == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .shop.Include(a => a.openingHours).ToListAsync();

            if (t == null)
            {
                return NotFound();
            }

            List<ShopInfo> ls = new List<ShopInfo>();

            foreach (Shop s in t) {
                ShopInfo si = new ShopInfo();
                si.id = s.id;
                si.title = s.title;
                ls.Add(si);
            }

            return ls;
        }


        [HttpPost("SaveFloor/{floorid}")]
        [Authorize]
        public async Task<IActionResult> SaveFloor(int floorid, FloorSaveData data) {

            List<Map_Shop> t = await _dbContext.map_shop.Where(a=>a.floorId == floorid).ToListAsync();

            if (t != null) {  _dbContext.map_shop.RemoveRange(t); }

            List<Map_Graph_Node> n = await _dbContext.map_graph_node.Where(a => a.floorId == floorid).ToListAsync();

            if (n != null) { _dbContext.map_graph_node.RemoveRange(n); }


            foreach (ShopNode s in data.shopsNodes) { 
                Map_Shop ms = new Map_Shop();
                ms.floorId = floorid;
                if (s.attachedShop != null)
                {
                    ms.shopId = s.attachedShop.id;
                    //ms.attachedNavPoint = s.attachedShop.title;
                }
                if (s.nodeName != null)
                {
                    ms.nodeName = s.nodeName;
                }
                _dbContext.map_shop.Add(ms);
            }

  
            _dbContext.map_graph_node.AddRange(data.navPoints);

            await _dbContext.SaveChangesAsync();

            return Ok(new PutResult { result = "Ok" });

        }


        #endregion

        #endregion
    }
}
