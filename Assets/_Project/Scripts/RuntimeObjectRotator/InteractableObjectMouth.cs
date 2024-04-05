using UnityEngine;

namespace Explore
{
    public class InteractableObjectMouth : InteractableObject
    {
        protected override void InitPlacement()
        {
            if (_MyPlacement == null)
            {
                gameObject.AddMeshCollider()
                    .AddComponent<ObjectPlacement>()
                    .Init()
                    .GetInstance(out _MyPlacement);
            }
            else
            {
                Debug.LogError("MyPlacement is not null!");
            }
        }
    }
}
