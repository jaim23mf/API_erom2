using euroma2.Models.Events;
using euroma2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using euroma2.Models.Blog_model;
using Microsoft.EntityFrameworkCore;
using euroma2.Models.Reach;
using Microsoft.AspNetCore.Authorization;
using euroma2.Services;

namespace euroma2.Controllers
{
    [Route("api/")]
    [ApiController]
    public class BlogController : Controller
    {
        private readonly DataContext _dbContext;
        private readonly PtaInfo _options;

        public BlogController(DataContext dbContext, IOptions<PtaInfo> options)
        {
            _dbContext = dbContext;
            this._options = options.Value;
        }

        [HttpGet("{lang}/Blog")]
        public async Task<ActionResult<IEnumerable<Blog>>> GetBlog(string lang)
        {
            if (_dbContext.blog == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .blog
                .ToListAsync();


            foreach (Blog s in t)
            {
                if (lang == "it")
                {
                    var it = await _dbContext
                    .blog_it
                    .FirstOrDefaultAsync(p => p.id == s.id);
                    if (it != null)
                    {
                        s.title = it.title;
                        s.description = it.description;
                        s.shortDescription = it.shortDescription;
                    }
                }
            }


            if (t == null)
            {
                return NotFound();
            }

            return t;

        }


        [HttpGet("BlogCMS")]
        public async Task<ActionResult<IEnumerable<BlogCMS>>> GetCMS()
        {
            if (_dbContext.reach == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .blog
                .ToListAsync();

            var l = await _dbContext.blog_it.ToListAsync();

            List<BlogCMS> sc = new List<BlogCMS>();

            foreach (Blog s in t)
            {
                BlogCMS res = new BlogCMS(s);
                var elem = l.Find(x => x.blog != null && x.blog.id == s.id);
                if (elem != null)
                {
                    res.title_it = elem.title;
                    res.description_it = elem.description;
                    res.shortDescription_it = elem.shortDescription;
                }
                sc.Add(res);
            }
            return sc;
        }

        [HttpGet("{lang}/Blog/{id}")]
        public async Task<ActionResult<Blog>> GetReach(int id, string lang)
        {
            if (_dbContext.interests == null)
            {
                return NotFound();
            }
            var t = await _dbContext
                .blog
                .FirstOrDefaultAsync(p => p.id == id); ;


            if (lang == "it")
            {
                var it = await _dbContext
                .blog_it
                .FirstOrDefaultAsync(p => p.id == id);
                if (it != null)
                {
                    t.title = it.title;
                    t.description = it.description;
                    t.shortDescription = it.shortDescription;
                }
            }

            if (t == null)
            {
                return NotFound();
            }

            return t;
        }
        [HttpPost("Blog")]
        [Authorize]
        public async Task<ActionResult<Blog>> Post(BlogCMS reach)
        {
            /*_dbContext.reach.Add(reach);
            await _dbContext.SaveChangesAsync();
            //return CreatedAtAction(nameof(GetShop), new { id = shop.id }, shop);
            return CreatedAtAction(nameof(GetReach), new { id = reach.id }, reach);*/

            Blog p = new Blog();
            Blog_it p_it = new Blog_it();
            p.title = reach.title;
            p.description = reach.description;
            p.shortDescription = reach.shortDescription;
            p.date = reach.date;
            p.image = reach.image;
            p.highlight = reach.highlight;
            p.thumb = reach.thumb;

            _dbContext.blog.Add(p);

            await _dbContext.SaveChangesAsync();
            var res = CreatedAtAction(nameof(GetBlog), new { id = p.id, lang = "en" }, p);

            p_it.id = p.id;
            p_it.blog = p;
            p_it.title = reach.title_it;
            p_it.description = reach.description_it;
            p_it.shortDescription = reach.shortDescription;

            _dbContext.blog_it.Add(p_it);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBlog), new { id = p.id, lang = "en" }, p); ;


        }


        [HttpPut("Blog/{id}")]
        [Authorize]
        public async Task<IActionResult> PutBlog(int id, BlogCMS reach)
        {



            if (id != reach.id)
            {
                return BadRequest();
            }

            Blog sc = new Blog();
            sc.id = reach.id;
            sc.image = reach.image;
            sc.description = reach.description;
            sc.title = reach.title;
            sc.shortDescription = reach.shortDescription;
            sc.highlight = reach.highlight;
            sc.thumb = reach.thumb;
            sc.date = reach.date;
           

            Blog_it scit = new Blog_it();
            scit.id = reach.id;
            scit.title = reach.title_it;
            scit.description = reach.description_it;
            scit.shortDescription = reach.shortDescription_it;
            scit.blog = sc;

            _dbContext.Entry(sc).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();


            if (_dbContext.blog_it.Any(e => e.id == reach.id))
            {
                _dbContext.Entry(scit).State = EntityState.Modified;
            }
            else
            {
                _dbContext.blog_it.Add(scit);
            }

            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BlogExists(id))
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

        private bool BlogExists(long id)
        {
            return (_dbContext.blog?.Any(e => e.id == id)).GetValueOrDefault();
        }


        [HttpDelete("Blog/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteBlog(int id)
        {
            if (_dbContext.blog == null)
            {
                return NotFound();
            }
            var ss = await _dbContext.blog.FindAsync(id);
            if (ss == null)
            {
                return NotFound();
            }
            _dbContext.blog.Remove(ss);
            await _dbContext.SaveChangesAsync();
            return Ok(new PutResult { result = "Ok" });
        }

        [HttpPost("Blog/ImgUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystem(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles(this._options);
            uf = await uf.UploadFileToAsync(Consts.BlogImg, file);
            return Ok(uf);
        }


        [HttpPost("Blog/ThumbUpload/{id}")]
        [Authorize]
        public async Task<IActionResult> UploadToFileSystemlogo(IFormFile file, int id)
        {
            UploadFiles uf = new UploadFiles(this._options);
            uf = await uf.UploadFileToAsync(Consts.ThBlogImg, file);
            return Ok(uf);
        }

        //
    }
}
