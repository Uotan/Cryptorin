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

        bool isReady = true;

        RSAUtil rSAUtil = new RSAUtil();

        ObservableCollection<classMessageTemplate> MessagesCurrent = new ObservableCollection<classMessageTemplate>();

        MyData myData = App.myDB.ReadMyData();

        classMessages classMess = new classMessages();

        classSignature signature = new classSignature();

        public ViewChat()
        {
            InitializeComponent();
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

            var fetchedMessagedData = App.myDB.GetMessages(user.id, myData.id);

            if (fetchedMessagedData != null)
            {
                foreach (var item in fetchedMessagedData)
                {
                    MessagesCurrent.Add(item);
                }
                collectionMessages.ItemsSource = MessagesCurrent;
                CountMessLocal = MessagesCurrent.Count;
            }
            
            

            //CheckKeyNumber();

            Device.StartTimer(new TimeSpan(0, 0, 2), () =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    CheckKeyNumber();
                    Task.Delay(500);
                    MessageShow();
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

            var MyCollectionView = sender as CollectionView;
            if (MyCollectionView.SelectedItem == null)
                return;

            classMessageTemplate messageItem = (classMessageTemplate)e.CurrentSelection.FirstOrDefault();
            Debug.WriteLine(messageItem.content);
            await Clipboard.SetTextAsync(messageItem.content);
            await this.DisplayToastAsync("Text copied", 2000);

            ((CollectionView)sender).SelectedItem = null;
        }


        private void MessageShow()
        {
            if (isReady==false)
            {
                return;
            }
            isReady = false;
            CountMessOnDB = classMess.GetCountOfMessages(myData.id, user.id, myData.login, myData.password);
            //CountMessLocal = App.myDB.GetCountOfMessagesLocal(myData.id, user.id);

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
                        template.from_whom = item.from_whom.ToString();
                        template.content = WebUtility.UrlDecode(DEcryptedText);
                        template.datetime = item.datetime;

                        MessagesCurrent.Add(template);
                        CountMessLocal = MessagesCurrent.Count;

                        Debug.WriteLine(item.from_whom + ": " + DEcryptedText + "[" + item.datetime + "]");


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
            isReady = true;

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
                Debug.WriteLine(myData.id+": "+ entrContent.Text+"["+result+"]");


                classMessageTemplate template = new classMessageTemplate();
                template.from_whom = myData.id.ToString();
                template.content = entrContent.Text;
                template.datetime = result;

                MessagesCurrent.Add(template);
                CountMessLocal = MessagesCurrent.Count;

                //add to local table
                Message mess = new Message();
                mess.from_whom = myData.id;
                mess.for_whom = user.id;
                mess.content = urlEncodedMessage;
                mess.datetime = result;
                App.myDB.AddMessageCompleted(mess);


                

            }
            entrContent.Text = null;
        }
    }
}