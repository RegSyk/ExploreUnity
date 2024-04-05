using System;
using UnityEngine;

namespace Explore
{
    [Serializable]
    public struct TransformData
    {
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
    }
}
