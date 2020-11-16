namespace com.petrushevskiapps.Oxo
{
    public interface INetworkProperties
    {
        INetworkProperties Set(string key, object value);
        void Sync();
        void Updated(string key);
        T GetProperty<T>(string key);
    }
}