using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tictactoe.Data;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace tictactoe
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OptionsPage : ContentPage
	{
        int value = 0;
        bool single = true;
        int WhoStarts = 2;
		public OptionsPage ()
		{
            if (Application.Current.Properties.ContainsKey("Level"))
            {
                value = Int32.Parse(Application.Current.Properties["Level"].ToString());
            }
            if (Application.Current.Properties.ContainsKey("Single"))
            {
                single = Convert.ToBoolean(Application.Current.Properties["Single"]);
            }
            if (Application.Current.Properties.ContainsKey("WhoStarts"))
            {
                WhoStarts = Int32.Parse(Application.Current.Properties["WhoStarts"].ToString());
            }
            InitializeComponent();

            sldr.Value = value;
            playerSwitch.IsToggled = single;

            ChangeLabelText(value);
            ChangeSwitchText(single);
            Changebtnbg();
			
		}
        private async void SaveClicked()
        {
            Application.Current.Properties["Level"] = value;
            Application.Current.Properties["Single"] = single;
            Application.Current.Properties["WhoStarts"] = WhoStarts;
            await Application.Current.SavePropertiesAsync();
            
        }
        void OnSliderValueChanged(object sender, ValueChangedEventArgs args)
        {
            value = Convert.ToInt32(args.NewValue);
            ChangeLabelText(value);
        }
        
        public void ChangeLabelText( int value)
        {
            if (value == 0)
            {
                displayLabel.Text = "That's just random";
            }
            else if(value == 1)
            {
                displayLabel.Text = "It's probably skilled, somehow";
            }
            else
            {
                displayLabel.Text = "That one thinks.. As AI can";
            }
        }
        void Handle_Toggled(object sender, Xamarin.Forms.ToggledEventArgs e)
        {
            single = e.Value;
            ChangeSwitchText(single);
        }
        public void ChangeSwitchText(bool single)
        {
            if (single == true)
            {
                lblSingle.Text = "Single Player Mode ON";

            }
            else
            {
                lblSingle.Text = "Single Player Mode OFF";
            }
        }
        private void btnXClicked()
        {
            WhoStarts = 0;
            Changebtnbg();
        }
        private void btnXOClicked()
        {
            WhoStarts = 1;
            Changebtnbg();
        }
        private void btnOClicked()
        {
            WhoStarts = 2;
            Changebtnbg();
        }
        public void Changebtnbg()
        {
            if (WhoStarts == 0)
            {
                btnX.BackgroundColor = Color.FromHex("c0c6cc");
                btnO.BackgroundColor = Color.AliceBlue;
                btnXO.BackgroundColor = Color.AliceBlue;
            }
            else if (WhoStarts == 1)
            {
                btnXO.BackgroundColor = Color.FromHex("c0c6cc");
                btnO.BackgroundColor = Color.AliceBlue;
                btnX.BackgroundColor = Color.AliceBlue;
            }
            else
            {
                btnO.BackgroundColor = Color.FromHex("c0c6cc");
                btnX.BackgroundColor = Color.AliceBlue;
                btnXO.BackgroundColor = Color.AliceBlue;
            }
        }
    }
}