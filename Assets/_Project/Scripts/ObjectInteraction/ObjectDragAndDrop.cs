using System;
using UnityEngine;

namespace Explore
{
    public class ObjectDragAndDrop : MonoBehaviour
    {
        public event Action<bool> DragEvent;

        [field: Header("State")]
        public bool IsBeingDragged { get; private set; }
        public bool Enabled { get; private set; }

        private Vector3 mOffset;
        private float mZCoord;

        [field: Header("References")]
        private ObjectPlacement _ObjectPlacement;

        public ObjectDragAndDrop GetInstance(out ObjectDragAndDrop dragAndDrop)
        {
            dragAndDrop = this;
            return dragAndDrop;
        }
        public ObjectDragAndDrop SetObjectPlacement(ObjectPlacement placement)
        {
            if(_ObjectPlacement == null)
            {
                _ObjectPlacement = placement;
            }
            return this;
        }

        public void SetEnabled(bool enabled)
        {
            Enabled = enabled;
        }

        private Vector3 GetMouseWorldPos()
        {
            Vector3 mousePoint = UnityEngine.Input.mousePosition;
            mousePoint.z = mZCoord;
            return Camera.main.ScreenToWorldPoint(mousePoint);
        }

        private void OnMouseDown()
        {
            IsBeingDragged = true;
            _ObjectPlacement.CalculateMeshOffset();
            mZCoord = Camera.main.WorldToScreenPoint(_ObjectPlacement.PositionWithOffset).z;
            mOffset = _ObjectPlacement.Position - GetMouseWorldPos();
            DragEvent?.Invoke(true);
        }
        private void OnMouseUp()
        {
            IsBeingDragged = false;
            DragEvent?.Invoke(false);
        }
        private void OnMouseDrag()
        {
            if (!Enabled)
            {
                return;
            }
            _ObjectPlacement.SetPosition(GetMouseWorldPos() + mOffset);
        }
    }
}
