namespace Core.Common
{
    /// <summary>
    /// Handles loading of saved data
    /// </summary>
    public interface ILoadHandler
    {
        bool SetLoadDirectory(string directory);

        T Load<T>(string fileName, string extension) where T : IDefault, new();
    }
}