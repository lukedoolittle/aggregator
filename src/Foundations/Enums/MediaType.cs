﻿using Foundations.Attributes;

namespace Foundations.Enums
{
    public enum MediaType
    {
        [Description("")]
        Undefined,
        [Description("application/json")]
        Json,
        [Description("application/xml")]
        Xml,
        [Description("text/json")]
        TextJson,
        [Description("text/x-json")]
        TextXJson,
        [Description("text/xml")]
        TextXml,
        [Description("text/plain")]
        Text,
        [Description("text/html")]
        Html,
        [Description("text/javascript")]
        Javascript,
        [Description("application/x-www-form-urlencoded")]
        Form,
        [Description("application/jwt")]
        JsonWebToken,
        [Description("audio/wav")]
        Wave
    }
}
