using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Cryptorin.Classes;
using Cryptorin.Classes.SQLiteClasses;
using Cryptorin.Data;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Diagnostics;

namespace Cryptorin.Common
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FlayoutHeaderTemplate : Grid
    {
        public ImageSource Source;

        public FlayoutHeaderTemplate()
        {
            InitializeComponent();
            Load();

        }

        async void Load()
        {

            MyData myData = await App.myDB.ReadMyDataAsync();
            //var byteArray = new WebClient().DownloadData("https://cryptorin.ru/images/" + myData.login + ".jpg");
            try
            {
                byte[] byteArray = Convert.FromBase64String(myData.image);
                Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
                myPhoto.Source = Source;
            }
            catch (Exception ex)
            {
                
            }
            lblId.Text = "#" + myData.id.ToString();
            lblPublicName.Text = WebUtility.UrlDecode(myData.public_name);


        }
    }
}