using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficEscape.Services;

namespace TrafficEscape.Pages;
    public partial class GaragePage : ContentPage
    {
    public GaragePage()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadOwnedSkins();
    }
    private void LoadOwnedSkins()
    {
        // only show what player owns
        var ownedSkins = SkinService.AvailableSkins
            .Where(skin => SkinService.IsSkinOwned(skin))
            .ToList();

        GarageCollection.ItemsSource = ownedSkins;
    }
    private async void OnEquipClicked(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        string skinName = (string)btn.CommandParameter;

        SkinService.EquippedSkin = skinName;

        await DisplayAlert("Garage", "Car Selected!", "OK");
        await Shell.Current.GoToAsync("..");
    }
    private async void OnBack(object sender, EventArgs e) => await Shell.Current.GoToAsync("..");
}

