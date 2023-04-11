using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    enum EnemyType {ASTEROID, UFO, SMALSTEROID}
    [SerializeField] EnemyType enemyType;
    float rotationSpeed;
    float xspeed, yspeed;
    Vector3 viewportPos;
    Camera cam;

    GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();

        cam = Camera.main;
        viewportPos = cam.WorldToViewportPoint(transform.position);

        if(enemyType == EnemyType.ASTEROID)
        {
            xspeed = Random.Range(-100f,100f);
            yspeed = Random.Range(-100f,100f);

            //child = transform.GetChild(0).gameObject;
            rotationSpeed = Random.Range(-90f,90f);
            gameObject.GetComponent<Rigidbody2D>().AddTorque(rotationSpeed);
            gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(xspeed,yspeed)* 10);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(transform.position, Vector3.forward,rotationSpeed*Time.deltaTime);
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
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Bullet")
        {
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }

    private void OnDestroy() {
        if(enemyType == EnemyType.ASTEROID)
        {
            manager.Score += 10;
            manager.AsteroidCount--;
        }
        else if(enemyType == EnemyType.SMALSTEROID)
            manager.Score += 50;
        else if(enemyType == EnemyType.UFO)
            manager.Score += 100;
    }
}
