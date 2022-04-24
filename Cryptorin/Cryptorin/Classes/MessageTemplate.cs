using System;
using System.Collections.Generic;
using System.Text;

namespace Cryptorin.Classes
{
    public class MessageTemplate
    {
        public int from_whom { get; set; }
        public int for_whom { get; set; }
        public string content { get; set; }
        public string datetime { get; set; }
    }
}
