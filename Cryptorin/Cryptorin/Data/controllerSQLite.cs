using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Cryptorin.Classes.SQLiteClasses;

namespace Cryptorin.Data
{
    public class controllerSQLite
    {
        readonly SQLiteAsyncConnection db;
        public controllerSQLite(string connectionString)
        {
            db = new SQLiteAsyncConnection(connectionString);
            db.CreateTableAsync<MyData>().Wait();
            db.CreateTableAsync<User>().Wait();
            db.CreateTableAsync<Message>().Wait();
            
        }

        public  void SaveMyDataAsync(MyData data)
        {
            db.InsertAsync(data);
   
        }


        public Task<MyData> GetMyData()
        {
            return db.Table<MyData>().FirstAsync();

        }



        public void DeleteAllData()
        {
            db.DeleteAllAsync<MyData>();
            db.DeleteAllAsync<Message>();
            db.DeleteAllAsync<User>();
        }

        public void WriteMyData(int _id,string _publicName,string _aesKey,string _privateKey,string _login, string _password, string _hexColor, int _keyNumber, string _image)
        {
            MyData myData = new MyData();
            myData.id = _id;
            myData.public_name = _publicName;
            myData.aes_key = _aesKey;
            myData.private_key = _privateKey;
            myData.login = _login;
            myData.password = _password;
            myData.hex_color = _hexColor;
            myData.key_number = _keyNumber;
            myData.image = _image;
            SaveMyDataAsync(myData);
        }
    }
}
