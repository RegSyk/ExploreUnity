using UnityEngine;

namespace Explore
{
    public abstract class MonoRegistry<T> : Registry<T> where T : MonoBehaviour
    {
    }
}
