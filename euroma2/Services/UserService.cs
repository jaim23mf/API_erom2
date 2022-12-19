using euroma2.Models;
using euroma2.Models.Events;
using euroma2.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace euroma2.Services
{

        public interface IUserService
        {
            bool IsValidUser(string userName, string password);
        }

        public class UserService : IUserService
        {
            private readonly ILogger<UserService> _logger;
            private readonly DataContext _dbContext;

            public UserService(ILogger<UserService> logger, DataContext dbContext)
            {
                _logger = logger;
                _dbContext = dbContext;
            }

            public bool IsValidUser(string userName, string password)
            {
                _logger.LogInformation($"Validating user [{userName}]");
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return false;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    return false;
                }

            if (!isUser(userName,password).Result) { return false; }

                return true;
            }

        private async Task<bool> isUser(string uName, string pwd) {

            var t = await _dbContext
              .user
              .Where(a => a.userName == uName)
              .Where(a => a.password == pwd)
              .FirstOrDefaultAsync(); ;

            Console.WriteLine(t);

            if (t != null) return true;
            else return false;
        }

    }
    
}
