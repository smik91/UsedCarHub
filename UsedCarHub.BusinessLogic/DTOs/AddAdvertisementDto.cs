namespace UsedCarHub.BusinessLogic.DTOs
{
    public class AddAdvertisementDto
    {
        public int Price { get; set; }
        public string Description { get; set; }
        public CarDto Car { get; set; }
    }
}