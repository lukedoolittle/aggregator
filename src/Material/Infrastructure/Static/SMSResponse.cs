using System.Collections.Generic;

namespace Material.Infrastructure.Requests
{
    public class SMSResponse : List<SMSMessage>
    { }

    public class SMSMessage
    {
        public long Date { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string Creator { get; set; }
        public string Address { get; set; }
        public long DateSent { get; set; }
    }
}
