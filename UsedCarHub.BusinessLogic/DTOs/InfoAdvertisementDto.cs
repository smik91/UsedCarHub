namespace UsedCarHub.BusinessLogic.DTOs
{
    public class InfoAdvertisementDto
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public string SellerId { get; set; }
        public CarDto Car { get; set; }
    }
}