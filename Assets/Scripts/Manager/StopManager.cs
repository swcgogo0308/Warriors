using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopManager : MonoBehaviour {

    public GameObject StopPopUp;
    public bool isClick;

	public void Click()
    {
        if (!isClick)
        {
            StopPopUp.SetActive(true);
            Time.timeScale = 0;
            isClick = true;
        }
        else
        {
            StopPopUp.SetActive(false);
            Time.timeScale = 1;
            isClick = false;
        }
    }

    public void ReStart()
    {
        Time.timeScale = 1;
        Application.LoadLevel("InGame");
    }

    public void Home()
    {
        Time.timeScale = 1;
        Application.LoadLevel("Main");
    }
}
