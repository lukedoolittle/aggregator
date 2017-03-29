using System;

namespace Material.Domain.Requests
{
    public partial class MicrosoftBingSpeechToText
    {
        public static Guid DeviceId { get; set; }

        public MicrosoftBingSpeechToText()
        {
            Requestid = Guid.NewGuid();
            Instanceid = DeviceId;
        }
    }
}