namespace Material.Contracts
{
    public interface IBluetoothAuthorizerUIFactory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        IBluetoothAuthorizerUI GetAuthorizer();
    }
}
