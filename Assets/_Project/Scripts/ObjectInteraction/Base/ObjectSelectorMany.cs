using UnityEngine;

namespace Explore
{
    public abstract class ObjectSelectorMany<T> : ObjectSelector<T> where T : InteractableObject
    {
        [field: Header("Settings")]
        [field: SerializeField] private float _ObjectDragCheckTime = 0.2f;

        [field: Header("References")]
        [field: SerializeField] private TimerUpdater _TimerUpdater { get; set; }
        private CountdownTimer _countdownTimer;

        private void Awake() => Init();

        private void Init()
        {
            _TimerUpdater = GameObjectExtensions.FindSingletonObjectOfType<TimerUpdater>();
            void SetupTimers()
            {
                _countdownTimer = new CountdownTimer(_ObjectDragCheckTime);
                _TimerUpdater.Register(_countdownTimer);
            }
            SetupTimers();
        }

        protected override void OnSelectInternal()
        {
            if (_DetectedObject)
            {
                if (_SelectedObjectRegistry.Registered(_DetectedObject))
                {
                    T detectedObject = _DetectedObject;
                    void UnregisterIfNotDragged()
                    {
                        if (!detectedObject.IsBeingDragged)
                        {
                            _SelectedObjectRegistry.Unregister(detectedObject);
                            InvokeOnObjectSelected(detectedObject, false);
                        }
                        _countdownTimer.OnTimerStop -= UnregisterIfNotDragged;
                    }
                    _countdownTimer.OnTimerStop += UnregisterIfNotDragged;
                    _countdownTimer.Start();
                }
                else
                {
                    _SelectedObjectRegistry.Register(_DetectedObject);
                    InvokeOnObjectSelected(_DetectedObject, true);
                }
            }
            else
            {
                _SelectedObjectRegistry.Clear();
                InvokeOnObjectSelected(null);
            }
        }
    }
}