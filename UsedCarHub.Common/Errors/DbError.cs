namespace UsedCarHub.Common.Errors
{
    public static class DbError
    {
        public static readonly Error FailSaveChanges =
            new Error("Db.FailSaveChanges", "Failed to save changes in DB");
    }
}