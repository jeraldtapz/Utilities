using System;

namespace Core.Common
{
    public interface IInitializable : IDisposable
    {
        void Initialize(object data = null);
    }
}