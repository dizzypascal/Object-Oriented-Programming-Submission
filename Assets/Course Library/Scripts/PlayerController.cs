using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    [SerializeField] GameObject gameOverScreen;
    static bool hasPowerup;
    static bool hasFirepower;
    static bool hasSmashpower;
    private float powerUpStrength = 15.0f;
    [SerializeField] GameObject powerupIndicator;
    [SerializeField] GameObject bullets;
    private bool bulletsDelay;

    private Vector3 startPos;
    private Vector3 endPos;
    private float jumpUpTime = 1f;
    private float jumpDownTime = 0.1f;
    private float elapsedTime;
    public bool aboutToSmash;

    private SpawnManager spawnManagerScript;
    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        spawnManagerScript = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        PowerUpIndicatorFollow();
        Shoot();
        SmashAttack();
        GameOver();
    }

    private void Move()
    {
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);
    }

    private void Shoot()
    {
        if (hasFirepower && bulletsDelay == true)
        {
            StartCoroutine(Shootingdelay());
            Instantiate(bullets, transform.position + new Vector3(1, 0, 0), Quaternion.Euler(new Vector3(0, 45, 0)));
            Instantiate(bullets, transform.position + new Vector3(0, 0, -1), Quaternion.Euler(new Vector3(0, 135, 0)));
            Instantiate(bullets, transform.position + new Vector3(-1, 0, 0), Quaternion.Euler(new Vector3(0, 225, 0)));
            Instantiate(bullets, transform.position + new Vector3(0, 0, 1), Quaternion.Euler(new Vector3(0, 315, 0)));
            bulletsDelay = false;
        }
    }

    private void PowerUpIndicatorFollow()
    {
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);
    }

    private void SmashAttack()
    {
        if (hasSmashpower && Input.GetKeyDown(KeyCode.Space) && !aboutToSmash)

        {
            startPos = transform.position;
            endPos = new Vector3(transform.position.x, 50, transform.position.z);
            elapsedTime += Time.deltaTime;
            float percentageCompleteUp = elapsedTime / jumpUpTime;
            transform.position = Vector3.Lerp(startPos, endPos, percentageCompleteUp);
            //StartCoroutine(JumpDown());
            aboutToSmash = true;
        }
    }

    IEnumerator Shootingdelay()
    {
        yield return new WaitForSeconds(2f);
        bulletsDelay = true;
    }

    IEnumerator JumpDown()
    {
        yield return new WaitForSeconds(1f);
        startPos = transform.position;
        endPos = new Vector3(transform.position.x, 0, transform.position.z);
        elapsedTime += Time.deltaTime;
        float percentageCompleteDown = elapsedTime / jumpDownTime;
        transform.position = Vector3.Lerp(startPos, endPos, percentageCompleteDown);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            powerupIndicator.gameObject.SetActive(true);
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountdownRoutine());
        }
 
        if (other.CompareTag("Firepower"))
        {
            hasFirepower = true;
            bulletsDelay = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountdownRoutine());
        }

        if (other.CompareTag("Smashpower"))
        {
            hasSmashpower = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountdownRoutine());
        }

    }

    IEnumerator PowerUpCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        hasPowerup = false;
        hasFirepower = false;
        hasSmashpower = false;
        powerupIndicator.gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRb = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = collision.gameObject.transform.position - transform.position;

            enemyRb.AddForce(awayFromPlayer * powerUpStrength, ForceMode.Impulse);

            Debug.Log("Colided with: " + collision.gameObject.name + " with powerup set to " + hasPowerup);
        }

        if (collision.gameObject.CompareTag("Floor") && aboutToSmash)
        {
            aboutToSmash = false;
        }
    }

    private void GameOver()
    {
        if (transform.position.y < -10f)
        {
            gameOverScreen.SetActive(true);
            spawnManagerScript.gameOver = true;

            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length > 0)
            {
                for (int e = 0; e < enemies.Length; e++)
                {
                    Destroy(enemies[e]);
                }
            }

            Destroy(gameObject);

        }
    }
}
