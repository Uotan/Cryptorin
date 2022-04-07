using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cryptorin.Classes;
using Cryptorin.Data;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewUsersList : ContentPage
    {
        public ViewUsersList()
        {
            InitializeComponent();


        }

        private async void tbiFindUser_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ViewChat));
        }

        private async void AddItemButton_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Find user", "Enter user ID:", keyboard: Keyboard.Numeric);
            try
            {
                classSignature classSignature = new classSignature();
                fetchedUser fetchedUser = classSignature.fetchUserData(Convert.ToInt32(result));
                await DisplayAlert("Luck", fetchedUser.public_name, "Ok");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",ex.Message,"Ok");
            }
            
        }
    }
}