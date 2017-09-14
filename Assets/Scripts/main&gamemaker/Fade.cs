using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fade : MonoBehaviour
{

    public void FadeMe()
    {
        StartCoroutine(DoFade());

    }
    IEnumerator DoFade()
    {
        CanvasGroup canvasGrop = GetComponent<CanvasGroup>();
        while (canvasGrop.alpha > 0)
        {
            canvasGrop.alpha -= Time.deltaTime / 2;
            yield return null;
        }
        canvasGrop.interactable = false;
        yield return null;
    }
}