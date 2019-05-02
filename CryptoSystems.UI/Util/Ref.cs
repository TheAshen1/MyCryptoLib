using System;

namespace CryptoSystems.UI.Util
{
    public class Ref<T>
    {
        private readonly Func<T> _getter;
        private readonly Action<T> _setter;
        public Ref(Func<T> getter, Action<T> setter)
        {
            this._getter = getter;
            this._setter = setter;
        }
        public T Value { get { return _getter(); } set { _setter(value); } }
    }
}
