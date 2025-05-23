using System.ComponentModel;
using System.Windows.Input;
using BillSplit.Services;
using Microsoft.Maui.Controls;

namespace BillSplit.ViewModels
{
    public partial class RegisterViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseAuthService _authService = new();
        private readonly FirebaseDbService _dbService = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        private string _email = string.Empty;
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

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        private string _username = string.Empty;
        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyChanged(nameof(Username));
                }
            }
        }

        public ICommand RegisterCommand => new Command(async () => await RegisterAsync());

        private async Task RegisterAsync()
        {
            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Username))
            {
                await Shell.Current.DisplayAlert("Error", "Please fill all fields", "OK");
                return;
            }

            // in RegisterViewModel.cs or where you register the user
            var authResult = await _authService.Register(Email, Password, Username);


            if (authResult.Success && authResult.UserId != null)
            {
                AppSession.UserId = authResult.UserId;
                AppSession.Email = Email;
                AppSession.DisplayName = authResult.DisplayName;

                var dbService = new FirebaseDbService();
                await dbService.SaveUserAsync(authResult.UserId, Username, Email);

                await Shell.Current.GoToAsync("//HomePage");
            }
            else
            {
                await Shell.Current.DisplayAlert("Registration Failed", authResult.ErrorMessage ?? "Unknown error", "OK");
            }

        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
