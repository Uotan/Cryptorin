using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Cryptorin.Classes;
using Cryptorin.Classes.SQLiteClasses;
using Cryptorin.Data;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewUsersList : ContentPage
    {

        //ObservableCollection<UserTemplate> userList;
        public ViewUsersList()
        {
            InitializeComponent();
            //this.BindingContext = this;
            //userList = new ObservableCollection<UserTemplate>();
        }

        private async void tbiFindUser_Clicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ViewChat));
        }

        private async void AddItemButton_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Find user", "Enter user ID:", keyboard: Keyboard.Numeric);
            try
            {
                classSignature classSignature = new classSignature();
                fetchedUser fetchedUser = classSignature.fetchUserData(Convert.ToInt32(result));
                MyData myData = await App.myDB.ReadMyData();
                User user = await App.myDB.getUser(fetchedUser.id);
                if (myData.id==fetchedUser.id)
                {
                    throw new Exception("Are you trying to add yourself -_-");
                }
                else if (user !=  null)
                {
                    throw new Exception("Such a user already exists   UwU");
                }
                else
                {
                    AddUser(fetchedUser);
                    //User addedUser = await AddUser(fetchedUser);
                    //await DisplayAlert("",addedUser.public_name,"");




                    //UserTemplate userTemplate = new UserTemplate();
                    //userTemplate.id = user2.id;
                    //userTemplate.public_name = user2.public_name;
                    //userTemplate.hex_color = user2.hex_color;
                    ////userTemplate.image_source = null;

                    ////userList.Add(userTemplate);


                    ////ImageSource image_Source;
                    ////try
                    ////{
                    ////    byte[] byteArray = Convert.FromBase64String(user.image);
                    ////    Image image = new Image();
                    ////    image_Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
                    ////    userTemplate.image_source = image_Source;
                    ////}
                    ////catch (Exception ex)
                    ////{
                    ////    userTemplate.image_source = null;
                    ////}
                    //userCollector.ItemsSource = userList;
                }
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",ex.Message,"Ok");
            }
            
        }


        private async void AddUser(fetchedUser _fetchedUser)
        {
            var outer = Task.Factory.StartNew(() =>
            {
                classSignature classSignature = new classSignature();
                string baseImage = classSignature.GetImage(_fetchedUser.id);
                var random = new Random();
                var color = String.Format("#{0:X6}", random.Next(0x1000000));
                App.myDB.AddUser(_fetchedUser.id, _fetchedUser.public_name, _fetchedUser.public_key, _fetchedUser.key_number, baseImage, color);
            });
            outer.Wait();

            User user2 = await App.myDB.getUser(_fetchedUser.id);
            await DisplayAlert("qwe", user2.public_name, "qwe");
            //return user2;

        }
    }
}