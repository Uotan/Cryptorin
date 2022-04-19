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
using Cryptorin.Classes;
using Cryptorin.Data;

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
        private async void btnChgPubName_Clicked(object sender, EventArgs e)
        {
            if (entryChgPubName.Text!=null)
            {
                classSignature signature = new classSignature();
                string result = signature.UpdatePublicName(myData.login, myData.password, entryChgPubName.Text);
                myData.public_name = entryChgPubName.Text;
                App.myDB.UpdateMyData(myData);
                entryChgPubName.Placeholder = myData.public_name;
                entryChgPubName.Text = "";
                await DisplayAlert("Report", result + "\nRestart the application", "Ok");
            }
            else
            {
                await DisplayAlert("Error", "Incorrectly entered data", "Ok");
            }
            
        }

        private async void btnCngPassword_Clicked(object sender, EventArgs e)
        {
            Argon argon = new Argon();
            string oldPassHash = argon.Argon2id(entrPassOld.Text);
            if (entrPassNew1.Text!=entrPassNew2.Text||entrPassNew1.Text==""||entrPassOld.Text==""|| oldPassHash != myData.password)
            {
                await DisplayAlert("Error", "Incorrectly entered data", "Ok");
                
            }
            else
            {
                classSignature classSign = new classSignature();
                string newPassHash = argon.Argon2id(entrPassNew1.Text);


                string result = classSign.UpdatePassword(myData.login, oldPassHash, newPassHash);
                if (result == "Updated")
                {
                    myData.password = newPassHash;
                    App.myDB.UpdateMyData(myData);
                    await DisplayAlert("Report", result , "Ok");
                }
            }
            entrPassNew1.Text = null;
            entrPassNew2.Text = null;
            entrPassOld.Text = null;
                
        }

        private async void btnChangeImage_Clicked(object sender, EventArgs e)
        {
            string baseImage = null;
            if (file != null)
            {
                byte[] imageArray = File.ReadAllBytes(file.FullPath);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);
                classSignature classSignature = new classSignature();
                string result = classSignature.UpdateImage(myData.login, myData.password,base64ImageRepresentation);
                baseImage = classSignature.GetImage(myData.id);
                myData.image = baseImage;
                App.myDB.UpdateMyData(myData);
                await DisplayAlert("Report", result+ "\nRestart the application", "Ok");
                imagePicker.Source = "iconImage.png";
                
            }
            else
            {
                await DisplayAlert("Error", "Image not selected...", "Ok");
            }
            

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

        //private void btnChgLogin_Clicked(object sender, EventArgs e)
        //{

        //}
    }
}