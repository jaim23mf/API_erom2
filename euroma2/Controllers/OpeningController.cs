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


        [HttpGet("OpeningMobile")]
        public async Task<ActionResult<OpeningView>> GetMobile()
        {
            if (_dbContext.opening_hours == null)
            {
                return NotFound();
            }
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

            OpeningView t = new OpeningView();
            //t.exceptions = e;
            t.exceptions = e.Select(ex => new Exception_Rule_View
            {
                dateRange = ex.dateRange,
                global = ex.global,
                food = new Day_Opening_Hours_Food_View
                {
                    from = ex.food.from,
                    fromWeekDay = ex.food.fromWeekDay.ToString(),
                    id = ex.food.id,
                    to = ex.food.to,
                    toWeekDay = ex.food.toWeekDay.ToString()
                },
                hypermarket = new Day_Opening_Hours_Hipermarket_View()
                {
                    from = ex.hypermarket.from,
                    fromWeekDay = ex.hypermarket.fromWeekDay.ToString(),
                    id = ex.hypermarket.id,
                    to = ex.hypermarket.to,
                    toWeekDay = ex.hypermarket.toWeekDay.ToString()
                },
                ourStores = new Day_Opening_Hours_Stores_View()
                {
                    from = ex.ourStores.from,
                    fromWeekDay = ex.ourStores.fromWeekDay.ToString(),
                    id = ex.ourStores.id,
                    to = ex.ourStores.to,
                    toWeekDay = ex.ourStores.toWeekDay.ToString()
                }
            }).ToList();
            t.general.global = g[0].global;
            t.general.id = g[0].id;

            t.general.food.id = g[0].food.id;
            t.general.food.from = g[0].food.from;
            t.general.food.to = g[0].food.to;
            t.general.food.fromWeekDay = g[0].food.fromWeekDay.ToString();
            t.general.food.toWeekDay = g[0].food.toWeekDay.ToString();

            t.general.hypermarket.id = g[0].hypermarket.id;
            t.general.hypermarket.from = g[0].hypermarket.from;
            t.general.hypermarket.to = g[0].hypermarket.to;
            t.general.hypermarket.fromWeekDay = g[0].hypermarket.fromWeekDay.ToString();
            t.general.hypermarket.toWeekDay = g[0].hypermarket.toWeekDay.ToString();

            t.general.ourStores.id = g[0].ourStores.id;
            t.general.ourStores.from = g[0].ourStores.from;
            t.general.ourStores.to = g[0].ourStores.to;
            t.general.ourStores.fromWeekDay = g[0].ourStores.fromWeekDay.ToString();
            t.general.ourStores.toWeekDay = g[0].ourStores.toWeekDay.ToString();



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
            return Ok(new PutResult { result = "Ok" });
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
            return Ok(new PutResult { result = "Ok" });
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
            return Ok(new PutResult { result = "Ok" });
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
            return Ok(new PutResult { result="Ok"});
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
            return Ok(new PutResult { result="Ok"});
         }*/
    }
}
