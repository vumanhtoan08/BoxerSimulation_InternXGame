using DG.Tweening;
using UnityEngine;

public class TimeEffect : Singleton<TimeEffect>
{
    public static void HitTimeEffect()
    {
        Time.timeScale = 0;

        DOVirtual.DelayedCall(0.1f, () =>
        {
            Time.timeScale = 1;
        });
    }
}
