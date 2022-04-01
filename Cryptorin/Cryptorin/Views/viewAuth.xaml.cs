using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Diagnostics;
using Cryptorin.Data;

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
            classAES qwe = new classAES("4elpsGky8'}|;I[*11111111");
            string _data = qwe.Encrypt("hello Xamarin");
            string decrypt = qwe.Decrypt(_data);
            Debug.WriteLine(decrypt);
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