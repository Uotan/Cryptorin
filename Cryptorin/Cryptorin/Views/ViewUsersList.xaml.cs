using System;
using System.Collections.Generic;
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
        public ViewUsersList()
        {
            InitializeComponent();


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

                List<User> users = await App.myDB.getUserList(fetchedUser.id);
                foreach (var item in users)
                {
                    await DisplayAlert("ok",item.id.ToString(),"ok");
                }

                if (myData.id==fetchedUser.id)
                {
                    throw new Exception("Are you trying to add yourself -_-");
                }
                if (users.Count>0)
                {
                    throw new Exception("Such a user already exists   UwU");
                }
                else
                {
                    AddUser(fetchedUser);
                }
                AddUser(fetchedUser);
                
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",ex.Message,"Ok");
            }
            
        }


        /// <summary>
        /// Add user to SQLite if it not exists
        /// </summary>
        /// <param name="_fetchedUser"></param>
        private void AddUser(fetchedUser _fetchedUser)
        {
            classSignature classSignature = new classSignature();
            string baseImage = classSignature.GetImage(_fetchedUser.id);


            //just create hex color just for fun
            var random = new Random();
            var color = String.Format("#{0:X6}", random.Next(0x1000000));
            
            App.myDB.AddUser(_fetchedUser.id,_fetchedUser.public_name,_fetchedUser.public_key,_fetchedUser.key_number,baseImage,color);

        }
    }
}