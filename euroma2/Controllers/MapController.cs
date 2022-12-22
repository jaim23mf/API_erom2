using euroma2.Models.Hours;
using euroma2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using euroma2.Models.Map;
using Microsoft.AspNetCore.Authorization;
using euroma2.Services;

namespace euroma2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly DataContext _dbContext;
        public MapController(DataContext dbContext)
        {
            _dbContext = dbContext;
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
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles();
            uf = await uf.UploadFileToAsync("FloorGltf", file);
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

        #endregion
    }
}
