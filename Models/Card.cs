namespace SecuLink.Models
{
    public class Card
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
