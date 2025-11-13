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

    public static void SlowMotionDuration(float duration, float scale)
    {
        Time.timeScale = scale;

        DOVirtual.DelayedCall(duration, () =>
        {
            Time.timeScale = 1;
        });
    }
}
