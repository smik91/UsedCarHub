using UsedCarHub.Domain.Enums;

namespace UsedCarHub.Domain.Entities
{
    public class CarEntity
    {
        public int Id { get; set; }
        public string RegistrationNumber {  get; set; }
        public int Price { get; set; }
        public Marks Mark { get; set; }
        public string Model { get; set; }
        public int YearOfProduction { get; set; }
        public Transmission TransmissionType { get; set; }
        public int EngineCapacity { get; set; }
        public int Mileage { get; set; }
        public string Description { get; set; }
        public UserEntity Owner { get; set; }
        public string OwnerId { get; set; }
    }
}