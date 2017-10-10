using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Main : MonoBehaviour {

    public AudioSource audioSource;
    CanvasGroup canvasGrop;

    void Start()
    {
        canvasGrop = GetComponent<CanvasGroup>();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut(){//FadeOut
        while (canvasGrop.alpha > 0)
        {
            canvasGrop.alpha -= Time.deltaTime / 1.2f;
            yield return null;
        }
        canvasGrop.interactable = false;
        yield return null;

        Application.LoadLevel("InGame");
    }

    IEnumerator FadeIn()
    {//FadeOut
        canvasGrop.alpha = 0;

        while (canvasGrop.alpha < 1)
        {
            canvasGrop.alpha += Time.deltaTime / 1.2f;
            yield return null;
        }

        audioSource.Play();
    }

    public void Starting()
    {
        StartCoroutine(FadeOut());
    }

    public void Exit()
    {
        Application.Quit();
    }
}