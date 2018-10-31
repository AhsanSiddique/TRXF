using Plugin.Geolocator;
using SQLite;
using System;
using System.Collections.Generic;
using TravelRecordApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MapPage : ContentPage
	{
		public MapPage ()
		{
			InitializeComponent();
		}
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            var locator = CrossGeolocator.Current;

            locator.PositionChanged += Locator_PositionChanged;
            TimeSpan ts = new TimeSpan(00, 00, 0);
            await locator.StartListeningAsync(ts, 100);
            var position = await locator.GetPositionAsync();

            var center = new Position(position.Latitude, position.Longitude);
            var span = new MapSpan(center, 2, 2);
            locationsMap.MoveToRegion(span);
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Post>();
                var posts = conn.Table<Post>().ToList();
                DisplayInMap(posts);
            }
        }
        private void DisplayInMap(List<Post> posts)
        {
           
                foreach (var post in posts)
                {
                try
                {
                    var position = new Position(post.Latitude, post.Longitude);
                    var pin = new Pin()
                    {
                        Type = PinType.SavedPin,
                        Position = position,
                        Label = post.VenueName,
                        Address = post.Address
                    };
                    locationsMap.Pins.Add(pin);
                }
                catch (NullReferenceException nre) { }
                catch (Exception ex) { }
            }
        }
        protected override async void OnDisappearing()
        {
            base.OnDisappearing();
            var locator = CrossGeolocator.Current;

            locator.PositionChanged -= Locator_PositionChanged;
            await locator.StopListeningAsync(); 
        }

        private void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
            var center = new Position(e.Position.Latitude, e.Position.Longitude);
            var span = new MapSpan(center, 2, 2);
            locationsMap.MoveToRegion(span);
        }
       
    }
}