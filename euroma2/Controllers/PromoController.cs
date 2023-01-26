using euroma2.Models;
using euroma2.Models.Events;
using euroma2.Models.Interest;
using euroma2.Models.Promo;
using euroma2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;

namespace euroma2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class PromoController : ControllerBase
    {


        private readonly DataContext _dbContext;
        private readonly PtaInfo _options;
        public PromoController(DataContext dbContext, IOptions<PtaInfo> options)
        {
            _dbContext = dbContext;
            _options = options.Value;
        }

        [HttpGet("Promo")]
        public async Task<ActionResult<IEnumerable<PromotionCMS>>> GetPromos()
        {
            /* if (_dbContext.promotion == null)
             {
                 return NotFound();
             }
             var t = await _dbContext
                 .promotion
                 .Include(a => a.interestIds)
                 .Include(a => a.dateRange)
                 .ToListAsync();
             return t;
            */
            if (_dbContext.promotion == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                      .promotion
                      .Include(a => a.interestIds)
                      .Include(a => a.dateRange)
                      .ToListAsync();
            var l = await _dbContext.promotion_it.ToListAsync();

            List<PromotionCMS> sc = new List<PromotionCMS>();

            foreach (Promotion s in t)
            {
                PromotionCMS res = new PromotionCMS(s);
                var elem = l.Find(x => x.promotion != null && x.promotion.id == s.id);
                if (elem != null)
                {
                    res.title_it = elem.title;
                    res.description_it = elem.description;
                }
                sc.Add(res);
            }
            return sc;
        }


        [HttpGet("{lang}/Promo/PromoView")]
        public async Task<ActionResult<IEnumerable<PromoView>>> GetPromosMobile(string lang)
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
                if (lang == "it")
                {
                    var it = await _dbContext
                    .promotion_it
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

        private  List<int> GetInterest(List<LineaInterest_promo> interest)
        {

            List<int> lsc = new List<int>();

            foreach (LineaInterest_promo ls in interest)
            {
                lsc.Add(ls.id_interest);
            }
            return lsc;
        }


        [HttpPost("Promo")]
        [Authorize]
        public async Task<ActionResult<PromotionCMS>> PostPromo(PromotionCMS promo)
        {
            /*_dbContext.promotion.Add(promo);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetPromo), new { id = promo.id }, promo);*/
            Promotion p = new Promotion();
            Promotion_it p_it = new Promotion_it();
            p.shopId = promo.shopId;
            p.dateRange = promo.dateRange;
            p.image = promo.image;
            p.title = promo.title;
            p.description = promo.description;

            _dbContext.promotion.Add(p);

            await _dbContext.SaveChangesAsync();
            var res = CreatedAtAction(nameof(GetPromo), new { id = p.id, lang = "en" }, p);

            p_it.id = p.id;
            p_it.promotion = p;
            p_it.title = promo.title_it;
            p_it.description = promo.description_it;

            _dbContext.promotion_it.Add(p_it);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPromo), new { id = p.id, lang = "en" }, p); ;
        }


        [HttpGet("{lang}/Promo/{id}")]
        public async Task<ActionResult<PromoView>> GetPromo(int id,string lang)
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

            if (lang == "it") {
                var it = await _dbContext
                .promotion_it
                .FirstOrDefaultAsync(p => p.id == id);

                res.title = it.title;
                res.description = it.description;
            }

          

            res.interestIds = GetInterest(t.interestIds);


            return res;
        }

        [HttpPut("Promo/{id}")]
        [Authorize]
        public async Task<IActionResult> PutPromo(int id, PromotionCMS promo)
        {
            if (id != promo.id)
            {
                return BadRequest();
            }

            Promotion sc = new Promotion();
            sc.id = promo.id;
            sc.shopId = promo.shopId;
            sc.dateRange = promo.dateRange;
            sc.image = promo.image;
            sc.description = promo.description;
            sc.title = promo.title;
            sc.interestIds = promo.interestIds;

            Promotion_it scit = new Promotion_it();
            scit.id = promo.id;
            scit.title = promo.title_it;
            scit.description = promo.description_it;
            scit.promotion = sc;

            _dbContext.Entry(sc).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();


            _dbContext.Entry(promo.dateRange).State = EntityState.Modified;

            await _dbContext.SaveChangesAsync();

            if (_dbContext.promotion_it.Any(e => e.id == promo.id))
            {
                _dbContext.Entry(scit).State = EntityState.Modified;
            }
            else
            {
                _dbContext.promotion_it.Add(scit);
            }


            //_dbContext.Entry(promo).State = EntityState.Modified;

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
            return Ok(new PutResult { result = "Ok" });
        }

        private bool PromoExists(long id)
        {
            return (_dbContext.promotion?.Any(e => e.id == id)).GetValueOrDefault();
        }

        private bool liPromoExists(long id)
        {
            return (_dbContext.liPromo?.Any(e => e.id == id)).GetValueOrDefault();
        }

        [HttpDelete("Promo/{id}")]
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
            return Ok(new PutResult { result = "Ok" });
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
                return Ok(new PutResult { result = "Ok" });
            }

            

            foreach (var item in student)
            {
                _dbContext.liPromo.Remove(item);
            }

            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }

        [HttpPost("Promo/ImgUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles(this._options);
            uf = await uf.UploadFileToAsync(Consts.PromoImg, file);
            return Ok(uf);
        }
    }
}
