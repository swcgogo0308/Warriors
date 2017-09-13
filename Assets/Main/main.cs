using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class main : MonoBehaviour {//씬이동
    /*new public void SceneTrans1_2() {
        Application.LoadLevel("InGame");
    }
        */
    new public void FadeMe(){//FadeOut호출
        StartCoroutine("DoFade");
    }

    IEnumerator DoFade(){//FadeOut
        CanvasGroup canvasGrop = GetComponent<CanvasGroup>();
        while (canvasGrop.alpha > 0)
        {
            canvasGrop.alpha -= Time.deltaTime / 2;
            yield return null;
        }
        canvasGrop.interactable = false;
        yield return null;

        Application.LoadLevel("InGame");
    }
    public void Starting()
    {
        StartCoroutine(DoFade());
    }
    void Update () { }
}