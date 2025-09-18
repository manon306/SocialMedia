using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMedia.BLL.Service.Abstraction
{
  
        public interface IEmailService
        {
            Task SendEmailAsync(string toEmail, string subject, string htmlMessage);
        }

}
