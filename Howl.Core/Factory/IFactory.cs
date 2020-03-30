using System;
using System.Collections.Generic;
using System.Text;

namespace Howl.Core
{
    public interface IFactory
    {

    }

    public interface IFactory<T> : IFactory
    {
        T Create();
    }
}
