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
        bool isReady2 = true;

        bool firstTime = true;

        RSAUtil rSAUtil = new RSAUtil();

        ObservableCollection<classMessageTemplate> MessagesCurrent = new ObservableCollection<classMessageTemplate>();

        MyData myData = App.myDB.ReadMyData();

        classMessages classMess = new classMessages();

        classSignature signature = new classSignature();

        public ViewChat()
        {
            InitializeComponent();
            firstKeycheck();

        }

        async void firstKeycheck()
        {
            await Task.Run(() =>
            {
                bool check = false;
                while (!check)
                {
                    if (user != null)
                    {
                        check = true;
                        keyNumber = signature.GetUserKeyNumber(user.id);
                        if (keyNumber != user.key_number)
                        {
                            var fetchUser = signature.fetchUserData(user.id);
                            user.key_number = fetchUser.key_number;
                            user.public_key = fetchUser.public_key;
                            App.myDB.DeleteMessagesWithUser(user.id);

                            App.myDB.UpdateUserData(user);

                            MessagesCurrent.Clear();
                            this.DisplayToastAsync("The user has updated the keys", 2000);
                            Debug.WriteLine("user public key updated");
                            //DisplayAlert("Attention", "The user has updated the keys - all messages will be deleted.", "Ok");
                        }
                        
                    }

                }
            });
            
        }

        public int UserID
        {
            set
            {
                ShowUserDataAndCheck(value);
            }
        }
        async void ShowUserDataAndCheck(int _id)
        {
            await Task.Run(() =>
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
                    
                    CountMessLocal = App.myDB.GetCountOfMessagesLocal(myData.id, user.id);
                }


                collectionMessages.ItemsSource = MessagesCurrent;

                if (CountMessLocal > 0)
                {
                    collectionMessages.ScrollTo(CountMessLocal - 1);
                }


                Device.StartTimer(new TimeSpan(0, 0, 2), () =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        CheckKeyNumber();
                    });
                    return true;
                });
            });
            
        }
        async void CheckKeyNumber()
        {
            if (isReady2 == false)
            {
                return;
            }
            isReady2 = false;
            keyNumber = signature.GetUserKeyNumber(user.id);
            if (keyNumber != user.key_number)
            {
                var fetchUser = signature.fetchUserData(user.id);
                user.key_number = fetchUser.key_number;
                user.public_key = fetchUser.public_key;
                App.myDB.DeleteMessagesWithUser(user.id);

                App.myDB.UpdateUserData(user);

                MessagesCurrent.Clear();

                await DisplayAlert("Attention", "The user has updated the keys - all messages will be deleted.", "Ok");
            }
            else
            {
                FetchMessages();
            }
            isReady2 = true;

        }
        


        


        async private void FetchMessages()
        {
            await Task.Run(() =>
            {
                if (isReady == false)
                {
                    return;
                }
                isReady = false;
                CountMessOnDB = classMess.GetCountOfMessagesFithUser(user.id, myData.id, myData.login, myData.password);
                CountMessLocal = App.myDB.GetCountOfMessagesWithUserLocal(user.id);

                if (CountMessLocal < CountMessOnDB)
                {
                    int fetchCount = CountMessOnDB - CountMessLocal;

                    var _searchAnswer = classMess.GetMessagesFromUser(user.id, myData.id, myData.login, myData.password, fetchCount);

                    foreach (var item in _searchAnswer)
                    {

                            string DEcryptedText = rSAUtil.Decrypt(myData.private_key, item.rsa_cipher);

                            classMessageTemplate template = new classMessageTemplate();
                            template.from_whom = item.from_whom.ToString();
                            template.content = WebUtility.UrlDecode(DEcryptedText);
                            template.datetime = item.datetime;

                            MessagesCurrent.Add(template);

                            Debug.WriteLine(item.from_whom + ": " + DEcryptedText + "[" + item.datetime + "]");

                            Message mess = new Message();
                            mess.from_whom = item.from_whom;
                            mess.for_whom = item.for_whom;
                            mess.content = DEcryptedText;
                            mess.datetime = item.datetime;

                            App.myDB.AddMessage(mess);


                        
                    }

                    collectionMessages.ScrollTo(App.myDB.GetCountOfMessagesLocal(myData.id, user.id) - 1);
                }
                isReady = true;
            });
        }



        void entrContent_Completed(object sender, EventArgs e)
        {
            if (entrContent.Text == ""|| entrContent.Text == null)
            {
                return;
            }

            isReady = false;
            
            var urlEncodedMessage = WebUtility.UrlEncode(entrContent.Text);

            RSAUtil rSAUtil = new RSAUtil();

            string cryptedText = rSAUtil.Encrypt(user.public_key, urlEncodedMessage);

            classMessages classMess = new classMessages();

            string result = classMess.SendMessage(myData.id, user.id, myData.login, myData.password, cryptedText);


            if (result != "error")
            {
                Debug.WriteLine(myData.id + ": " + entrContent.Text + "[" + result + "]");


                classMessageTemplate template = new classMessageTemplate();
                template.from_whom = myData.id.ToString();
                template.content = entrContent.Text;
                template.datetime = result;

                MessagesCurrent.Add(template);

                Message mess = new Message();
                mess.from_whom = myData.id;
                mess.for_whom = user.id;
                mess.content = urlEncodedMessage;
                mess.datetime = result;
                App.myDB.AddMessageCompleted(mess);




            }
            entrContent.Text = null;
            isReady = true;
        }


        private async void collectionMessages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //copy text of selected message
            var MyCollectionView = sender as CollectionView;
            if (MyCollectionView.SelectedItem == null)
                return;

            classMessageTemplate messageItem = (classMessageTemplate)e.CurrentSelection.FirstOrDefault();
            Debug.WriteLine(messageItem.content);
            await Clipboard.SetTextAsync(messageItem.content);
            await this.DisplayToastAsync("Text copied", 2000);

            ((CollectionView)sender).SelectedItem = null;
        }
    }
}