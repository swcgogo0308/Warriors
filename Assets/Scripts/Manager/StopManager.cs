using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StopManager : MonoBehaviour {

    public GameObject stopPopUp;
    public GameObject stopButton;

	public void Click()
    {
        stopPopUp.SetActive(true);
        Time.timeScale = 0;
        stopButton.SetActive(false);
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

    public void Return()
    {
        Time.timeScale = 1;
        stopPopUp.SetActive(false);
        stopButton.SetActive(true);
    }
}
