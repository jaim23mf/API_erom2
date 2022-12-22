using euroma2.Models;
using euroma2.Models.Promo;
using euroma2.Models.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace euroma2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly DataContext _dbContext;
        public UserController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            _dbContext.user.Add(user);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }


        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            if (_dbContext.user == null)
            {
                return NotFound();
            }
            var t = await _dbContext.user.FirstOrDefaultAsync(p => p.Id == id); ;

            if (t == null)
            {
                return NotFound();
            }
            return t;
        }



        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _dbContext.Entry(user).State = EntityState.Modified;


            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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


        [HttpPut("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> PutUser(User user)
        {
            var t = await _dbContext
            .user
            .Where(a => a.userName == user.userName)
            .Where(a => a.password == user.password)
            .FirstOrDefaultAsync(); ;

            Console.WriteLine(t);

            if (t == null) return BadRequest();

            t.RefreshToken = user.RefreshToken;
            t.RefreshTokenExpires = user.RefreshTokenExpires;

            _dbContext.Entry(t).State = EntityState.Modified;


            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {     
                    return NotFound();
            }
            return Ok(new PutResult { result = "Ok" });
        }

        private bool UserExists(long id)
        {
            return (_dbContext.user?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
