using System;
using System.Collections.Generic;

namespace Core.Common
{
    public interface ICompositeInitializable : IInitializable
    {
        List<IInitializable> Initializables { get; }
        List<IDisposable> Disposables { get; }
    }
}