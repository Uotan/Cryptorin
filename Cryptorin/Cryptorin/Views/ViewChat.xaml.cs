using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewChat : ContentPage
    {
        public ViewChat()
        {
            InitializeComponent();

            var byteArray = new WebClient().DownloadData("https://cryptorin.ru/images/filename.jpg");
            this.testIcon.IconImageSource = ImageSource.FromStream(() => new MemoryStream(byteArray));
        }
    }
}