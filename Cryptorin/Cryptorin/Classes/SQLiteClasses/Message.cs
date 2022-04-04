using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace Cryptorin.Classes.SQLiteClasses
{
    public class Message
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public int from_whom  { get; set; }
        public int for_whom  { get; set; }
        public string datetime { get; set; }
        public string content { get; set; }
    }
}
