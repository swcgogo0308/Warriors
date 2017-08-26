using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Score : MonoBehaviour {
    public int scorePoint = 0;

    private Text scoreTx;

    void Awake()
    {
        scoreTx = GameObject.Find("Panel").transform.Find("ScoreTx").GetComponent<Text>();
        scoreTx.text = "버틴시간 : 0";
        StartCoroutine("PlusScoreRoutine");
    }

    public void PlusScore(int plusPoint)
    {
        scorePoint += plusPoint;
        scoreTx.text = "버틴시간 : " + scorePoint.ToString();
    }
    IEnumerator PlusScoreRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            PlusScore(1);
        }
    }
}
