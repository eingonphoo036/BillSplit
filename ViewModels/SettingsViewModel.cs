using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using BillSplit.Services;
using Microsoft.Maui.Controls;

namespace BillSplit.ViewModels
{
    public class SettingsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        private readonly FirebaseAuthService _authService;
        private string _userName = "Anonymous";
        private string _email = "No email";
        private string _newPassword = string.Empty;

        public string Username
        {
            get => _userName;
            set { _userName = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _email;
            set { _email = value; OnPropertyChanged(); }
        }

        public string NewPassword
        {
            get => _newPassword;
            set { _newPassword = value; OnPropertyChanged(); }
        }

        public ICommand ChangePasswordCommand { get; }
        public ICommand LogoutCommand { get; }
        public ICommand NavigateHomeCommand { get; }

        public SettingsViewModel(FirebaseAuthService authService)
        {
            _authService = authService;

            ChangePasswordCommand = new Command(async () => await ChangePasswordAsync());
            LogoutCommand = new Command(async () =>
            {
                AppSession.ClearSession();
                await Shell.Current.GoToAsync("//LoginPage");
            });

            NavigateHomeCommand = new Command(async () =>
            {
                await Shell.Current.GoToAsync("//HomePage");
            });
        }

        public void LoadUserData()
        {
            Username = AppSession.DisplayName ?? "Anonymous";
            Email = AppSession.Email ?? "No email";
        }

        private async Task ChangePasswordAsync()
        {
            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                await Shell.Current.DisplayAlert("Error", "Password cannot be empty.", "OK");
                return;
            }

            var result = await _authService.ChangePassword(NewPassword);
            if (result)
            {
                await Shell.Current.DisplayAlert("Success", "Password changed successfully.", "OK");
                NewPassword = string.Empty;
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", "Failed to change password.", "OK");
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
