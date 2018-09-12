using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tictactoe
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        Library library = new Library();
        public GamePage()
        {
            InitializeComponent();
        }

        private async void NewGameClicked(object sender, EventArgs e)
        {
            if (btnGame.Text == "Start Again" && library._won == false) {
                if (await DisplayAlert("tiktaktoł", "you will lose this game", "Ok", "Cancel") == true)
                {
                    library.SaveStats(2);
                    library.New(this, Display);
                }
            }
            else
            {
                btnGame.Text = "Start Again";
                library.New(this, Display);
            }
        }
        private async void StatsClicked(object sender, EventArgs e)
        {
            string[] StatsL0 = { "0", "0", "0" }, StatsL1 = { "0", "0", "0" }, StatsL2 = { "0", "0", "0" }; 
            if (Application.Current.Properties.ContainsKey("StatsL0"))
            {
                StatsL0 = Application.Current.Properties["StatsL0"].ToString().Split(',');
            }
            if (Application.Current.Properties.ContainsKey("StatsL1"))
            {
                StatsL1 = Application.Current.Properties["StatsL1"].ToString().Split(',');
            }
            if (Application.Current.Properties.ContainsKey("StatsL2"))
            {
                 StatsL2 = Application.Current.Properties["StatsL2"].ToString().Split(',');
            }
            await DisplayAlert("Stats", 
                "Level / Wins / Draws / Loses\n"+
                "Easy: "+StatsL0[0]+" / "+StatsL0[1]+" / "+StatsL0[2]+
                "\nMedium: "+StatsL1[0]+" / "+StatsL1[1]+" / "+StatsL1[2]+
                "\nHahard: " + StatsL2[0] + " / " + StatsL2[1] + " / " + StatsL2[2] , "Close");
        }
    }
}