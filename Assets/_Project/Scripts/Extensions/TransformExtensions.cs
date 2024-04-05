using UnityEngine;
using System.Collections.Generic;

namespace Explore
{
    public static class TransformExtensions
    {
        public static List<Transform> FindChildrenByName(this Transform parent, string name)
        {
            List<Transform> foundTransforms = new List<Transform>();

            foreach (Transform child in parent)
            {
                if (child.name.StartsWith(name))
                {
                    foundTransforms.Add(child);
                }

                foundTransforms.AddRange(FindChildrenByName(child, name));
            }

            return foundTransforms;
        }
    }
}
