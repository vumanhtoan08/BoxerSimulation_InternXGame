using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Helper;

namespace Immortal
{
    public class ImmortalObject : MonoBehaviour
    {
        public static ImmortalObject Instance;
        public event Action<float> OnUpdate;
        private static List<Counter> counters;
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            counters = new List<Counter>();
        }
        private void Update()
        {
            for (int i = 0; i < counters.Count; i++)
            {
                counters[i].UpdateCounter(Time.deltaTime);
            }
            OnUpdate?.Invoke(Time.deltaTime);
        }
        public Counter GetCounter(float duration, Action complete)
        {
            for (int i = 0; i < counters.Count; i++)
            {
                if (!counters[i].IsActive)
                {
                    counters[i].Setup(duration, complete);
                    return counters[i];
                }
            }
            Counter counter = new Counter(duration, complete);
            AddCounter(counter);
            return counter;
        }
        public void AddCounter(Counter counter)
        {
            counters.Add(counter);
        }
    }
}
