using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    enum EnemyType {ASTEROID, UFO, SMALSTEROID}
    [SerializeField] EnemyType enemyType;
    float rotationSpeed, xspeed, yspeed, smolroidAmount;
    Vector3 viewportPos;
    Camera cam;
    bool destroySelf;
    GameManager manager;
    [SerializeField] GameObject asteroidFab;
    AudioSource audioSource;
    [SerializeField] AudioClip explosionClip;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        audioSource = transform.GetComponent<AudioSource>();

        smolroidAmount = 3;

        cam = Camera.main;
        viewportPos = cam.WorldToViewportPoint(transform.position);

        if(enemyType == EnemyType.ASTEROID || enemyType == EnemyType.SMALSTEROID)
        {
            xspeed = Random.Range(-100f,100f);
            yspeed = Random.Range(-100f,100f);

            //child = transform.GetChild(0).gameObject;
            rotationSpeed = Random.Range(-90f,90f);
            gameObject.GetComponent<Rigidbody2D>().AddTorque(rotationSpeed);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(xspeed,yspeed)* 10);
        }

        if(enemyType == EnemyType.SMALSTEROID)
        {
            transform.localScale = new Vector3(0.5f,0.5f,0.5f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyType != EnemyType.UFO)
        {
            viewportPos = cam.WorldToViewportPoint(transform.position);
            Vector3 newPosition = transform.position;
            if (viewportPos.x > 1.01f || viewportPos.x < -0.01f)
            {
                newPosition.x = -(newPosition.x*0.95f);
            }
            if (viewportPos.y > 1.01f || viewportPos.y < -0.01f)
            {
               newPosition.y = -(newPosition.y*0.95f);
            }
            transform.position = newPosition;
            //transform.RotateAround(transform.position, Vector3.forward,rotationSpeed*Time.deltaTime);
        }
        else if(enemyType == EnemyType.UFO)
        {
            transform.GetChild(0).Rotate(0f,-90f*Time.deltaTime,0f);
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.left * 1000 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Bullet" && !destroySelf)
        {
            Destroy(other.gameObject);
            KillSelf();
            destroySelf = true;
        }
    }

    public void KillSelf()
    {
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<ParticleSystem>().Play();
        switch(enemyType)
        {
            case EnemyType.ASTEROID:
                manager.AsteroidCount--;
                for (int i = 0; i < smolroidAmount; i++)
                {
                    GameObject newroid = Instantiate(asteroidFab, transform.position, transform.rotation);
                    newroid.GetComponent<Collider2D>().enabled = true;
                    newroid.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                    newroid.GetComponent<EnemyControl>().enemyType = EnemyType.SMALSTEROID;
                }
                manager.Score += 10;
                audioSource.clip = explosionClip;
                audioSource.Play();
                Destroy(gameObject, 2f);
                break;
            case EnemyType.SMALSTEROID:
                manager.Score += 25;
                audioSource.clip = explosionClip;
                audioSource.Play();
                Destroy(gameObject, 2f);
                break;
            case EnemyType.UFO:
                manager.Score += 100;
                audioSource.clip = explosionClip;
                audioSource.Play();
                Destroy(gameObject, 2f);
                break;
            default:
                Destroy(gameObject);
                break;
        }
        manager.UpdateUI();
    }

    private void OnDestroy() {
        //if(enemyType == EnemyType.ASTEROID)
        //else if(enemyType == EnemyType.SMALSTEROID)
        //else if(enemyType == EnemyType.UFO)
    }
}
