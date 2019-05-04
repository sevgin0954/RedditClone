namespace RedditClone.Common.Validation
{
    public static class UnitOfWorkValidator
    {
        public static bool IsUnitOfWorkCompletedSuccessfully(int rowsAffected)
        {
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
