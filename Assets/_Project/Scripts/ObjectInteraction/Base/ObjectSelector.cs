using System;
using UnityEngine;

namespace Explore
{
    public abstract class ObjectSelector<T> : MonoBehaviour where T : InteractableObject
    {
        public event Action<T, bool> OnObjectSelected;

        protected T _DetectedObject => _ObjectDetector.DetectedObject;
        protected T _LastSelectedObject => _SelectedObjectRegistry.LastElement;

        [field: Header("References")]
        [field: SerializeField] protected MonoRegistry<T> _SelectedObjectRegistry { get; set; }
        [field: SerializeField] protected MonoRegistry<T> _AllObjectsRegistry { get; set; }

        [field: SerializeField] protected ObjectDetector<T> _ObjectDetector { get; set; }

        public ObjectSelector<T> SetSelectedObjectRegistry(MonoRegistry<T> registry)
        {
            if (_SelectedObjectRegistry == null)
            {
                _SelectedObjectRegistry = registry;
            }
            return this;
        }
        public ObjectSelector<T> SetAllObjectsRegistry(MonoRegistry<T> registry)
        {
            if (_AllObjectsRegistry == null)
            {
                _AllObjectsRegistry = registry;
            }
            return this;
        }
        public ObjectSelector<T> SetObjectDetector(ObjectDetector<T> objectDetector)
        {
            if (_ObjectDetector == null)
            {
                _ObjectDetector = objectDetector;
            }
            return this;
        }

        public void OnSelect()
        {
            OnSelectInternal();
        }

        protected void InvokeOnObjectSelected(T detectedObject, bool selected = false) => OnObjectSelected?.Invoke(detectedObject, selected);
        protected abstract void OnSelectInternal();
    }
}
