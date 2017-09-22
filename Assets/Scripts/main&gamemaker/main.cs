using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Main : MonoBehaviour {

    IEnumerator FadeOut(){//FadeOut
        CanvasGroup canvasGrop = GetComponent<CanvasGroup>();
        while (canvasGrop.alpha > 0)
        {
            canvasGrop.alpha -= Time.deltaTime ;
            yield return null;
        }
        canvasGrop.interactable = false;
        yield return null;

        Application.LoadLevel("InGame");
    }

    IEnumerator FadeIn()
    {//FadeOut
        CanvasGroup canvasGrop = GetComponent<CanvasGroup>();
        while (canvasGrop.alpha < 1)
        {
            canvasGrop.alpha += Time.deltaTime;
            yield return null;
        }
        canvasGrop.interactable = false;
        yield return null;

        Application.LoadLevel("Main");
    }

    public void Starting()
    {
        StartCoroutine(FadeOut());
    }
}