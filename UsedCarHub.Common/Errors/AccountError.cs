namespace UsedCarHub.Common.Errors
{
    public static class AccountError
    {
        public static readonly Error SameEmail = 
            new Error("Account.SameEmail", "A user with this email already exists");
        
        public static readonly Error SameUserName =
            new Error("Account.SameUserName", "A user with this username already exists");
        
        public static readonly Error NotFountByUserName =
            new Error("Account.NameNotFound", "There is no user with such username");
        
        public static readonly Error NotFoundById =
            new Error("Account.NotFoundById", "The user with this Id does not exist");

        public static readonly Error InvalidPassword =
            new Error("Account.InvalidPassword", "Invalid password");
    }
}