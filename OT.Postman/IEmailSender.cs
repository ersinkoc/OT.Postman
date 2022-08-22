using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OT.Postman
{
    public interface IEmailSender
    {
        public Task SendEmailAsync(Constants emailProvider, string recipientName, string recipientEmail, string subject, string message);
    }
}
