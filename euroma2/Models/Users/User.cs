namespace euroma2.Models.Users
{
    public class User
    {
        public int Id { get; set; }
        public string userName { get; set; }   

        public string password { get; set; }

        public int profile { get; set; }

        public string RefreshToken { get; set; }
        public string RefreshTokenExpires { get; set; }
    }
}
