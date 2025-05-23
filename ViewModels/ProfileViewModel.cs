using System.ComponentModel;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using BillSplit.Services;
using System.Runtime.CompilerServices; // Needed for CallerMemberName

namespace BillSplit.ViewModels
{
    public partial class ProfileViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;


        private string _userName = "Unknown User";
        private string _email = "No email";

        public string Username
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand NavigateHomeCommand => new Command(async () =>
        {
            await Shell.Current.GoToAsync("//HomePage");
        });

        public void LoadUserData()
        {
            Username = AppSession.DisplayName ?? "Unknown User";
            Email = AppSession.Email ?? "No email";
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
