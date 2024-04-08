using CarAPI.Enums;

namespace CarAPI.Models
{
    public sealed class Car
    {
        public Guid Id { get; set; }
        public int Price { get; set; }
        public Marks Mark {  get; set; }
        public string Model { get; set; }
        public int YearOfProduction { get; set; }
        public Transmission TransmissionType { get; set; }
        public int EngineCapacity { get; set; }
        public int Mileage { get; set; }
        public string Description { get; set; }

        //Relationships
        public User Owner { get; set; }
        public Guid OwnerId { get; set; }
    }
}
