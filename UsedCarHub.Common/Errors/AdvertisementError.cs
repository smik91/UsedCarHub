namespace UsedCarHub.Common.Errors
{
    public class AdvertisementError
    {
        public static readonly Error NotFoundById =
            new Error("Advertisement.NotFoundById", "Advertisement with this Id does not exist");
    }
}