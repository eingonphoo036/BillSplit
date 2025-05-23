namespace BillSplit;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Explicitly register all your page routes
        Routing.RegisterRoute("LoginPage", typeof(Views.LoginPage));
        Routing.RegisterRoute("RegisterPage", typeof(Views.RegisterPage));
        Routing.RegisterRoute("HomePage", typeof(Views.HomePage));
        Routing.RegisterRoute("CreateEventPage", typeof(Views.CreateEventPage));
        Routing.RegisterRoute("AddParticipantsPage", typeof(Views.AddParticipantsPage));
        Routing.RegisterRoute("SplitSummaryPage", typeof(Views.SplitSummaryPage));
        Routing.RegisterRoute("ProfilePage", typeof(Views.ProfilePage));
        Routing.RegisterRoute("SettingsPage", typeof(Views.SettingsPage));
        GoToAsync("//LoginPage");
    }
}
