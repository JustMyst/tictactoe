using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Share;
using Plugin.Share.Abstractions;

namespace tictactoe
{
	public partial class MainPage : ContentPage
	{
		public MainPage()
		{
            InitializeComponent();
        }

        internal async void btnNewGameClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new GamePage());
        }
        internal async void btnOptionsClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new OptionsPage());
        }
        internal void btnShareClicked(object sender, EventArgs e)
        {
            CrossShare.Current.Share(new ShareMessage()
            {
                Text = "Omg, gram w tiktaktoł.",
                Title = "Tiktaktoł"
            });
        }
    }
}
