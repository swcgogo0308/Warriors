using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Main : MonoBehaviour {

    public Text bestRoundText;
    public AudioSource audioSource;
    private float bestRound;
    CanvasGroup canvasGrop;

    void Start()
    {
        PlayerPrefs.SetInt("DamageUp", 0);
        canvasGrop = GetComponent<CanvasGroup>();
        StartCoroutine(FadeIn());
        bestRound = PlayerPrefs.GetFloat("BestRound");
        bestRoundText.text = "Best Round : " + bestRound;
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