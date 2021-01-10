using System.Collections;

namespace com.petrushevskiapps.Oxo
{
    public interface IAuth
    {
        IEnumerator Connect(string gameVersion);
        bool IsConnecting();
    }
}