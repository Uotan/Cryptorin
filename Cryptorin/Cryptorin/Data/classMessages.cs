﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using Cryptorin.Classes.SQLiteClasses;
using Newtonsoft.Json;

namespace Cryptorin.Data
{
    public class classMessages
    {
        public int GetCountOfMessages(int _from,int _to, string _login, string _password)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("fromID", _from.ToString());
            param.Add("toID", _to.ToString());
            param.Add("login", _login);
            param.Add("password", _password);
            try
            {
                var response = client.UploadValues("https://cryptorin.ru/API/getCountOfMessages.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                return Convert.ToInt32(result);
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public string SendMessage(int _from, int _to, string _login, string _password, string _content)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("fromID", _from.ToString());
            param.Add("toID", _to.ToString());
            param.Add("login", _login);
            param.Add("password", _password);
            param.Add("content", _content);
            try
            {
                var response = client.UploadValues("https://cryptorin.ru/API/sendMessage.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                result = result.Trim();
                return result;
            }
            catch (Exception)
            {
                return "error";
            }
        }


        public List<Message> GetMessages(int _from, int _to, string _login, string _password, int _count)
        {
            WebClient client = new WebClient();
            NameValueCollection param = new NameValueCollection();
            param.Add("fromID", _from.ToString());
            param.Add("toID", _to.ToString());
            param.Add("login", _login);
            param.Add("password", _password);
            param.Add("count", _count.ToString());
            try
            {
                var response = client.UploadValues("https://cryptorin.ru/API/getMessages.php", "POST", param);
                string result = Encoding.Default.GetString(response);
                List<Message> Data = JsonConvert.DeserializeObject<List<Message>>(result);
                return Data;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}