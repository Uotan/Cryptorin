using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptorin.Classes.SQLiteClasses
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int IDlocal { get; set; }
        public int id { get; set; }
        public string public_name { get; set; }
        public string public_key { get; set; }
        public string key_number { get; set; }
        public string image { get; set; }
        public string hex_color { get; set; }
    }
}
