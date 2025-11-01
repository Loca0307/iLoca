namespace Api.Models // define the definition space, so when needed you just import "Api.Models" and can directly use the User class 
{
    public class User
    { // TO DO: define the User properties to be saved in the database
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
    }
}