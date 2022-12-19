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

        // GET: api/<InterestController>
        [HttpGet("Floor")]
        [Authorize]
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
        [Authorize]
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
            _dbContext.floorInfo.Add(serv);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
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
            return NoContent();
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
            return NoContent();
        }


        [HttpPost("Floor/GltfFile/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles();
            uf = await uf.UploadFileToAsync("FloorGltf", file);
            return Ok(uf);
        }
    }
}
