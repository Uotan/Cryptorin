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
using System.Net;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViesSettings : ContentPage
    {
        MyData myData;
        FileResult file;

        classSignature signature = new classSignature();
        RSAUtil rSAUtil = new RSAUtil();

        public ViesSettings()
        {
            InitializeComponent();
            setRadioBtnTheme();
            LoadMyData();
        }





        async void LoadMyData()
        {
            try
            {
                myData = await App.myDB.ReadMyDataAsync();
                entryChgPubName.Placeholder = WebUtility.UrlDecode(myData.public_name);
                //if (myData.image != null)
                //{
                //    byte[] byteArray = Convert.FromBase64String(myData.image);
                //    ImageSource Source = ImageSource.FromStream(() => new MemoryStream(byteArray));
                //    imagePicker.Source = Source;
                //}
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
                var urlEncodedPublicName = WebUtility.UrlEncode(entryChgPubName.Text);
                classSignature signature = new classSignature();
                string result = signature.UpdatePublicName(myData.login, myData.password, urlEncodedPublicName);
                if (result== "Updated")
                {
                    myData.public_name = urlEncodedPublicName;

                    int _indexChanges = Convert.ToInt32(myData.changes_index);
                    _indexChanges++;
                    myData.changes_index = _indexChanges.ToString();

                    App.myDB.UpdateMyData(myData);
                    entryChgPubName.Placeholder = WebUtility.UrlDecode(myData.public_name);
                    entryChgPubName.Text = "";
                    await DisplayAlert("Report", result + "\nRestart the application", "Ok");
                }
                else
                {
                    await DisplayAlert("Error", result, "Ok");
                }
                
            }
            else
            {
                await DisplayAlert("Error", "Incorrectly entered data", "Ok");
            }
            
        }





        private async void btnCngPassword_Clicked(object sender, EventArgs e)
        {

            classSHA256 classSHA256instance = new classSHA256();
            string hashSaltOld = classSHA256instance.ComputeSha256Hash(myData.login + entrPassOld.Text);

            Argon argon = new Argon();
            string oldPassHash = argon.Argon2id(entrPassOld.Text, hashSaltOld);

            if (entrPassNew1.Text!=entrPassNew2.Text||entrPassNew1.Text==""||entrPassOld.Text==""|| oldPassHash != myData.password)
            {
                await DisplayAlert("Error", "Incorrectly entered data", "Ok");
                
            }
            else
            {
                classSignature classSign = new classSignature();

                string hashSaltNew = classSHA256instance.ComputeSha256Hash(myData.login + entrPassNew1.Text);
                string newPassHash = argon.Argon2id(entrPassNew1.Text, hashSaltNew);


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
                if (result== "Updated")
                {
                    baseImage = classSignature.GetImage(myData.id);
                    myData.image = baseImage;

                    int _indexChanges = Convert.ToInt32(myData.changes_index);
                    _indexChanges++;
                    myData.changes_index = _indexChanges.ToString();

                    App.myDB.UpdateMyData(myData);
                    await DisplayAlert("Report", result + "\nRestart the application", "Ok");
                    imagePicker.Source = "iconImage.png";
                }
                
                
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
            if (answer)
            {

                App.myDB.DeleteAllMessages();

                
                List<string> keys = rSAUtil.CreateKeys();

                
                string newKeyNumb = signature.UpdateKeys(myData.login, myData.password, keys[1]);

                classAES aES = new classAES(keyClass.AESkey);
                string symmetricallyEncryptedKey = aES.Encrypt(keys[0]);


                //myData.private_key = keys[0];
                myData.private_key = symmetricallyEncryptedKey;
                myData.key_number = newKeyNumb;
                App.myDB.UpdateMyData(myData);
                await DisplayAlert("Result", "The keys have been updated. All messages deleted.", "Ok");



            }
        }

        void setRadioBtnTheme()
        {
            string theme = Preferences.Get("theme", null);
            switch (theme)
            {
                case "light": radioBtnLight.IsChecked = true; break; 
                case "dark": radioBtnDark.IsChecked = true; break; 
                case "system": radioBtnSystem.IsChecked = true; break; 
                default:break;
            }
        }

        private void radioBtnSystem_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            string theme = "system";
            Preferences.Set("theme", theme);
            ThemeManager themeManager = new ThemeManager();
            OSAppTheme currentTheme = Application.Current.RequestedTheme;
            if (currentTheme == OSAppTheme.Dark)
            {
                themeManager.SetDark();
            }
            if (currentTheme == OSAppTheme.Light)
            {
                themeManager.SetLight();
            }
        }

        private void radioBtnDark_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            string theme = "dark";
            Preferences.Set("theme", theme);
            ThemeManager themeManager = new ThemeManager();
            themeManager.SetDark();
        }

        private void radioBtnLight_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            string theme = "light";
            Preferences.Set("theme", theme);
            ThemeManager themeManager = new ThemeManager();
            themeManager.SetLight();
        }
    }
}