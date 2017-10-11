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
    public UpgradeManager upgradeManagerScript;

    public Weapon[] weapons;

    public Enemy[] enemys;
     
    private Enemy[] enemyScripts;

    private GameObject[] enemysObject;

    private State roundState;

    public PlayerHealth playerHealth;

    public AudioSource audioSource;

    public Transform player;

    public float spawnViewportMargin = 0.6f;

    public RectTransform roundPanel;

    public Text roundPanelText;

    public int isEpicProbabile;

    private Transform enemyStorage;

    private bool isMoving;

    public Text roundText;

    private float round;

    public int maxSpawnCount;

    public int startSpawnCount;

    void Start () {
        roundPanel.anchoredPosition = new Vector2(1925, 0);
        //Transform fallenWeaponStorage = new GameObject("FallenStorage").transform;
        //fallenWeaponStorage.tag = "FallenStorage";
        enemyStorage = new GameObject("EnemyStorage").transform;

        PlayerPrefs.SetInt("EnemyStrenge_H", 0);

        audioSource.Play();

        StartCoroutine(StartGame());
        StartCoroutine(GameOverCheck());
    }

    void Update()
    {
        if (isMoving)
        {
            if (roundPanel.anchoredPosition.x > 0f)
            {
                roundPanel.localPosition += Vector3.left * 30f;
            }
            else
                roundPanel.anchoredPosition = new Vector2(0, 0);
        }
        else
        {
            if (roundPanel.anchoredPosition.x < 1925f)
            {
                roundPanel.localPosition += Vector3.right * 30f;
            }
            else
                roundPanel.anchoredPosition = new Vector2(1925, 0);
        }
    }

    private IEnumerator GameOverCheck()
    {
        while(true)
        {
            yield return null;

            if (playerHealth.isDead)
            {

                enemyScripts = FindObjectsOfType<Enemy>();

                for (int i = 0; i < enemyScripts.Length; i++)
                    enemyScripts[i].gameObject.SetActive(false);

                yield return new WaitForSeconds(4f);
                break;
            }
        }



        SceneManager.LoadScene("Main");
    }

    private IEnumerator StartGame()
    {
        round = 1f;

        while (true)
        {
            if(round >= PlayerPrefs.GetFloat("BestRound"))
                PlayerPrefs.SetFloat("BestRound", round);

            PlayerPrefs.SetInt("EnemyStrenge_H",(int)round / 5);

            if (round != 1f)
                playerHealth.FillHealth();

            isMoving = true;

            roundPanelText.text = "Round " + round;

            roundText.text = "Round : " + round;

            yield return new WaitForSeconds(2f);

            isMoving = false;

            roundState = State.Spawning;

            yield return EnemySpawn(round);

            while (roundState == State.Spawning)
            {
                yield return null;
            }

            if(round % 5 == 0 && round != 0)
            {
                upgradeManagerScript.isLevelUp = true;
                upgradeManagerScript.isOnButton = false;
            }

            round += 1f;
            
        }
    }

    IEnumerator EnemySpawn(float round)
    {
        if (playerHealth.isDead) yield break ;

        WaitForSeconds spawnDelay = new WaitForSeconds(0.5f);

        int spawnMonsterCount = startSpawnCount + (int)(round * 0.2f);

        if (spawnMonsterCount >= maxSpawnCount) spawnMonsterCount = maxSpawnCount;

        for (int count = 0; count < spawnMonsterCount; count++)
        {
            yield return spawnDelay;

            int randomMonb = Random.Range(0, enemys.Length);
            int randomWeapon = Random.Range(0, weapons.Length);

            Enemy enemy = enemys[randomMonb];
            Weapon weapon = weapons[randomWeapon];

            if (round % 10 == 0)
            {

                if (count < spawnMonsterCount - 1)
                {
                    while (weapon.weaponGrade != Weapon.Grade.Normal)
                    {
                        randomWeapon = Random.Range(0, weapons.Length);
                        weapon = weapons[randomWeapon];
                    }
                }
                else if (count == spawnMonsterCount - 1)
                {
                    while (weapon.weaponGrade != Weapon.Grade.Speacial)
                    {
                        randomWeapon = Random.Range(0, weapons.Length);
                        weapon = weapons[randomWeapon];
                    }
                }

                Enemy enemyObject = Instantiate(enemy.gameObject).GetComponent<Enemy>();
                Weapon weaponObject = Instantiate(weapon.gameObject).GetComponent<Weapon>();

                enemyObject.transform.position = GetRandomSpawnPoint();
                enemyObject.transform.SetParent(enemyStorage);


                weaponObject.transform.parent = enemyObject.transform;

                weaponObject.playerHealth = playerHealth;

                weaponObject.getButton = FindObjectOfType<Button>();

                if (count < spawnMonsterCount - 1)
                    enemyObject.SetStrength(PlayerPrefs.GetInt("EnemyStrenge_H"));

                else if (count == spawnMonsterCount - 1)
                {
                    enemyObject.SetStrength(PlayerPrefs.GetInt("EnemyStrenge_H") * 4);
                    enemyObject.GetComponent<SpriteRenderer>().color = new Color(255f, 100f, 0);
                }

            }

            else
            {

                if (Random.Range(1, 100) <= isEpicProbabile)
                {
                    while (weapon.weaponGrade != Weapon.Grade.Epic)
                    {
                        randomWeapon = Random.Range(0, weapons.Length);
                        weapon = weapons[randomWeapon];
                    }
                }
                else
                {
                    while (weapon.weaponGrade != Weapon.Grade.Normal)
                    {
                        randomWeapon = Random.Range(0, weapons.Length);
                        weapon = weapons[randomWeapon];
                    }
                }

                Enemy enemyObject = Instantiate(enemy.gameObject).GetComponent<Enemy>();
                Weapon weaponObject = Instantiate(weapon.gameObject).GetComponent<Weapon>();

                enemyObject.transform.position = GetRandomSpawnPoint();
                enemyObject.transform.SetParent(enemyStorage);


                weaponObject.transform.parent = enemyObject.transform;

                weaponObject.playerHealth = playerHealth;

                weaponObject.getButton = FindObjectOfType<Button>();

                enemyObject.SetStrength(PlayerPrefs.GetInt("EnemyStrenge_H"));
            }

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
                yield return new WaitForSeconds(4f);
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
