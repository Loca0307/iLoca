namespace Api.DTO  // define the definition space, so when needed you just import "Api.Models" and can directly use the User class 
{// This class will be used to access user data without exposing the modelnamespace Api.DTOs

    public class UserDTO
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
