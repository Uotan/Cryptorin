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

            var byteArray = new WebClient().DownloadData("https://cryptorin.ru/images/filename.jpg");
            this.testIcon.IconImageSource = ImageSource.FromStream(() => new MemoryStream(byteArray));
            //this.testIcon.IconImageSource = FileImageSource.FromStream(() => new MemoryStream(byteArray));

        }
    }
}