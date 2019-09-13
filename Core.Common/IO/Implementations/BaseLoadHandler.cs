using Core.Common;
using System;
using System.IO;

namespace Core.SaveSystem
{
    public abstract class BaseLoadHandler : ILoadHandler
    {
        #region Protected Fields

        protected string loadDirectory;
        protected readonly Subject<string> logObservable;

        #endregion Protected Fields

        #region ILogger Properties

        public IObservable<string> LogObservable => logObservable;

        #endregion ILogger Properties

        #region Constructors

        protected BaseLoadHandler()
        {
            loadDirectory = string.Empty;
            logObservable = new Subject<string>();
        }

        #endregion Constructors

        #region ILoadHandler Methods

        public virtual bool SetLoadDirectory(string directory)
        {
            loadDirectory = directory;

            if (Directory.Exists(directory))
                return true;
            try
            {
                Directory.CreateDirectory(directory);
            }
            catch (Exception e)
            {
                Log($"An error has occured while setting the load directory: {e.Message}");
                return false;
            }

            Log("Load Directory successfully configured!");
            return true;
        }

        public abstract T Load<T>(string fileName, string extension) where T : IDefault, new();

        #endregion ILoadHandler Methods

        #region Private Methods

        protected virtual void Log(string log)
        {
            logObservable.OnNext(log);
        }

        #endregion Private Methods
    }
}