using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody enemyRb;
    private GameObject player;
    public float speed = 3.0f;

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
        Vector3 lookDirection = player.transform.position - transform.position; 

        enemyRb.AddForce(lookDirection.normalized * speed);

        if (transform.position.y < -10)
        {
            Destroy(gameObject);
        }

        if (playerControllerScript.aboutToSmash && player.transform.position.y < 0.3)
        {
            enemyRb.AddForce(-lookDirection * 150);
        }
    }
}
