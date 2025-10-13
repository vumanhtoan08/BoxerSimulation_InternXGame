using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Immortal;
using System.Linq;

public static class Helper
{
    private static System.Random rng = new System.Random();
    public static ImmortalObject Object
    {
        get
        {
            if (_obj == null)
            {
                _obj = new GameObject("ImmortalObject").AddComponent<ImmortalObject>();
            }
            return _obj;
        }
    }
    private static ImmortalObject _obj;
    public static void StartCoroutine(IEnumerator ienumerator)
    {
        Object.StartCoroutine(ienumerator);
    }
    public static Counter CreateCounter(float duration, Action complete)
    {
        Counter counter = Object.GetCounter(duration, complete);
        return counter;
    }

    public static string ConvertMoneyToString(long money)
    {
        if (money >= 1000000000)
        {
            return (money / 1000000000).ToString() + "B";
        }
        if (money >= 1000000)
        {
            return (money / 1000000).ToString() + "M";
        }
        if (money >= 10000)
        {
            return (money / 1000).ToString() + "K";
        }
        return money.ToString();
    }
    public class Counter
    {
        public Counter(float duration, Action complete)
        {
            Setup(duration, complete);
        }
        public bool IsActive { get; private set; }
        public float Timer { get; private set; }
        public bool IsPause { get; set; }
        public event Action OnComplete;
        public void Setup(float duration, Action complete)
        {
            this.OnComplete = complete;
            IsPause = false;
            IsActive = true;
            if (duration <= 0)
            {
                duration = 0.000001f;
            }
            Timer = duration;
        }
        public void UpdateCounter(float delta)
        {
            if (IsPause || Timer <= 0 || !IsActive) return;
            Timer -= delta;
            if (Timer <= 0)
            {
                IsActive = false;
                OnComplete?.Invoke();
            }
        }
        public void Cancel()
        {
            Timer = 0;
            IsActive = false;
        }
        public static implicit operator bool(Counter exists)
        {
            return exists != null;
        }
    }
    /*public static void Shuffle<T>(this List<T> list)
    {
        list = list.OrderBy(_ => rng.Next()).ToList();
    }*/
}
