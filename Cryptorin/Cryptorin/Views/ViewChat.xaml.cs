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
using System.Collections.ObjectModel;

namespace Cryptorin.Views
{
    [QueryProperty(nameof(UserID),nameof(UserID))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewChat : ContentPage
    {
        User user;
        string keyNumber;
        int CountMessOnDB;
        int CountMessLocal;

        RSAUtil rSAUtil = new RSAUtil();

        ObservableCollection<classMessageTemplate> MessagesCurrent;

        MyData myData = App.myDB.ReadMyData();

        classMessages classMess = new classMessages();

        classSignature signature = new classSignature();

        public ViewChat()
        {
            InitializeComponent();
            MessagesCurrent = new ObservableCollection<classMessageTemplate>();
        }


        public int UserID
        {
            set
            {
                ShowUserDataAndCheck(value);
            }
        }
        void ShowUserDataAndCheck(int _id)
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
                Debug.WriteLine(ex.Message);
            }

            frameTop.BackgroundColor = Color.FromHex(user.hex_color);
            userName.Text = WebUtility.UrlDecode(user.public_name);

            MessagesCurrent = App.myDB.GetMessages(user.id, myData.id);

            collectionMessages.ItemsSource = MessagesCurrent;

            CheckKeyNumber();

            Device.StartTimer(new TimeSpan(0, 0, 2), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CheckKeyNumber();
                    //MessageShow();
                });
                return true;
            });


        }
        async void CheckKeyNumber()
        {
            keyNumber = signature.GetUserKeyNumber(user.id);
            if (keyNumber != user.key_number)
            {
                user.key_number = keyNumber;
                App.myDB.DeleteMessagesWithUser(user.id);
                App.myDB.UpdateUserData(user);
                Debug.WriteLine("key updated");
                await DisplayAlert("Attention", "The user has updated the keys - all messages will be deleted.", "Ok");
            }
        }
        
        private async void collectionMessages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            classMessageTemplate messageItem = (classMessageTemplate)e.CurrentSelection.FirstOrDefault();
            Debug.WriteLine(messageItem.content);
            await Clipboard.SetTextAsync(messageItem.content);
            await this.DisplayToastAsync("Text copied", 2000);
        }


        private void MessageShow()
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
                        

                        string DEcryptedText = rSAUtil.Decrypt(myData.private_key,item.rsa_cipher);

                        classMessageTemplate template = new classMessageTemplate();
                        template.from_whom = item.from_whom;
                        template.content = DEcryptedText;
                        //template.content = WebUtility.UrlDecode(DEcryptedText); ;
                        template.datetime = item.datetime;

                        Debug.WriteLine(template.content);


                        MessagesCurrent.Add(template);

                        Message mess = new Message();
                        mess.from_whom = item.from_whom;
                        mess.for_whom = item.for_whom;
                        //mess.content = WebUtility.UrlDecode(DEcryptedText);
                        mess.content = DEcryptedText;
                        mess.datetime = item.datetime;

                        App.myDB.AddMessage(mess);

                        
                    }
                }

                //collectionMessages.ScrollTo(App.myDB.GetCountOfMessagesLocal(myData.id, user.id) - 1);
            }

        }






        private void entrContent_Completed(object sender, EventArgs e)
        {
            var urlEncodedMessage = WebUtility.UrlEncode(entrContent.Text);

            RSAUtil rSAUtil = new RSAUtil();

            string cryptedText = rSAUtil.Encrypt(user.public_key, urlEncodedMessage);

            classMessages classMess = new classMessages();
            string result = classMess.SendMessage(myData.id,user.id,myData.login,myData.password, cryptedText);


            if (result != "error")
            {
                Debug.WriteLine(result);

                classMessageTemplate template = new classMessageTemplate();
                template.from_whom = myData.id;
                template.content = entrContent.Text;
                //template.datetime = result;
                template.datetime = null;

                MessagesCurrent.Add(template);

                //add to local table
                Message mess = new Message();
                mess.from_whom = myData.id;
                mess.for_whom = user.id;
                mess.content = entrContent.Text;
                mess.datetime = result;
                App.myDB.AddMessageCompleted(mess);


                

            }
            entrContent.Text = null;
        }
    }
}