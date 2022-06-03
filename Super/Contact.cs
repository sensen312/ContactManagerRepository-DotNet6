namespace Super
{
    public class Contact
    {
        public Guid ID { get; set; } = Guid.NewGuid();
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTime BirthDate { get; set; } = DateTime.MinValue;

    }
}
