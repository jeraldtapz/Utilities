using Core.Common;

namespace Core.SaveSystem
{
    /// <summary>
    /// Handles loading of saved data
    /// </summary>
    public interface ILoadHandler : ILogger
    {
        bool SetLoadDirectory(string directory);

        T Load<T>(string fileName, string extension) where T : IDefault, new();
    }
}