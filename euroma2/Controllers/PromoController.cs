using euroma2.Models;
using euroma2.Models.Events;
using euroma2.Models.Interest;
using euroma2.Models.Promo;
using euroma2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

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
        public async Task<ActionResult<IEnumerable<Promotion>>> GetPromos()
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
            return t;

        }

        private  List<int> GetInterest(List<LineaInterest_promo> interest)
        {

            List<int> lsc = new List<int>();

            foreach (LineaInterest_promo ls in interest)
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
            _dbContext.Entry(promo.dateRange).State = EntityState.Modified;

            await DeleteInterestPromo(id);


            promo.interestIds.ForEach(item => _dbContext.liPromo.Add(item));



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

        private bool liPromoExists(long id)
        {
            return (_dbContext.liPromo?.Any(e => e.id == id)).GetValueOrDefault();
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
            await DeleteInterestPromo(id);

            _dbContext.promotion.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }


        private async Task<IActionResult> DeleteInterestPromo(int id)
        {
            if (_dbContext.liPromo == null)
            {
                return NotFound();
            }
            // var ss = await _dbContext.liShop.;

            var query = from st in _dbContext.liPromo
                        where st.id_promo == id
                        select st;

            var student = query.ToList<LineaInterest_promo>();

            if (student == null ||student.Count == 0)
            {
                return NotFound();
            }

            

            foreach (var item in student)
            {
                _dbContext.liPromo.Remove(item);
            }

            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("ImgUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles();
            uf = await uf.UploadFileToAsync("PromoImg", file);
            return Ok(uf);
        }
    }
}
