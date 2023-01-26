using euroma2.Models.Interest;
using euroma2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using euroma2.Models.Reach;
using Microsoft.AspNetCore.Authorization;
using euroma2.Services;
using Microsoft.Extensions.Options;
using euroma2.Models.Promo;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace euroma2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ReachController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly PtaInfo _options;
        public ReachController(DataContext dbContext, IOptions<PtaInfo> options)
        {
            _dbContext = dbContext;
            _options = options.Value;
        }

        // GET: api/<InterestController>
        [HttpGet("{lang}/Reach")]
        public async Task<ActionResult<IEnumerable<Reach_Us>>> Get(string lang)
        {
            if (_dbContext.reach == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .reach
                .ToListAsync();


            foreach (Reach_Us s in t)
            {
                if (lang == "it")
                {
                    var it = await _dbContext
                    .reach_it
                    .FirstOrDefaultAsync(p => p.id == s.id);
                    if (it != null)
                    {
                        s.title = it.title;
                        s.description = it.description;
                    }
                }
            }


            if (t == null)
            {
                return NotFound();
            }

            return t;
        }


        [HttpGet("ReachCMS")]
        public async Task<ActionResult<IEnumerable<Reach_UsCMS>>> GetCMS()
        {
            if (_dbContext.reach == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .reach
                .ToListAsync();

            var l = await _dbContext.reach_it.ToListAsync();

            List<Reach_UsCMS> sc = new List<Reach_UsCMS>();

            foreach (Reach_Us s in t)
            {
                Reach_UsCMS res = new Reach_UsCMS(s);
                var elem = l.Find(x => x.reach != null && x.reach.id == s.id);
                if (elem != null)
                {
                    res.title_it = elem.title;
                    res.description_it = elem.description;
                }
                sc.Add(res);
            }
            return sc;
        }

        // GET api/<InterestController>/5
        [HttpGet("{lang}/Reach/{id}")]
        public async Task<ActionResult<Reach_Us>> GetReach(int id,string lang)
        {
            if (_dbContext.interests == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .reach
                .FirstOrDefaultAsync(p => p.id == id); ;


            if (lang == "it")
            {
                var it = await _dbContext
                .reach_it
                .FirstOrDefaultAsync(p => p.id == id);
                if (it != null)
                {
                    t.title = it.title;
                    t.description = it.description;
                }
            }

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        // POST api/<InterestController>
        [HttpPost("Reach")]
        [Authorize]
        public async Task<ActionResult<Reach_Us>> Post(Reach_UsCMS reach)
        {
            /*_dbContext.reach.Add(reach);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetReach), new { id = reach.id }, reach);*/

            Reach_Us p = new Reach_Us();
            Reach_Us_it p_it = new Reach_Us_it();
            p.icon = reach.icon;
            p.order = reach.order;
            p.title = reach.title;
            p.description = reach.description;

            _dbContext.reach.Add(p);

            await _dbContext.SaveChangesAsync();
            var res = CreatedAtAction(nameof(GetReach), new { id = p.id, lang = "en" }, p);

            p_it.id = p.id;
            p_it.reach = p;
            p_it.title = reach.title_it;
            p_it.description = reach.description_it;

            _dbContext.reach_it.Add(p_it);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetReach), new { id = p.id, lang = "en" }, p); ;


        }

        // PUT api/<InterestController>/5
        [HttpPut("Reach/{id}")]
        [Authorize]
        public async Task<IActionResult> PutReach(int id, Reach_UsCMS reach)
        {



            if (id != reach.id)
            {
                return BadRequest();
            }

            Reach_Us sc = new Reach_Us();
            sc.id = reach.id;
            sc.icon = reach.icon;
            sc.description = reach.description;
            sc.title = reach.title;
            sc.order = reach.order;

            Reach_Us_it scit = new Reach_Us_it();
            scit.id = reach.id;
            scit.title = reach.title_it;
            scit.description = reach.description_it;
            scit.reach = sc;

            _dbContext.Entry(sc).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();


            if (_dbContext.reach_it.Any(e => e.id == reach.id))
            {
                _dbContext.Entry(scit).State = EntityState.Modified;
            }
            else
            {
                _dbContext.reach_it.Add(scit);
            }

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReachExists(id))
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

        private bool ReachExists(long id)
        {
            return (_dbContext.reach?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpDelete("Reach/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteReach(int id)
        {
            if (_dbContext.reach == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.reach.FindAsync(id);
            if (ss == null)
            {
                return NotFound();
            }
            _dbContext.reach.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }


        [HttpPost("Reach/ImgUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles(this._options);
            uf = await uf.UploadFileToAsync(Consts.ReachImg, file);
            return Ok(uf);
        }
    }
}
