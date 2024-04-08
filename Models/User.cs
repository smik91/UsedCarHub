namespace CarAPI.Models
{
    public sealed class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<Car>? CarsForSale { get; set; }
    }
}
