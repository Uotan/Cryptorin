using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptorin.Classes.SQLiteClasses
{
    public class Message
    {
        public int id { get; set; }
        public int from_whom { get; set; }
        public int for_whom { get; set; }
        public string rsa_cipher { get; set; }
        public string datetime { get; set; }
    }
}
