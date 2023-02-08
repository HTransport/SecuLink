namespace SecuLink.Models
{
    public class NewUser
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Role { get; set; }
        public string Email { get; set; }
        public string SerialNumber { get; set; }
        public string Pin { get; set; }
    }
}
