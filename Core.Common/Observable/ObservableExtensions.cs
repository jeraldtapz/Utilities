using System;

namespace Core.Common
{
    public static class ObservableExtensions
    {
        public static IDisposable Subscribe<T>(this IObservable<T> observable, Action<T> onNext, Action<Exception> onError = null, Action onCompleted = null)
        {
            return observable.Subscribe(new Subject<T>.SubjectObserver(onNext, onError, onCompleted));
        }
    }
}