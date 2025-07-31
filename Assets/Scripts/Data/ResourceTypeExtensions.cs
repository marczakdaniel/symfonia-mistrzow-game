using UnityEngine;

namespace DefaultNamespace.Data
{
    public static class ResourceTypeExtensions
    {
        private static ResourceTypeImages _resourceTypeImages;
        
        // Static reference to be assigned in inspector or during runtime
        private static ResourceTypeImages ResourceTypeImagesAsset { get; set; }
        
        private static ResourceTypeImages ResourceTypeImages
        {
            get
            {
                if (_resourceTypeImages == null)
                {
                    // First try the manually assigned asset
                    if (ResourceTypeImagesAsset != null)
                    {
                        _resourceTypeImages = ResourceTypeImagesAsset;
                    }
                    else
                    {
                        // Try to load from Resources folder
                        var allResourceTypeImages = Resources.LoadAll<ResourceTypeImages>("");
                        if (allResourceTypeImages.Length > 0)
                        {
                            _resourceTypeImages = allResourceTypeImages[0];
                        }
                    }
                }
                return _resourceTypeImages;
            }
        }
        
        public static SingleResourceTypeImages GetSingleResourceTypeImages(this ResourceType resourceType)
        {
            if (ResourceTypeImages?.SingleResourceTypeImages == null) 
            {
                // Fallback: return null, let the caller handle missing icon
                Debug.LogWarning($"No ResourceTypeImages asset found for ResourceType: {resourceType}");
                return null;
            }
            
            int tokenTypeIndex = (int)resourceType;
                if (tokenTypeIndex >= 0 && tokenTypeIndex < ResourceTypeImages.SingleResourceTypeImages.Length)
            {
                var tokenImage = ResourceTypeImages.SingleResourceTypeImages[tokenTypeIndex];
                return tokenImage;
            }
            
            Debug.LogWarning($"No token image found for ResourceType: {resourceType} at index {tokenTypeIndex}");
            return null;
        }
        
        public static Color GetColor(this ResourceType resourceType)
        {
            return resourceType switch
            {
                ResourceType.Melody => new Color(0.1254902f, 0.254902f, 0.2627451f),
                ResourceType.Harmony => new Color(0.2588235f, 0.3215686f, 0.1098039f),
                ResourceType.Rhythm => new Color(0.4352942f, 0.1490196f, 0.04313726f),
                ResourceType.Instrumentation => new Color(0.3568628f, 0.1529412f, 0.2588235f), // Brown
                ResourceType.Dynamics => new Color(0.4156863f, 0.227451f, 0.09411766f),
                ResourceType.Inspiration => new Color(0.7215686f, 0.4313726f, 0f),
                _ => Color.white
            };
        }
        
        public static string GetDisplayName(this ResourceType resourceType)
        {
            return resourceType switch
            {
                ResourceType.Melody => "Melodia",
                ResourceType.Harmony => "Harmonia", 
                ResourceType.Rhythm => "Rytm",
                ResourceType.Instrumentation => "Instrumentacja",
                ResourceType.Dynamics => "Dynamika",
                ResourceType.Inspiration => "Inspiracja",
                _ => resourceType.ToString()
            };
        }
    }
} 