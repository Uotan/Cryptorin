using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Cryptorin.Classes.SQLiteClasses;
using Cryptorin.Classes;


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


        public void DeleteMessages()
        {
            db.DeleteAll<Message>();
        }

        public void DeleteMessagesWithUser(int _userId)
        {
            MyData myData = ReadMyData();
            List<Message> messages = db.Table<Message>().Where(x => (x.for_whom == _userId && x.from_whom == myData.id) || (x.for_whom == myData.id && x.from_whom == _userId)).ToList();
            foreach (var item in messages)
            {
                db.Delete<Message>(item);
            }
        }


        public void WriteMyData(int _id,string _publicName,string _privateKey,string _login, string _password, string _keyNumber,string _image)
        {
            MyData myData = new MyData();
            myData.id = _id;
            myData.public_name = _publicName;
            myData.private_key = _privateKey;
            myData.login = _login;
            myData.password = _password;
            myData.key_number = _keyNumber;
            myData.image = _image;
            db.Insert(myData);
        }

        public void AddUser(int _id, string _publicName, string _publicKey, string _keyNumber, string _image, string _hexColor)
        {
            User newUserData = new User();
            newUserData.id = _id;
            newUserData.public_name = _publicName;
            newUserData.public_key = _publicKey;
            newUserData.key_number = _keyNumber;
            newUserData.image = _image;
            newUserData.hex_color = _hexColor;
            db.Insert(newUserData);
        }

        public void AddMessage(fetchedMessage _message, string _content)
        {
            Message newMessage = new Message();
            //newMessage.id = _message.id;
            newMessage.from_whom = _message.from_whom;
            newMessage.for_whom = _message.for_whom;
            newMessage.content = _content;
            newMessage.datetime = _message.datetime;
            db.Insert(newMessage);
        }

        public void AddMessageCompleted(Message _message)
        {
            db.Insert(_message);
        }

        public List<Message> GetMessages(int _userIDfirst, int _userIDsecond)
        {
            int count = db.Table<Message>().Where(x => (x.for_whom == _userIDfirst && x.from_whom == _userIDsecond) || (x.for_whom == _userIDsecond && x.from_whom == _userIDfirst)).Count();
            //int count = 3;
            if (count>0)
            {
                List<Message> messages = db.Table<Message>().Where(x => (x.for_whom == _userIDfirst && x.from_whom == _userIDsecond) || (x.for_whom == _userIDsecond && x.from_whom == _userIDfirst)).ToList();
                //List<Message> messages = db.Table<Message>().Where(x => x.for_whom == _userIDfirst && x.from_whom == _userIDsecond).ToList();


                return messages;
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

        public void UpdateUserData(User _data)
        {
            db.Update(_data);
        }
    }
}
