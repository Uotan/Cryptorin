using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        ObservableCollection<UserTemplate> userList = new ObservableCollection<UserTemplate>();
        ObservableCollection<UserTemplate> userListSorted = new ObservableCollection<UserTemplate>();
        List<User> userListFromDB = new List<User>();
        public ViewUsersList()
        {
            InitializeComponent();
            Appearing += ViewUsersList_Appearing; ;
            //Disappearing += ViewUsersList_Disappearing; ;

            //userListFromDB = App.myDB.GetUsers();
            //foreach (var item in userListFromDB)
            //{
            //    UserTemplate userForList = new UserTemplate();
            //    userForList.id = item.id;
            //    //userForList.public_name = item.public_name;
            //    userForList.public_name = WebUtility.UrlDecode(item.public_name);
            //    userForList.hex_color = Color.FromHex(item.hex_color);
            //    userForList.image_source = null;
            //    try
            //    {
            //        if (item.image != null || item.image != "")
            //        {
            //            byte[] byteArray = Convert.FromBase64String(item.image);
            //            ImageSource image_Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
            //            userForList.image_source = image_Source;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //    userList.Add(userForList);
            //}
            //userCollector.ItemsSource = userList;

        }

        private async void ViewUsersList_Appearing(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                Debug.WriteLine("Appear");
                userListFromDB.Clear();
                userList.Clear();
                userListFromDB = App.myDB.GetUsers();
                foreach (var item in userListFromDB)
                {
                    UserTemplate userForList = new UserTemplate();
                    userForList.id = item.id;
                    //userForList.public_name = item.public_name;
                    userForList.public_name = WebUtility.UrlDecode(item.public_name);
                    userForList.hex_color = Color.FromHex(item.hex_color);
                    userForList.image_source = null;
                    try
                    {
                        if (item.image != null || item.image != "")
                        {
                            byte[] byteArray = Convert.FromBase64String(item.image);
                            ImageSource image_Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
                            userForList.image_source = image_Source;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                    userList.Add(userForList);
                }
                userCollector.ItemsSource = userList;
            });
            
        }

        private async void AddItemButton_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Find user", "Enter user ID:", keyboard: Keyboard.Numeric);
            try
            {
                if (result==null||result=="")
                {
                    return;
                }
                classSignature classSignature = new classSignature();
                fetchedUser fetchedUser = classSignature.fetchUserData(Convert.ToInt32(result));
                MyData myData = App.myDB.ReadMyData();
                User user = App.myDB.GetUser(fetchedUser.id);
                if (myData.id==fetchedUser.id)
                {
                    throw new Exception("Are you trying to add yourself -_-");
                }
                else if (user != null)
                {
                    throw new Exception("Such a user already exists\n\nThis is - "+ user.public_name);
                }
                else
                {
                    AddUser2DataBase(fetchedUser);

                    User user2 = App.myDB.GetUser(fetchedUser.id);

                    UserTemplate userTemplate = new UserTemplate();
                    userTemplate.id = user2.id;
                    userTemplate.public_name = WebUtility.UrlDecode(user2.public_name);
                    userTemplate.hex_color = Color.FromHex(user2.hex_color);
                    
                    try
                    {
                        if (user2.image != null || user2.image != "")
                        {
                            byte[] byteArray = Convert.FromBase64String(user2.image);
                            ImageSource image_Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
                            userTemplate.image_source = image_Source;
                        }
                    }
                    catch (Exception ex)
                    {
                        userTemplate.image_source = null;
                    }
                    searchField.Text = null;
                    userList.Add(userTemplate);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",ex.Message,"Ok");
            }
            
        }








        private void AddUser2DataBase(fetchedUser _fetchedUser)
        {
            classSignature classSignature = new classSignature();
            string baseImage = classSignature.GetImage(_fetchedUser.id);
            var random = new Random();
            var color = String.Format("#{0:X6}", random.Next(0x1000000));
            App.myDB.AddUser(_fetchedUser.id, _fetchedUser.public_name, _fetchedUser.public_key, _fetchedUser.key_number, baseImage, color,_fetchedUser.changes_index);
        }







        private async void userCollector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var MyCollectionView = sender as CollectionView;
            if (MyCollectionView.SelectedItem == null)
                return;


            UserTemplate userItem = (UserTemplate)e.CurrentSelection.FirstOrDefault();
            await Shell.Current.GoToAsync($"{nameof(ViewChat)}?{nameof(ViewChat.UserID)}={userItem.id}");
            ((CollectionView)sender).SelectedItem = null;


            
        }

        private void searchField_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (searchField.Text == null||searchField.Text == "")
            {
                userCollector.ItemsSource = userList;
            }
            else
            {
                userListSorted.Clear();
                var sortedList = userList.Where(x => x.public_name.Contains(searchField.Text)).ToList();
                foreach (var item in sortedList)
                {
                    userListSorted.Add(item);
                }
                userCollector.ItemsSource = userListSorted;
            }

        }
    }
}