using euroma2.Auth;
using euroma2.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

namespace euroma2.Controllers
{
    [Route("identity")]
    [Authorize]
    public class IdentityController : ControllerBase
    {

        private readonly ILogger<IdentityController> _logger;
        private readonly IUserService _userService;
        private readonly TokenManagement _tokenManagement;
        public IdentityController(ILogger<IdentityController> logger, IUserService userService, TokenManagement tokenManagement)
        {
            _logger = logger;
            _userService = userService;
            _tokenManagement = tokenManagement;
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
                return BadRequest("Invalid Request");
            }

            if (!_userService.IsValidUser(request.UserName, request.Password))
            {
                return BadRequest("Invalid Request");
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
                Logged = true
            });
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

        public class LoginResult
        {
            /// <summary>
            /// 
            /// </summary>
            /// <example>admin</example>
            public string UserName { get; set; }
            public string Token { get; set; }

            public Boolean Logged { get; set; }
        }

    }
}
