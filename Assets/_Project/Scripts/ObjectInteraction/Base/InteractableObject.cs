using System;
using UnityEngine;

namespace Explore
{
    public abstract class InteractableObject : MonoBehaviour
    {
        public event Action<bool, InteractableObject> DragEvent;

        public bool IsBeingDragged => _MyDragAndDrop ? _MyDragAndDrop.IsBeingDragged : false;
        public TransformData TransformData => _MyPlacement.TransformData;
        public Vector3 Position => _MyPlacement.Position;

        [field: Header("State")]
        [field: SerializeField] public bool Selected { get; private set; }
        [field: SerializeField] public bool SelectedHighlighted { get; private set; }
        [field: SerializeField] public bool Pinpointed { get; private set; }
        [field: SerializeField] public bool PinpointedHighlighted { get; private set; }

        [field: Header("References")]
        [field: SerializeField] protected Outline _MyOutline;
        [field: SerializeField] protected ObjectPlacement _MyPlacement;
        [field: SerializeField] protected ObjectDragAndDrop _MyDragAndDrop;

        public InteractableObject Init()
        {
            InitPlacement();
            InitOutline();
            InitDragAndDrop();
            return this;
        }
        public InteractableObject RestoreTransformParent()
        {
            _MyPlacement.RestoreTransformParent();
            return this;
        }
        public InteractableObject RestorePosition()
        {
            _MyPlacement.RestorePosition();
            return this;
        }
        public InteractableObject SetEnabledDragAndDrop(bool enabled)
        {
            _MyDragAndDrop.SetEnabled(enabled);
            return this;
        }

        public void SetPosition(Vector3 position) => _MyPlacement.SetPosition(position);
        public void SetRotation(Quaternion rotation) => _MyPlacement.SetRotation(rotation);
        public void SetTransformData(TransformData transformData) => _MyPlacement.SetTransformData(transformData);
        public void SetSelection(bool select)
        {
            Selected = select;
        }
        public void SetPinpoint(bool pinpoint)
        {
            Pinpointed = pinpoint;
        }
        public void HighlightPinpoint(bool highlight)
        {
            if (highlight)
            {
                EnableOutline(Color.blue);
                SelectedHighlighted = false;
            }
            else
            {
                DisableOutline();
            }
            PinpointedHighlighted = highlight;
        }
        public void HighlightSelected(bool highlight)
        {
            if (highlight)
            {
                EnableOutline(Color.yellow, 10);
                PinpointedHighlighted = false;
            }
            else
            {
                DisableOutline();
            }
            SelectedHighlighted = highlight;
        }

        protected virtual void InitDragAndDrop()
        {
            if (_MyDragAndDrop == null)
            {
                gameObject.AddComponent<ObjectDragAndDrop>()
                    .SetObjectPlacement(_MyPlacement)
                    .GetInstance(out _MyDragAndDrop)
                    .SetEnabled(false);
                AddEventListeners();
            }
            else
            {
                Debug.LogError("MyDragAndDrop is not null!");
            }
        }
        protected virtual void InitOutline()
        {
            if (_MyOutline == null)
            {
                _MyOutline = gameObject.AddComponent<Outline>();
                DisableOutline();
            }
            else
            {
                Debug.LogError("MyOutline is not null!");
            }
        }
        protected virtual void InitPlacement()
        {
            if (_MyPlacement == null)
            {
                gameObject.AddMeshCollider()
                    .AddComponent<ObjectPlacement>()
                    .Init()
                    .CalculateMeshOffset()
                    .BackupCurrentPosition()
                    .GetInstance(out _MyPlacement);
            }
            else
            {
                Debug.LogError("MyPlacement is not null!");
            }
        }

        private void OnDrag(bool dragStarted) => DragEvent?.Invoke(dragStarted, this);
        private void AddEventListeners()
        {
            _MyDragAndDrop.DragEvent += OnDrag;
        }
        private void DisableOutline()
        {
            _MyOutline.OutlineMode = Outline.Mode.OutlineHidden;
            _MyOutline.enabled = false;
        }
        private void EnableOutline(Color color, float width = 6)
        {
            _MyOutline.enabled = true;
            _MyOutline.OutlineMode = Outline.Mode.OutlineAll;
            _MyOutline.OutlineColor = color;
            _MyOutline.OutlineWidth = width;
        }

        private void OnEnable()
        {
            if (_MyDragAndDrop)
            {
                AddEventListeners();
            }
        }
        private void OnDisable()
        {
            if(_MyDragAndDrop)
            {
                _MyDragAndDrop.DragEvent -= OnDrag;
            }
        }
    }
}
