using System;
using UnityEngine;

namespace Explore.Input
{
    public abstract class InputReaderBase : MonoBehaviour
    {
        public event Action<Vector2> OnMoveEvent;
        public event Action<bool> OnMoveCameraEvent;
        public event Action OnSelectEvent;
        public event Action<bool> OnSelectManyEvent;
        public event Action<Vector2, bool> OnLookEvent;
        public event Action<bool> OnRotateEvent;

        public abstract InputReaderBase Init();
        public abstract InputReaderBase SetEnabled(bool enabled);

        protected void InvokeOnMove(Vector2 move) => OnMoveEvent?.Invoke(move);
        protected void InvokeOnMoveCamera(bool startFlying) => OnMoveCameraEvent?.Invoke(startFlying);
        protected void InvokeOnSelect() => OnSelectEvent?.Invoke();
        protected void InvokeOnSelectMany(bool selectionStarted) => OnSelectManyEvent?.Invoke(selectionStarted);
        protected void InvokeOnLook(Vector2 rotate, bool isDeviceMouse) => OnLookEvent?.Invoke(rotate, isDeviceMouse);
        protected void InvokeOnRotate(bool rotationStarted) => OnRotateEvent?.Invoke(rotationStarted);
    }
}
