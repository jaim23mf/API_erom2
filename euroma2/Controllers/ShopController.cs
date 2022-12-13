using euroma2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace euroma2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ShopController : ControllerBase
    {
        private readonly DataContext _dbContext;
        public ShopController(DataContext dbContext) {
            _dbContext = dbContext;
        }

        #region SHOP

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<ShopView>>> GetShops() {
            if (_dbContext.shop == null) {
                return NotFound();
            }
            var t = await _dbContext
                .shop
                .Include(a => a.openingHours)
                .Include(a => a.category)
                .Include(a => a.subcategory)
                .ToListAsync();

            List<ShopView> sc = new List<ShopView>();

            foreach (Shop s in t)
            {
                ShopView res = new ShopView(s);

                res.category = await GetCategories(s.category);
                res.subcategory = await GetSubCategories(s.subcategory);
                sc.Add(res);
            }

            return sc;

        }
        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<ShopView>> GetShop(int id)
        {
            if (_dbContext.shop == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .shop.Include(a => a.openingHours)
                .Include(a => a.category)
                .Include(a => a.subcategory).FirstOrDefaultAsync(p => p.id == id);

            if (t == null)
            {
                return NotFound();
            }

            ShopView res = new ShopView(t);

            res.category =  await GetCategories(t.category);
            res.subcategory = await GetSubCategories(t.subcategory);

            return res;
        }


        [HttpGet("ShopInfo/{id}")]
        [Authorize]
        public async Task<ActionResult<ShopInfo>> GetShopInfo(int id)
        {
            if (_dbContext.shop == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .shop.Include(a => a.openingHours)
                .Include(a => a.category)
                .Include(a => a.subcategory).FirstOrDefaultAsync(p => p.id == id);

            if (t == null)
            {
                return NotFound();
            }

            ShopInfo res = new ShopInfo(t);

            

            return res;
        }

        private async Task<List<ShopCategory>> GetCategories(List<LineaShopCategory> cat) {

            List<ShopCategory> lsc = new List<ShopCategory>();

            foreach (LineaShopCategory ls in cat) {
                var sc = await GetCat(ls.id_category);
                lsc.Add(sc);
            }
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

        private async Task<List<ShopSubCategory>> GetSubCategories(List<LineaShopSubCategory> cat)
        {
            List<ShopSubCategory> lsc = new List<ShopSubCategory>();

            foreach (LineaShopSubCategory ls in cat)
            {
                var sc = await GetSubCat(ls.id_subcategory);
                lsc.Add(sc);
            }
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
            return CreatedAtAction(nameof(GetShop), new { id = shop.id, category = setCat(shop.id, shop.category), subcategory = setSubCat(shop.id, shop.subcategory) }, shop);
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
            return NoContent();
        }

        private bool ShopExists(long id)
        {
            return (_dbContext.shop?.Any(e=>e.id==id)).GetValueOrDefault() ;
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
            _dbContext.shop.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return NoContent();
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
        [Authorize]
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
        [Authorize]
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
            return NoContent();
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
            return NoContent();
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
        [Authorize]
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
        [Authorize]
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
            return NoContent();
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
            return NoContent();
        }

        private bool SubCategoryExists(long id)
        {
            return (_dbContext.shopSubCategory?.Any(e => e.id == id)).GetValueOrDefault();
        }

        #endregion

    }
}
