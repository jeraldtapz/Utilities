using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.SaveSystem
{
    /// <inheritdoc />
    /// Saves data to binary format
    public class BinarySaveHandler : BaseSaveHandler
    {
        #region Constructor

        public BinarySaveHandler(string directory)
        {
            SetSaveDirectory(directory);
        }

        #endregion Constructor

        #region ISaveHandler Methods

        public override bool Save(SaveData obj, string overrideName = "savefile", string extension = ".bin")
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (string.IsNullOrEmpty(overrideName))
                throw new NullReferenceException();

            if (string.IsNullOrEmpty(extension))
                throw new NullReferenceException();

            string saveName = string.IsNullOrEmpty(overrideName) ? obj.BaseName : overrideName;
            string savePath = $"{saveDirectory}{Path.DirectorySeparatorChar}{saveName}.{extension}";

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = null;

            try
            {
                fileStream = File.Create(savePath);
                binaryFormatter.Serialize(fileStream, obj);

                Log($"Saved {savePath} successfully! {DateTime.Now}");
                return true;
            }
            catch (Exception e)
            {
                Log($"An error occured while saving : {e.Message}");
                return false;
            }
            finally
            {
                fileStream?.Close();
            }
        }

        #endregion ISaveHandler Methods
    }
}