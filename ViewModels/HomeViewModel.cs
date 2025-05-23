using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using BillSplit.Services; // To access AppSession

namespace BillSplit.ViewModels
{
    public partial class HomeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string? _email;
        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public HomeViewModel()
        {
            // Fetch the logged-in user's email from session
            Email = AppSession.Email ?? "unknown@email.com";
        }

        public ICommand NavigateToSettingsCommand => new Command(async () =>
            await Shell.Current.GoToAsync("///SettingsPage"));

        public ICommand NavigateToProfileCommand => new Command(async () =>
            await Shell.Current.GoToAsync("///ProfilePage"));

        public ICommand CreateEventCommand => new Command(async () =>
            await Shell.Current.GoToAsync("///CreateEventPage"));

        public ICommand AddParticipantsCommand => new Command(async () =>
            await Shell.Current.GoToAsync("///AddParticipantsPage"));

        public ICommand ViewSummaryCommand => new Command(async () =>
            await Shell.Current.GoToAsync("///SplitSummaryPage"));

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
