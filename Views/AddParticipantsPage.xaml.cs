using BillSplit.ViewModels;

namespace BillSplit.Views
{
    public partial class AddParticipantsPage : ContentPage
    {
        private readonly AddParticipantViewModel _viewModel;

        public AddParticipantsPage()
        {
            InitializeComponent();
            _viewModel = new AddParticipantViewModel();
            BindingContext = _viewModel;
        }
        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadFriendsCommand.Execute(null);
        }
    }

}
