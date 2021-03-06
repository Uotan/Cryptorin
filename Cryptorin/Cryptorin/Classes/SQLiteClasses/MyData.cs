using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptorin.Classes.SQLiteClasses
{
    public class MyData
    {
        [PrimaryKey, AutoIncrement]
        public int IDlocal { get; set; }
        public int id { get; set; }
        public string public_name { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string private_key { get; set; }
        public string key_number { get; set; }
        public string changes_index { get; set; }
        public string image { get; set; }
        
    }
}
