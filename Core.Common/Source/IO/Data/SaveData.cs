using Newtonsoft.Json;
using System;

namespace Core.Common
{
    [Serializable]
    public class SaveData : IDefault
    {
        #region Public Fields

        [JsonProperty] public string BaseName;
        [JsonProperty] public string Extension;
        [JsonProperty] public int Version;

        #endregion Public Fields

        #region Constructors

        public SaveData()
        {
            BaseName = "Unknown Save File";
            Extension = ".savefile";
            Version = 0;
        }

        public SaveData(string baseName) : this()
        {
            BaseName = baseName;
        }

        public SaveData(string baseName, string extension) : this()
        {
            BaseName = baseName;
            Extension = extension;
        }

        #endregion Constructors

        #region IDefault Methods

        public virtual void Default()
        {
            BaseName = "savefile";
            Version = 0;
            Extension = ".bin";
        }

        #endregion IDefault Methods
    }
}