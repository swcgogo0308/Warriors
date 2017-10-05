using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Logo : MonoBehaviour {
    void Start() {
        StartCoroutine(Fadein());
        
    }
    IEnumerator Fadein()
    {//FadeOut
        CanvasGroup canvasGrop = GetComponent<CanvasGroup>();
        while (canvasGrop.alpha <0)
        {
            canvasGrop.alpha += Time.deltaTime;
            yield return null;
            Debug.Log(1);
        }
        canvasGrop.interactable = false;        
        StartCoroutine(FadeOut());
        //yield return null;


    }
    IEnumerator FadeOut()
    {//FadeOut
        CanvasGroup canvasGrop = GetComponent<CanvasGroup>();
        while (canvasGrop.alpha > 0)
        {
            Debug.Log(2);
            canvasGrop.alpha -= Time.deltaTime;
            yield return null;
        }
        canvasGrop.interactable = false;
        yield return null;

        Application.LoadLevel("Main");
    }
}