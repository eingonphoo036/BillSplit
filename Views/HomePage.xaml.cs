using BillSplit.ViewModels;

namespace BillSplit.Views
{
    public partial class HomePage : ContentPage
    {
        public HomePage()
        {
            InitializeComponent();
            BindingContext = new HomeViewModel();
        }

        private void OnLogoutClicked(object sender, EventArgs e)
        {
            // Recommended method for MAUI:
            Application.Current.Windows[0].Page = new LoginPage();
        }
    }
}
