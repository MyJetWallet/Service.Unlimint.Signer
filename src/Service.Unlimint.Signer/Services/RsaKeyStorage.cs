using System.Collections.Generic;

namespace Service.Unlimint.Signer.Services
{
    public class RsaKeyStorage
    {
        private readonly object _lock = new object();

        private readonly Dictionary<string, RsaKeyEntry> _storage = new Dictionary<string, RsaKeyEntry>();
        public RsaKeyStorage()
        {
        }

        public RsaKeyEntry GetKey(string keyId)
        {
            lock (_lock)
            {
                if (_storage.TryGetValue(keyId, out var value))
                {
                    return value;
                }
            }
            return null;
        }

        public void SetKey(string keyId, RsaKeyEntry rsaKeyEntry)
        {
            lock(_lock)
            {
                _storage[keyId] = rsaKeyEntry;
            }
        }
    }
}
