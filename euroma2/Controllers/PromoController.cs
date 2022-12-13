using euroma2.Models;
using euroma2.Models.Events;
using euroma2.Models.Interest;
using euroma2.Models.Promo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace euroma2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromoController : ControllerBase
    {


        private readonly DataContext _dbContext;
        public PromoController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<PromoView>>> GetPromos()
        {
            if (_dbContext.promotion == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .promotion
                .Include(a => a.interestIds)
                .Include(a => a.dateRange)
                .ToListAsync();

            List<PromoView> sc = new List<PromoView>();

            foreach (Promotion s in t)
            {
                PromoView res = new PromoView(s);

                res.interestIds = GetInterest(s.interestIds);
                sc.Add(res);
            }

            return sc;

        }

        private  List<int> GetInterest(List<LineaInterest> interest)
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
        public async Task<ActionResult<Promotion>> PostPromo(Promotion promo)
        {
            _dbContext.promotion.Add(promo);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetPromo), new { id = promo.id }, promo);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<PromoView>> GetPromo(int id)
        {
            if (_dbContext.promotion == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .promotion
                .Include(a => a.interestIds)
                .Include(a => a.dateRange)
                .FirstOrDefaultAsync(p => p.id == id); ;

            if (t == null)
            {
                return NotFound();
            }

            PromoView res = new PromoView(t);

            res.interestIds = GetInterest(t.interestIds);


            return res;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutPromo(int id, Promotion promo)
        {
            if (id != promo.id)
            {
                return BadRequest();
            }

            _dbContext.Entry(promo).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PromoExists(id))
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

        private bool PromoExists(long id)
        {
            return (_dbContext.promotion?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeletePromo(int id)
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
