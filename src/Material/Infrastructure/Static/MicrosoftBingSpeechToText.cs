using System;
using Foundations.Enums;

namespace Material.Infrastructure.Requests
{
    public partial class MicrosoftBingSpeechToText
    {
        public static Guid DeviceId { get; set; }

        public MicrosoftBingSpeechToText()
        {
            Requestid = Guid.NewGuid();
            Instanceid = DeviceId;
            BodyType = MediaType.Wave;
        }
    }
}