namespace backend.Models;

public class UserRegisterDto
{
    public  required string Email { get; set; }
    public  required string Password { get; set; }
    public  required string FullName { get; set; }
}
