using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemy : Enemy
{
    private SpawnManager spawnManagerScript;
    private bool bossDelay;

    // Start is called before the first frame update
    protected override void Start()
    {
        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!bossDelay)
        {
            StartCoroutine(BossSpawn());
            bossDelay = true;
        }
        base.Update();
    }

    protected override void Target()
    {

    }

    protected override void EnemyCleanUp()
    {
        base.EnemyCleanUp();
        spawnManagerScript.bossWave = false;
    }

    IEnumerator BossSpawn()
    {
        yield return new WaitForSeconds(3);
        spawnManagerScript.SpawnEnemyWave((spawnManagerScript.waveNumber)/2);
        bossDelay = false;
    }
}
