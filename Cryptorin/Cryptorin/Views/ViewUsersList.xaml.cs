using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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
    }
}