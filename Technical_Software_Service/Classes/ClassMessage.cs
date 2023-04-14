using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace Technical_Software_Service
{
    class ClassMessage
    {
        public MimeMessage message { get; set; }
        public int id { get; set; }
        public string subject { get; set; }
        public string from { get; set; }
        
    }
}
