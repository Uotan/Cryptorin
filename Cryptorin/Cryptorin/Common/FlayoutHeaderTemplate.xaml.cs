using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cryptorin.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlayoutHeaderTemplate : Grid
    {
        public FlayoutHeaderTemplate()
        {
            InitializeComponent();



            //var byteArray = new WebClient().DownloadData("https://cryptorin.ru/images/filename.jpg");
            //myPhoto.Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
            
        }
    }
}