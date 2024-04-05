using System;
namespace Explore
{
    [Serializable]
    public struct AppData
    {
        public string Name { get; set; }
        public TransformData RootTransformData { get; set; }
    }
}
