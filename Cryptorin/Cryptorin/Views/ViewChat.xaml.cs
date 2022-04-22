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

namespace Cryptorin.Views
{
    [QueryProperty(nameof(UserID),nameof(UserID))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewChat : ContentPage
    {
        User user;
        int CountMessOnDB;
        int CountMessLocal;
        MyData myData;
        public ViewChat()
        {
            InitializeComponent();
            myData = App.myDB.ReadMyData();
            classMessages classMess = new classMessages();
            CountMessOnDB = classMess.GetCountOfMessages(myData.id,user.id,myData.login,myData.password);
            CountMessLocal = App.myDB.GetCountOfMessagesLocal(myData.id, user.id);
        }

        public int UserID
        {
            set
            {
                ShowUserData(value);
            }
        }
        void ShowUserData(int _id)
        {
            user = App.myDB.GetUser(_id);
            try
            {
                if (user.image != null || user.image != "")
                {
                    byte[] byteArray = Convert.FromBase64String(user.image);
                    ImageSource image_Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
                    imageUser.Source = image_Source;
                }
            }
            catch (Exception ex)
            {
            }

            frameTop.BackgroundColor = Color.FromHex(user.hex_color);
            //userName.Text = user.public_name;
            userName.Text = WebUtility.UrlDecode(user.public_name);
        }




    }
}