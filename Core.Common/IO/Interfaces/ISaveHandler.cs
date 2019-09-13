using Core.Common;

namespace Core.SaveSystem
{
    /// <summary>
    /// Handles saving of data
    /// </summary>
    public interface ISaveHandler : ILogger
    {
        bool SetSaveDirectory(string directory);

        bool Save(SaveData obj, string overrideName = "savefile", string extension = "bin");
    }
}