using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Explore
{
    public abstract class SelectableObjectRegistry<T> : MonoRegistry<T> where T : InteractableObject
    {
        [field: Header("Selectable")]
        [field: SerializeField] public int MaxElements { get; private set; } = 3;

        private T _DraggedObject;
        private Dictionary<T, Vector3> _DictionaryOfOffsets = new Dictionary<T, Vector3>();

        public override void Register(T element)
        {
            //if(Elements.Count() == MaxElements)
            //{
            //    Unregister(Elements.First());
            //}
            if(Elements.Count() == MaxElements)
            {
                return;
            }
            base.Register(element);
            element.SetSelection(true);
            element.SetEnabledDragAndDrop(true);
            element.DragEvent += OnElementDrag;
        }
        public override void Unregister(T element)
        {
            UnregisterSelectable(element);
            base.Unregister(element);
        }
        public override void Clear()
        {
            foreach (T e in Elements)
            {
                UnregisterSelectable(e);
            }
            base.Clear();
        }

        private void UnregisterSelectable(T element)
        {
            element.SetSelection(false);
            element.SetEnabledDragAndDrop(false);
            element.DragEvent -= OnElementDrag;
        }
        private void OnElementDrag(bool dragStarted, InteractableObject sender)
        {
            if (dragStarted)
            {
                _DraggedObject = sender as T;
                foreach(var e in Elements)
                {
                    if(e != _DraggedObject)
                    {
                        if (e.IsBeingDragged)
                        {
                            Debug.LogError($"Multiple objects are being dragged! Objects: {e.name}/{_DraggedObject.name}.");
                        }
                        Vector3 offset = e.Position - _DraggedObject.Position;
                        if (_DictionaryOfOffsets.ContainsKey(e))
                        {
                            _DictionaryOfOffsets[e] = offset;
                        }
                        else
                        {
                            _DictionaryOfOffsets.Add(e, offset);
                        }
                    }
                }
            }
            else
            {
                _DraggedObject = null;
            }
        }

        private void Update()
        {
            if(_DraggedObject != null)
            {
                foreach(T e in Elements)
                {
                    if(e != _DraggedObject)
                    {
                        e.SetPosition(_DraggedObject.Position + _DictionaryOfOffsets[e]);
                    }
                }
            }
        }
    }
}
