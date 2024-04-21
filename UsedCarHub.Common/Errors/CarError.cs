namespace UsedCarHub.Common.Errors
{
    public static class CarError
    {
        public static readonly Error RegNumberIsNull =
            new Error("Car.RegNumberIsNull", "Registration number is required");
        
        public static readonly Error SameRegNumber = 
            new Error("Car.SameRegNumber", "A car with such reg. number already exists");

        public static readonly Error ModelIsNull =
            new Error("Car.ModelIsNull", "Model is required");
        
        public static readonly Error NotFoundById =
            new Error("Car.NotFoundById", "The car with this Id does not exist");
    }
}