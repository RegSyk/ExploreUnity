using Cinemachine;
using Explore.Input;
using UnityEngine;

namespace Explore
{
    public class SpectatorCameraController : MonoBehaviour
    {
        [field: Header("Settings")]
        [field: SerializeField] public float Sensitivity { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

        [field: SerializeField] private float _LookSmoothSpeed { get; set; } = 0.5f;

        [field: Header("References")]
        [field: SerializeField] private CinemachineVirtualCamera _CinemachineCamera { get; set; }
        [field: SerializeField] private InputReaderBase _InputReader { get; set; }

        [field: Header("State")]
        [field: SerializeField] private bool _CameraMovement { get; set; }
        [field: SerializeField] private Vector2 _MoveVector { get; set; }
        [field: SerializeField] private Vector2 _LookVector { get; set; }

        public SpectatorCameraController SetCamera(CinemachineVirtualCamera camera)
        {
            if(_CinemachineCamera == null)
            {
                _CinemachineCamera = camera;
            }
            return this;
        }
        public SpectatorCameraController SetInputReader(InputReaderBase inputReader)
        {
            if(_InputReader == null)
            {
                _InputReader = inputReader;
            }
            return this;
        }

        private void OnEnable()
        {
            _InputReader.OnLookEvent += OnLook;
            _InputReader.OnMoveEvent += OnMove;
            _InputReader.OnMoveCameraEvent += OnMoveCamera;
        }
        private void OnDisable()
        {
            _InputReader.OnLookEvent -= OnLook;
            _InputReader.OnMoveEvent -= OnMove;
            _InputReader.OnMoveCameraEvent -= OnMoveCamera;
        }
        private void Update()
        {
            if (_CameraMovement)
            {
                HandleLook();
                HandleMovement();
            }
        }

        private void OnLook(Vector2 lookVector, bool isDeviceMouse)
        {
            _LookVector = lookVector;
        }
        private void OnMove(Vector2 moveVector)
        {
            _MoveVector = moveVector;
        }
        private void OnMoveCamera(bool startMoving)
        {
            _CameraMovement = startMoving;
            if (_CameraMovement)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        private void HandleLook()
        {
            float vertical = -_LookVector.y;
            float horizontal = _LookVector.x;

            Vector3 input = new Vector3(vertical, horizontal, 0);
            Quaternion desiredRotation = Quaternion.Euler(_CinemachineCamera.transform.rotation.eulerAngles + 50 * Sensitivity * Time.deltaTime * input);
            Quaternion smoothedRotation = Quaternion.Slerp(_CinemachineCamera.transform.rotation, desiredRotation, _LookSmoothSpeed);
            _CinemachineCamera.transform.rotation = smoothedRotation;
        }
        private void HandleMovement()
        {
            Vector3 input = new(_MoveVector.x, 0, _MoveVector.y);
            _CinemachineCamera.transform.Translate(Speed * Time.deltaTime * input);
        }
    }
}
