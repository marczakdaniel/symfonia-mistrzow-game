using R3;

namespace SplendorGame.Core.MVP
{
    /// <summary>
    /// Base interface for all models in MVP pattern
    /// </summary>
    public interface IModel
    {
        /// <summary>
        /// Initialize the model
        /// </summary>
        void Initialize();
        
        /// <summary>
        /// Cleanup the model
        /// </summary>
        void Dispose();
    }
} 