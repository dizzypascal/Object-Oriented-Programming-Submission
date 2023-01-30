using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;
    private float speed = 3.0f;
    private Vector3 lookDirection;

    private PlayerController playerControllerScript;

    // Start is called before the first frame update
    void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        playerControllerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

        CheckForPlayer();
        Target();
        EnemyCleanUp();
        Smashed();
    }

    private void Target()
    {

        enemyRb.AddForce(lookDirection.normalized * speed);
    }

    private void EnemyCleanUp()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    private void Smashed()
    {
        if (playerControllerScript.aboutToSmash && player.transform.position.y < 0.3)
        {
            enemyRb.AddForce(-lookDirection * 150);
        }
    }

    private void CheckForPlayer()
    {
        if (player != null)
        {
            lookDirection = player.transform.position - transform.position;
        }
    }
}
