using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Rigidbody enemyRb;
    protected GameObject player;
    protected float speed = 3.0f;
    protected Vector3 lookDirection;

    protected PlayerController playerControllerScript;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        enemyRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");

        playerControllerScript = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {

        CheckForPlayer();
        Target();
        EnemyCleanUp();
        Smashed();
    }

    protected virtual void Target()
    {
        enemyRb.AddForce(lookDirection.normalized * speed);
    }

    protected virtual void EnemyCleanUp()
    {
        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void Smashed()
    {
        if (playerControllerScript.aboutToSmash && player.transform.position.y < 0.3)
        {
            enemyRb.AddForce(-lookDirection * 150) ;
        }
    }

    protected void CheckForPlayer()
    {
        if (player != null)
        {
            lookDirection = player.transform.position - transform.position;
        }
    }
}
