using Microsoft.Maui.Controls;
using BillSplit.ViewModels;

namespace BillSplit.Views
{
    public partial class SplitSummaryPage : ContentPage
    {
        public SplitSummaryPage()
        {
            InitializeComponent();
            BindingContext = new SummaryViewModel();
        }
    }
}
