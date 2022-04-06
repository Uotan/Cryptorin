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
        readonly SQLiteAsyncConnection db;
        public controllerSQLite(string connectionString)
        {
            db = new SQLiteAsyncConnection(connectionString);
            db.CreateTableAsync<MyData>().Wait();
            db.CreateTableAsync<User>().Wait();
            db.CreateTableAsync<Message>().Wait();
            
        }

        public async Task SaveMyDataAsync(MyData data)
        {
            await db.InsertAsync(data);
   
        }


        public Task<MyData> GetMyData()
        {
            return db.Table<MyData>().FirstAsync();

        }



        public void DeleteAllData()
        {
            db.DeleteAllAsync<MyData>().Wait();
            db.DeleteAllAsync<Message>().Wait();
            db.DeleteAllAsync<User>().Wait();
        }

        public async void WriteMyData(int _id,string _publicName,string _aesKey,string _privateKey,string _login, string _password, string _keyNumber)
        {
            MyData myData = new MyData();
            myData.id = _id;
            myData.public_name = _publicName;
            myData.aes_key = _aesKey;
            myData.private_key = _privateKey;
            myData.login = _login;
            myData.password = _password;
            myData.key_number = _keyNumber;
            //myData.image = _image;
            await SaveMyDataAsync(myData);
            //staticUserData.myData = await ReadMyData();
        }


        public Task<MyData> ReadMyData()
        {
            return db.Table<MyData>().FirstAsync();
        }



    }
}
