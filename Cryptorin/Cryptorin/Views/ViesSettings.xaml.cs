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
using System.Diagnostics;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViesSettings : ContentPage
    {
        MyData myData;
        FileResult file = null;
        string passwordHex;
        string loginHex;

        classSignature signature = new classSignature();
        RSAUtil rSAUtil = new RSAUtil();
        classAES aES;

        public ViesSettings()
        {
            InitializeComponent();
            Appearing += ViewUsersList_Appearing;
            setRadioBtnTheme();
            
        }

        private async void ViewUsersList_Appearing(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {

                if (!keyClass.isUnlock)
                {
                    setRadioBtnTheme();
                    btnChangeImage.IsEnabled = false;
                    btnChgPubName.IsEnabled = false;
                    btnCngPassword.IsEnabled = false;
                    btnUpdateKeys.IsEnabled = false;
                    btnSecurityCode.IsEnabled = false;
                }
                else
                {
                    LoadMyData();
                }

            });

        }



        async void LoadMyData()
        {
            try
            {
                myData = App.myDB.ReadMyData();
                entryChgPubName.Placeholder = WebUtility.UrlDecode(myData.public_name);
                if (keyClass.isUnlock)
                {
                    aES = new classAES(keyClass.AESkey);
                    passwordHex = aES.Decrypt(myData.password).Trim();
                    loginHex = aES.Decrypt(myData.login).Trim();
                }
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
                string result = signature.UpdatePublicName(loginHex, passwordHex, urlEncodedPublicName);
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
            string hashSaltOld = classSHA256instance.ComputeSha256Hash(entrPassOld.Text);

            Argon argon = new Argon();
            string oldPassHash = argon.Argon2id(entrPassOld.Text, hashSaltOld);

            if (entrPassNew1.Text!=entrPassNew2.Text||entrPassNew1.Text==""||entrPassOld.Text==""|| oldPassHash != passwordHex)
            {
                await DisplayAlert("Error", "Incorrectly entered data", "Ok");
                
            }
            else
            {
                classSignature classSign = new classSignature();

                string hashSaltNew = classSHA256instance.ComputeSha256Hash(entrPassNew1.Text);
                string newPassHash = argon.Argon2id(entrPassNew1.Text, hashSaltNew);

                string NewPassHexEncrypted = aES.Encrypt(newPassHash).Trim();
                //passwordHex = newPassHash.Trim();

                string result = classSign.UpdatePassword(loginHex, oldPassHash, newPassHash);
                if (result == "Updated")
                {
                    myData.password = NewPassHexEncrypted;
                    App.myDB.UpdateMyData(myData);
                    //passwordHex = newPassHash;
                    LoadMyData();
                    await DisplayAlert("Report", result , "Ok");
                }
                else
                {
                    await DisplayAlert("Report", result, "Ok");
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
                string result = classSignature.UpdateImage(loginHex, passwordHex, base64ImageRepresentation);
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
                else
                {
                    await DisplayAlert("Report", "Error", "Ok");
                }

                file = null;
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
            bool answer = await DisplayAlert("Are you sure?", "This will delete all data!", "Yes", "No");
            if (answer)
            {

                App.myDB.DeleteAllMessages();

                
                List<string> keys = rSAUtil.CreateKeys();

                string newKeyNumb = signature.UpdateKeys(loginHex, passwordHex, keys[1]);

                if (newKeyNumb=="error")
                {
                    await DisplayAlert("Error", "The keys have NOT been updated.", "Ok");
                    return;
                }
                else
                {
                    classAES aES = new classAES(keyClass.AESkey);
                    string symmetricallyEncryptedKey = aES.Encrypt(keys[0]);


                    //myData.private_key = keys[0];
                    myData.private_key = symmetricallyEncryptedKey;
                    myData.key_number = newKeyNumb;
                    App.myDB.UpdateMyData(myData);
                    await DisplayAlert("Result", "The keys have been updated. All messages deleted.", "Ok");
                }

                



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

        private async void btnSecurityCode_Clicked(object sender, EventArgs e)
        {
            bool answer = await DisplayAlert("Are you sure?", "The data will be re-encrypted, it may take time", "Yes", "No");
            if (!answer)
            {
                return;
            }
            if (entryCurrenCode.Text == null || entryNewCode.Text == null || entryNewCodeRepeat.Text == null||entryNewCode.Text!=entryNewCodeRepeat.Text)
            {
                await DisplayAlert("Error", "Data entered incorrectly", "Ok");
                return;
            }

            classSHA256 sHA256 = new classSHA256();
            string hash_secureCodeOld = sHA256.ComputeSha256Hash(entryCurrenCode.Text);
            hash_secureCodeOld = hash_secureCodeOld.Remove(16);

            classAES _aES = new classAES(hash_secureCodeOld);

            string secretCodeFromMemory = Preferences.Get("secretCode", null);

            string decryptedSecreCode = _aES.Decrypt(secretCodeFromMemory);

            if (decryptedSecreCode != null)
            {
                decryptedSecreCode = decryptedSecreCode.Trim();
            }

            if (decryptedSecreCode != hash_secureCodeOld)
            {
                await DisplayAlert("Error", "Data entered incorrectly", "Ok");
                return;
            }

            //проверили, что корректно ввели старый код


            string hash_secureCodeNew = sHA256.ComputeSha256Hash(entryNewCode.Text);
            hash_secureCodeNew = hash_secureCodeNew.Remove(16);

            keyClass.AESkey = hash_secureCodeNew;
            aES.ChangeAESkey(hash_secureCodeNew);


            var EncryptedNewSecurityCode = aES.Encrypt(hash_secureCodeNew);
            Preferences.Set("secretCode", EncryptedNewSecurityCode);

            App.myDB.ReEncryptAllData(hash_secureCodeOld,hash_secureCodeNew);

            LoadMyData();
            await DisplayAlert("Success", "The data was re-encrypted", "Ok");
            entryCurrenCode.Text = null;
            entryNewCode.Text = null;
            entryNewCodeRepeat.Text = null;
        }
    }
}