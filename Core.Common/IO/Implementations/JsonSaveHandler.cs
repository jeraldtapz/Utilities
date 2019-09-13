using Newtonsoft.Json;
using System;
using System.IO;

namespace Core.SaveSystem
{
    public class JsonSaveHandler : BaseSaveHandler
    {
        public JsonSaveHandler(string directory)
        {
            SetSaveDirectory(directory);
        }

        public override bool Save(SaveData obj, string overrideName = "savefile", string extension = "json")
        {
            if (obj == null)
                throw new ArgumentNullException(nameof(obj));

            if (string.IsNullOrEmpty(overrideName))
                throw new NullReferenceException();

            if (string.IsNullOrEmpty(extension))
                throw new NullReferenceException();

            string fileName = string.IsNullOrEmpty(overrideName) ? obj.BaseName : overrideName;
            string savePath = $"{saveDirectory}{Path.DirectorySeparatorChar}{fileName}.{extension}";
            FileStream fs = null;
            StreamWriter writer = null;
            try
            {
                fs = File.Create(savePath);
                writer = new StreamWriter(fs);
                JsonSerializer serializer = new JsonSerializer { Formatting = Formatting.Indented };
                serializer.Serialize(writer, obj);

                Log($"Saved {savePath} successfully! {DateTime.Now}");
                return true;
            }
            catch (Exception e)
            {
                Log($"An error occured while saving {obj.BaseName}.{obj.Extension}: {e.Message}");
                return false;
            }
            finally
            {
                writer?.Close();
                fs?.Close();
            }
        }
    }
}