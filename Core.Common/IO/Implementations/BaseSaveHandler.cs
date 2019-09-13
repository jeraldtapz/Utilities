using Core.Common;
using System;
using System.IO;

namespace Core.SaveSystem
{
    public abstract class BaseSaveHandler : ISaveHandler
    {
        #region Protected Fields

        protected string saveDirectory;
        protected readonly Subject<string> logObservable;

        #endregion Protected Fields

        #region ILogger Properties

        public IObservable<string> LogObservable => logObservable;

        #endregion ILogger Properties

        #region Constructors

        protected BaseSaveHandler()
        {
            saveDirectory = string.Empty;
            logObservable = new Subject<string>();
        }

        #endregion Constructors

        #region ISaveHandler Methods

        public bool SetSaveDirectory(string directory)
        {
            saveDirectory = directory;

            if (Directory.Exists(directory))
                return true;
            try
            {
                Directory.CreateDirectory(directory);
            }
            catch (Exception e)
            {
                Log($"An error has occured while setting the save directory: {e.Message}");
                return false;
            }

            Log("Save Directory successfully configured!");
            return true;
        }

        public abstract bool Save(SaveData obj, string overrideName = "savefile", string extension = "bin");

        #endregion ISaveHandler Methods

        #region Private Methods

        protected void Log(string log)
        {
            logObservable.OnNext(log);
        }

        #endregion Private Methods
    }
}