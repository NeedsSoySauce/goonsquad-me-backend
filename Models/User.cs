namespace NeedsSoySauce.Models
{
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }

        public User(string userId, string username, string email)
        {
            UserId = userId;
            Username = username;
            Email = email;
        }
    }
}