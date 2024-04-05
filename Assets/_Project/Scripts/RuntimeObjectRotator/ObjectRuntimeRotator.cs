using RuntimeHandle;
using UnityEngine;

namespace Explore
{
    public class ObjectRuntimeRotator : MonoBehaviour 
    {
        [field: Header("State")]
        [field: SerializeField] public bool Enabled { get; private set; }

        [field: Header("References")]
        [field: SerializeField] private InteractableObject _Target;
        [field: SerializeField] private Transform _TargetTransform;
        [field: SerializeField] private RuntimeTransformHandle _Handle;

        public ObjectRuntimeRotator Init()
        {
            if (_Target == null)
            {
                throw new System.ArgumentNullException("Target is null!");
            }
            _TargetTransform = _Target.transform;
            _Handle = RuntimeTransformHandle.Create(_TargetTransform, HandleType.ROTATION);
            _Handle.gameObject.transform.localScale = Vector3.one * 4;
            return this;
        }
        public ObjectRuntimeRotator SetTarget(InteractableObject target)
        {
            if(_Target == null)
            {
                _Target = target;
            }
            return this;
        }
        public ObjectRuntimeRotator SetEnabled(bool enabled)
        {
            Enabled = enabled;
            _Handle.gameObject.SetActive(enabled);
            return this;
        }
    }
}
