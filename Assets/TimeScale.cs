using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScale : MonoBehaviour {
    public AnimationCurve timeDestructCurve;
    float time = 2f;
    Coroutine actualCoroutine;

    public void DestructTimeScale()
    {
        if (actualCoroutine != null)
        {
            StopCoroutine(DestructTimeScaleCoroutine());
            actualCoroutine = null;
        }
        actualCoroutine = StartCoroutine(DestructTimeScaleCoroutine());

    }

    public IEnumerator DestructTimeScaleCoroutine()
    {
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            Time.timeScale = timeDestructCurve.Evaluate(elapsedTime/time);
            Debug.Log(timeDestructCurve.Evaluate(elapsedTime / time));
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
