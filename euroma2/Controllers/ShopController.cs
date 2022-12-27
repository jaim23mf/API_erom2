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
    [Route("[controller]")]
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



        [HttpPost("ImgUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles(this._options);
            uf = await uf.UploadFileToAsync(Consts.StoreImg, file);
            return Ok(uf);
        }


        [HttpPost("LogoUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystemlogo(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles(this._options);
            uf = await uf.UploadFileToAsync(Consts.LogoImg, file);
            return Ok(uf);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Shop>>> GetShops() {
            if (_dbContext.shop == null) {
                return NotFound();
            }
            var t = await _dbContext
                .shop
                .Include(a => a.openingHours)
                .Include(a => a.interestIds)
                .AsNoTracking()
                .ToListAsync();

            return t;

        }

        [HttpGet ("ShopView")]
        public async Task<ActionResult<IEnumerable<ShopView>>> GetShopsView()
        {
            if (_dbContext.shop == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .shop
                .Include(a => a.openingHours)
                .ToListAsync();

            List<ShopView> sc = new List<ShopView>();

            foreach (Shop s in t)
            {
                ShopView res = new ShopView(s);

                res.categoryId = await GetCategories(s.categoryId);
                res.subcategoryId = await GetSubCategories(s.subcategoryId);
                res.interestIds = GetInterest(s.interestIds);
                //res.logo = $"{this._options.BaseFileUrl}/{Consts.LogoImg}/{Path.GetFileName(s.logo)}";
                //res.photo = $"{this._options.BaseFileUrl}/{Consts.StoreImg}/{Path.GetFileName(s.photo)}";
                sc.Add(res);
            }

            return sc;

        }

        [HttpGet("ShopView/{id}")]
        public async Task<ActionResult<ShopView>> GetShop(int id)
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

            res.categoryId = await GetCategories(t.categoryId);
            res.subcategoryId = await GetSubCategories(t.subcategoryId);
            res.interestIds = GetInterest(t.interestIds);
            return res;
        }


        [HttpGet("ShopInfo/{id}")]
        public async Task<ActionResult<ShopInfo>> GetShopInfo(int id)
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

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Shop>> PostShop(Shop shop)
        {
            _dbContext.shop.Add(shop);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
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

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutShop(int id, Shop shop)
        {
            if (id!= shop.id) {
                return BadRequest();
            }

            _dbContext.Entry(shop).State = EntityState.Modified;

            foreach (oDay li in shop.openingHours)
            {
                _dbContext.Entry(li).State = EntityState.Modified;
            }


            await DeleteInterestShop(shop.id);

            

            shop.interestIds.ForEach(item => 
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
        [HttpDelete("{id}")]
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

        [HttpPost("Category")]
        [Authorize]
        public async Task<ActionResult<ShopCategory>> PostCategory(ShopCategory shop)
        {
            _dbContext.shopCategory.Add(shop);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetCategory), new { id = shop.id }, shop);
        }


        [HttpGet("Category/{id}")]
        public async Task<ActionResult<ShopCategory>> GetCategory(int id)
        {
            if (_dbContext.shopCategory == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .shopCategory.FirstOrDefaultAsync(p => p.id == id);

            if (t == null)
            {
                return NotFound();
            }
            return t;
        }

        [HttpGet("Category")]
        public async Task<ActionResult<IEnumerable<ShopCategory>>> GetCategory()
        {
            if (_dbContext.shopCategory == null)
            {
                return NotFound();
            }
            return await _dbContext
                .shopCategory
                .ToListAsync();
        }


        [HttpGet("CategoryM")]
        public async Task<ActionResult<IEnumerable<ShopCategoryM>>> GetCategoryM()
        {
            if (_dbContext.shopCategory == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .shopCategory
                .ToListAsync();

            List<ShopCategoryM> lsc= new List<ShopCategoryM>();

            for(int i = 0; i < t.Count; i++)
            {
                ShopCategoryM sc = new ShopCategoryM();
                sc.id = t[i].id;
                sc.title = t[i].title;
                var st = (StoreType)t[i].shopType;
                sc.shopType = st.ToString();
                lsc.Add(sc);
            }

            return lsc;
        }

        [HttpDelete("Category/{id}")]
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


        [HttpPut("Category/{id}")]
        [Authorize]
        public async Task<IActionResult> PutCategory(int id, ShopCategory shopCategory)
        {
            if (id != shopCategory.id)
            {
                return BadRequest();
            }

            _dbContext.Entry(shopCategory).State = EntityState.Modified;

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
        [HttpPost("SubCategory")]
        [Authorize]
        public async Task<ActionResult<ShopSubCategory>> PostSubCategory(ShopSubCategory shop)
        {
            _dbContext.shopSubCategory.Add(shop);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSubCategory), new { id = shop.id }, shop);
        }


        [HttpGet("SubCategory/{id}")]
        public async Task<ActionResult<ShopSubCategory>> GetSubCategory(int id)
        {
            if (_dbContext.shopSubCategory == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .shopSubCategory.FirstOrDefaultAsync(p => p.id == id);

            if (t == null)
            {
                return NotFound();
            }
            return t;
        }

        [HttpGet("SubCategory")]
        public async Task<ActionResult<IEnumerable<ShopSubCategory>>> GetSubCategory()
        {
            if (_dbContext.shopSubCategory == null)
            {
                return NotFound();
            }
            return await _dbContext
                .shopSubCategory
                .ToListAsync();
        }

        [HttpDelete("SubCategory/{id}")]
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


        [HttpPut("SubCategory/{id}")]
        [Authorize]
        public async Task<IActionResult> PutSubCategory(int id, ShopSubCategory shopSubCategory)
        {
            if (id != shopSubCategory.id)
            {
                return BadRequest();
            }

            _dbContext.Entry(shopSubCategory).State = EntityState.Modified;

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubCategoryExists(id))
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
