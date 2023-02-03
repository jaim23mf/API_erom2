using euroma2.Models;
using euroma2.Models.Interest;
using euroma2.Models.Promo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace euroma2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class InterestController : ControllerBase
    {

        private readonly DataContext _dbContext;
        public InterestController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/<InterestController>
        [HttpGet("Interest")]
        public async Task<ActionResult<IEnumerable<Interest_modelCMS>>> Get()
        {
 
            if (_dbContext.interests == null)
            {
                return NotFound();
            }

            var t = await _dbContext
                      .interests
                      .ToListAsync();
            var l = await _dbContext.interests_it.ToListAsync();

            List<Interest_modelCMS> sc = new List<Interest_modelCMS>();

            foreach (Interest_model s in t)
            {
                Interest_modelCMS res = new Interest_modelCMS(s);
                var elem = l.Find(x => x.interest_model != null && x.interest_model.id == s.id);
                if (elem != null)
                    res.name_it = elem.name;
                sc.Add(res);
            }
            return sc;

        }


        [HttpGet("{lang}/InterestMobile")]
        public async Task<ActionResult<IEnumerable<Interest_View>>> GetIn(string lang)
        {
            if (_dbContext.interests == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .interests
                .ToListAsync();

            if (t == null)
            {
                return NotFound();
            }

            List<Interest_View> liv = new List<Interest_View>();
            for (int i = 0; i < t.Count; i++) { 
                Interest_View inter = new Interest_View();
                inter.id = t[i].id;
                inter.name = t[i].name;

                if (lang == "it") {
                    var l = await _dbContext
                          .interests_it.
                          FirstOrDefaultAsync(p => p.id == inter.id);
                    if (inter != null && l != null && l.name != null)
                        inter.name = l.name;
                }

                var st = (Interest_Group)t[i].group;
                inter.group = st.ToString();

                liv.Add(inter);
            }

            return liv;
        }

        // GET api/<InterestController>/5
        [HttpGet("Interest/{id}")]
        public async Task<ActionResult<Interest_model>> GetInterest(int id)
        {
            if (_dbContext.interests == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .interests
                .FirstOrDefaultAsync(p => p.id == id); ;

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }

        // POST api/<InterestController>
        [HttpPost("Interest")]
        [Authorize]
        public async Task<ActionResult<Interest_modelCMS>> Post(Interest_modelCMS interest)
        {

            Interest_model i = new Interest_model();
            Interest_model_it in_it = new Interest_model_it();
            i.name = interest.name;
            i.group = interest.group;
            in_it.name = interest.name_it;

            _dbContext.interests.Add(i);

            await _dbContext.SaveChangesAsync();
            var res = CreatedAtAction(nameof(GetInterest), new { id = i.id, lang = "en" }, i);

            in_it.id = i.id;
            in_it.interest_model = i;
            in_it.name = interest.name_it;

            _dbContext.interests_it.Add(in_it);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInterest), new { id = i.id, lang = "en" }, i); ;
        }

        // PUT api/<InterestController>/5
        [HttpPut("Interest/{id}")]
        [Authorize]
        public async Task<IActionResult> PutInterest(int id, Interest_modelCMS interest)
        {
            if (id != interest.id)
            {
                return BadRequest();
            }

            Interest_model sc = new Interest_model();
            sc.id = interest.id;
            sc.name = interest.name;
            sc.group = interest.group;

            Interest_model_it scit = new Interest_model_it();
            scit.id = interest.id;
            scit.name = interest.name_it;
            scit.interest_model = sc;

            _dbContext.Entry(sc).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();

            if (_dbContext.interests_it.Any(e => e.id == interest.id))
            {
                _dbContext.Entry(scit).State = EntityState.Modified;
            }
            else
            {
                _dbContext.interests_it.Add(scit);
            }


            await _dbContext.SaveChangesAsync();

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InterestExists(id))
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

        private bool InterestExists(long id)
        {
            return (_dbContext.interests?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpDelete("Interest/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteInterest(int id)
        {
            if (_dbContext.interests == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.interests.FindAsync(id);
            if (ss == null)
            {
                return NotFound();
            }
            _dbContext.interests.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }
    }
}
