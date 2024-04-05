using UnityEngine;
using UnityEngine.EventSystems;

namespace Explore
{
    public abstract class ObjectDetector<T> : MonoBehaviour where T : InteractableObject
    {
        [field: Header("State")]
        [field: SerializeField] public T DetectedObject { get; set; }

        [field: Header("References")]
        [field: SerializeField] private MonoRegistry<T> _AllObjectsRegistry { get; set; }

        public ObjectDetector<T> SetAllObjectsRegistry(MonoRegistry<T> registry)
        {
            if (_AllObjectsRegistry == null)
            {
                _AllObjectsRegistry = registry;
            }
            return this;
        }

        public void UpdateFrame()
        {
            HandlePointerOverSelectable();
        }

        private void HandlePointerOverSelectable()
        {
            bool pointerOverSelectableObject = false;
            Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);

            if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(ray, out RaycastHit raycastHit))
            {
                if (raycastHit.transform.TryGetComponent<T>(out T selection))
                {
                    _AllObjectsRegistry.RegisterIfNotRegistered(selection);

                    pointerOverSelectableObject = true;
                    DetectedObject = selection;
                }
                else
                {
                }
            }
            else
            {
            }
            if (!pointerOverSelectableObject)
            {
                if (DetectedObject)
                {
                    DetectedObject = null;
                }
            }
        }
    }
}
