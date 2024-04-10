namespace UsedCarHub.Domain.Entities
{
    public sealed class UserEntity
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public List<CarEntity> CarsForSale { get; set; }
    }
}
