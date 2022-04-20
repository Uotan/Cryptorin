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

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewRegister : ContentPage
    {
        string base64ImageRepresentation = null;
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
            if (file!=null)
            {
                byte[] imageArray = File.ReadAllBytes(file.FullPath);
                base64ImageRepresentation = Convert.ToBase64String(imageArray);
            }

            //string regexpass = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{6,}$";
            //if (Regex.IsMatch(tbPassw.Text, regexpass)&& Regex.IsMatch(tbRepPassw.Text, regexpass)&&tbPassw.Text==tbRepPassw.Text)
            //{
            //    await DisplayAlert("Done", "Registration is completed!", "Ok");
            //    await Navigation.PopAsync();
            //}

            try
            {
                
                if (tbPassw.Text == tbRepPassw.Text && tbPassw.Text!="" && tbPublicName.Text!="")
                {
                    var urlEncodedPublicName = System.Net.WebUtility.UrlEncode(tbPublicName.Text);
                    classSignature signInstance = new classSignature();
                    if (signInstance.CheckLoginExists(tbLogin.Text) == "ok")
                    {
                        //classSHA256 classSHA256instance = new classSHA256();
                        //string hashPassword = classSHA256instance.ComputeSha256Hash(tbPassw.Text);
                        Argon argon = new Argon();
                        string hashPasswordHex = argon.Argon2id(tbPassw.Text);
                        string result = signInstance.SignUp(urlEncodedPublicName, tbLogin.Text, hashPasswordHex, base64ImageRepresentation);
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
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",ex.Message,"Ok");
            }
            
        }
    }
}