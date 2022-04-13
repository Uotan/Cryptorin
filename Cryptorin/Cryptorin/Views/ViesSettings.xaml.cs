using Cryptorin.Classes.SQLiteClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViesSettings : ContentPage
    {
        MyData myData;
        FileResult file;
        public ViesSettings()
        {
            InitializeComponent();
            LoadMyData();
        }

        async void LoadMyData()
        {
            try
            {
                myData = await App.myDB.ReadMyDataAsync();
                entryChgPubName.Placeholder = myData.public_name;
                if (myData.image != null)
                {
                    byte[] byteArray = Convert.FromBase64String(myData.image);
                    ImageSource Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
                    imagePicker.Source = Source;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",ex.Message,"Ok");
            }
            

        }
        private void btnChgPubName_Clicked(object sender, EventArgs e)
        {

        }

        private void btnCngPassword_Clicked(object sender, EventArgs e)
        {

        }

        private void btnChangeImage_Clicked(object sender, EventArgs e)
        {

        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                file = await FilePicker.PickAsync(new PickOptions { FileTypes = FilePickerFileType.Images, PickerTitle = "Please pick the image" });
                if (file == null)
                {
                    return;
                }
                imagePicker.Source = file.FullPath;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        private async void btnUpdateKeys_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Are you sure?", "This will delete all data!!!", "Yes", "No");
            //if (answer)
            //{
            //    //delete all data methods
            //    App.myDB.DeleteAllData();
            //    App.Current.MainPage = new NavigationPage(new ViewAuth());
            //}
        }
    }
}