using BillSplit.Services;
using BillSplit.ViewModels;

namespace BillSplit.Views
{
    public partial class SettingsPage : ContentPage
    {
        private SettingsViewModel viewModel;

        public SettingsPage()
        {
            InitializeComponent();

            var authService = new FirebaseAuthService();
            viewModel = new SettingsViewModel(authService);
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel?.LoadUserData(); // null check avoids crash
        }
    }
}
