using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Cryptorin.Classes;
using System.Diagnostics;

namespace Cryptorin.Data
{
    public class classSignature
    {
        /// <summary>
        /// Checking for the existence of a login
        /// </summary>
        /// <param name="_login"></param>
        /// <returns></returns>
        public string CheckLoginExists(string _login)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("login", _login);
            try
            {
                var response = client.UploadValues(ServerAddress.srvrAddress + "/API/checkLoginCoincidence.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                result = result.Trim();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }





        /// <summary>
        /// Registration method
        /// </summary>
        /// <param name="_publicName"></param>
        /// <param name="_login"></param>
        /// <param name="_pass"></param>
        /// <param name="_imageBade64"></param>
        /// <returns></returns>
        public string SignUp(string _publicName, string _login, string _pass, string _imageBade64)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("publicName", _publicName);
            param.Add("login", _login);
            param.Add("password", _pass);
            param.Add("image", _imageBade64);
            try
            {
                var response = client.UploadValues(ServerAddress.srvrAddress + "/API/signup.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                result = result.Trim();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }






        /// <summary>
        /// Sign In method
        /// </summary>
        /// <param name="_login"></param>
        /// <param name="_pass"></param>
        /// <returns></returns>
        public fetchedUser SignIn(string _login, string _pass, string _publicKey)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("login", _login);
            param.Add("password", _pass);
            param.Add("publicKey", _publicKey);
            try
            {
                var response = client.UploadValues(ServerAddress.srvrAddress + "/API/signin.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                Debug.WriteLine(result);
                fetchedUser userData = JsonConvert.DeserializeObject<fetchedUser>(result);
                return userData;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Update RSA public key in a Data Base
        /// </summary>
        /// <param name="_login"></param>
        /// <param name="_pass"></param>
        /// <param name="_publicKey"></param>
        /// <returns>return the current key number in the database</returns>
        public string UpdateKeys(string _login, string _pass, string _publicKey)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("login", _login);
            param.Add("password", _pass);
            param.Add("publicKey", _publicKey);
            try
            {
                var response = client.UploadValues(ServerAddress.srvrAddress + "/API/updatePublicKey.php", "POST", param);
                string stringResult = Encoding.Default.GetString(response);

                stringResult = stringResult.Trim();
                return stringResult;

            }
            catch (Exception)
            {
                //according to the logic of the work, the key number cannot be zero
                //therefore, in case of an error, the function will return 0
                return "error";
                //return 0;
            }
        }

        /// <summary>
        /// Get the user's image in Base64 format
        /// </summary>
        /// <param name="_id">user ID</param>
        /// <returns>Base64 (image)</returns>
        public string GetImage(int _id)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("id", _id.ToString());
            try
            {
                var response = client.UploadValues(ServerAddress.srvrAddress + "/API/getbase64.php", "POST", param);
                string resultBase64 = Encoding.Default.GetString(response);
                resultBase64 = resultBase64.Trim();
                return resultBase64;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Get the index of the user's public key
        /// </summary>
        /// <param name="_id">ID пользователя</param>
        /// <returns>INT value</returns>
        public string GetUserKeyNumber(int _id)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("id", _id.ToString());
            try
            {
                var response = client.UploadValues(ServerAddress.srvrAddress + "/API/getKeyNumber.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                result = result.Trim();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Get the user's public key
        /// </summary>
        /// <param name="_id"></param>
        /// <returns>Base64 public key</returns>
        public string GetUserPublicKey(int _id)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("id", _id.ToString());
            try
            {
                var response = client.UploadValues(ServerAddress.srvrAddress + "/API/getUserPublicKey.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }




        /// <summary>
        /// Get public user data
        /// </summary>
        /// <param name="_id">User ID</param>
        /// <returns>Returns all available public user data</returns>
        public fetchedUser fetchUserData(int _id)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("id", _id.ToString());
            try
            {
                var response = client.UploadValues(ServerAddress.srvrAddress + "/API/findUser.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                fetchedUser userData = JsonConvert.DeserializeObject<fetchedUser>(result);
                return userData;
            }
            catch (Exception)
            {
                return null;
            }
        }



        /// <summary>
        /// Update your profile picture
        /// </summary>
        /// <param name="_login">Your login</param>
        /// <param name="_password">Your password</param>
        /// <param name="_imageBase64">Your new image (base64)</param>
        /// <returns>returns "Updated" or "error"</returns>
        public string UpdateImage(string _login, string _password, string _imageBase64)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("login", _login);
            param.Add("password", _password);
            param.Add("image", _imageBase64);
            try
            {
                var response = client.UploadValues(ServerAddress.srvrAddress + "/API/updateImage.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                result = result.Trim();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Password update method
        /// </summary>
        /// <param name="_login"></param>
        /// <param name="_password"></param>
        /// <param name="_newPassword"></param>
        /// <returns>returns "Updated" or "error"</returns>
        public string UpdatePassword(string _login, string _password, string _newPassword)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("login", _login);
            param.Add("password", _password);
            param.Add("newPassword", _newPassword);
            try
            {
                var response = client.UploadValues(ServerAddress.srvrAddress + "/API/updatePassword.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                result = result.Trim();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Update Public username
        /// </summary>
        /// <param name="_login"></param>
        /// <param name="_password"></param>
        /// <param name="_newPublicName"></param>
        /// <returns>returns "Updated" or "error"</returns>
        public string UpdatePublicName(string _login, string _password, string _newPublicName)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("login", _login);
            param.Add("password", _password);
            param.Add("newPublicName", _newPublicName);
            try
            {
                var response = client.UploadValues(ServerAddress.srvrAddress + "/API/updatePublicName.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                result = result.Trim();
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }


        /// <summary>
        /// Get the index of user profile changes
        /// </summary>
        /// <param name="_id"></param>
        /// <returns>INT value</returns>
        public string GetUserChangeIndex(int _id)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("id", _id.ToString());
            try
            {
                var response = client.UploadValues(ServerAddress.srvrAddress + "/API/getUserChangeIndex.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                return result;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
