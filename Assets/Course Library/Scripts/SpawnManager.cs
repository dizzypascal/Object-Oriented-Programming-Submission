using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    private float spawnRange = 9;
    public int enemyCount;
    public int waveNumber = 0;
    public GameObject powerupPrefab;
    public GameObject firepowerPrefab;
    public GameObject smashpowerPrefab;
    public GameObject[] powerups;
    private int enemyIndex;
    private int powerupIndex;

    public bool bossWave;
    public int bossCount;
    public int bossWaveCounter = 0;
    public GameObject bossEnemy;

    // Start is called before the first frame update
    void Start()
    {

        //SpawnEnemyWave(waveNumber);
        //Instantiate(RandomPowerup(), GenerateSpawnPosition(), powerupPrefab.transform.rotation);
        //Instantiate(firepowerPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
        //Instantiate(smashpowerPrefab, GenerateSpawnPosition(), powerupPrefab.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Enemy>().Length;

        if (enemyCount == 0 && bossWave)
        {
            Instantiate(bossEnemy, GenerateSpawnPosition(), bossEnemy.transform.rotation);
            SpawnEnemyWave(waveNumber);
            StartCoroutine(BossSpawn());
        }

        if (enemyCount == 0 && !bossWave)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
            Instantiate(RandomPowerup(), GenerateSpawnPosition(), powerupPrefab.transform.rotation);
            bossWaveCounter ++;
            if (bossWaveCounter == 4)
            {
                bossWave = true;
                bossWaveCounter = 0;
            }

        }




    }


    IEnumerator BossSpawn()
    {
        yield return new WaitForSeconds(1);
        bossWave = false;
       
    }

    void SpawnEnemyWave(int enemiesToSpawn)
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
