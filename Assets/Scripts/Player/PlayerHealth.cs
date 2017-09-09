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
    bool isDead;
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
        if (isHitDelay) return;
        StartCoroutine(HitDelay());
        isDamage = true;
        currentHealth -= damage;
        HealthBar.value = currentHealth;

        if(currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    IEnumerator HitDelay()
    {
        if (isHitDelay) yield break;
        isHitDelay = true;
        yield return new WaitForSeconds(hitDelay);
        isHitDelay = false;
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
        Instantiate(myWeapon);
        gameObject.SetActive(false);
    }
}
