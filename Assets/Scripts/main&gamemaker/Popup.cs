using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour {

    public void Up()
    {
        gameObject.SetActive(true);
    }

    public void Down()
    {
        gameObject.SetActive(false);
    }
}
