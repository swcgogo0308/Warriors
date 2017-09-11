using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerHealth : MonoBehaviour {

    public Slider HealthBar;

    public Animator anim;

    public PlayerMove playerMovement;

    public Image damageImage;

    public Weapon myWeapon;

    public float flashSpeed = 1f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);

    public float hitDelay;

    public int maxHealth;

    private int currentHealth;

    bool isDamage;
    public bool isDead;
    bool isHitDelay;

	void Start () {
        HealthBar.value = maxHealth;
        currentHealth = maxHealth;

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Weapon"))
                myWeapon = child.GetComponent<Weapon>();
        }
    }

    void Update()
    {
        DamageEffect();
    }

    void DamageEffect()
    {
        if (isDead) return; 

        if (isDamage)
        {
            damageImage.color = flashColor;
        }
        else
        {
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        isDamage = false;

    }

    public void TakeDamage(int damage)
    {
        isDamage = true;
        currentHealth -= damage;
        HealthBar.value = currentHealth;

        if(currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }


    void Death()
    {
        isDead = true;
        StartCoroutine(DeathEffect());
        
        playerMovement.enabled = false;
    }

    IEnumerator DeathEffect()
    {
        anim.SetBool("isDead", true);
        yield return new WaitForSeconds(1.5f);
        Transform fallenWeaponStorage = GameObject.FindGameObjectWithTag("Fallen").transform;
        fallenWeaponStorage.position = this.transform.position;

        myWeapon.gameObject.transform.parent = fallenWeaponStorage;
        gameObject.SetActive(false);
    }
}
