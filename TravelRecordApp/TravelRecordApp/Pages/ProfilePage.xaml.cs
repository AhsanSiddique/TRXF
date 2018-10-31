using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelRecordApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TravelRecordApp.Pages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
		public ProfilePage ()
		{
			InitializeComponent ();
		}
        protected override void OnAppearing()
        {
            base.OnAppearing();

            using(SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                //Linq Distinct - Find Unique values from a List
                var postTable = conn.Table<Post>().ToList();

                //Using query
                var categories = (from p in postTable
                                  orderby p.CategoryId
                                  select p.CategoryName).Distinct().ToList();

                //using linq query same as above 
                var categories2 = postTable.OrderBy(p => p.CategoryId).Select(p => p.CategoryName).Distinct().ToList();
                
                //Linq where - Filtering a List
                Dictionary<string,int> categoriesCount = new Dictionary<string, int>();
                foreach(var category in categories2)
                {
                    //Using query
                    var count = (from post in postTable
                                 where post.CategoryName == category
                                 select post).ToList().Count();
                    
                    //Using Linq same as above
                    var count2 = postTable.Where(p => p.CategoryName == category).ToList().Count();

                    //Add to Dictionary
                    categoriesCount.Add(category, count2);
                }
                //ListView Item Source
                categoriesListView.ItemsSource = categoriesCount;

                //Post Label Text Binding
                postCountLabel.Text = postTable.Count().ToString();
            }
        }
    }
}