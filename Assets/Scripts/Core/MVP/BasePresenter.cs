using System;
using R3;

namespace SplendorGame.Core.MVP
{
    /// <summary>
    /// Base abstract presenter class with common functionality
    /// </summary>
    /// <typeparam name="TModel">Model type</typeparam>
    /// <typeparam name="TView">View type</typeparam>
    public abstract class BasePresenter<TModel, TView> : IPresenter<TModel, TView>
        where TModel : IModel
        where TView : IView
    {
        protected readonly CompositeDisposable _disposables = new();
        
        public TModel Model { get; private set; }
        public TView View { get; private set; }
        
        protected BasePresenter(TModel model, TView view)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            View = view ?? throw new ArgumentNullException(nameof(view));
        }
        
        public virtual void Initialize()
        {
            Model.Initialize();
            View.Initialize();
            BindViewToModel();
        }
        
        protected abstract void BindViewToModel();
        
        public virtual void Dispose()
        {
            _disposables?.Dispose();
            Model?.Dispose();
            View?.Dispose();
        }
    }
} 