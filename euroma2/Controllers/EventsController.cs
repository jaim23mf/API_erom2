using euroma2.Models.Events;
using euroma2.Models.Promo;
using euroma2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace euroma2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly DataContext _dbContext;
        public EventsController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<EventView>>> GetPromos()
        {
            if (_dbContext.events == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .events
                .Include(a => a.interestIds)
                .Include(a => a.dateRange)
                .ToListAsync();

            List<EventView> sc = new List<EventView>();

            foreach (Mall_Event s in t)
            {
                EventView res = new EventView(s);

                res.interestIds = GetInterest(s.interestIds);
                sc.Add(res);
            }

            return sc;

        }

        private List<int> GetInterest(List<LineaInterest> interest)
        {

            List<int> lsc = new List<int>();

            foreach (LineaInterest ls in interest)
            {
                lsc.Add(ls.id_interest);
            }
            return lsc;
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Mall_Event>> PostEvents(Mall_Event events)
        {
            _dbContext.events.Add(events);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetEvents), new { id = events.id }, events);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<EventView>> GetEvents(int id)
        {
            if (_dbContext.events == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .events
                .Include(a => a.interestIds)
                .Include(a => a.dateRange)
                .FirstOrDefaultAsync(p => p.id == id); ;

            if (t == null)
            {
                return NotFound();
            }

            EventView res = new EventView(t);

            res.interestIds = GetInterest(t.interestIds);


            return res;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutEvents(int id, Mall_Event events)
        {
            if (id != events.id)
            {
                return BadRequest();
            }

            _dbContext.Entry(events).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EventsExists(id))
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

        private bool EventsExists(long id)
        {
            return (_dbContext.events?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteEvents(int id)
        {
            if (_dbContext.promotion == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.promotion.FindAsync(id);
            if (ss == null)
            {
                return NotFound();
            }
            _dbContext.promotion.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}
