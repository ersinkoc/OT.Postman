using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Postman
{
    public class Constants
    {
        public string FromName { get; set; }
        public string FromMail { get; set; }
        public string EmailPassword { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SourceUrl { get; set; }
        public string Secret { get; set; }
        public string ReturnKey { get; set; }

        

        public Constants(string FromName, string SenderMail, string EmailPassword, string SmtpHost, int SmtpPort, string SourceUrl, string Secret, string ReturnKey)
        {
            this.FromName = FromName;
            this.FromMail = SenderMail;
            this.EmailPassword = EmailPassword;
            this.SmtpHost = SmtpHost;
            this.SmtpPort = SmtpPort;
            this.SourceUrl = SourceUrl;
            this.Secret = Secret;
            this.ReturnKey = ReturnKey;
        }

        public Constants()
        {
            this.FromName = "";
            this.FromMail = "";
            this.EmailPassword = "";
            this.SmtpHost = "";
            this.SmtpPort = 0;
            this.SourceUrl = "";
            this.Secret = "";
            this.ReturnKey = "";
        }

    }

}
