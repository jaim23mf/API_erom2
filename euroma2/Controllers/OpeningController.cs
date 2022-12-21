using euroma2.Models.Service;
using euroma2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using euroma2.Models.Hours;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace euroma2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpeningController : ControllerBase
    {
        private readonly DataContext _dbContext;
        public OpeningController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<InterestController>
        [HttpGet]
        public async Task<ActionResult<Opening>> Get()
        {
            if (_dbContext.opening_hours == null)
            {
                return NotFound();
            }
           /* var t = await _dbContext
                .opening_hours
                .Include(a=>a.general.global)
                .Include(a=>a.general.food)
                .Include(a=>a.general.hypermarket)
                .Include(a=>a.general.ourStores)
                .Include(a => a.exceptions).ThenInclude(a=>a.global)
                .Include(a => a.exceptions).ThenInclude(a=>a.dateRange)
                .Include(a => a.exceptions).ThenInclude(a=>a.food)
                .Include(a => a.exceptions).ThenInclude(a=>a.hypermarket)
                .Include(a => a.exceptions).ThenInclude(a=>a.ourStores)
                .ToListAsync();*/
            var g = await _dbContext
                .opening_general
                .Include(a => a.global)
                .Include(a => a.food)
                .Include(a => a.hypermarket)
                .Include(a => a.ourStores)
                .ToListAsync();

            var e = await _dbContext
                .opening_exceptions
                .Include(a => a.dateRange)
                .Include(a => a.global)
                .Include(a => a.food)
                .Include(a => a.hypermarket)
                .Include(a => a.ourStores)
                .ToListAsync();

            Opening t = new Opening();
            t.exceptions = e;
            t.general = g[0];

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        [HttpGet("General")]
        public async Task<ActionResult<IEnumerable<General>>> GetGeneral()
        {
            if (_dbContext.opening_general == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .opening_general
                .Include(a => a.global)
                .Include(a => a.food)
                .Include(a => a.hypermarket)
                .Include(a => a.ourStores)
                .ToListAsync();

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        [HttpGet("Exceptions")]
        public async Task<ActionResult<IEnumerable<Exception_Rules>>> GetException()
        {
            if (_dbContext.opening_exceptions == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .opening_exceptions
                .Include(a => a.dateRange)
                .Include(a => a.global)
                .Include(a => a.food)
                .Include(a => a.hypermarket)
                .Include(a => a.ourStores)
                .ToListAsync();

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        //POST

        [HttpPost("General")]
        [Authorize]
        public async Task<ActionResult<General>> Post(General serv)
        {
            _dbContext.opening_general.Add(serv);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetGeneralId), new { id = serv.id }, serv);
        }

        [HttpPost("Exceptions")]
        [Authorize]
        public async Task<ActionResult<Exception_Rules>> Post(Exception_Rules serv)
        {
            _dbContext.opening_exceptions.Add(serv);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetExceptionId), new { id = serv.id }, serv);
        }



        //GET BY ID

        [HttpGet("General/{id}")]
        public async Task<ActionResult<General>> GetGeneralId(int id)
        {
            if (_dbContext.opening_general == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .opening_general
                .Include(a => a.global)
                .Include(a => a.food)
                .Include(a => a.hypermarket)
                .Include(a => a.ourStores)
                .FirstOrDefaultAsync(p => p.id == id); ;

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }


        [HttpGet("Exception/{id}")]
        public async Task<ActionResult<Exception_Rules>> GetExceptionId(int id)
        {
            if (_dbContext.opening_exceptions == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .opening_exceptions
                 .Include(a => a.dateRange)
                .Include(a => a.global)
                .Include(a => a.food)
                .Include(a => a.hypermarket)
                .Include(a => a.ourStores)
                .FirstOrDefaultAsync(p => p.id == id); ;

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        //PUT GENERAL && PUT EXCEPTION

        [HttpPut("General/{id}")]
        [Authorize]
        public async Task<IActionResult> PutGeneral(int id, General serv)
        {
            if (id != serv.id)
            {
                return BadRequest();
            }

            _dbContext.Entry(serv).State = EntityState.Modified;
            _dbContext.Entry(serv.global).State = EntityState.Modified;
            //_dbContext.Entry(serv.food.fromWeekDay).State = EntityState.Modified;
            //_dbContext.Entry(serv.food.toWeekDay).State = EntityState.Modified;
            _dbContext.Entry(serv.food).State = EntityState.Modified;
            _dbContext.Entry(serv.hypermarket).State = EntityState.Modified;
            //_dbContext.Entry(serv.hypermarket.fromWeekDay).State = EntityState.Modified;
            //_dbContext.Entry(serv.hypermarket.toWeekDay).State = EntityState.Modified;
            _dbContext.Entry(serv.ourStores).State = EntityState.Modified;
            //_dbContext.Entry(serv.ourStores.fromWeekDay).State = EntityState.Modified;
            //_dbContext.Entry(serv.ourStores.toWeekDay).State = EntityState.Modified;


            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeneralExists(id))
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

        private bool GeneralExists(long id)
        {
            return (_dbContext.opening_general?.Any(e => e.id == id)).GetValueOrDefault();
        }


        [HttpPut("Exception/{id}")]
        [Authorize]
        public async Task<IActionResult> PutExceptions(int id, Exception_Rules serv)
        {
            if (id != serv.id)
            {
                return BadRequest();
            }

            _dbContext.Entry(serv).State = EntityState.Modified;
            _dbContext.Entry(serv.dateRange).State = EntityState.Modified;
            _dbContext.Entry(serv.global).State = EntityState.Modified;
            _dbContext.Entry(serv.food).State = EntityState.Modified;
            _dbContext.Entry(serv.hypermarket).State = EntityState.Modified;
            _dbContext.Entry(serv.ourStores).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExExists(id))
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

        private bool ExExists(long id)
        {
            return (_dbContext.opening_exceptions?.Any(e => e.id == id)).GetValueOrDefault();
        }


        //DELETE SOLO PARA EXCEPTION

        [HttpDelete("Exception/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteExceptions(int id)
        {
            if (_dbContext.opening_exceptions == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.opening_exceptions.FindAsync(id);
            if (ss == null)
            {
                return NotFound();
            }
            _dbContext.opening_exceptions.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        // GET api/<InterestController>/5
        /* [HttpGet("{id}")]
         [Authorize]
         public async Task<ActionResult<Opening>> GetOpening(int id)
         {
             if (_dbContext.opening_hours == null)
             {
                 return NotFound();
             }
             var t = await _dbContext
                 .opening_hours
                 .Include(a => a.general)
                 .Include(a => a.exceptions)
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
         public async Task<ActionResult<Opening>> Post(Opening serv)
         {
             _dbContext.opening_hours.Add(serv);
             await _dbContext.SaveChangesAsync();
             //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
             return CreatedAtAction(nameof(GetOpening), new { id = serv.id }, serv);
         }

         // PUT api/<InterestController>/5
         [HttpPut("{id}")]
         [Authorize]
         public async Task<IActionResult> PutOpening(int id, Opening serv)
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
                 if (!OpeningExists(id))
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

         private bool OpeningExists(long id)
         {
             return (_dbContext.opening_hours?.Any(e => e.id == id)).GetValueOrDefault();
         }

         [HttpDelete("{id}")]
         [Authorize]
         public async Task<IActionResult> DeleteOpening(int id)
         {
             if (_dbContext.opening_hours == null)
             {
                 return NotFound();
             }
             var ss = await _dbContext.opening_hours.FindAsync(id);
             if (ss == null)
             {
                 return NotFound();
             }
             _dbContext.opening_hours.Remove(ss);
             await _dbContext.SaveChangesAsync();
             return NoContent();
         }*/
    }
}
