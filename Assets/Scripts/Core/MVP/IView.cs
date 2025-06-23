using UnityEngine;

namespace SplendorGame.Core.MVP
{
    /// <summary>
    /// Base interface for all views in MVP pattern
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// GameObject of this view
        /// </summary>
        GameObject gameObject { get; }
        
        /// <summary>
        /// Transform of this view
        /// </summary>
        Transform transform { get; }
        
        /// <summary>
        /// Initialize the view
        /// </summary>
        void Initialize();
        
        /// <summary>
        /// Show the view
        /// </summary>
        void Show();
        
        /// <summary>
        /// Hide the view
        /// </summary>
        void Hide();
        
        /// <summary>
        /// Cleanup the view
        /// </summary>
        void Dispose();
    }
} 