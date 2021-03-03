using System.Collections;

namespace com.petrushevskiapps.Oxo
{
    public interface IAuth
    {
        IEnumerator Connect();
        bool IsConnecting();
    }
}