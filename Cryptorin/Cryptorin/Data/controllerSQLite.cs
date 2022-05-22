using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Cryptorin.Classes.SQLiteClasses;
using Cryptorin.Classes;
using System.Collections.ObjectModel;
using System.Net;

namespace Cryptorin.Data
{
    public class controllerSQLite
    {
        readonly SQLiteAsyncConnection dbAsync;
        readonly SQLiteConnection db;
        public controllerSQLite(string _connectionString)
        {
            dbAsync = new SQLiteAsyncConnection(_connectionString);
            db = new SQLiteConnection(_connectionString);
            dbAsync.CreateTableAsync<MyData>().Wait();
            dbAsync.CreateTableAsync<User>().Wait();
            dbAsync.CreateTableAsync<Message>().Wait();
        }
        public Task<MyData> ReadMyDataAsync()
        {
            return dbAsync.Table<MyData>().FirstAsync();
        }

        public MyData ReadMyData()
        {
            try
            {
                return db.Table<MyData>().First();
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public void DeleteAllData()
        {
            db.DeleteAll<MyData>();
            db.DeleteAll<Message>();
            db.DeleteAll<User>();
        }


        public void DeleteAllMessages()
        {
            db.DeleteAll<Message>();
        }

        public void DeleteMessagesWithUser(int _userId)
        {
            MyData myData = ReadMyData();
            List<Message> messages = db.Table<Message>().Where(x => (x.for_whom == _userId && x.from_whom == myData.id) || (x.for_whom == myData.id && x.from_whom == _userId)).ToList();
            foreach (var item in messages)
            {
                db.Delete<Message>(item.IDlocal);
            }
        }


        public void WriteMyData(int _id,string _publicName,string _privateKey,string _login, string _password, string _keyNumber,string _image, string _changes_index)
        {
            MyData myData = new MyData();
            myData.id = _id;
            myData.public_name = _publicName;
            myData.private_key = _privateKey;
            myData.login = _login;
            myData.password = _password;
            myData.key_number = _keyNumber;
            myData.changes_index = _changes_index;
            myData.image = _image;
            db.Insert(myData);
        }

        public void AddUser(int _id, string _publicName, string _publicKey, string _keyNumber, string _image, string _hexColor, string _changes_index)
        {
            User newUserData = new User();
            newUserData.id = _id;
            newUserData.public_name = _publicName;
            newUserData.public_key = _publicKey;
            newUserData.key_number = _keyNumber;
            newUserData.image = _image;
            newUserData.hex_color = _hexColor;
            newUserData.changes_index = _changes_index;
            db.Insert(newUserData);
        }

        public void AddMessage(Message _message)
        {

            db.Insert(_message);
        }

        public void AddMessageCompleted(Message _message)
        {
            db.Insert(_message);
        }

        public ObservableCollection<classMessageTemplate> GetMessages(int _userIDfirst, int _userIDsecond)
        {
            classAES aES = new classAES(keyClass.AESkey);
            
            int count = db.Table<Message>().Where(x => (x.for_whom == _userIDfirst && x.from_whom == _userIDsecond) || (x.for_whom == _userIDsecond && x.from_whom == _userIDfirst)).Count();

            if (count>0)
            {
                List<Message> messages = db.Table<Message>().Where(x => (x.for_whom == _userIDfirst && x.from_whom == _userIDsecond) || (x.for_whom == _userIDsecond && x.from_whom == _userIDfirst)).ToList();
                //List<Message> messages = db.Table<Message>().Where(x => x.for_whom == _userIDfirst && x.from_whom == _userIDsecond).ToList();

                ObservableCollection<classMessageTemplate> messages2return = new ObservableCollection<classMessageTemplate>();

                foreach (var item in messages)
                {
                    classMessageTemplate template = new classMessageTemplate();
                    template.from_whom = item.from_whom.ToString();
                    var decryptedCipher = WebUtility.UrlDecode(aES.Decrypt(item.content));
                    template.content = decryptedCipher.Trim();
                    template.datetime = item.datetime;
                    messages2return.Add(template);
                }

                return messages2return;

            }
            else
            {
                return null;
            }
            
        }

        public int GetCountOfMessagesLocal(int _userIDfirst, int _userIDsecond)
        {
            int count = db.Table<Message>().Where(x => (x.for_whom == _userIDfirst && x.from_whom == _userIDsecond) || (x.for_whom == _userIDsecond && x.from_whom == _userIDfirst)).Count();
            return count;
        }

        public int GetCountOfMessagesWithUserLocal(int _userID)
        {
            int count = db.Table<Message>().Where(x => x.from_whom == _userID).Count();
            return count;
        }


        public User GetUser(int _id)
        {
            User user = db.Table<User>().Where(x => x.id == _id).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
            }
        }

        public List<User> GetUsers()
        {
            List<User> users = db.Table<User>().ToList();
            if (users != null)
            {
                return users;
            }
            else
            {
                return null;
            }
        }
        public void DeleteUser(int _id)
        {
            db.Table<User>().Delete(x => x.id == _id);
        }





        public void UpdateMyData(MyData _data)
        {
            db.Update(_data);
        }


        public void ReEncryptAllData(string _oldPassword, string _newPassword)
        {
            classAES AES_old = new classAES(_oldPassword);
            classAES AES_new = new classAES(_newPassword);

            var AllMessages = db.Table<Message>().ToList();
            MyData myData = ReadMyData();

            string decryptedPrivateKey = AES_old.Decrypt(myData.private_key).Trim();
            string decryptedPassword = AES_old.Decrypt(myData.password).Trim();
            string decryptedLogin = AES_old.Decrypt(myData.login).Trim();

            myData.login = AES_new.Encrypt(decryptedLogin);
            myData.password = AES_new.Encrypt(decryptedPassword);
            myData.private_key = AES_new.Encrypt(decryptedPrivateKey);
            App.myDB.UpdateMyData(myData);

            for (int i = 0; i < AllMessages.Count; i++)
            {
                var DecryptedText = WebUtility.UrlDecode(AES_old.Decrypt(AllMessages[i].content).Trim());
                AllMessages[i].content = AES_new.Encrypt(DecryptedText);
            }

            foreach (var item in AllMessages)
            {
                db.Update(item);
            }
        }

        public void UpdateUserData(User _data)
        {
            db.Update(_data);
        }
    }
}
