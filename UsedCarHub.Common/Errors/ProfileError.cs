namespace UsedCarHub.Common.Errors
{
    public class ProfileError
    {
        public static readonly Error NotFoundById =
            new Error("Profile.NotFoundById", "The profile with this Id does not exist");

    }
}