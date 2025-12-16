using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrafficEscape.Pages;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();
    }

    private async void OnBack(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
