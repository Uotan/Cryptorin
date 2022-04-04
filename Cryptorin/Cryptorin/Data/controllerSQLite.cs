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

    }
}
