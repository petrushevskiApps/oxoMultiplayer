namespace com.petrushevskiapps.Oxo
{
    public interface INetworkProperties
    {
        INetworkProperties Set(string key, object value);
        void Sync();
        void Updated();
        T GetProperty<T>(string key);
    }
}