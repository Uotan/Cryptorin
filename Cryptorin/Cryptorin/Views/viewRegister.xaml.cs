using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Cryptorin.Data;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System.IO;
using System.Net;
using Cryptorin.Classes;
using System.Diagnostics;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRegister : ContentPage
    {
        
        FileResult file;
        public ViewRegister()
        {
            InitializeComponent();
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
                await DisplayAlert("error", ex.Message, "close");
            }
        }







        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            CheckConnection checker = new CheckConnection();
            bool connectionResult = checker.ConnectionAvailable(ServerAddress.srvrAddress);
            if (!connectionResult)
            {
                await DisplayAlert("Error", "The connection to the server is not established", "Ok");
                return;
            }
            string base64ImageRepresentation = null;
            if (file!=null)
            {
                byte[] imageArray = File.ReadAllBytes(file.FullPath);
                base64ImageRepresentation = Convert.ToBase64String(imageArray);
            }

            try
            {
                
                if (tbPassw.Text == tbRepPassw.Text && tbPassw.Text!="" && tbPublicName.Text!=""&&tbLogin.Text!="" && tbPassw.Text != null && tbPublicName.Text != null && tbLogin.Text != null)
                {
                    var urlEncodedPublicName = WebUtility.UrlEncode(tbPublicName.Text);

                    ClassSHA256 classSHA256instance = new ClassSHA256();
                    string hashSaltPassword = classSHA256instance.ComputeSha256Hash(tbPassw.Text);
                    string hashSaltLogin = classSHA256instance.ComputeSha256Hash(tbLogin.Text);

                    Argon argon = new Argon();
                    string hashPasswordHex = argon.Argon2id(tbPassw.Text, hashSaltPassword);
                    string hashLoginHex = argon.Argon2id(tbLogin.Text, hashSaltLogin);


                    ClassSignature signInstance = new ClassSignature();
                    if (signInstance.CheckLoginExists(hashLoginHex) == "ok")
                    {

                        

                        string result = signInstance.SignUp(urlEncodedPublicName, hashLoginHex, hashPasswordHex, base64ImageRepresentation);
                        if (result == "created")
                        {
                            await DisplayAlert("Done", "Registration is completed!", "Ok");
                            await Navigation.PopAsync();
                        }
                        else
                        {
                            await DisplayAlert("Error", "error(((", "Ok");
                        }
                        
                    }
                    else
                    {
                        await DisplayAlert("Error", "This login already exists", "Ok");
                    }

                }
                else
                {
                    await DisplayAlert("Error", "Incorrectly entered data", "Ok");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",ex.Message,"Ok");
            }
            
        }
    }
}