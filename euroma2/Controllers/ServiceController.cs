using euroma2.Models.Interest;
using euroma2.Models.Reach;
using euroma2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using euroma2.Models.Service;
using Microsoft.AspNetCore.Authorization;
using euroma2.Services;
using Microsoft.Extensions.Options;

namespace euroma2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly PtaInfo _options;
        public ServiceController(DataContext dbContext, IOptions<PtaInfo> options)
        {
            _dbContext = dbContext;
            _options = options.Value;
        }

        // GET: api/<InterestController>
        [HttpGet("{lang}/Service")]
        public async Task<ActionResult<IEnumerable<Service_model>>> Get(string lang)
        {
            if (_dbContext.service == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .service
                .ToListAsync();
            foreach (Service_model s in t)
            {
                if (lang == "it")
                {
                    var it = await _dbContext
                    .service_it
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

        [HttpGet("Service")]
        public async Task<ActionResult<IEnumerable<Service_modelCMS>>> Get()
        {
            if (_dbContext.service == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .service
                .ToListAsync();

            var l = await _dbContext.service_it.ToListAsync();

            List<Service_modelCMS> sc = new List<Service_modelCMS>();

            foreach (Service_model s in t)
            {
                Service_modelCMS res = new Service_modelCMS(s);
                var elem = l.Find(x => x.sm != null && x.sm.id == s.id);
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
        [HttpGet("{lang}/Service/{id}")]
        public async Task<ActionResult<Service_model>> GetService(int id, string lang)
        {
            if (_dbContext.service == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .service
                .FirstOrDefaultAsync(p => p.id == id); 

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
        [HttpPost("Service")]
        [Authorize]
        public async Task<ActionResult<Service_model>> Post(Service_modelCMS serv)
        {
            /*_dbContext.service.Add(serv);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetService), new { id = serv.id }, serv);*/
            Service_model p = new Service_model();
            Service_model_it p_it = new Service_model_it();
            p.icon = serv.icon;
            p.order = serv.order;
            p.title = serv.title;
            p.description = serv.description;

            _dbContext.service.Add(p);

            await _dbContext.SaveChangesAsync();
            var res = CreatedAtAction(nameof(GetService), new { id = p.id, lang = "en" }, p);

            p_it.id = p.id;
            p_it.sm = p;
            p_it.title = serv.title_it;
            p_it.description = serv.description_it;

            _dbContext.service_it.Add(p_it);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetService), new { id = p.id, lang = "en" }, p); ;
        }

        // PUT api/<InterestController>/5
        [HttpPut("Service/{id}")]
        [Authorize]
        public async Task<IActionResult> PutService(int id, Service_modelCMS serv)
        {
            if (id != serv.id)
            {
                return BadRequest();
            }

            // _dbContext.Entry(serv).State = EntityState.Modified;
            Service_model sc = new Service_model();
            sc.id = serv.id;
            sc.icon = serv.icon;
            sc.description = serv.description;
            sc.title = serv.title;
            sc.order = serv.order;

            Service_model_it scit = new Service_model_it();
            scit.id = serv.id;
            scit.title = serv.title_it;
            scit.description = serv.description_it;
            scit.sm = sc;

            _dbContext.Entry(sc).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            if (_dbContext.service_it.Any(e => e.id == serv.id))
            {
                _dbContext.Entry(scit).State = EntityState.Modified;
            }
            else
            {
                _dbContext.service_it.Add(scit);
            }


            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ServiceExists(id))
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

        private bool ServiceExists(long id)
        {
            return (_dbContext.service?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpDelete("Service/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteService(int id)
        {
            if (_dbContext.service == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.service.FindAsync(id);
            if (ss == null)
            {
                return NotFound();
            }
            _dbContext.service.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }


        [HttpPost("Service/ImgUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles(this._options);
            uf = await uf.UploadFileToAsync(Consts.ServiceImg, file);
            return Ok(uf);
        }
    }
}
