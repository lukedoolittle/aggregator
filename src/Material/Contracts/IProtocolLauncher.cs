using System;

namespace Material.Contracts
{
    public interface IProtocolLauncher
    {
        Action<Uri> ProtocolLaunch { get; set; }
    }
}
