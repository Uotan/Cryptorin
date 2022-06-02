using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptorin.Classes.SQLiteClasses
{
    public class Message
    {
        [PrimaryKey, AutoIncrement]
        public int IDlocal { get; set; }
        [Indexed]
        public int from_whom { get; set; }
        [Indexed]
        public int for_whom { get; set; }
        public string content { get; set; }
        public string datetime { get; set; }
        //точно не уверен как тут внешние ключи работают
    }
}
