using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAuth : ContentPage
    {
        public ViewAuth()
        {
            InitializeComponent();

        }



        private void btnSignIn_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new AppShell();
        }

        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ViewRegister());
        }
    }
}