using BillSplit.ViewModels;

namespace BillSplit.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = new RegisterViewModel();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        // Use Shell navigation instead of accessing MainPage directly
        await Shell.Current.GoToAsync("//LoginPage");
    }
}
