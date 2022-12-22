using euroma2.Models.Interest;
using euroma2.Models.Reach;
using euroma2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using euroma2.Models.Service;
using Microsoft.AspNetCore.Authorization;
using euroma2.Services;

namespace euroma2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly DataContext _dbContext;
        public ServiceController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<InterestController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service_model>>> Get()
        {
            if (_dbContext.service == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .service
                .ToListAsync();

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        // GET api/<InterestController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Service_model>> GetService(int id)
        {
            if (_dbContext.service == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .service
                .FirstOrDefaultAsync(p => p.id == id); ;

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        // POST api/<InterestController>
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Service_model>> Post(Service_model serv)
        {
            _dbContext.service.Add(serv);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetService), new { id = serv.id }, serv);
        }

        // PUT api/<InterestController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutService(int id, Service_model serv)
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

        [HttpDelete("{id}")]
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


        [HttpPost("ImgUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles();
            uf = await uf.UploadFileToAsync("ServiceImg", file);
            return Ok(uf);
        }
    }
}
