using System;
using System.Collections.Generic;

namespace Core.Common
{
    public class Subject<T> : IObservable<T>, IObserver<T>, IDisposable
    {
        private readonly object observerLock = new object();
        public List<IObserver<T>> observers;
        private bool isDisposed;
        private bool isStopped;

        public Subject()
        {
            observers = new List<IObserver<T>>();
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (observer == null)
                throw new ArgumentNullException(nameof(observer));

            lock (observerLock)
            {
                ThrowIfDisposed();

                if (isStopped) throw new Exception("This subject has had an error or has been completed before!");

                if (!observers.Contains(observer))
                {
                    observers.Add(observer);
                }
            }

            return new Subscription(this, observer);
        }

        public void OnNext(T value)
        {
            lock (observerLock)
            {
                ThrowIfDisposed();
                if (isStopped) return;

                foreach (IObserver<T> observer in observers)
                {
                    observer.OnNext(value);
                }
            }
        }

        public void OnError(Exception error)
        {
            if (error == null)
                throw new ArgumentNullException(nameof(error));

            lock (observerLock)
            {
                ThrowIfDisposed();

                if (isStopped)
                    return;

                foreach (IObserver<T> observer in observers)
                {
                    observer.OnError(error);
                }

                isStopped = true;
            }
        }

        public void OnCompleted()
        {
            lock (observerLock)
            {
                ThrowIfDisposed();

                if (isStopped)
                    return;

                foreach (IObserver<T> observer in observers)
                {
                    observer.OnCompleted();
                }
                isStopped = true;
            }
        }

        private void ThrowIfDisposed()
        {
            if (isDisposed)
                throw new Exception("The Subject instance is already disposed!");
        }

        public void Dispose()
        {
            lock (observerLock)
            {
                isDisposed = true;
            }
        }

        internal class SubjectObserver : IObserver<T>
        {
            private readonly Action<T> onNext;
            private readonly Action<Exception> onError;
            private readonly Action onCompleted;

            public SubjectObserver(Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
            {
                this.onNext = onNext;
                this.onError = onError;
                this.onCompleted = onCompleted;
            }

            public void OnNext(T value)
            {
                onNext?.Invoke(value);
            }

            public void OnError(Exception error)
            {
                onError?.Invoke(error);
            }

            public void OnCompleted()
            {
                onCompleted?.Invoke();
            }
        }

        internal class Subscription : IDisposable
        {
            private readonly object lockObject = new object();
            private Subject<T> observable;
            private IObserver<T> observer;

            public Subscription(Subject<T> observable, IObserver<T> observer)
            {
                this.observer = observer;
                this.observable = observable;
            }

            public void Dispose()
            {
                lock (lockObject)
                {
                    if (observable != null)
                    {
                        lock (observable.observerLock)
                        {
                            observable.observers.Remove(observer);

                            observable = null;
                            observer = null;
                        }
                    }
                }
            }
        }
    }
}