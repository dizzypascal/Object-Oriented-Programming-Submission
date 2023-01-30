using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootForward : MonoBehaviour
{
    private float bulletspeed = 10f;

    private Rigidbody objectRb;

    private Transform _target;

    private bool startHome;

    private GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        objectRb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {

        if ( transform.position.x > 15 || transform.position.x < -15 || transform.position.z > 15 || transform.position.z < -15)
        {
            Destroy(gameObject);
        }

        if (Vector3.Distance(transform.position, player.transform.position) < 0.5f)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {

        if (FindClosestEnemy() != null && !startHome)
        {
            objectRb.velocity = transform.right * bulletspeed;
            StartCoroutine(homingDelay());
        }

        if (FindClosestEnemy() != null && startHome)
        {

            objectRb.velocity = transform.up * bulletspeed;
            _target = FindClosestEnemy().transform;
            Vector3 dir = (_target.position - objectRb.position).normalized;
            float rotateAmount = Vector3.Cross(dir, transform.right).z;
            objectRb.angularVelocity = _target.position * rotateAmount;
            objectRb.velocity = dir * bulletspeed;
        }
    }

    IEnumerator homingDelay()
    {
        yield return new WaitForSeconds(0.6f);
        startHome = true;

    }

    public GameObject FindClosestEnemy()
    {
        GameObject[] enemies;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;

        foreach (GameObject go in enemies)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;
            enemyRb.AddForce(awayFromPlayer * bulletspeed, ForceMode.Impulse);

        }

        if (collision.gameObject.CompareTag("Player"))
        {

            Destroy(gameObject);


        }

        if (collision.gameObject)
        {
            Destroy(gameObject);
        }


    }
}
