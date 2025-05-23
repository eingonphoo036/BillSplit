namespace BillSplit;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState activationState)
    {
        return new Window(new AppShell()); // or new LoginPage() if you want to skip Shell
    }
}
