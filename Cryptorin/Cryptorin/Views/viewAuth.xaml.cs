﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Cryptorin.Data;
using Cryptorin.Classes;
using PasswordGenerator;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Cryptorin.Classes.SQLiteClasses;
using System.Diagnostics;
using Xamarin.Essentials;
using System.Drawing;

namespace Cryptorin.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewAuth : ContentPage
    {



        public ViewAuth()
        {
            InitializeComponent();
            checkConnection2server();


            //tbLogin.Completed += (s, e) =>
            //{
            //    tbPassword.Focus();
            //};

        }






        async void checkConnection2server()
        {
            checkConnection checker = new checkConnection();
            bool result = checker.ConnectionAvailable(ServerAddress.srvrAddress);
            if (!result)
            {
                await DisplayAlert("Error", "The connection to the server is not established", "Ok");
            }
        }



        async Task<bool> checkConnet()
        {
            await Task.Delay(50);
            checkConnection checker = new checkConnection();
            bool connectionResult = checker.ConnectionAvailable(ServerAddress.srvrAddress);
            if (!connectionResult)
            {
                //btnSignIn.IsEnabled = true;
                //btnSignUp.IsEnabled = true;
                //btnSignIn.Text = "Sign In";

                await DisplayAlert("Error", "The connection to the server is not established", "Ok");
                
            }

            return connectionResult;
        }



        private async void btnSignIn_Clicked(object sender, EventArgs e)
        {
            if (tbLogin.Text == null || tbPassword.Text == null|| tbLogin.Text==""|| tbPassword.Text == "")
            {
                await DisplayAlert("Oh, I'm sorry!", "No login or password entered", "ok");
            }
            else
            {
                btnSignIn.Text = "Please Wait";
                btnSignIn.IsEnabled = false;
                btnSignUp.IsEnabled = false;

                var connectResult = await checkConnet();

                if (connectResult)
                {
                    var writeResult = await AuthMethod();
                    if (writeResult)
                    {
                        App.Current.MainPage = new AppShell();
                    }
                    else
                    {
                        await DisplayAlert("Oh, I'm sorry!", "Something's wrong", "ok");
                        btnSignIn.IsEnabled = true;
                        btnSignUp.IsEnabled = true;
                        btnSignIn.Text = "Sign In";
                    }
                }
                else
                {
                    btnSignIn.IsEnabled = true;
                    btnSignUp.IsEnabled = true;
                    btnSignIn.Text = "Sign In";
                    return;
                }



            }
        }

        async Task<bool> AuthMethod()
        {
            await Task.Delay(50);
            classSHA256 classSHA256instance = new classSHA256();
            string hashSalt = classSHA256instance.ComputeSha256Hash(tbLogin.Text + tbPassword.Text);

            Argon argon = new Argon();
            string hashPasswordHex = argon.Argon2id(tbPassword.Text, hashSalt);

            classSignature classSign = new classSignature();


            RSAUtil rSAUtil = new RSAUtil();
            List<string> keys = rSAUtil.CreateKeys();


            fetchedUser fetchedData = classSign.SignIn(tbLogin.Text, hashPasswordHex, keys[1]);

            if (fetchedData != null)
            {
                string code = await DisplayPromptAsync("Come up with a security code", "Enter code:", keyboard: Keyboard.Email);
                
                if (code==null||code=="")
                {
                    return false;
                }
                code = code.Trim();
                classSHA256 sHA256 = new classSHA256();
                string hash_secureCode = sHA256.ComputeSha256Hash(code);
                hash_secureCode = hash_secureCode.Remove(16);
                Debug.WriteLine(hash_secureCode);
                keyClass.AESkey = hash_secureCode;
                classAES aES = new classAES(hash_secureCode);


                var EncryptedSecurityCode = aES.Encrypt(hash_secureCode);
                var EncryptedPassword = aES.Encrypt(hashPasswordHex);


                Debug.WriteLine(EncryptedSecurityCode);

                Preferences.Set("secretCode", EncryptedSecurityCode);

                string symmetricallyEncryptedKey = aES.Encrypt(keys[0]);
                Debug.WriteLine("key encrypted: " + symmetricallyEncryptedKey);
                WriteLocalData(fetchedData, tbLogin.Text, EncryptedPassword, symmetricallyEncryptedKey);
                Debug.WriteLine("My data WRITED");
                return true;
                
            }
            else
            {
                await DisplayAlert("Oh, I'm sorry!", "Authorization error.", "ok");
                return false;
            }
        }





        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            tbLogin.Text = null;
            tbPassword.Text = null;
            await Navigation.PushAsync(new ViewRegister());
        }






        void WriteLocalData(fetchedUser _fetcheData, string _login, string _password, string _privateKey)
        {
            App.myDB.DeleteAllData();

            classSignature signInstance = new classSignature();

            string imageBase64 = signInstance.GetImage(_fetcheData.id);

            App.myDB.WriteMyData(_fetcheData.id, _fetcheData.public_name, _privateKey, _login, _password, _fetcheData.key_number, imageBase64, _fetcheData.changes_index);
        }

        private async void toolItmChangeDomain_Clicked(object sender, EventArgs e)
        {
            string result = await DisplayPromptAsync("Change domain", "Enter new domain (default: https://cryptorin.ru):", keyboard: Keyboard.Email);
            if (result == null || result == "")
            {
                return;
            }
            Preferences.Set("serverAddress", result);
            ServerAddress.srvrAddress = Preferences.Get("serverAddress", null);
        }
    }
}