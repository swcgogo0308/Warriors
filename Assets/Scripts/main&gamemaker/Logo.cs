using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour {

    void Start() {
        StartCoroutine(FadeIn());
        StartCoroutine(FadeOut());
       
    }
    IEnumerator FadeIn()
    {//Fadein
        CanvasGroup canvasGrop = GetComponent<CanvasGroup>();
        while (canvasGrop.alpha > 1)
        {
            canvasGrop.alpha -= Time.deltaTime;
            yield return null;
        }
        canvasGrop.interactable = false;
        yield return null;

        Application.LoadLevel("Main");
    }

    IEnumerator FadeOut()
    {//FadeOut-점차 희어짐
        CanvasGroup canvasGrop = GetComponent<CanvasGroup>();
        while (canvasGrop.alpha > 0)
        {
            canvasGrop.alpha -= Time.deltaTime;
            yield return null;
        }
        canvasGrop.interactable = false;
        yield return null;

        Application.LoadLevel("Logo");
    }


}
