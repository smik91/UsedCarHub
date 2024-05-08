using UsedCarHub.Domain.Enums;

namespace UsedCarHub.BusinessLogic.DTOs
{
    public class CarDto
    {
        public string RegistrationNumber { get; set; }
        public string VIN { get; set; }
        public Marks Mark { get; set; }
        public string Model { get; set; }
        public int YearOfProduction { get; set; }
        public Transmission TransmissionType { get; set; }
        public float EngineCapacity { get; set; }
        public int Mileage { get; set; }
    }
}