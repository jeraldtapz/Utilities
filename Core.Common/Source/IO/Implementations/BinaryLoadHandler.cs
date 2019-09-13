using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Core.Common
{
    /// <inheritdoc />
    /// Loads binary data
    public class BinaryLoadHandler : BaseLoadHandler
    {
        #region Constructor

        public BinaryLoadHandler(string loadDirectory)
        {
            this.loadDirectory = loadDirectory;
        }

        #endregion Constructor

        #region ILoadHandler Methods

        public override T Load<T>(string fileName, string extension)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("FileName is either null or empty");

            if (string.IsNullOrEmpty(extension))
                throw new ArgumentException("Extension is either null or empty");

            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = null;

            string loadName = $"{loadDirectory}{Path.DirectorySeparatorChar}{fileName}.{extension}";

            try
            {
                fileStream = File.Open(loadName, FileMode.Open);
                T data = (T)binaryFormatter.Deserialize(fileStream);

                Log($"Successfully loaded {loadName}!");
                return data;
            }
            catch (Exception e)
            {
                Log($"Failed to load file: {e.Message}");
                T data = new T();
                data.Default();

                return data;
            }
            finally
            {
                fileStream?.Close();
            }
        }

        #endregion ILoadHandler Methods
    }
}