using System;
using UnityEngine;
namespace Explore
{
    public abstract class ObjectPinpointer<T> : MonoBehaviour where T : InteractableObject
    {
        public event Action<T, bool> OnObjectPinpointed;

        [field: Header("State")]
        [field: SerializeField] private T _PinpointedObject { get; set; }

        [field: Header("References")]
        [field: SerializeField] private ObjectDetector<T> _ObjectDetector { get; set; }

        public ObjectPinpointer<T> SetObjectDetector(ObjectDetector<T> objectDetector)
        {
            if (_ObjectDetector == null)
            {
                _ObjectDetector = objectDetector;
            }
            return this;
        }

        public void UpdateFrame()
        {
            HandlePinpoint();
        }

        protected void OnObjectPinpointedInvoke(T pinpointedObject, bool pinpointed = false) 
            => OnObjectPinpointed?.Invoke(pinpointedObject, pinpointed);
        
        private void HandlePinpoint()
        {
            if(_ObjectDetector.DetectedObject != _PinpointedObject)
            {
                if(_PinpointedObject != null)
                {
                    _PinpointedObject.SetPinpoint(false);
                    
                    OnObjectPinpointedInvoke(_PinpointedObject, false);
                }
                if(_ObjectDetector.DetectedObject != null)
                {
                    _ObjectDetector.DetectedObject.SetPinpoint(true);
                    
                    OnObjectPinpointedInvoke(_ObjectDetector.DetectedObject, true);
                }
                _PinpointedObject = _ObjectDetector.DetectedObject;
            }
        }
    }
}
