using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnManager : MonoBehaviour {

    public bool allKill = true;
    public GameObject ojt = null;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	    	
	}

    void ojtMaker(){
        if(allKill = true){
            for(int i = 0; i < 5; i++){
                Instantiate(ojt);
            }
        }
        else if (allKill){
            Debug.Log("");
        }

    }
}
