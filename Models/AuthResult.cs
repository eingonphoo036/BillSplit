using System.Collections.ObjectModel;

namespace BillSplit.Models
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string? UserId { get; set; }
        public string? IdToken { get; set; }
        public string? ErrorMessage { get; set; }
        public string? Email { get; set; }
        public string? DisplayName { get; set; }

    }

    public class Event
    {
        public string? Id { get; set; } // Nullable Id property
        public string Name { get; set; } = string.Empty;
        public decimal TotalCost { get; set; }
        public DateTime Date { get; set; }
        public string CreatorEmail { get; set; } = string.Empty;
        public ObservableCollection<Participant> Participants { get; set; } = new ObservableCollection<Participant>();
    }



    public class Participant
    {
        public string Email { get; set; } = string.Empty;
        public decimal AmountDue { get; set; }
        public bool HasPaid { get; set; }
        public string Name { get; set; } = string.Empty; // Add this line
    }


    public class Friend
    {
        public string Email { get; set; } = string.Empty; // Ensuring non-null default initialization
        public string Name { get; set; } = string.Empty; // Ensuring non-null default initialization
    }
}
