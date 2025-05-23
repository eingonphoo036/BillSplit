using BillSplit.ViewModels;

namespace BillSplit.Views
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage()
        {
            InitializeComponent();
            var vm = new ProfileViewModel();
            vm.LoadUserData();
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ((ProfileViewModel)BindingContext).LoadUserData(); // Reload user data when the page appears
        }
    }
}
