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
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using euroma2.Migrations;
using euroma2.Models.Reach;
using System.Xml;

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
            this._dbContext = dbContext;
            this._options = options.Value;
        }

        #region CMS

        [HttpDelete("Floor/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteFloor(int id)
        {
            var ss = await this._dbContext.floor_info.FindAsync(id);
            if (ss == null)
                return this.NotFound();
            this._dbContext.floor_info.Remove(ss);
            await this._dbContext.SaveChangesAsync();
            return this.Ok(new PutResult { result = "Ok" });
        }

        [HttpGet("{lang}/Floor")]
        public async Task<ActionResult<IEnumerable<FloorInfo>>> Get(string lang)
        {

            var t = await this._dbContext
                .floor_info
                .ToListAsync();
            foreach (FloorInfo s in t)
            {
                if (lang == "it")
                {
                    var it = await _dbContext
                    .floor_info_it
                    .FirstOrDefaultAsync(p => p.id == s.id);
                    if (it != null)
                    {
                        s.name = it.name;
                    }
                }
            }
            return t;
        }

        [HttpGet("FloorCMS")]
        public async Task<ActionResult<IEnumerable<FloorInfoUpdate>>> GetCMS()
        {

            var t = await _dbContext
                 .floor_info
                 .ToListAsync();

            var l = await _dbContext.floor_info_it.ToListAsync();

            List<FloorInfoUpdate> sc = new List<FloorInfoUpdate>();

            foreach (FloorInfo s in t)
            {
                FloorInfoUpdate res = new FloorInfoUpdate();

                res.id = s.id;
                res.name = s.name;
                res.modelUrl = s.modelUrl;
                res.modelBinUrl = s.modelBinUrl;
                res.floor = s.floor;

                var elem = l.Find(x => x.fi != null && x.fi.id == s.id);
                if (elem != null)
                {
                    res.name_it = elem.name;
                }
                sc.Add(res);
            }
            return sc;
        }

        [HttpGet("{lang}/Floor/{id}")]
        public async Task<ActionResult<FloorInfo>> GetFloor(int id, string lang)
        {
            var t = await this._dbContext
                .floor_info
                .FirstOrDefaultAsync(p => p.id == id);

            if (lang == "it") {
                var it = await _dbContext
                   .floor_info_it
                   .FirstOrDefaultAsync(p => p.id == id);
                if (it != null)
                {
                    t.name = it.name;
                }
            }

            if (t == null)
                return this.NotFound();

            return t;
        }



        [HttpPost("Floor")]
        [Authorize]
        public async Task<ActionResult<FloorInfo>> Post(FloorInfoUpdate serv)
        {
            /*this._dbContext.floor_info.Add(serv);
            await this._dbContext.SaveChangesAsync();
            return this.CreatedAtAction(nameof(this.GetFloor), new { serv.id }, serv);*/

            FloorInfo p = new FloorInfo();
            FloorInfo_it p_it = new FloorInfo_it();
            p.floor = serv.floor;
            p.id = serv.id;
            p.modelBinUrl = serv.modelBinUrl;
            p.modelUrl = serv.modelUrl;
            p.name = serv.name;
            p.navPoints = new List<FloorNavPoint>();
            p.shopsNodes = new List<FloorShop>();

            _dbContext.floor_info.Add(p);

            await _dbContext.SaveChangesAsync();
            var res = CreatedAtAction(nameof(GetFloor), new { id = p.id, lang = "en" }, p);

            p_it.id = p.id;
            p_it.fi = p;
            p_it.name = serv.name_it != null ? serv.name_it : "";

            _dbContext.floor_info_it.Add(p_it);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFloor), new { id = p.id, lang = "en" }, p); ;

        }

        [HttpPut("Floor/{id}")]
        [Authorize]
        public async Task<IActionResult> PutFloor(int id, FloorInfoUpdate serv)
        {
            if (id != serv.id)
                return this.BadRequest();

            var floorInfo = this._dbContext.floor_info.FirstOrDefault(p => p.id == id);
            if (floorInfo == null)
                return this.BadRequest();
            floorInfo.floor = serv.floor;
            floorInfo.modelBinUrl = serv.modelBinUrl;
            floorInfo.modelUrl = serv.modelUrl;
            floorInfo.name = serv.name;

            FloorInfo_it scit = new FloorInfo_it();
            scit.id = serv.id;
            scit.name = serv.name_it != null ? serv.name_it : "";
            scit.fi = floorInfo;

            if (_dbContext.floor_info_it.Any(e => e.id == serv.id))
            {
                _dbContext.Entry(scit).State = EntityState.Modified;
            }
            else
            {
                _dbContext.floor_info_it.Add(scit);
            }


            try
            {
                await this._dbContext.SaveChangesAsync();
                return this.Ok(new PutResult { result = "Ok" });
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!await this.FloorExistsAsync(id))
                    return this.NotFound();
                throw;

            }
        }

        [HttpPost("Floor/GltfFile/{id}")]
        [HttpPost("Floor/BinFile/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            var uf = new UploadFiles(this._options);
            uf = await uf.UploadFileToAsync(Consts.FloorGltf, file);
            return this.Ok(uf);
        }

        private async Task<bool> FloorExistsAsync(long id)
        {
            return await this._dbContext.floor_info.AnyAsync(e => e.id == id);
        }

        #region BABYLON

        [HttpGet("{lang}/FloorView/{id}")]
        public async Task<ActionResult<FloorInfoView>> GetFloorView(int id, string lang)
        {
            var floorModel = await this._dbContext.floor_info
                .AsNoTracking()
                .Include(f => f.navPoints)
                .ThenInclude(np => np.relations)
                .Include(s => s.shopsNodes)
                .FirstOrDefaultAsync(f => f.id == id);
            if (floorModel == null)
                return this.NotFound();


            if (lang == "it")
            {
                var it = await _dbContext
                   .floor_info_it
                   .FirstOrDefaultAsync(p => p.fi.id == id);
                if (it != null)
                {
                    floorModel.name = it.name;
                }
            }

            var shopNodes = new List<FloorShopView>();
            var info = new FloorInfoView
            {
                id = floorModel.id,
                modelUrl = floorModel.modelUrl,
                navPoints = floorModel.navPoints,
                name = floorModel.name,
                shopsNodes = shopNodes
            };


            foreach (var sn in floorModel.shopsNodes)
            {
                var shopView = new FloorShopView
                {
                    id = sn.id,
                    nodeName = sn.nodeName
                };
                shopNodes.Add(shopView);

                if (sn.attachedShopId == null)
                    continue;

                var shop = await this._dbContext.shop.FirstOrDefaultAsync(s => s.id == sn.attachedShopId);
                if (shop == null)
                    continue;
                shopView.attachedShop = new FloorShopInfo
                {
                    id = shop.id,
                    name = shop.title
                };
            }

            return info;

            //var s = await _dbContext.map_shop.Where(s => s.floorId == id).ToListAsync();

            //var x = await _dbContext
            //  .floorInfo
            //  .FirstOrDefaultAsync(p => p.id == id);

            //if (x == null) {
            //    return NotFound();
            //}


            //if (t == null)
            //{
            //    return NotFound();
            //}

            //FloorInfoView fw = new FloorInfoView();
            //fw.id = x.id;
            //fw.name= x.name;
            //fw.modelUrl= x.modelUrl;
            //fw.navPoints = new List<Map_Graph_Node>();
            //fw.shopsNodes = new List<ShopNode>();

            //if (t.Count > 0) {
            //    foreach (Map_Graph_Node e in t) {
            //        if (e.floorId == x.id) {
            //            fw.navPoints.Add(e);
            //        }
            //    }
            //}


            //if (s.Count > 0)
            //{
            //    foreach (Map_Shop e in s)
            //    {
            //        if (e.floorId == x.id)
            //        {
            //            ShopNode sn = new ShopNode();
            //            sn.nodeName = e.nodeName;
            //            if (e.shopId != 0)
            //            {
            //                sn.attachedShop = new ShopInfo();
            //                sn.attachedShop.id = e.shopId;

            //                var y = await _dbContext.shop.FirstOrDefaultAsync(p => p.id == e.shopId);
            //                if (y != null)
            //                {
            //                    sn.attachedShop.title = y.title;
            //                }
            //            }
            //            fw.shopsNodes.Add(sn);
            //        }
            //    }
            //}

            //return fw;
        }

        [HttpGet("NavInfoPoint")]
        public async Task<ActionResult<IReadOnlyCollection<FloorNavPointsInfo>>> GetNavInfoPoint()
        {

            var floors = await this._dbContext.floor_info
                .Include(f => f.navPoints)
                .ToArrayAsync();
            return floors.Select(f => new FloorNavPointsInfo
            {
                id = f.id,
                name = f.name,
                navPoints = f.navPoints.Select(np => np.nodeName)
                    .ToArray()
            }).ToArray();
        }

        [HttpGet("ShopInfo")]
        public async Task<ActionResult<List<FloorShopInfo>>> GetShopInfo()
        {
            var t = await _dbContext
                .shop
                .Select(s => new FloorShopInfo
                {
                    id = s.id,
                    name = s.title
                })
                .ToListAsync();

            return t;
        }


        [HttpPost("SaveFloor/{floorid}")]
        [Authorize]
        public async Task<IActionResult> SaveFloor(int floorid, FloorSaveData data)
        {
            var floor = await this._dbContext.floor_info
                .Include(fi => fi.navPoints)
                .ThenInclude(np => np.relations)
                .Include(f => f.shopsNodes)
                .FirstOrDefaultAsync(f => f.id == floorid);

            if (floor == null)
                return this.NotFound();

            this._dbContext.floor_navpoint.RemoveRange(floor.navPoints);
            this._dbContext.floor_navpoint_relation.RemoveRange(floor.navPoints.SelectMany(np => np.relations));
            this._dbContext.floor_shop.RemoveRange(floor.shopsNodes);
            await this._dbContext.SaveChangesAsync();
            foreach (var np in data.navPoints)
            {
                try
                {
                    np.id = 0;
                    foreach (var r in np.relations)
                        r.id = 0;
                    floor.navPoints.Add(np);
                    //await this._dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {

                }
            }

            foreach (var sn in data.shopsNodes)
                floor.shopsNodes.Add(new FloorShop
                {
                    attachedShopId = sn.attachedShop?.id,
                    nodeName = sn.nodeName
                });

            await this._dbContext.SaveChangesAsync();

            //List<Map_Shop> t = await _dbContext.map_shop.Where(a=>a.floorId == floorid).ToListAsync();

            //if (t != null) {  _dbContext.map_shop.RemoveRange(t); }

            //List<Map_Graph_Node> n = await _dbContext.map_graph_node.Where(a => a.floorId == floorid).ToListAsync();

            //if (n != null) { _dbContext.map_graph_node.RemoveRange(n); }


            //foreach (ShopNode s in data.shopsNodes) { 
            //    Map_Shop ms = new Map_Shop();
            //    ms.floorId = floorid;
            //    if (s.attachedShop != null)
            //    {
            //        ms.shopId = s.attachedShop.id;
            //        //ms.attachedNavPoint = s.attachedShop.title;
            //    }
            //    if (s.nodeName != null)
            //    {
            //        ms.nodeName = s.nodeName;
            //    }
            //    _dbContext.map_shop.Add(ms);
            //}


            //_dbContext.map_graph_node.AddRange(data.navPoints);

            //await _dbContext.SaveChangesAsync();

            return Ok(new PutResult { result = "Ok" });

        }
        #endregion


        #endregion

        #region Mobile

        [HttpGet("{lang}")]
        public async Task<MapView> GetMap(string lang)
        {
            var floorsInfo = await this._dbContext.floor_info.AsNoTracking()
                .OrderBy(f => f.floor)
                .Include(f => f.navPoints)
                .ThenInclude(np => np.relations)
                .Include(f => f.shopsNodes)
                .ToArrayAsync();



            var floors = new List<MapFloorView>();
            var completedGraph = new List<MapGraphNodeView>();
            var accessibilityGraph = new List<MapGraphNodeView>();
            var shops = new List<MapShopView>();

            foreach (var floorInfo in floorsInfo)
            {

                if (lang == "it")
                {
                    var it = await _dbContext
                       .floor_info_it
                       .FirstOrDefaultAsync(p => p.fi.id == floorInfo.id);
                    if (it != null)
                    {
                        floorInfo.name = it.name;
                    }
                }

                floors.Add(new MapFloorView
                {
                    id = floorInfo.id,
                    name = floorInfo.name,
                    modelUrl = floorInfo.modelUrl
                });



                foreach (var np in floorInfo.navPoints)
                {
                    completedGraph.Add(new MapGraphNodeView
                    {
                        relations = np.relations.Select(r => new MapGraphNodeRelationView
                        {
                            targetFloorId = r.targetFloorId,
                            targetNavPointNodeName = r.targetNode,
                            weight = r.linkWeight
                        }).ToArray(),
                        nodeName = np.nodeName,
                        attachedShopNodeName = np.attachedShopNode,
                        floorId = floorInfo.id
                    });

                    accessibilityGraph.Add(new MapGraphNodeView
                    {
                        relations = np.relations
                            .Where(r => r.accessibility)
                            .Select(r => new MapGraphNodeRelationView
                        {
                            targetFloorId = r.targetFloorId,
                            targetNavPointNodeName = r.targetNode,
                            weight = r.linkWeight
                        }).ToArray(),
                        nodeName = np.nodeName,
                        attachedShopNodeName = np.attachedShopNode,
                        floorId = floorInfo.id
                    });
                }

                foreach (var sn in floorInfo.shopsNodes)
                {
                    if (sn.attachedShopId == null)
                        continue;

                    shops.Add(new MapShopView
                    {
                        nodeName = sn.nodeName,
                        shopId = sn.attachedShopId.Value,
                        floorId = floorInfo.id
                    });
                }
            }

            return new MapView
            {
                accessibilityGraph = accessibilityGraph,
                completeGraph = completedGraph,
                floors = floors,
                shops = shops
            };

            //var t = await _dbContext
            //    .map
            //    .Include(a => a.floors)
            //    .Include(a => a.shops)
            //    .Include(a => a.completeGraph).ThenInclude(b => b.relations)
            //    .FirstAsync();

            //if (t == null)
            //{
            //    return NotFound();
            //}

            //Map_View mv = new Map_View();
            //mv.floors = t.floors;
            //mv.shops = t.shops;
            //mv.completeGraph = t.completeGraph;

            //mv.accessibilityGraph = GetCompletGrap(t.completeGraph);

            //return mv;
        }

        #endregion

        //        // GET api/<InterestController>/5
        //        [HttpGet("Floor/{id}")]
        //        public async Task<ActionResult<FloorInfo>> GetFloor(int id)
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.floorInfo == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //var t = await _dbContext
        //            //    .floorInfo
        //            //    .FirstOrDefaultAsync(p => p.id == id); ;

        //            //if (t == null)
        //            //{
        //            //    return NotFound();
        //            //}

        //            //return t;
        //        }

        //        // POST api/<InterestController>
        //        [HttpPost("Floor")]
        //        [Authorize]
        //        public async Task<ActionResult<FloorInfo>> Post(FloorInfo serv)
        //        {
        //            throw new NotImplementedException();
        //            //Map_Map mp = await GetMapPriv();
        //            //if (mp!= null) { 
        //            //    mp.floors.Add(serv);
        //            //}
        //            //_dbContext.floorInfo.Add(serv);
        //            //await _dbContext.SaveChangesAsync();
        //            //return CreatedAtAction(nameof(GetFloor), new { id = serv.id }, serv);
        //        }

        //        // PUT api/<InterestController>/5
        //        [HttpPut("Floor/{id}")]
        //        [Authorize]
        //        public async Task<IActionResult> PutFloor(int id, FloorInfo serv)
        //        {
        //            if (id != serv.id)
        //            {
        //                return BadRequest();
        //            }

        //            _dbContext.Entry(serv).State = EntityState.Modified;

        //            try
        //            {
        //                await _dbContext.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!FloorExists(id))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }

        //            }
        //            return Ok(new PutResult { result="Ok"});
        //        }

        //        private bool FloorExists(long id)
        //        {
        //            throw new NotImplementedException();
        //            //return (_dbContext.floorInfo?.Any(e => e.id == id)).GetValueOrDefault();
        //        }

        //        [HttpDelete("Floor/{id}")]
        //        [Authorize]
        //        public async Task<IActionResult> DeleteFloor(int id)
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.floorInfo == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //var ss = await _dbContext.floorInfo.FindAsync(id);
        //            //if (ss == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //_dbContext.floorInfo.Remove(ss);
        //            //await _dbContext.SaveChangesAsync();
        //            //return Ok(new PutResult { result = "Ok" });
        //        }


        //        [HttpPost("Floor/GltfFile/{id}")]
        //        [HttpPost("Floor/BinFile/{id}")]
        //        [Authorize]
        //        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        //        {
        //            UploadFiles uf = new UploadFiles(this._options);
        //            uf = await uf.UploadFileToAsync(Consts.FloorGltf, file);
        //            return Ok(uf);
        //        }

        //        #endregion

        //        #region MOBILE

        //        #region MAP

        //        [HttpGet]
        //        public async Task<ActionResult<Map_View>> GetMap()
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.map == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //var t = await _dbContext
        //            //    .map
        //            //    .Include(a => a.floors)
        //            //    .Include(a => a.shops)
        //            //    .Include(a => a.completeGraph).ThenInclude(b => b.relations)
        //            //    .FirstAsync();

        //            //if (t == null)
        //            //{
        //            //    return NotFound();
        //            //}

        //            //Map_View mv = new Map_View();
        //            //mv.floors = t.floors;
        //            //mv.shops = t.shops;
        //            //mv.completeGraph = t.completeGraph;

        //            //mv.accessibilityGraph = GetCompletGrap(t.completeGraph);

        //            //return mv;
        //        }

        //        private async Task<Map_Map> GetMapPriv()
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.map == null)
        //            //{
        //            //    return null;
        //            //}
        //            //var t = await _dbContext
        //            //    .map
        //            //    .Include(a => a.floors)
        //            //    .Include(a => a.shops)
        //            //    .Include(a => a.completeGraph)
        //            //    .FirstAsync();

        //            //if (t == null)
        //            //{
        //            //    return null;
        //            //}


        //            //return t;
        //        }

        //        private List<Map_Graph_Node> GetCompletGrap(List<Map_Graph_Node> lt) { 
        //            List<Map_Graph_Node> result = new List<Map_Graph_Node>();
        //            if (lt != null)
        //            {
        //                foreach (Map_Graph_Node l in lt)
        //                {
        //                    if (l.accessibility == 1)
        //                    {
        //                        result.Add(l);
        //                    }

        //                }
        //            }
        //            return result;
        //        }

        //        [HttpGet("{id}")]
        //        public async Task<ActionResult<Map_Map>> GetMapId(int id)
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.map == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //var t = await _dbContext
        //            //    .map
        //            //    .FirstOrDefaultAsync(p => p.id == id); ;

        //            //if (t == null)
        //            //{
        //            //    return NotFound();
        //            //}

        //            //return t;
        //        }

        //        [HttpPost]
        //        [Authorize]
        //        public async Task<ActionResult<Map_Map>> PostMap(Map_Map m)
        //        {
        //            throw new NotImplementedException();
        //            //_dbContext.map.Add(m);
        //            //await _dbContext.SaveChangesAsync();
        //            ////return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
        //            //return CreatedAtAction(nameof(GetMapId), new { id = m.id }, m);
        //        }

        //        [HttpPut("{id}")]
        //        [Authorize]
        //        public async Task<IActionResult> PutMap(int id, Map_Map s)
        //        {
        //            if (id != s.id)
        //            {
        //                return BadRequest();
        //            }

        //            _dbContext.Entry(s).State = EntityState.Modified;

        //            try
        //            {
        //                await _dbContext.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!MapExists(id))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }

        //            }
        //            return Ok(new PutResult { result = "Ok" });
        //        }

        //        private bool MapExists(long id)
        //        {
        //            throw new NotImplementedException();
        //            //return (_dbContext.map?.Any(e => e.id == id)).GetValueOrDefault();
        //        }

        //        [HttpDelete("{id}")]
        //        [Authorize]
        //        public async Task<IActionResult> DeleteMap(int id)
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.map == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //var ss = await _dbContext.map.FindAsync(id);
        //            //if (ss == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //_dbContext.map.Remove(ss);
        //            //await _dbContext.SaveChangesAsync();
        //            //return Ok(new PutResult { result = "Ok" });
        //        }
        //        #endregion

        //        #region Map_Shop

        //        [HttpPost("MapShop")]
        //        [Authorize]
        //        public async Task<ActionResult<Map_Shop>> PostMapShop(Map_Shop s)
        //        {
        //            throw new NotImplementedException();
        ////Map_Map mp = await GetMapPriv();
        //            //   if (mp!= null) { 
        //            //       mp.shops.Add(s);
        //            //   }
        //            //   _dbContext.map_shop.Add(s);
        //            //   await _dbContext.SaveChangesAsync();
        //            //   return CreatedAtAction(nameof(GetMapShop), new { id = s.id }, s);
        //        }


        //        [HttpGet("MapShop/{id}")]
        //        public async Task<ActionResult<Map_Shop>> GetMapShop(int id)
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.map_shop == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //var t = await _dbContext
        //            //    .map_shop
        //            //    .FirstOrDefaultAsync(p => p.id == id); ;

        //            //if (t == null)
        //            //{
        //            //    return NotFound();
        //            //}

        //            //return t;
        //        }

        //        [HttpPut("MapShop/{id}")]
        //        [Authorize]
        //        public async Task<IActionResult> PutMapShop(int id, Map_Shop s)
        //        {
        //            if (id != s.id)
        //            {
        //                return BadRequest();
        //            }

        //            _dbContext.Entry(s).State = EntityState.Modified;

        //            try
        //            {
        //                await _dbContext.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!MapShopExists(id))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }

        //            }
        //            return Ok(new PutResult { result="Ok"});
        //        }

        //        private bool MapShopExists(long id)
        //        {
        //            throw new NotImplementedException();
        //            //return (_dbContext.map_shop?.Any(e => e.id == id)).GetValueOrDefault();
        //        }

        //        [HttpDelete("MapShop/{id}")]
        //        [Authorize]
        //        public async Task<IActionResult> DeleteMapShop(int id)
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.map_shop == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //var ss = await _dbContext.map_shop.FindAsync(id);
        //            //if (ss == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //_dbContext.map_shop.Remove(ss);
        //            //await _dbContext.SaveChangesAsync();
        //            //return Ok(new PutResult { result="Ok"});
        //        }

        //        #endregion

        //        #region MAP GRAPH NODE
        //        [HttpPost("MapNode")]
        //        [Authorize]
        //        public async Task<ActionResult<Map_Graph_Node>> PostMapNode(Map_Graph_Node s)
        //        {
        //            throw new NotImplementedException();
        //            //Map_Map mp = await GetMapPriv();
        //            //if (mp != null)
        //            //{
        //            //    mp.completeGraph.Add(s);
        //            //}
        //            //_dbContext.map_graph_node.Add(s);
        //            //await _dbContext.SaveChangesAsync();
        //            //return CreatedAtAction(nameof(GetMapNode), new { id = s.id }, s);
        //        }


        //        [HttpGet("MapNode/{id}")]
        //        public async Task<ActionResult<Map_Graph_Node>> GetMapNode(int id)
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.map_graph_node == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //var t = await _dbContext
        //            //    .map_graph_node
        //            //    .FirstOrDefaultAsync(p => p.id == id); ;

        //            //if (t == null)
        //            //{
        //            //    return NotFound();
        //            //}

        //            //return t;
        //        }

        //        [HttpPut("MapNode/{id}")]
        //        [Authorize]
        //        public async Task<IActionResult> PutMapNode(int id, Map_Graph_Node s)
        //        {
        //            if (id != s.id)
        //            {
        //                return BadRequest();
        //            }

        //            _dbContext.Entry(s).State = EntityState.Modified;

        //            try
        //            {
        //                await _dbContext.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!MapNodeExists(id))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }

        //            }
        //            return Ok(new PutResult { result = "Ok" });
        //        }

        //        private bool MapNodeExists(long id)
        //        {
        //            throw new NotImplementedException();
        //            //return (_dbContext.map_graph_node?.Any(e => e.id == id)).GetValueOrDefault();
        //        }

        //        [HttpDelete("MapNode/{id}")]
        //        [Authorize]
        //        public async Task<IActionResult> DeleteMapNode(int id)
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.map_graph_node == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //var ss = await _dbContext.map_graph_node.FindAsync(id);
        //            //if (ss == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //_dbContext.map_graph_node.Remove(ss);
        //            //await _dbContext.SaveChangesAsync();
        //            //return Ok(new PutResult { result = "Ok" });
        //        }
        //        #endregion

        //        #region Map Graph Node RELATIONS

        //        [HttpPost("NodeRelation")]
        //        [Authorize]
        //        public async Task<ActionResult<Map_Graph_Node_Relations>> PostMapRelation(Map_Graph_Node_Relations s)
        //        {
        //            throw new NotImplementedException();
        //            //Map_Graph_Node mp = await GetMapRelationPriv(s.targetNavPointNodeName);
        //            //if (mp != null)
        //            //{
        //            //    mp.relations.Add(s);
        //            //}
        //            //_dbContext.map_graph_node_relations.Add(s);
        //            //await _dbContext.SaveChangesAsync();
        //            //return CreatedAtAction(nameof(GetMapRelation), new { id = s.id }, s);
        //        }


        //        private async Task<Map_Graph_Node> GetMapRelationPriv(string nodeName)
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.map == null)
        //            //{
        //            //    return null;
        //            //}
        //            //var t = await _dbContext
        //            //    .map_graph_node
        //            //    .Include(a => a.relations).Where(a=>a.nodeName == nodeName)
        //            //    .FirstAsync();

        //            //if (t == null)
        //            //{
        //            //    return null;
        //            //}


        //            //return t;
        //        }

        //        [HttpGet("NodeRelation/{id}")]
        //        public async Task<ActionResult<Map_Graph_Node_Relations>> GetMapRelation(int id)
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.map_graph_node_relations == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //var t = await _dbContext
        //            //    .map_graph_node_relations
        //            //    .FirstOrDefaultAsync(p => p.id == id); ;

        //            //if (t == null)
        //            //{
        //            //    return NotFound();
        //            //}

        //            //return t;
        //        }

        //        [HttpPut("NodeRelation/{id}")]
        //        [Authorize]
        //        public async Task<IActionResult> PutMapRelation(int id, Map_Graph_Node_Relations s)
        //        {
        //            if (id != s.id)
        //            {
        //                return BadRequest();
        //            }

        //            _dbContext.Entry(s).State = EntityState.Modified;

        //            try
        //            {
        //                await _dbContext.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!MapRelationExists(id))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }

        //            }
        //            return Ok(new PutResult { result = "Ok" });
        //        }

        //        private bool MapRelationExists(long id)
        //        {
        //            throw new NotImplementedException();
        //            //return (_dbContext.map_graph_node_relations?.Any(e => e.id == id)).GetValueOrDefault();
        //        }

        //        [HttpDelete("NodeRelation/{id}")]
        //        [Authorize]
        //        public async Task<IActionResult> DeleteMapRelation(int id)
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.map_graph_node_relations == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //var ss = await _dbContext.map_graph_node_relations.FindAsync(id);
        //            //if (ss == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //_dbContext.map_graph_node_relations.Remove(ss);
        //            //await _dbContext.SaveChangesAsync();
        //            //return Ok(new PutResult { result = "Ok" });
        //        }

        //        #endregion


        //        #region BABYLON

        //        [HttpGet("FloorView/{id}")]
        //        public async Task<ActionResult<FloorInfoView>> GetFloorView(int id)
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.map == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //var t = await _dbContext.map_graph_node.Where(s => s.floorId == id).ToListAsync();
        //            //var s = await _dbContext.map_shop.Where(s => s.floorId == id).ToListAsync();

        //            //var x = await _dbContext
        //            //  .floorInfo
        //            //  .FirstOrDefaultAsync(p => p.id == id);

        //            //if (x == null) {
        //            //    return NotFound();
        //            //}


        //            //if (t == null)
        //            //{
        //            //    return NotFound();
        //            //}

        //            //FloorInfoView fw = new FloorInfoView();
        //            //fw.id = x.id;
        //            //fw.name= x.name;
        //            //fw.modelUrl= x.modelUrl;
        //            //fw.navPoints = new List<Map_Graph_Node>();
        //            //fw.shopsNodes = new List<ShopNode>();

        //            //if (t.Count > 0) {
        //            //    foreach (Map_Graph_Node e in t) {
        //            //        if (e.floorId == x.id) {
        //            //            fw.navPoints.Add(e);
        //            //        }
        //            //    }
        //            //}


        //            //if (s.Count > 0)
        //            //{
        //            //    foreach (Map_Shop e in s)
        //            //    {
        //            //        if (e.floorId == x.id)
        //            //        {
        //            //            ShopNode sn = new ShopNode();
        //            //            sn.nodeName = e.nodeName;
        //            //            if (e.shopId != 0)
        //            //            {
        //            //                sn.attachedShop = new ShopInfo();
        //            //                sn.attachedShop.id = e.shopId;

        //            //                var y = await _dbContext.shop.FirstOrDefaultAsync(p => p.id == e.shopId);
        //            //                if (y != null)
        //            //                {
        //            //                    sn.attachedShop.title = y.title;
        //            //                }
        //            //            }
        //            //            fw.shopsNodes.Add(sn);
        //            //        }
        //            //    }
        //            //}

        //            //return fw;
        //        }

        //        [HttpGet("NavInfoPoint")]
        //        public async Task<ActionResult<List<NavPointInfo>>> GetNavInfoPoint()
        //        {
        //            throw new NotImplementedException();
        //            //if (_dbContext.map == null)
        //            //{
        //            //    return NotFound();
        //            //}
        //            //var t = await _dbContext
        //            //    .floorInfo
        //            //    .ToListAsync();

        //            //var m = await _dbContext
        //            //.map
        //            //.Include(a => a.shops)
        //            //.Include(a => a.completeGraph).ThenInclude(b => b.relations)
        //            //.FirstAsync();

        //            //if (t == null)
        //            //{
        //            //    return NotFound();
        //            //}

        //            //List<NavPointInfo> lnp = new List<NavPointInfo>();

        //            //foreach (FloorInfo e in t)
        //            //{
        //            //    NavPointInfo fw = new NavPointInfo();
        //            //    fw.id = e.id;
        //            //    fw.name = e.name;
        //            //    fw.navPoints = new List<string>();
        //            //    if(m.completeGraph != null)
        //            //    foreach (Map_Graph_Node mg in m.completeGraph) {
        //            //            if (mg.floorId == e.id) {
        //            //                fw.navPoints.Add(mg.nodeName);
        //            //            }    
        //            //    }

        //            //    lnp.Add(fw);
        //            //}


        //            //return lnp;
        //        }


        //        [HttpGet("ShopInfo")]
        //        public async Task<ActionResult<List<ShopInfo>>> GetShopInfo()
        //        {
        //            if (_dbContext.shop == null)
        //            {
        //                return NotFound();
        //            }
        //            var t = await _dbContext
        //                .shop.Include(a => a.openingHours).ToListAsync();

        //            if (t == null)
        //            {
        //                return NotFound();
        //            }

        //            List<ShopInfo> ls = new List<ShopInfo>();

        //            foreach (Shop s in t) {
        //                ShopInfo si = new ShopInfo();
        //                si.id = s.id;
        //                si.title = s.title;
        //                ls.Add(si);
        //            }

        //            return ls;
        //        }


        //        [HttpPost("SaveFloor/{floorid}")]
        //        [Authorize]
        //        public async Task<IActionResult> SaveFloor(int floorid, FloorSaveData data) {
        //            throw new NotImplementedException();
        //            //List<Map_Shop> t = await _dbContext.map_shop.Where(a=>a.floorId == floorid).ToListAsync();

        //            //if (t != null) {  _dbContext.map_shop.RemoveRange(t); }

        //            //List<Map_Graph_Node> n = await _dbContext.map_graph_node.Where(a => a.floorId == floorid).ToListAsync();

        //            //if (n != null) { _dbContext.map_graph_node.RemoveRange(n); }


        //            //foreach (ShopNode s in data.shopsNodes) { 
        //            //    Map_Shop ms = new Map_Shop();
        //            //    ms.floorId = floorid;
        //            //    if (s.attachedShop != null)
        //            //    {
        //            //        ms.shopId = s.attachedShop.id;
        //            //        //ms.attachedNavPoint = s.attachedShop.title;
        //            //    }
        //            //    if (s.nodeName != null)
        //            //    {
        //            //        ms.nodeName = s.nodeName;
        //            //    }
        //            //    _dbContext.map_shop.Add(ms);
        //            //}


        //            //_dbContext.map_graph_node.AddRange(data.navPoints);

        //            //await _dbContext.SaveChangesAsync();

        //            //return Ok(new PutResult { result = "Ok" });

        //        }


        //        #endregion

        //        #endregion
    }
}
