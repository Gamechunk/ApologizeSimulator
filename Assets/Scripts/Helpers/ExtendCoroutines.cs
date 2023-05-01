using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtendCoroutines : MonoBehaviour
{
    public static IEnumerator Fade01(bool isFadeDirectionIn, float fadeTime, Action<float> onTakeValue, Action onEnd = null)
    {
        var originTime = Time.time;
        float maxValue = 1f;
        var valueIn = 0f;
        var valueOut = maxValue;
        do
        {
            yield return null;
            valueIn = (Time.time - originTime) / fadeTime;
            if (!isFadeDirectionIn)
                valueOut = maxValue - valueIn;

            if (!isFadeDirectionIn)
                onTakeValue?.Invoke(valueOut);
            else
                onTakeValue?.Invoke(valueIn);

        } while (valueIn <= maxValue);
        onEnd?.Invoke();
    }

    public static IEnumerator DoAfterTime(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action?.Invoke();

    }

    /// <summary>
    /// Cycle Coroutine
    /// </summary>
    /// <param name="everyTime"> If 0, than updated every frame</param>
    /// <param name="action"></param>
    /// <param name="condition">If null, than infinity cycle</param>
    /// <returns></returns>
    public static IEnumerator CycleAction(float everyTime, Action action, Action onEnd = null, Func<bool> condition = null)
    {
        if (condition == null)
            condition += () => true;
        do
        {
            if(everyTime > 0)
                yield return new WaitForSeconds(everyTime);
            else
                yield return null;
            action?.Invoke();
        } while (condition.Invoke());
        onEnd?.Invoke();
    }
}
