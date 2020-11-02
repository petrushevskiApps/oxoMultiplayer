namespace com.petrushevskiapps.Oxo
{
    public interface INetworkProperties
    {
        INetworkProperties Set(string key, object value);
        void Update();
        T GetProperty<T>(string key);
    }
}