using BillSplit.ViewModels;

namespace BillSplit.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage()
    {
        InitializeComponent();
        BindingContext = new LoginViewModel();
    }

    private void TogglePasswordVisibility(object sender, EventArgs e)
    {
        if (BindingContext is LoginViewModel vm)
        {
            vm.IsPasswordHidden = !vm.IsPasswordHidden;
        }
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//RegisterPage");

    }
}
