using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchManager : MonoBehaviour, IPointerClickHandler {
    public Ball_Moter player;
	// Use this for initialization
	void Start () {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        player.Ontouch();
    }

	// Update is called once per frame
	void Update () {
        
    }
}
