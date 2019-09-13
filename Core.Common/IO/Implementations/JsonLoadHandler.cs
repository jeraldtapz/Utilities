using Newtonsoft.Json;
using System;
using System.IO;

namespace Core.SaveSystem
{
    public class JsonLoadHandler : BaseLoadHandler
    {
        public JsonLoadHandler(string loadDirectory)
        {
            this.loadDirectory = loadDirectory;
        }

        public override T Load<T>(string fileName, string extension)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("FileName is either null or empty");

            if (string.IsNullOrEmpty(extension))
                throw new ArgumentException("Extension is either null or empty");

            string loadName = $"{loadDirectory}{Path.DirectorySeparatorChar}{fileName}.{extension}";
            FileStream fs = null;
            StreamReader reader = null;
            try
            {
                JsonSerializer serializer = new JsonSerializer();
                fs = File.Open(loadName, FileMode.Open);

                reader = new StreamReader(fs);
                T obj = (T)serializer.Deserialize(reader, typeof(T));

                Log($"Successfully loaded {loadName}");
                return obj;
            }
            catch (Exception e)
            {
                Log($"An exception has occured while loading: {e}. Creating a default instance instead");
                T obj = new T();
                obj.Default();

                return obj;
            }
            finally
            {
                reader?.Close();
                fs?.Close();
            }
        }
    }
}