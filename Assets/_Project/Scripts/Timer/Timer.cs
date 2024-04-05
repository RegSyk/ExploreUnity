using System;
namespace Explore
{
    public abstract class Timer
    {
        public bool IsRunning { get; protected set; }
        public float Progress => _Time / _initialTime;

        public Action OnTimerStart = delegate { };
        public Action OnTimerStop = delegate { };

        protected float _initialTime;
        protected float _Time { get; set; }

        protected Timer(float value)
        {
            _initialTime = value;
            IsRunning = false;
        }

        public void Start()
        {
            _Time = _initialTime;
            if (!IsRunning)
            {
                IsRunning = true;
                OnTimerStart.Invoke();
            }
        }
        public void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                OnTimerStop.Invoke();
            }
        }
        public void Resume() => IsRunning = true;
        public void Pause() => IsRunning = false;
        public void Cancel()
        {
            IsRunning = false;
            OnTimerStart = null;
            OnTimerStop = null;
        }
        public abstract void Tick(float deltaTime);
    }
    public class CountdownTimer : Timer
    {
        public bool IsFinished => _Time <= 0;
        public CountdownTimer(float value) : base(value) { }
        public override void Tick(float deltaTime)
        {
            if (IsRunning && _Time > 0)
            {
                _Time -= deltaTime;
            }

            if (IsRunning && _Time <= 0)
            {
                Stop();
            }
        }
        public void Reset() => _Time = _initialTime;
        public void Reset(float newTime)
        {
            _initialTime = newTime;
            Reset();
        }
    }

    public class StopwatchTimer : Timer
    {
        public StopwatchTimer() : base(0) { }
        public override void Tick(float deltaTime)
        {
            if (IsRunning)
            {
                _Time += deltaTime;
            }
        }
        public void Reset() => _Time = 0;
        public float GetTime() => _Time;
    }
}
