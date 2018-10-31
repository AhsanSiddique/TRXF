using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TravelRecordApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            var assembly = typeof(MainPage);
            iconImage.Source = ImageSource.FromResource("TravelRecordApp.Assets.Images.plane.png",assembly);
        }

        private async void LogIn_Clicked(object sender, EventArgs e)
        {
            bool isEmailEmpty = string.IsNullOrEmpty(emailEntry.Text);
            bool isPasswordEmpty = string.IsNullOrEmpty(passwordEntry.Text);
            if(isEmailEmpty || isPasswordEmpty)
            {
               await DisplayAlert("Error", "Login feild!", "OK");
            }
            else
            {
               await Navigation.PushAsync(new HomePage());
            }
        }
    }
}
