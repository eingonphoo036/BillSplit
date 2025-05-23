using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace BillSplit.Views
{
    public partial class SplashPage : ContentPage
    {
        public SplashPage()
        {
            InitializeComponent();
            NavigateToLogin();
        }

        private async void NavigateToLogin()
        {
            await Task.Delay(2000); // Wait 2 seconds
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
