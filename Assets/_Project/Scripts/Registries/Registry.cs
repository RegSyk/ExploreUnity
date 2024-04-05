using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

namespace Explore
{
    public abstract class Registry<T> : MonoBehaviour
    {
        public IEnumerable<T> Elements => _Elements;
        public T LastElement => _Elements.LastOrDefault();

        [field: SerializeField] protected List<T> _Elements { get; set; }
            = new List<T>();

        public virtual void Register(T element)
        {
            if (Registered(element))
            {
                throw new InvalidOperationException("Element is already registered!");
            }
            _Elements.Add(element);
        }
        public void Register(T[] elements)
        {
            foreach (T element in elements)
            {
                Register(element);
            }
        }
        public void RegisterIfNotRegistered(T element)
        {
            if (!Registered(element))
            {
                Register(element);
            }
        }
        public void UnregisterIfRegistered(T element)
        {
            if (Registered(element))
            {
                Unregister(element);
            }
        }
        public virtual void Unregister(T element)
        {
            if (!Registered(element))
            {
                throw new InvalidOperationException("Element is not registered!");
            }
            _Elements.Remove(element);
        }
        public void Unregister(T[] elements)
        {
            foreach (T element in elements)
            {
                Unregister(element);
            }
        }
        public bool Registered(T element)
        {
            return _Elements.Contains(element);
        }
        public virtual void Clear()
        {
            _Elements.Clear();
        }
    }
}
