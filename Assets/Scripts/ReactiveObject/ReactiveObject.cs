using System;

namespace R3
{
    public abstract class ReactiveObject : IDisposable
    {
        private readonly Subject<Unit> _onChanged = new();

        /// <summary>
        /// Observable that emits when any property changes
        /// </summary>
        public Observable<Unit> OnChanged => _onChanged.AsObservable();

        /// <summary>
        /// Notifies subscribers that any property has changed
        /// </summary>
        protected void NotifyChanged()
        {
            _onChanged.OnNext(Unit.Default);
        }

        public virtual void Dispose()
        {
            _onChanged?.Dispose();
        }
    }
}