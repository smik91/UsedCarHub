namespace UsedCarHub.Common.Errors
{
    public class AdvertisementError
    {
        public static readonly Error NotFoundById =
            new Error("Car.NotFoundById", "The car with this Id does not exist");
    }
}