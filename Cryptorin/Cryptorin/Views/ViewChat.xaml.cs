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
    [QueryProperty(nameof(UserID), nameof(UserID))]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewChat : ContentPage
    {
        User user;


        string keyNumber = "";

        int CountMessOnDB;
        int CountMessLocal;

        bool timerAlive = false;

        bool isReady = true;
        bool isReady2 = true;
        bool isReady3 = true;

        bool firstTime = true;

        RSAUtil rSAUtil = new RSAUtil();

        ObservableCollection<classMessageTemplate> MessagesCurrent = new ObservableCollection<classMessageTemplate>();


        
        MyData myData = App.myDB.ReadMyData();
        string passwordHex;
        classAES aES = new classAES(keyClass.AESkey);
        string decryptedPrivateKey = null;


        classMessages classMess = new classMessages();

        classSignature signature = new classSignature();

        public ViewChat()
        {
            InitializeComponent();
            if (keyClass.isUnlock)
            {
                passwordHex = aES.Decrypt(myData.password);
                passwordHex = passwordHex.Trim();
            }

        }



        async void CheckChangeIndex()
        {
            while (timerAlive)
            {
                if (isReady3 == false)
                {
                    return;
                }
                string changeIndex = signature.GetUserChangeIndex(user.id);

                Debug.WriteLine("user change INDEX:" + changeIndex);

                if (changeIndex != user.changes_index)
                {
                    var fetchUser = signature.fetchUserData(user.id);
                    user.public_name = WebUtility.UrlDecode(fetchUser.public_name);
                    user.image = signature.GetImage(fetchUser.id);
                    user.changes_index = changeIndex;
                    App.myDB.UpdateUserData(user);

                    await this.DisplayToastAsync("Public data has been updated", 2000);
                    Debug.WriteLine("Updated User data");

                    //DisplayUserInfo();

                }

                isReady3 = false;
                await Task.Delay(3000);
                isReady3 = true;



            }


        }

        async void CheckAnotherEntry()
        {
            while (timerAlive)
            {
                if (otherEntryController.myKeyChanged)
                {
                    await Shell.Current.GoToAsync("..");
                }
                await Task.Delay(1000);
            }
        }

        void DisplayUserInfo()
        {
            User userTemp = App.myDB.GetUser(user.id);
            frameTop.BackgroundColor = Color.FromHex(userTemp.hex_color);
            userName.Text = WebUtility.UrlDecode(userTemp.public_name);

            try
            {

                if (userTemp.image != null || userTemp.image != "")
                {
                    byte[] byteArray = Convert.FromBase64String(user.image);
                    ImageSource image_Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
                    imageUser.Source = image_Source;
                }
            }
            catch (Exception ex)
            {
                imageUser.Source = null;
            }
        }


        //async void DisplayUserInfo()
        //{
        //    await Task.Run(() =>
        //    {
        //        frameTop.BackgroundColor = Color.FromHex(user.hex_color);
        //        userName.Text = WebUtility.UrlDecode(user.public_name);

        //        try
        //        {

        //            if (user.image != null || user.image != "")
        //            {
        //                byte[] byteArray = Convert.FromBase64String(user.image);
        //                ImageSource image_Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
        //                imageUser.Source = image_Source;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            imageUser.Source = null;
        //        }

        //    });
        //}

        public int UserID
        {
            set
            {
                ShowUserDataAndCheck(value);
            }
        }

        async void ShowUserDataAndCheck(int _id)
        {
            decryptedPrivateKey = aES.Decrypt(myData.private_key);
            await Task.Run(() =>
            {
                user = App.myDB.GetUser(_id);

                DisplayUserInfo();

                var fetchedMessagedData = App.myDB.GetMessages(user.id, myData.id);

                if (fetchedMessagedData != null)
                {
                    foreach (var item in fetchedMessagedData)
                    {
                        MessagesCurrent.Add(item);
                    }
                }
                collectionMessages.ItemsSource = MessagesCurrent;

                timerAlive = true;

                checkConnection checker = new checkConnection();
                bool result = checker.ConnectionAvailable(ServerAddress.srvrAddress);
                if (!result)
                {
                    entrContent.IsEnabled = false;
                    DisplayAlert("Error", "The connection to the server is not established", "Ok");
                }
                else
                {
                    CheckAnotherEntry();
                    CheckChangeIndex();
                    //Task.Delay(200);
                    CheckKeyNumber();
                }

                




                
            });

        }

        async void CheckKeyNumber()
        {
            while (timerAlive)
            {
                if (isReady2 == false)
                {
                    return;
                }
                isReady2 = false;

                Debug.WriteLine("check key number method starts");

                CountMessOnDB = classMess.GetCountOfMessagesWithUser(user.id, myData.id, myData.login, passwordHex);
                CountMessLocal = App.myDB.GetCountOfMessagesWithUserLocal(user.id);

                keyNumber = signature.GetUserKeyNumber(user.id);
                if (keyNumber != user.key_number)
                {
                    var fetchUser = signature.fetchUserData(user.id);
                    user.key_number = fetchUser.key_number;
                    user.public_key = fetchUser.public_key;
                    App.myDB.DeleteMessagesWithUser(user.id);

                    App.myDB.UpdateUserData(user);

                    MessagesCurrent.Clear();

                    Debug.WriteLine("user public key updated");
                    await this.DisplayToastAsync("The user has updated the keys", 2000);
                    //await DisplayAlert("Attention", "The user has updated the keys - all messages will be deleted.", "Ok");
                }
                else
                {
                    FetchMessages();
                }
                await Task.Delay(2000);

                isReady2 = true;
                Debug.WriteLine("check key number method END");


            }


        }


        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var result = await this.DisplayAlert("Alert!", "Do you really want to exit?", "Yes", "No");
                if (result)
                {
                    timerAlive = false;
                    await Shell.Current.GoToAsync("..");
                }
            });

            return true;
        }



        async private void FetchMessages()
        {
            await Task.Run(() =>
            {
                if (isReady != false)
                {
                    isReady = false;
                    if (firstTime)
                    {
                        if (CountMessLocal > 0)
                        {
                            collectionMessages.ScrollTo(App.myDB.GetCountOfMessagesLocal(myData.id, user.id) - 1);

                        }

                        firstTime = false;
                    }
                    //CountMessOnDB = classMess.GetCountOfMessagesWithUser(user.id, myData.id, myData.login, myData.password);
                    //CountMessLocal = App.myDB.GetCountOfMessagesWithUserLocal(user.id);

                    if (CountMessLocal < CountMessOnDB)
                    {
                        int fetchCount = CountMessOnDB - CountMessLocal;

                        var _searchAnswer = classMess.GetMessagesFromUser(user.id, myData.id, myData.login, passwordHex, fetchCount);

                        foreach (var item in _searchAnswer)
                        {

                            string DEcryptedText = rSAUtil.Decrypt(decryptedPrivateKey, item.rsa_cipher);

                            string AESmessage = aES.Encrypt(DEcryptedText);


                            classMessageTemplate template = new classMessageTemplate();
                            template.from_whom = item.from_whom.ToString();
                            template.content = WebUtility.UrlDecode(DEcryptedText);
                            template.datetime = item.datetime;

                            Message mess = new Message();
                            mess.from_whom = item.from_whom;
                            mess.for_whom = item.for_whom;
                            //mess.content = DEcryptedText;
                            mess.content = AESmessage;
                            mess.datetime = item.datetime;

                            App.myDB.AddMessage(mess);

                            MessagesCurrent.Add(template);



                        }

                        collectionMessages.ScrollTo(App.myDB.GetCountOfMessagesLocal(myData.id, user.id) - 1);
                    }
                    isReady = true;
                }

            });
        }



        async void entrContent_Completed(object sender, EventArgs e)
        {
            if (entrContent.Text == "" || entrContent.Text == null||user.key_number=="0")
            {
                entrContent.Text = null;
                return;
            }

            var urlEncodedMessage = WebUtility.UrlEncode(entrContent.Text);
            var notEncodedText = entrContent.Text;

            entrContent.Text = null;

            await Task.Run(() =>
            {
                

                //isReady = false;

                

                RSAUtil rSAUtil = new RSAUtil();

                string cryptedText = rSAUtil.Encrypt(user.public_key, urlEncodedMessage);

                classMessages classMess = new classMessages();

                string result = classMess.SendMessage(myData.id, user.id, myData.login, passwordHex, cryptedText);


                if (result != "error")
                {
                    //Debug.WriteLine(myData.id + ": " + entrContent.Text + "[" + result + "]");


                    classMessageTemplate template = new classMessageTemplate();
                    template.from_whom = myData.id.ToString();
                    //template.content = entrContent.Text;
                    template.content = notEncodedText;
                    template.datetime = result;

                    string AESmessage = aES.Encrypt(urlEncodedMessage);


                    Message mess = new Message();
                    mess.from_whom = myData.id;
                    mess.for_whom = user.id;
                    //mess.content = urlEncodedMessage;
                    mess.content = AESmessage;
                    mess.datetime = result;
                    App.myDB.AddMessageCompleted(mess);

                    MessagesCurrent.Add(template);

                    collectionMessages.ScrollTo(App.myDB.GetCountOfMessagesLocal(myData.id, user.id) - 1);
                }
                
                //isReady = true;

            } );

            

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

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            DisplayUserInfo();
        }
    }
}