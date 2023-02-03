using euroma2.Models;
using euroma2.Models.Events;
using euroma2.Models.Hours;
using euroma2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace euroma2.Controllers
{
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly DataContext _dbContext;
        private readonly PtaInfo _options;
        public ShopController(DataContext dbContext, IOptions<PtaInfo> options) {
            _dbContext = dbContext;
            this._options = options.Value;
        }

        #region SHOP



        [HttpPost("Shop/ImgUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles(this._options);
            uf = await uf.UploadFileToAsync(Consts.StoreImg, file);
            return Ok(uf);
        }


        [HttpPost("Shop/LogoUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystemlogo(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles(this._options);
            uf = await uf.UploadFileToAsync(Consts.LogoImg, file);
            return Ok(uf);
        }

        [HttpGet("{lang}/Shop")]
        public async Task<ActionResult<IEnumerable<Shop>>> GetShops(string lang) {
            if (_dbContext.shop == null) {
                return NotFound();
            }
            var t = await _dbContext
                .shop
                .Include(a => a.openingHours)
                .Include(a => a.interestIds)
                .AsNoTracking()
                .ToListAsync();

            foreach (Shop s in t)
            {
                if (lang == "it")
                {
                    var it = await _dbContext
                    .shop_it
                    .FirstOrDefaultAsync(p => p.shop.id == s.id);
                    if (it != null)
                    {
                        s.title = it.title;
                        s.description = it.description;
                    }
                }
            }
            return t;

        }

        [HttpGet("Shop")]
        public async Task<ActionResult<IEnumerable<ShopCMS>>> GetShops()
        {
            if (_dbContext.shop == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .shop
                .Include(a => a.openingHours)
                .Include(a => a.interestIds)
                .AsNoTracking()
                .ToListAsync();

            var l = await _dbContext.shop_it.ToListAsync();

            List<ShopCMS> sc = new List<ShopCMS>();

            foreach (Shop s in t)
            {
                ShopCMS res = new ShopCMS(s);
                var elem = l.Find(x => x.id == s.id);
                if (elem != null)
                {
                    res.title_it = elem.title;
                    res.description_it = elem.description;
                }
                sc.Add(res);
            }
            return sc;


        }

        [HttpGet ("{lang}/Shop/ShopView")]
        public async Task<ActionResult<IEnumerable<ShopView>>> GetShopsView(string lang)
        {
            if (_dbContext.shop == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .shop
                .Include(a => a.openingHours)
                .Include(a => a.interestIds)
                .ToListAsync();

            List<ShopView> sc = new List<ShopView>();

            foreach (Shop s in t)
            {
                ShopView res = new ShopView(s);

                //res.categoryId = await GetCategories(s.categoryId);
                //res.subcategoryId = await GetSubCategories(s.subcategoryId);
                res.interestIds = GetInterest(s.interestIds);
                //res.logo = $"{this._options.BaseFileUrl}/{Consts.LogoImg}/{Path.GetFileName(s.logo)}";
                //res.photo = $"{this._options.BaseFileUrl}/{Consts.StoreImg}/{Path.GetFileName(s.photo)}";

                if (lang == "it")
                {
                    var it = await _dbContext
                    .shop_it
                    .FirstOrDefaultAsync(p => p.shop.id == s.id);
                    if (it != null)
                    {
                        res.title = it.title;
                        res.description = it.description;
                    }
                }

                sc.Add(res);
            }

            return sc;

        }

        [HttpGet("{lang}/Shop/ShopView/{id}")]
        public async Task<ActionResult<ShopView>> GetShop(int id, string lang)
        {
            if (_dbContext.shop == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .shop.Include(a => a.openingHours).FirstOrDefaultAsync(p => p.id == id);

            if (t == null)
            {
                return NotFound();
            }

            ShopView res = new ShopView(t);

            //res.categoryId = await GetCategories(t.categoryId);
            //res.subcategoryId = await GetSubCategories(t.subcategoryId);
            res.interestIds = GetInterest(t.interestIds);

            if (lang == "it")
            {
                var it = await _dbContext
                .shop_it
                .FirstOrDefaultAsync(p => p.shop.id == res.id);
                if (it != null)
                {
                    res.title = it.title;
                    res.description = it.description;
                }
            }

            return res;
        }


        [HttpGet("{lang}/Shop/ShopInfo/{id}")]
        public async Task<ActionResult<ShopInfo>> GetShopInfo(int id,string lang)
        {
            if (_dbContext.shop == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .shop.Include(a => a.openingHours).FirstOrDefaultAsync(p => p.id == id);

            if (t == null)
            {
                return NotFound();
            }

            ShopInfo res = new ShopInfo(t);

            if (lang == "it")
            {
                var it = await _dbContext
                .shop_it
                .FirstOrDefaultAsync(p => p.shop.id == res.id);
                if (it != null)
                {
                    res.title = it.title;
                }
            }

            return res;
        }

        private List<int> GetInterest(List<LineaInterest_shop> interest)
        {

            List<int> lsc = new List<int>();
            if (interest == null) { return lsc; }
            foreach (LineaInterest_shop ls in interest)
            {
                lsc.Add(ls.id_interest);
            }
            return lsc;
        }

        private async Task<ShopCategory> GetCategories(int cat) {

            ShopCategory lsc = new ShopCategory();
            if (cat == 0) { return lsc; }

            lsc = await GetCat(cat);
            
            return lsc;
        }

        private async Task<ShopCategory> GetCat(int id)
        {
            ShopCategory sc = new ShopCategory();
            if (_dbContext.shopCategory == null)
            {
                return sc;
            }
            var t  = await _dbContext
                .shopCategory.FirstOrDefaultAsync(p => p.id == id);

            if (t == null)
            {
                return sc;
            }
            sc = t;
            return sc;
        }

        private async Task<ShopSubCategory> GetSubCategories(int cat)
        {
            ShopSubCategory lsc = new ShopSubCategory();
            if (cat == 0) { return lsc; }


            lsc = await GetSubCat(cat);
            
            return lsc;

        }

        private async Task<ShopSubCategory> GetSubCat(int id)
        {
            ShopSubCategory sc = new ShopSubCategory();
            if (_dbContext.shopCategory == null)
            {
                return sc;
            }
            var t = await _dbContext
                .shopSubCategory.FirstOrDefaultAsync(p => p.id == id);

            if (t == null)
            {
                return sc;
            }
            sc = t;
            return sc;
        }

        [HttpPost("Shop")]
        [Authorize]
        public async Task<ActionResult<Shop>> PostShop(ShopCMS s)
        {
            /**_dbContext.shop.Add(shop);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);*/

            Shop p = new Shop();
            Shop_it p_it = new Shop_it();
            p.id = s.id;
            p.photo = s.photo;
            p.title = s.title;
            p.categoryId = s.categoryId;
            p.type = s.type;
            p.subcategoryId = s.subcategoryId;
            p.logo = s.logo;
            p.openingHours = s.openingHours;
            p.phoneNumber = s.phoneNumber;
            p.description = s.description;
            p.firstOpeningDay = s.firstOpeningDay;
            p.interestIds = s.interestIds;

            _dbContext.shop.Add(p);

            await _dbContext.SaveChangesAsync();
            var res = CreatedAtAction(nameof(GetShop), new { id = p.id, lang = "en" }, p);

            p_it.id = p.id;
            p_it.shop = p;
            p_it.title = s.title_it != null ? s.title_it:"";
            p_it.description = s.description_it != null ? s.description_it:"";

            _dbContext.shop_it.Add(p_it);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetShop), new { id = p.id, lang = "en" }, p); ;

        }

        private List<LineaShopCategory> setCat(int id , List<LineaShopCategory> ls) {

            foreach (LineaShopCategory l in ls) { 
                l.id_shop = id;
            }
            return ls;
        }

        private List<LineaShopSubCategory> setSubCat(int id, List<LineaShopSubCategory> ls)
        {
            foreach (LineaShopSubCategory l in ls)
            {
                l.id_shop = id;
            }
            return ls;
        }

        [HttpPut("Shop/{id}")]
        [Authorize]
        public async Task<IActionResult> PutShop(int id, ShopCMS s)
        {
            if (id!= s.id) {
                return BadRequest();
            }
            ////
            Shop p = new Shop();
            p.id = s.id;
            p.photo = s.photo;
            p.title = s.title;
            p.categoryId = s.categoryId;
            p.type = s.type;
            p.subcategoryId = s.subcategoryId;
            p.logo = s.logo;
            p.openingHours = s.openingHours;
            p.phoneNumber = s.phoneNumber;
            p.description = s.description;
            p.firstOpeningDay = s.firstOpeningDay;
            p.interestIds = s.interestIds;

            Shop_it scit = new Shop_it();
            scit.id = s.id;
            scit.title = s.title_it;
            scit.description = s.description_it;
            scit.shop = p;

            _dbContext.Entry(p).State = EntityState.Modified;

            if (_dbContext.shop_it.Any(e => e.id == s.id))
            {
                _dbContext.Entry(scit).State = EntityState.Modified;
            }
            else
            {
                _dbContext.shop_it.Add(scit);
            }





            ////

            foreach (oDay li in s.openingHours)
            {
                _dbContext.Entry(li).State = EntityState.Modified;
            }


            await DeleteInterestShop(s.id);

            

            s.interestIds.ForEach(item => 
                    _dbContext.liShop.Add(item)
            );

            /*await DeleteOpeningShop(shop.id);
            foreach (oDay li in shop.openingHours)
            {
                    _dbContext.oDay.Add(li);      
            }*/

            //await DeleteInterestShop(shop.id);
            //await PostInterest(shop);

            //await DeleteOpeningShop(shop.id);
            //await PostOpening(shop);


            try
            {
                await _dbContext.SaveChangesAsync();
            } catch(DbUpdateConcurrencyException) {
                if (!ShopExists(id)) { 
                    return NotFound(); 
                } else { 
                    throw; 
                }

            }
            return Ok(new PutResult { result = "Ok" });
        }


        private bool ShopExists(long id)
        {
            return (_dbContext.shop?.Any(e => e.id == id)).GetValueOrDefault();
        }
        private bool LiShopExists(long id)
        {
            return (_dbContext.liShop?.Any(e=>e.id==id)).GetValueOrDefault() ;
        }
        private bool oDayExists(long id)
        {
            return (_dbContext.oDay?.Any(e => e.id == id)).GetValueOrDefault();
        }
        [HttpDelete("Shop/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteShop(int id)
        {
            if(_dbContext.shop == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.shop.FindAsync(id);
            if (ss == null) {             
                return NotFound();
            }

            await DeleteInterestShop(ss.id);
            await DeleteOpeningShop(ss.id);

            _dbContext.shop.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }


        private async Task<IActionResult> DeleteInterestShop(int id)
        {
            if (_dbContext.liShop == null)
            {
                return NotFound();
            }
            // var ss = await _dbContext.liShop.;

            var query = from st in _dbContext.liShop
                        where st.id_shop == id
                        select st;

            var student = query.ToList<LineaInterest_shop>();

            if (student == null || student.Count == 0)
            {
                //return NotFound();
                return Ok(new PutResult { result = "Ok" });
            }


            foreach (var item in student)
            {
                _dbContext.liShop.Remove(item);
            }

            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }

        private async Task<IActionResult> DeleteOpeningShop(int id)
        {
            if (_dbContext.oDay == null)
            {
                return NotFound();
            }
            // var ss = await _dbContext.liShop.;

            /*var query = from st in _dbContext.oDay
                        where st.id_shop == id
                        select st;*/

            Shop t = await _dbContext.shop.Include(st => st.openingHours).Where(s => s.id == id).FirstAsync();

            //var student = query.ToList<oDay>();

            if (t == null || t.openingHours.Count == 0)
            {
                return NotFound();
            }


            //foreach (var item in t.openingHours)
           // {
                _dbContext.oDay.RemoveRange(t.openingHours);
            //}

            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }

        private async Task<IActionResult> PostInterest(Shop shop)
        {
            foreach (LineaInterest_shop li in shop.interestIds)
            {
                _dbContext.liShop.Add(li);
            }
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return Ok(new PutResult { result = "Ok" });
        }

        private async Task<IActionResult> PostOpening(Shop shop)
        {
            foreach (oDay li in shop.openingHours)
            {
                _dbContext.oDay.Add(li);
            }
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return Ok(new PutResult { result = "Ok" });
        }

        #endregion

        #region CATEGORY

        [HttpPost("Shop/Category")]
        [Authorize]
        public async Task<ActionResult<ShopCategory>> PostCategory(ShopCategoryCMS shop)
        {
            ShopCategory cat = new ShopCategory();
            ShopCategory_it cat_it = new ShopCategory_it();
            cat.title = shop.title;
            cat.shopType = shop.shopType;

            cat_it.title = shop.title_it;

            _dbContext.shopCategory.Add(cat);

            await _dbContext.SaveChangesAsync();
            var i = CreatedAtAction(nameof(GetCategory), new { id = cat.id, lang = "en" }, cat);

            cat_it.id = cat.id;
            cat_it.shopCategory = cat;
            cat_it.cat_id = cat.id;
            cat_it.title = shop.title_it;

            _dbContext.shopCategory_it.Add(cat_it);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = cat.id, lang = "en" }, cat); ;
        }


        [HttpGet("Shop/{lang}/Category/{id}")]
        public async Task<ActionResult<ShopCategory>> GetCategory(int id,string lang)
        {
            if (_dbContext.shopCategory == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .shopCategory.FirstOrDefaultAsync(p => p.id == id);

            if (lang == "it") {
                var l = await _dbContext
                           .shopCategory_it.
                           FirstOrDefaultAsync(p => p.id == id);
                if(t!=null && l!= null && l.title!=null)
                    t.title = l.title;
            }

            if (t == null)
            {
                return NotFound();
            }
            return t;
        }



        [HttpGet("Shop/{lang}/Category")]
        public async Task<ActionResult<IEnumerable<ShopCategory>>> GetCategory(string lang)
        {
            if (_dbContext.shopCategory == null)
            {
                return NotFound();
            }

            var list = await _dbContext
                    .shopCategory
                    .ToListAsync();

            if (lang == "en")
            {
                return list;
            }
            else if (lang == "it")
            {
                //cambiar de la lista el titulo por el titulo en italiano...
                var t = await _dbContext
                        .shopCategory_it
                        .ToListAsync();
                foreach (var cat in list) {
                    var elem = t.Find(x => x.id == cat.id);
                    if ( elem!=null && elem.title != null)
                        cat.title = elem.title;
                }
                return list;
            }
            else {
                return NotFound();
            }
        }

        [HttpGet("Shop/Category")]
        public async Task<ActionResult<IEnumerable<ShopCategoryCMS>>> GetFullCategory()
        {
            if (_dbContext.shopCategory == null)
            {
                return NotFound();
            }

            var t = await _dbContext
                      .shopCategory
                      .ToListAsync();
            var l = await _dbContext.shopCategory_it.ToListAsync();

            List<ShopCategoryCMS> sc = new List<ShopCategoryCMS>();

            foreach (ShopCategory s in t)
            {
                ShopCategoryCMS res = new ShopCategoryCMS(s);
                var elem = l.Find(x => x.shopCategory!= null && x.shopCategory.id == s.id);
                if(elem != null)
                    res.title_it = elem.title;
                sc.Add(res);
            }
            return sc;
        }

        [HttpDelete("Shop/Category/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            if (_dbContext.shopCategory == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.shopCategory.FindAsync(id);
            if (ss == null)
            {
                return NotFound();
            }
            _dbContext.shopCategory.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }


        [HttpPut("Shop/Category/{id}")]
        [Authorize]
        public async Task<IActionResult> PutCategory(int id, ShopCategoryCMS shopCategory)
        {
            if (id != shopCategory.id)
            {
                return BadRequest();
            }

            ShopCategory sc = new ShopCategory();
            sc.id = shopCategory.id;
            sc.title = shopCategory.title;
            sc.shopType = shopCategory.shopType;

            ShopCategory_it scit = new ShopCategory_it();
            scit.id = shopCategory.id;
            scit.cat_id = shopCategory.id;
            scit.title = shopCategory.title_it;
            scit.shopCategory = sc;

            _dbContext.Entry(sc).State = EntityState.Modified;
            if (_dbContext.shopCategory_it.Any(e => e.id == shopCategory.id))
            {
                _dbContext.Entry(scit).State = EntityState.Modified;
            }
            else
            {
                _dbContext.shopCategory_it.Add(scit);
            }

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        private bool CategoryExists(long id)
        {
            return (_dbContext.shopCategory?.Any(e => e.id == id)).GetValueOrDefault();
        }

        #endregion

        #region SUBCATEGORY 
        [HttpPost("Shop/SubCategory")]
        [Authorize]
        public async Task<ActionResult<ShopSubCategory>> PostSubCategory(ShopSubCategoryCMS shop)
        {
            /*_dbContext.shopSubCategory.Add(shop);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSubCategory), new { id = shop.id }, shop);*/

            ShopSubCategory cat = new ShopSubCategory();
            ShopSubCategory_it cat_it = new ShopSubCategory_it();
            cat.title = shop.title;
            cat.categoryId = shop.categoryId;

            cat_it.title = shop.title_it;

            _dbContext.shopSubCategory.Add(cat);

            await _dbContext.SaveChangesAsync();
            var i = CreatedAtAction(nameof(GetSubCategory), new { id = cat.id, lang = "en" }, cat);

            cat_it.id = cat.id;
            cat_it.shopSubCategory = cat;
            cat_it.title = shop.title_it;

            _dbContext.shopSubCategory_it.Add(cat_it);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSubCategory), new { id = cat.id, lang = "en" }, cat); ;
        }


        [HttpGet("Shop/{lang}/SubCategory/{id}")]
        public async Task<ActionResult<ShopSubCategory>> GetSubCategory(int id, string lang)
        {
            if (_dbContext.shopSubCategory == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .shopSubCategory.FirstOrDefaultAsync(p => p.id == id);


            if (lang == "it")
            {
                var l = await _dbContext
                           .shopSubCategory_it.
                           FirstOrDefaultAsync(p => p.id == id);
                if (t!= null && l != null && l.title != null)
                    t.title = l.title;
            }

            if (t == null)
            {
                return NotFound();
            }
            return t;
        }

        [HttpGet("Shop/SubCategory")]
        public async Task<ActionResult<IEnumerable<ShopSubCategoryCMS>>> GetSubCategory()
        {
            /*if (_dbContext.shopSubCategory == null)
            {
                return NotFound();
            }
            return await _dbContext
                .shopSubCategory
                .ToListAsync();*/
            if (_dbContext.shopSubCategory == null)
            {
                return NotFound();
            }

            var t = await _dbContext
                      .shopSubCategory
                      .ToListAsync();
            var l = await _dbContext.shopSubCategory_it.ToListAsync();

            List<ShopSubCategoryCMS> sc = new List<ShopSubCategoryCMS>();

            foreach (ShopSubCategory s in t)
            {
                ShopSubCategoryCMS res = new ShopSubCategoryCMS(s);
                var elem = l.Find(x => x.shopSubCategory != null && x.shopSubCategory.id == s.id);
                if (elem != null)
                    res.title_it = elem.title;
                sc.Add(res);
            }
            return sc;



        }

        [HttpGet("Shop/{lang}/SubCategory")]
        public async Task<ActionResult<IEnumerable<ShopSubCategory>>> GetSubCategory(string lang)
        {
            if (_dbContext.shopSubCategory == null)
            {
                return NotFound();
            }

            var list = await _dbContext
                    .shopSubCategory
                    .ToListAsync();

            if (lang == "en")
            {
                return list;
            }
            else if (lang == "it")
            {
                //cambiar de la lista el titulo por el titulo en italiano...
                var t = await _dbContext
                        .shopSubCategory_it
                        .ToListAsync();
                foreach (var cat in list)
                {
                    var elem = t.Find(x => x.id == cat.id);
                    if (elem != null && elem.title != null)
                        cat.title = elem.title;
                }
                return list;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpDelete("Shop/SubCategory/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteSubCategory(int id)
        {
            if (_dbContext.shopSubCategory == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.shopSubCategory.FindAsync(id);
            if (ss == null)
            {
                return NotFound();
            }
            _dbContext.shopSubCategory.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }


        [HttpPut("Shop/SubCategory/{id}")]
        [Authorize]
        public async Task<IActionResult> PutSubCategory(int id, ShopSubCategoryCMS shopSubCategory)
        {
            if (id != shopSubCategory.id)
            {
                return BadRequest();
            }

            ShopSubCategory sc = new ShopSubCategory();
            sc.id = shopSubCategory.id;
            sc.title = shopSubCategory.title;
            sc.categoryId = shopSubCategory.categoryId;

            ShopSubCategory_it scit = new ShopSubCategory_it();
            scit.id = shopSubCategory.id;
            scit.title = shopSubCategory.title_it;
            scit.shopSubCategory = sc;

            _dbContext.Entry(sc).State = EntityState.Modified;
            if (_dbContext.shopSubCategory_it.Any(e => e.id == shopSubCategory.id))
            {
                _dbContext.Entry(scit).State = EntityState.Modified;
            }
            else
            {
                _dbContext.shopSubCategory_it.Add(scit);
            }


            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
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

        private bool SubCategoryExists(long id)
        {
            return (_dbContext.shopSubCategory?.Any(e => e.id == id)).GetValueOrDefault();
        }

        #endregion

    }
}
