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

        rt.anchoredPosition = new Vector2(0, -300f);
    }
	
	// Update is called once per frame
	void Update () {
        
        if(isLevelUp)
        {
            if (rt.anchoredPosition.y < 0)
            {
                rt.localPosition += Vector3.up * 30f;
            }
            else
                rt.anchoredPosition = new Vector2(0, 0f);
        }
        else
        {
            if (rt.anchoredPosition.y > -300f)
            {
                rt.localPosition += Vector3.down * 30f;
            }
            else
                rt.anchoredPosition = new Vector2(0, -300f);
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

        playerHealthScript.SetStrength(20);

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
