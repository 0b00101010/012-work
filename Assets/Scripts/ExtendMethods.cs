using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendMethods
{
    public static void SetColor(this LineRenderer renderer, Color color)
    {
        renderer.startColor = renderer.endColor = color;
    }
    public static void Invoke(this MonoBehaviour behaviour, System.Action action, float time)
    {
        IEnumerator Coroutine()
        {
            yield return new WaitForSeconds(time);
            action();
        }

        behaviour.StartCoroutine(Coroutine());
    }
}
