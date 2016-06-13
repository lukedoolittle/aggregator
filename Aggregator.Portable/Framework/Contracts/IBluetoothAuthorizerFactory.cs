namespace Aggregator.Framework.Contracts
{
    public interface IBluetoothAuthorizerFactory
    {
        IBluetoothAuthorizer GetAuthorizer(object context);
    }
}
