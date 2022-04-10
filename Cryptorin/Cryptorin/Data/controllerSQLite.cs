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
        public controllerSQLite(string connectionString)
        {
            dbAsync = new SQLiteAsyncConnection(connectionString);
            db = new SQLiteConnection(connectionString);
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
            return db.Table<MyData>().First();
        }

        public void DeleteAllData()
        {
            db.DeleteAll<MyData>();
            db.DeleteAll<Message>();
            db.DeleteAll<User>();
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
    }
}
