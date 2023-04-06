using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    GameObject child;
    float rotationSpeed;
    float xspeed, yspeed;

    // Start is called before the first frame update
    void Start()
    {
        xspeed = Random.Range(-100f,100f);
        yspeed = Random.Range(-100f,100f);

        //child = transform.GetChild(0).gameObject;
        rotationSpeed = Random.Range(-90f,90f);
        gameObject.GetComponent<Rigidbody2D>().AddTorque(rotationSpeed);
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(xspeed,yspeed)* 10);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(transform.position, Vector3.forward,rotationSpeed*Time.deltaTime);
    }
}
