using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using BillSplit.Models;
using BillSplit.Services;
using System.Windows.Input;

namespace BillSplit.ViewModels
{
    public class AddParticipantViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseDbService _firebaseDbService = new FirebaseDbService();

        private string? _email;
        private string? _name;


        public event PropertyChangedEventHandler? PropertyChanged;


        public ObservableCollection<Friend> Friends { get; } = new ObservableCollection<Friend>();

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged();
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged();
            }
        }

        public ICommand AddFriendCommand { get; }
        public ICommand LoadFriendsCommand { get; }
        public ICommand NavigateHomeCommand { get; }

        public AddParticipantViewModel()
        {
            AddFriendCommand = new Command(async () => await AddFriend());
            LoadFriendsCommand = new Command(async () => await LoadFriends());
            NavigateHomeCommand = new Command(async () => await Shell.Current.GoToAsync("//HomePage"));
            LoadFriendsCommand.Execute(null);
        }

        private async Task AddFriend()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlert("Validation Failed", "Email and Name cannot be empty.", "OK");
                return;
            }

            // Null check for FirebaseDbService and AppSession.UserId
            if (AppSession.UserId == null)
            {
                await Shell.Current.DisplayAlert("Error", "User not logged in.", "OK");
                return;
            }

            var friend = new Friend { Email = Email ?? "", Name = Name ?? "" };
            await _firebaseDbService.AddFriend(AppSession.UserId, friend);
            Shell.Current.Dispatcher.Dispatch(() => Friends.Add(friend));

            Email = string.Empty;
            Name = string.Empty;
        }


        private async Task LoadFriends()
        {
            if (string.IsNullOrEmpty(AppSession.UserId))
            {
                await Shell.Current.DisplayAlert("Error", "User not logged in or UserId missing.", "OK");
                return;
            }

            var friends = await _firebaseDbService.GetFriends(AppSession.UserId);
            if (friends == null || !friends.Any())
            {
                Console.WriteLine("No friends data fetched");
                return;
            }

            Shell.Current.Dispatcher.Dispatch(() =>
            {
                Friends.Clear();
                foreach (var friend in friends)
                {
                    Friends.Add(friend);
                    Console.WriteLine($"Added friend: {friend.Name}");
                }
            });
        }


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
