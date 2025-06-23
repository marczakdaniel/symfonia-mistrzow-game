namespace SplendorGame.Core.MVP
{
    /// <summary>
    /// Base interface for all presenters in MVP pattern
    /// </summary>
    public interface IPresenter
    {
        /// <summary>
        /// Initialize the presenter
        /// </summary>
        void Initialize();
        
        /// <summary>
        /// Cleanup the presenter
        /// </summary>
        void Dispose();
    }
    
    /// <summary>
    /// Generic presenter interface for specific model-view combinations
    /// </summary>
    /// <typeparam name="TModel">Model type</typeparam>
    /// <typeparam name="TView">View type</typeparam>
    public interface IPresenter<TModel, TView> : IPresenter
        where TModel : IModel
        where TView : IView
    {
        /// <summary>
        /// The model managed by this presenter
        /// </summary>
        TModel Model { get; }
        
        /// <summary>
        /// The view managed by this presenter
        /// </summary>
        TView View { get; }
    }
} 