namespace UsedCarHub.Domain.Entities
{
    public class AdvertisementEntity
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public UserEntity Seller { get; set; }
        public string SellerId { get; set; }
        public CarEntity Car { get; set; }
        public int CarId { get; set; }
    }
}