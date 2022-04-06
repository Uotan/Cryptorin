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
        public FlayoutHeaderTemplate()
        {
            InitializeComponent();



            //var byteArray = new WebClient().DownloadData("https://cryptorin.ru/images/filename.jpg");
            //var byteArray = Convert.FromBase64String(staticUserData.myData.image);
            //myPhoto.Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
            //frameTamplate.BackgroundColor = Color.FromHex(staticUserData.myData.hex_color);
            Load();
            
        }

        async void Load()
        {

            MyData myData = await App.myDB.ReadMyData();

            //var byteArray = new WebClient().DownloadData("https://cryptorin.ru/images/" + myData.login + ".jpg");
            try
            {
                classSignature classSignature = new classSignature();
                string imageBase64 = classSignature.GetImage(myData.id);
                byte[] byteArray = Convert.FromBase64String(imageBase64);
                myPhoto.Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
            }
            catch (Exception ex)
            {

            }
            lblId.Text = "#" + myData.id.ToString();
            lblPublicName.Text = myData.public_name;


        }
    }
}