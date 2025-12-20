using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrafficEscape.Services;

namespace TrafficEscape.Pages;

public partial class ShopPage : ContentPage
{
    public ShopPage()
    {
        InitializeComponent();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        UpdateUI();
    }
    private void UpdateUI()
    {
        BalanceLabel.Text = SaveService.Coins.ToString();
        SkinsCollection.ItemsSource = null;
        SkinsCollection.ItemsSource = SkinService.AvailableSkins;
    }
    private async void OnSkinActionClicked(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        string skinName = (string)btn.CommandParameter;

        if (SkinService.IsSkinOwned(skinName))
        {
            SkinService.EquippedSkin = skinName;
            await DisplayAlert("Shop", "This car is already yours! Equipped.", "OK");
            UpdateUI();
            return;
        }
        if (SaveService.Coins >= SkinService.SkinPrice)
        {
            SaveService.Coins -= SkinService.SkinPrice;
            SkinService.SetSkinOwned(skinName);
            SkinService.EquippedSkin = skinName;

            await DisplayAlert("Shop", "Purchased and Equipped!", "OK");
        }
        else
        {
            await DisplayAlert("Shop", "You need more coins!", "OK");
        }

        UpdateUI();
    }
    private void OnButtonLoaded(object sender, EventArgs e)
    {
        var btn = (Button)sender;
        string skinName = (string)btn.CommandParameter;

        if (SkinService.IsSkinOwned(skinName))
        {
            btn.Text = "OWNED";
            btn.BackgroundColor = Colors.Green;
        }
    }

    private async void OnBack(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
