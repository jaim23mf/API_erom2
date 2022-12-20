using euroma2.Auth;
using euroma2.Models;
using euroma2.Models.Users;
using euroma2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace euroma2.Controllers
{
    [EnableCors("corsapp")]
    [Route("identity")]
    [Authorize]
    public class IdentityController : ControllerBase
    {

        private readonly ILogger<IdentityController> _logger;
        private readonly IUserService _userService;
        private readonly TokenManagement _tokenManagement;
        private DataContext _dataContext;
        public IdentityController(ILogger<IdentityController> logger, IUserService userService, TokenManagement tokenManagement, DataContext dbcontext)
        {
            _logger = logger;
            _userService = userService;
            _tokenManagement = tokenManagement;
            _dataContext = dbcontext;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest("Invalid Request");
                return Ok(new LoginResult
                {
                    UserName = request.UserName,
                    Token = String.Empty,
                    RefreshToken = null,
                    Logged = false
                });
            }

            if (!_userService.IsValidUser(request.UserName, request.Password))
            {
                return Ok(new LoginResult
                {
                    UserName = request.UserName,
                    Token = String.Empty,
                    RefreshToken = null,
                    Logged = false
                });
            }

            var claims = new[]
            {
                new Claim(ClaimTypes.Name,request.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
            _tokenManagement.Issuer,
                _tokenManagement.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            _logger.LogInformation($"User [{request.UserName}] logged in the system.");


            return Ok(new LoginResult
            {
                UserName = request.UserName,
                Token = token,
                RefreshToken = GenerateRefreshToken(request.UserName),
                Logged = true
            });
        }



         [AllowAnonymous]
         [HttpPost("refresh-token")]
         public IActionResult RefreshToken([FromBody] TokenReq tq)
         {
            var refreshToken = tq.refreshToken;
            var refreshTokenE = tq.refreshTokenE;

            if (refreshToken == null) {

                return Ok(new LoginResult
                {
                    UserName = null,
                    Token = null,
                    RefreshToken = null,
                    Logged = false
                });
            }
            if (refreshTokenE == null) {
                return Ok(new LoginResult
                {
                    UserName = null,
                    Token = null,
                    RefreshToken = null,
                    Logged = false
                });
            }

            User u = new User();
            u = GetUserByToken(refreshToken).Result;

            if (u == null) {
                return Ok(new LoginResult
                {
                    UserName = null,
                    Token = null,
                    RefreshToken = null,
                    Logged = false
                });
            }

            RefreshToken r = new RefreshToken();
            r.Token = refreshToken;
            r.Expires = DateTime.Parse(refreshTokenE);

            if (r.Expires < DateTime.Now) return BadRequest();
            var claims = new[]
           {
                new Claim(ClaimTypes.Name,u.userName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
            _tokenManagement.Issuer,
                _tokenManagement.Audience,
                claims,
                expires: DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return Ok(new LoginResult
            {
                UserName = u.userName,
                Token = token,
                RefreshToken = r,
                Logged = true
            });
         }

        private async Task<User> GetUserByToken(string rToken) {

            var t = await _dataContext
            .user
            .Where(a => a.RefreshToken == rToken)
            .FirstOrDefaultAsync(); ;

            Console.WriteLine(t);

            if (t == null) return null;

            return t;

        }

        private RefreshToken GenerateRefreshToken(string ipAddress)
        {
            var refreshToken = new RefreshToken
            {
                Token = getUniqueToken(),
                Expires = DateTime.Now.AddDays(_tokenManagement.RefreshExpiration),
                CreatedByIp = ipAddress
            };

            return refreshToken;

            string getUniqueToken()
            {
                var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
                return token;
            }
        }

        public class LoginRequest
        {
            /// <summary>
            /// 
            /// </summary>
            /// <example>admin</example>
            [Required]
            [JsonPropertyName("username")]
            public string UserName { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <example>securePassword</example>
            [Required]
            [JsonPropertyName("password")]
            public string Password { get; set; }

        }

        public class TokenReq
        { 
            public string refreshToken { get; set; }
            public string refreshTokenE { get; set; }

        }

        public class LoginResult
        {
            /// <summary>
            /// 
            /// </summary>
            /// <example>admin</example>
            public string UserName { get; set; }
            public string Token { get; set; }
            public RefreshToken RefreshToken { get; set; }
            public Boolean Logged { get; set; }
        }

    }
}
