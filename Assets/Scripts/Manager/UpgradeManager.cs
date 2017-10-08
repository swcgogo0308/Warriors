using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeManager : MonoBehaviour {

    public Button attackButton;
    public Button healthButton;
    public Button speedButton;
    private RectTransform rt;
    public PlayerHealth playerHealthScript;
    public PlayerMove playerMoveScript;

    public bool isLevelUp;

    public bool isOnButton;

    // Use this for initialization
    void Start () {
        PlayerPrefs.SetInt("DamageUp", 0);

        rt = GetComponent<RectTransform>();

        rt.anchoredPosition = new Vector2(0, -115f);
    }
	
	// Update is called once per frame
	void Update () {
        
        if(isLevelUp)
        {
            if (rt.anchoredPosition.y <= 0)
            {
                rt.localPosition += Vector3.up * 3f;
            }
        }
        else
        {
            if (rt.anchoredPosition.y >= -115f)
            {
                rt.localPosition += Vector3.down * 3f;
            }
        }
    }

    public void DamageUp()
    {
        if (isOnButton) return;

        isOnButton = true;

        int saveDamage = PlayerPrefs.GetInt("DamageUp");

        PlayerPrefs.SetInt("DamageUp", saveDamage + 10);

        playerMoveScript.myWeapon.damage += PlayerPrefs.GetInt("DamageUp");

        playerHealthScript.LevelUp();

        isLevelUp = false;
    }

    public void HealthUp()
    {
        if (isOnButton) return;

        isOnButton = true;

        playerHealthScript.SetStrength(50);

        playerHealthScript.LevelUp();

        isLevelUp = false;
    }

    public void SpeedUp()
    {
        if (isOnButton) return;

        isOnButton = true;

        playerMoveScript.moveSpeed += 1f;

        playerHealthScript.LevelUp();

        isLevelUp = false;
    }
}
