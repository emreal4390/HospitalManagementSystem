public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Password { get; set; } // Gerçek projelerde şifreler hashing ile saklanmalıdır
    public string Role { get; set; } // Kullanıcı rolü (ör. "Admin", "User")
}
