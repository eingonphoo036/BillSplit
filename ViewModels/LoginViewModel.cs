using System.ComponentModel;
using System.Windows.Input;
using BillSplit.Services;
using BillSplit.Views;
using Microsoft.Maui.Controls;

namespace BillSplit.ViewModels
{
    public partial class LoginViewModel : INotifyPropertyChanged
    {
        private readonly FirebaseAuthService _authService = new();

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

        private bool _isPasswordHidden = true;
        public bool IsPasswordHidden
        {
            get => _isPasswordHidden;
            set
            {
                _isPasswordHidden = value;
                OnPropertyChanged(nameof(IsPasswordHidden));
                OnPropertyChanged(nameof(PasswordToggleIcon));
            }
        }

        public string PasswordToggleIcon => IsPasswordHidden ? "eye_off.png" : "eye_on.png";

        public ICommand LoginCommand => new Command(async () => await LoginAsync());

        private async Task LoginAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
                {
                    await Shell.Current.DisplayAlert("Error", "Please fill all fields", "OK");
                    return;
                }

                // Call the Login method and get the result
                var result = await _authService.Login(Email, Password);

                // Ensure the result is not null and check for success
                if (result != null && result.Success && result.UserId != null)
                {
                    // Assign values to the AppSession
                    AppSession.UserId = result.UserId;
                    AppSession.Email = result.Email;
                    AppSession.IdToken = result.IdToken;
                    AppSession.DisplayName = result.DisplayName;

                    // Navigate to HomePage after a successful login
                    await Shell.Current.GoToAsync("//HomePage");
                }
                else
                {
                    // Handle login failure by displaying the error message
                    await Shell.Current.DisplayAlert("Login Failed", result?.ErrorMessage ?? "Unknown error", "OK");
                }
            }
            catch (Exception ex)
            {
                // Catch any exception during the login process and display it
                await Shell.Current.DisplayAlert("Crash", $"Login error: {ex.Message}", "OK");
            }
        }

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
