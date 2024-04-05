using UnityEngine;
using UnityEngine.InputSystem;
using static Explore.Input.InputActions;

namespace Explore.Input
{
    public class InputReader : InputReaderBase, IPlayerActions
    {
        private InputActions _InputActions;
        
        public override InputReaderBase Init()
        {
            if (_InputActions == null)
            {
                _InputActions = new InputActions();
                _InputActions.Player.SetCallbacks(this);
            }
            return this;
        }
        public override InputReaderBase SetEnabled(bool enabled)
        {
            if (enabled)
            {
                _InputActions.Enable();
            }
            else
            {
                _InputActions.Disable();
            }
            return this;
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 direction = context.ReadValue<Vector2>();
            InvokeOnMove(direction);
        }
        public void OnMoveCamera(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    InvokeOnMoveCamera(true);
                    break;
                case InputActionPhase.Canceled:
                    InvokeOnMoveCamera(false);
                    break;
            }
        }
        public void OnSelect(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    InvokeOnSelect();
                    break;
            }
        }
        public void OnSelectMany(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    InvokeOnSelectMany(true);
                    break;
                case InputActionPhase.Canceled:
                    InvokeOnSelectMany(false);
                    break;
            }
        }
        public void OnLook(InputAction.CallbackContext context)
        {
            InvokeOnLook(context.ReadValue<Vector2>(), IsDeviceMouse(context));
        }
        public void OnRotate(InputAction.CallbackContext context)
        {
            switch (context.phase)
            {
                case InputActionPhase.Started:
                    InvokeOnRotate(true);
                    break;
                case InputActionPhase.Canceled:
                    InvokeOnRotate(false);
                    break;
            }
        }

        private bool IsDeviceMouse(InputAction.CallbackContext context) => context.control.device.name == "Mouse";
    }
}
