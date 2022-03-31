using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Cryptorin.Data
{
    public class controllerSQLite
    {
        readonly SQLiteAsyncConnection db;
        public controllerSQLite(string connectionString)
        {
            db = new SQLiteAsyncConnection(connectionString);
            //db.CreateTableAsync<Note>().Wait();
        }
    }
}
