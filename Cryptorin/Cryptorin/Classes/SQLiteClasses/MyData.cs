using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Cryptorin.Classes.SQLiteClasses
{
    public class MyData
    {
        public int id { get; set; }
        public string public_name { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string private_key { get; set; }
        public string aes_key { get; set; }
        public int key_number { get; set; }
        public string image { get; set; }
        public string hex_color { get; set; }
        
    }
}
