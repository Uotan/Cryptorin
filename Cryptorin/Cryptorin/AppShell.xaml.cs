using Cryptorin.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cryptorin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(viewAuth), typeof(viewAuth));

            //App.Current.MainPage = this;

            //chane();
        }
        async void chane()
        {
            await Shell.Current.GoToAsync(nameof(viewAuth));
        }

    }
}