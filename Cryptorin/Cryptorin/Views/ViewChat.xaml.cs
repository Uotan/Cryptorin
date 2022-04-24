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
using Xamarin.Essentials;
using Xamarin.CommunityToolkit.Extensions;
using System.Diagnostics;

namespace Cryptorin.Views
{
    [QueryProperty(nameof(UserID),nameof(UserID))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewChat : ContentPage
    {
        User user;
        int CountMessOnDB;
        int CountMessLocal;
        MyData myData = App.myDB.ReadMyData();
        classMessages classMess = new classMessages();

        List<MessageTemplate> messages;


        public ViewChat()
        {
            InitializeComponent();
            messages = new List<MessageTemplate>();
            messages = GetMessagesFromLocal(myData.id, user.id);

            if (messages.Count != 0)
            {
                collectionMessages.ItemsSource = messages;
                collectionMessages.ScrollTo(messages.Count - 1);
            }


            Device.StartTimer(new TimeSpan(0, 0, 1), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    MessageShow();
                });
                return true;
            });


        }

        List <MessageTemplate> GetMessagesFromLocal(int _idFirst, int _idSecond)
        {
            var list = App.myDB.GetMessages(_idFirst,_idSecond);
            return list;
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
            userName.Text = WebUtility.UrlDecode(user.public_name);
        }

        private async void collectionMessages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Message messageItem = (Message)e.CurrentSelection.FirstOrDefault();
            Debug.WriteLine(messageItem.content);
            await Clipboard.SetTextAsync(messageItem.content);
            await this.DisplayToastAsync("Text copied", 2000);
        }


        private async void MessageShow()
        {

            CountMessOnDB = classMess.GetCountOfMessages(myData.id, user.id, myData.login, myData.password);
            CountMessLocal = App.myDB.GetCountOfMessagesLocal(myData.id, user.id);

            if (CountMessLocal < CountMessOnDB)
            {
                int fetchCount = CountMessOnDB - CountMessLocal;

                var _searchAnswer = classMess.GetMessages(myData.id,user.id,myData.login,myData.password,fetchCount);

                foreach (var item in _searchAnswer)
                {
                    if (item.from_whom==user.id)
                    {
                        classRSA rsa = new classRSA();
                        string decryptedText = rsa.Decrypt(item.rsa_cipher,myData.private_key);
                        App.myDB.AddMessage(item, decryptedText);
                    }
                }
                //messages = GetMessagesFromLocal(myData.id, user.id);
                //collectionMessages.ItemsSource = messages;
                collectionMessages.ScrollTo(App.myDB.GetCountOfMessagesLocal(myData.id, user.id) - 1);
            }

        }






        private void entrContent_Completed(object sender, EventArgs e)
        {
            var urlEncodedMessage = WebUtility.UrlEncode(entrContent.Text);

            classRSA rsa = new classRSA();

            string cryptedText = rsa.Encrypt(urlEncodedMessage, user.public_key);

            classMessages classMess = new classMessages();
            string result = classMess.SendMessage(myData.id,user.id,myData.login,myData.password, cryptedText);
            if (result != "error")
            {
                //add to local table
                Message mess = new Message();
                mess.from_whom = myData.id;
                mess.for_whom = user.id;
                mess.content = urlEncodedMessage;
                mess.datetime = result;
                App.myDB.AddMessageCompleted(mess);
            }
            //messages = GetMessagesFromLocal(myData.id, user.id);
            //collectionMessages.ItemsSource = messages;
            collectionMessages.ScrollTo(App.myDB.GetCountOfMessagesLocal(myData.id, user.id) - 1);

            entrContent.Text = null;
        }
    }
}