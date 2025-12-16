namespace TrafficEscape.Pages;

public partial class PausePage : ContentPage
{
    public PausePage()
    {
        InitializeComponent();
    }

    private async void OnResume(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }

    private async void OnQuit(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainMenuPage");
    }
}
