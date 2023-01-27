using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody playerRb;
    private GameObject focalPoint;
    public bool hasPowerup;
    public bool hasFirepower;
    public bool hasSmashpower;
    private float powerUpStrength = 15.0f;
    public GameObject powerupIndicator;
    public GameObject bullets;
    private bool bulletsDelay;

    private Vector3 startPos;
    private Vector3 endPos;
    private float jumpUpTime = 1f;
    private float jumpDownTime = 0.1f;
    private float elapsedTime;
    public bool aboutToSmash;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");


    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");

        playerRb.AddForce(focalPoint.transform.forward * forwardInput * speed);
        
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        if (hasFirepower && bulletsDelay == true)
        {
            StartCoroutine(Shootingdelay());
            Instantiate(bullets, transform.position + new Vector3(1, 0, 0), Quaternion.Euler(new Vector3(0, 45, 0)));
            Instantiate(bullets, transform.position + new Vector3(0, 0, -1), Quaternion.Euler(new Vector3(0, 135, 0)));
            Instantiate(bullets, transform.position + new Vector3(-1, 0, 0), Quaternion.Euler(new Vector3(0, 225, 0)));
            Instantiate(bullets, transform.position + new Vector3(0, 0, 1), Quaternion.Euler(new Vector3(0, 315, 0)));
            bulletsDelay = false;
        }

        SmashAttack();

    }

    public void SmashAttack()
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
}