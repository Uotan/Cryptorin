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

        public async void WriteMyData(int _id,string _publicName,string _aesKey,string _privateKey,string _login, string _password, string _keyNumber,string _image)
        {
            MyData myData = new MyData();
            myData.id = _id;
            myData.public_name = _publicName;
            myData.aes_key = _aesKey;
            myData.private_key = _privateKey;
            myData.login = _login;
            myData.password = _password;
            myData.key_number = _keyNumber;
            myData.image = _image;
            await SaveMyDataAsync(myData);
        }

        public async Task SaveUserDataAsync(User newUserData)
        {
            await db.InsertAsync(newUserData);

        }


        public async void AddUser(int _id, string _publicName, string _publicKey, string _keyNumber, string _image, string _hexColor)
        {
            User newUserData = new User();
            newUserData.id = _id;
            newUserData.public_name = _publicName;
            newUserData.public_key = _publicKey;
            newUserData.key_number = _keyNumber;
            newUserData.image = _image;
            newUserData.hex_color = _hexColor;
            await SaveUserDataAsync(newUserData);
        }


        public async Task<List<User>> getUserList(int _id)
        {
            var queryString = db.Table<User>().Where(s => s.id==_id);
            var result = await queryString.ToListAsync();
            return result;

        }


        public Task<MyData> ReadMyData()
        {
            return db.Table<MyData>().FirstAsync();
        }



    }
}
