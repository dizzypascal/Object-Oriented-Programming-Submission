using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyPrefabs;
    private float spawnRange = 9;
    private int enemyCount;
    public int waveNumber = 0;
    [SerializeField] GameObject powerupPrefab;
    [SerializeField] GameObject firepowerPrefab;
    [SerializeField] GameObject smashpowerPrefab;
    [SerializeField] GameObject[] powerups;
    private int enemyIndex;
    private int powerupIndex;

    public bool bossWave;
    private int bossWaveCounter = 0;
    [SerializeField] GameObject bossEnemy;

    public bool gameOver;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameOver)
        {
            BossWave();
            NormalWave();
        }


    }




    void BossWave()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;

        if (enemyCount == 0 && bossWave)
        {
            Instantiate(bossEnemy, GenerateSpawnPosition(), bossEnemy.transform.rotation);
        }
    }

    void NormalWave()
    {
        if (enemyCount == 0 && !bossWave)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            Instantiate(RandomPowerup(), GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            bossWaveCounter++;
            if (bossWaveCounter == 4)
            {
                bossWave = true;
                bossWaveCounter = 0;
            }

        }
    }

    public void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            enemyIndex = Random.Range(0, enemyPrefabs.Length);
            Instantiate(enemyPrefabs[enemyIndex], GenerateSpawnPosition(), enemyPrefabs[enemyIndex].transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRange, spawnRange);
        float spawnPosZ = Random.Range(-spawnRange, spawnRange);
        Vector3 randPos = new Vector3(spawnPosX, 0, spawnPosZ);

        return randPos;
    }

    private GameObject RandomPowerup()
    {
        powerupIndex = Random.Range(0, powerups.Length);
        return powerups[powerupIndex];
    }


}
