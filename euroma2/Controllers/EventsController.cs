using euroma2.Models.Events;
using euroma2.Models.Promo;
using euroma2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using euroma2.Services;
using Microsoft.Extensions.Options;

namespace euroma2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly PtaInfo _options;

        public EventsController(DataContext dbContext, IOptions<PtaInfo> options)
        {
            _dbContext = dbContext;
            this._options = options.Value;
        }

        [HttpGet("{lang}/Events")]
        public async Task<ActionResult<IEnumerable<EventView>>> GetEvents(string lang)
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
                if (lang == "it")
                {
                    var it = await _dbContext
                    .events_it
                    .FirstOrDefaultAsync(p => p.id == s.id);
                    if (it != null)
                    {
                        res.title = it.title;
                        res.description = it.description;
                    }
                }
                res.interestIds = GetInterest(s.interestIds);
                sc.Add(res);
            }

            return sc;

        }

        [HttpGet("Events/EventCMS")]
        public async Task<ActionResult<IEnumerable<Mall_EventCMS>>> GetPromos()
        {
            /*if (_dbContext.promotion == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .events
                .Include(a => a.interestIds)
                .Include(a => a.dateRange)
                .ToListAsync();
            return t;
            */
            if (_dbContext.events == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                      .events
                      .Include(a => a.interestIds)
                      .Include(a => a.dateRange)
                      .ToListAsync();
            var l = await _dbContext.events_it.ToListAsync();

            List<Mall_EventCMS> sc = new List<Mall_EventCMS>();

            foreach (Mall_Event s in t)
            {
                Mall_EventCMS res = new Mall_EventCMS(s);
                var elem = l.Find(x => x.mall_event!= null && x.mall_event.id == s.id);
                if (elem != null)
                {
                    res.title_it = elem.title;
                    res.description_it = elem.description;
                }
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


        [HttpPost("Events")]
        [Authorize]
        public async Task<ActionResult<Mall_EventCMS>> PostEvents(Mall_EventCMS promo)
        {
            /*_dbContext.events.Add(events);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetEvents), new { id = events.id }, events);*/

            Mall_Event p = new Mall_Event();
            Mall_Event_it p_it = new Mall_Event_it();
            p.dateRange = promo.dateRange;
            p.image = promo.image;
            p.title = promo.title;
            p.description = promo.description;

            _dbContext.events.Add(p);

            await _dbContext.SaveChangesAsync();
            var res = CreatedAtAction(nameof(GetEvents), new { id = p.id, lang = "en" }, p);

            p_it.id = p.id;
            p_it.mall_event = p;
            p_it.title = promo.title_it;
            p_it.description = promo.description_it;

            _dbContext.events_it.Add(p_it);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvents), new { id = p.id, lang = "en" }, p); ;
        }


        [HttpGet("{lang}/Events/{id}")]
        public async Task<ActionResult<EventView>> GetEvents(int id, string lang)
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
            if (lang == "it")
            {
                var it = await _dbContext
                .events_it
                .FirstOrDefaultAsync(p => p.id == id);
                if (it != null)
                {
                    res.title = it.title;
                    res.description = it.description;
                }
            }
            res.interestIds = GetInterest(t.interestIds);


            return res;
        }

        [HttpPut("Events/{id}")]
        [Authorize]
        public async Task<IActionResult> PutEvents(int id, Mall_EventCMS events)
        {
            if (id != events.id)
            {
                return BadRequest();
            }


            Mall_Event sc = new Mall_Event();
            sc.id = events.id;
            sc.dateRange = events.dateRange;
            sc.image = events.image;
            sc.description = events.description;
            sc.title = events.title;
            sc.interestIds = events.interestIds;
            sc.youtubeLink = events.youtubeLink;

            Mall_Event_it scit = new Mall_Event_it();
            scit.id = events.id;
            scit.title = events.title_it;
            scit.description = events.description_it;
            scit.mall_event = sc;

            _dbContext.Entry(sc).State = EntityState.Modified;

            if (_dbContext.events_it.Any(e => e.id == events.id))
            {
                _dbContext.Entry(scit).State = EntityState.Modified;
            }
            else
            {
                _dbContext.events_it.Add(scit);
            }




            _dbContext.Entry(events.dateRange).State = EntityState.Modified;



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
            return Ok(new PutResult { result = "Ok" });
        }

        private bool EventsExists(long id)
        {
            return (_dbContext.events?.Any(e => e.id == id)).GetValueOrDefault();
        }

        private bool LiEventsExists(long id)
        {
            return (_dbContext.liEvents?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpDelete("Events/{id}")]
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
                return Ok(new PutResult { result = "Ok" });
            }

            await DeleteInterestEvent(ss.id);

            _dbContext.promotion.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }


        private async Task<IActionResult> DeleteInterestEvent(int id)
        {
            if (_dbContext.liEvents == null)
            {
                return Ok(new PutResult { result = "Ok" });
            }
            // var ss = await _dbContext.liShop.;

            var query = from st in _dbContext.liEvents
                        where st.id_event == id
                        select st;

            var student = query.ToList<LineaInterest_event>();

            if (student == null || student.Count == 0)
            {
                //return NotFound();
                return Ok(new PutResult { result = "Ok" });
            }


            foreach (var item in student)
            {
                _dbContext.liEvents.Remove(item);
            }

            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }


        [HttpPost("Events/ImgUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles(this._options);
            uf = await uf.UploadFileToAsync(Consts.EventsImg, file);
            return Ok(uf);
        }
    }
}
