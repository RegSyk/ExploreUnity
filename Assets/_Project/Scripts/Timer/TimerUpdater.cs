using UnityEngine;

namespace Explore
{
    public class TimerUpdater : Registry<Timer>
    {
        private void Update()
        {
            foreach (Timer timer in Elements)
                timer.Tick(Time.deltaTime);
        }
    }
}
