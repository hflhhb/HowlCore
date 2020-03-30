using System;
using System.Collections.Generic;
using System.Text;

namespace Howl.Core
{ 
    public class DelegateFactory<T> : IFactory<T>
    {
        public DelegateFactory(Func<T> handler)
        {
            Handler = handler;
        }
        protected Func<T> Handler { get; }

        T IFactory<T>.Create()
        {
            return Handler.Invoke();
        }
    }
}
