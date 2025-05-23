using BillSplit.ViewModels;

namespace BillSplit.Views;

public partial class CreateEventPage : ContentPage
{
    public CreateEventPage()
    {
        InitializeComponent();
        BindingContext = new EventViewModel();
    }
}
