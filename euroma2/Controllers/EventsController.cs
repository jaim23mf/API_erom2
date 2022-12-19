using euroma2.Models.Events;
using euroma2.Models.Promo;
using euroma2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using euroma2.Services;

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
        public async Task<ActionResult<IEnumerable<EventView>>> GetEvents()
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

        private List<int> GetInterest(List<LineaInterest_event> interest)
        {

            List<int> lsc = new List<int>();

            foreach (LineaInterest_event ls in interest)
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

            /*await DeleteInterestEvent(promo.id);
            await PostInterest(promo);
            */
            //_dbContext.Entry(events.interestIds).State = EntityState.Modified;

            _dbContext.Entry(events.dateRange).State = EntityState.Modified;

            _dbContext.Entry(events).State = EntityState.Modified;


            await DeleteInterestEvent(events.id);

            events.interestIds.ForEach(item => 
                    _dbContext.liEvents.Add(item)
            );

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

        private bool LiEventsExists(long id)
        {
            return (_dbContext.liEvents?.Any(e => e.id == id)).GetValueOrDefault();
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

            await DeleteInterestEvent(ss.id);

            _dbContext.promotion.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }


        private async Task<IActionResult> DeleteInterestEvent(int id)
        {
            if (_dbContext.liEvents == null)
            {
                return NotFound();
            }
            // var ss = await _dbContext.liShop.;

            var query = from st in _dbContext.liEvents
                        where st.id_event == id
                        select st;

            var student = query.ToList<LineaInterest_event>();

            if (student == null || student.Count == 0)
            {
                return NotFound();
            }


            foreach (var item in student)
            {
                _dbContext.liEvents.Remove(item);
            }

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }


        [HttpPost("ImgUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles();
            uf = await uf.UploadFileToAsync("EventsImg", file);
            return Ok(uf);
        }
    }
}
