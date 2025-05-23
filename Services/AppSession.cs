namespace BillSplit.Services
{
    public static class AppSession
    {
        public static string? UserId { get; set; }
        public static string? Email { get; set; }
        public static string? DisplayName { get; set; }
        public static string? IdToken { get; set; }

        // Helper property to check if the session is valid
        public static bool HasValidSession => !string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(IdToken);


        // Method to clear the session
        public static void ClearSession()
        {
            UserId = null;
            Email = null;
            DisplayName = null;
            IdToken = null;
        }
    }
}
