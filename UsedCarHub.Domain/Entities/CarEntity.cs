using UsedCarHub.Domain.Enums;

namespace UsedCarHub.Domain.Entities
{
    public class CarEntity
    {
        public int Id { get; set; }
        public string RegistrationNumber { get; set; }
        public string VIN { get; set; }
        public Marks Mark { get; set; }
        public string Model { get; set; }
        public int YearOfProduction { get; set; }
        public Transmission TransmissionType { get; set; }
        public float EngineCapacity { get; set; }
        public int Mileage { get; set; }
        public AdvertisementEntity Advertisement { get; set; }
        public int AdvertisementId { get; set; }
    }
}