using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnManager : MonoBehaviour {

    private enum State
    {
        Spawning, AllKill, Survive, Idle
    }


    public Weapon[] weapons;

    public Enemy[] enemys;

    private Enemy[] enemyScripts;

    private GameObject[] enemysObject;

    private State roundState;

    public PlayerHealth playerHealth;

    public Transform player;

    public float spawnViewportMargin = 0.6f;

    private Transform enemyStorage;

    void Start () {
        Transform fallenWeaponStorage = new GameObject("FallenStorage").transform;
        fallenWeaponStorage.tag = "Fallen";
        enemyStorage = new GameObject("EnemyStorage").transform;

        StartCoroutine(StartGame());
        StartCoroutine(GameOver());
    }

    
    void Update()
    {
        
    }

    private IEnumerator GameOver()
    {
        while(true)
        {
            yield return null;

            if (playerHealth.isDead)
            {

                enemyScripts = FindObjectsOfType<Enemy>();

                for(int i = 0; i < enemyScripts.Length; i++)
                    enemyScripts[i].enabled = false;

                yield return new WaitForSeconds(3f);
                break;
            }
        }
        SceneManager.LoadScene("Main");
    }

    private IEnumerator StartGame()
    {
        float i = 0f;
        while (true)
        {
            roundState = State.Spawning;

            yield return EnemySpawn(i);

            while (roundState == State.Spawning)
            {
                yield return null;
            }

            i += 0.1f;
        }
    }

    IEnumerator EnemySpawn(float round)
    {
        WaitForSeconds spawnDelay = new WaitForSeconds(0.5f);

        int spawnMonsterCount = 1 + (int)round;

        if (spawnMonsterCount >= 3) spawnMonsterCount = 5;

        for (int count = 0; count < spawnMonsterCount; count++)
        {

            yield return spawnDelay;
            int randomMonb = Random.Range(0, enemys.Length);
            int randomWeapon = Random.Range(0, weapons.Length);

            Enemy enemy = enemys[randomMonb];
            Weapon weapon = weapons[randomWeapon];

            Enemy enemyObject = Instantiate(enemy.gameObject).GetComponent<Enemy>();
            Weapon weaponObject = Instantiate(weapon.gameObject).GetComponent<Weapon>();

            enemyObject.transform.position = GetRandomSpawnPoint();
            enemyObject.transform.SetParent(enemyStorage);


            weaponObject.transform.parent = enemyObject.transform;

            weaponObject.playerHealth = playerHealth;

            weaponObject.getButton = FindObjectOfType<Button>();

            enemyObject.SetStrength(0 + round);
        }

        yield return CheackEnemyCount();
    }

    IEnumerator CheackEnemyCount()
    {
        while (true)
        {
            yield return null;
            enemysObject = GameObject.FindGameObjectsWithTag("Enemy");

            if (enemysObject.Length == 0)
            {
                roundState = State.AllKill;
                yield break;
            }
        }
    }

    #region Random point

    private Vector3 GetRandomSpawnPoint()
    {
        Vector3 result;
        float x, y;
        float randomAxis = GetRandomAxis();
        float randomPosition = GetRandomPosition();

        randomAxis += 0.5f;
        randomPosition += 0.5f;

        RandomInit(randomAxis, randomPosition, out x, out y);

        result = Camera.main.ViewportToWorldPoint(new Vector2(x, y));
        result.z = 0;
        return result;
    }

    private void RandomInit(float value1, float value2, out float a, out float b)
    {
        if (Random.Range(0, 2) == 0)
        {
            float temp = value1;
            value1 = value2;
            value2 = temp;
        }

        a = value1;
        b = value2;
    }

    private float GetRandomAxis()
    {
        float result = spawnViewportMargin;
        if (Random.Range(0, 2) == 0)
        {
            result *= -1;
        }

        return result;
    }

    private float GetRandomPosition()
    {
        float result = Random.Range(0, spawnViewportMargin * 2) - spawnViewportMargin;
        return result;
    }
    #endregion
}
