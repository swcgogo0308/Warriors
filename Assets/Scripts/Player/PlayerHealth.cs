﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayerHealth : MonoBehaviour {

    public Slider HealthBar;

    public Animator anim;

    public PlayerMove playerMovement;

    public AudioSource audioSource;

    public AudioClip takeDamage;

    public AudioClip death;

    public Image damageImage;

    public Weapon myWeapon;

    public float flashSpeed = 1f;
    public Color flashColor = new Color(1f, 0f, 0f, 0.1f);

    public float hitDelay;

    public int maxHealth;

    public int currentHealth;

    public Text healthText;

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

        healthText.text = "" + currentHealth ;

        HealthBar.value = currentHealth;
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

        if(currentHealth <= 0 && !isDead)
        {
            Death();
        }

        audioSource.clip = takeDamage;
        audioSource.Play();
    }

    public void SetStrength(int strength)
    {
        maxHealth = strength + maxHealth;

        HealthBar.maxValue = maxHealth;
    }

    public void LevelUp()
    {
        currentHealth = maxHealth;
    }

    public void FillHealth()
    {
        currentHealth += maxHealth / 10;

        if (currentHealth >= maxHealth)
            currentHealth = maxHealth;
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
        yield return new WaitForSeconds(0.8f);
        audioSource.clip = death;
        audioSource.Play();
        yield return new WaitForSeconds(0.4f);
        Transform fallenWeaponStorage = GameObject.FindGameObjectWithTag("Fallen").transform;
        fallenWeaponStorage.position = transform.position;

        myWeapon.gameObject.transform.parent = fallenWeaponStorage;
        Destroy(gameObject);
    }
}
