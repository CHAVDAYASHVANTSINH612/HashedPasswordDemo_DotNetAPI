namespace PasswordHashedDemo_DotNetAPI.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public byte[] PasswordHashed { get; set; }
        public byte[] SaltHashed { get; set; }
    }
}
