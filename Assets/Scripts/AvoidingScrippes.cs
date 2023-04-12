using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidingScrippes : MonoBehaviour
{
    [SerializeField] Transform circle, square, triangle;
    [SerializeField] Vector3 velocity, acceleration, direction;
    [SerializeField] float distance, maxDistance = 2f, shootTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
        distance = (circle.position-square.position).magnitude;
        direction = (circle.position-square.position).normalized;
        if(distance < maxDistance)
        {
            Debug.Log("Too Close!");
            square.GetComponent<Renderer>().material.color = Color.red;
            square.position += -direction * (maxDistance-distance) * Time.deltaTime;
            square.transform.up = direction;
            if(shootTime <= 0)
            {
                GameObject newTri = Instantiate(triangle, square.position, square.rotation).gameObject;
                newTri.GetComponent<Rigidbody2D>().AddForce(direction*100f);
                Destroy(newTri, 1f);
                shootTime = 0.5f;
            }
            else
                shootTime -= Time.deltaTime;
        }
        else
        {
            Debug.Log("This is fine...");
            square.GetComponent<Renderer>().material.color = Color.white;
        }
    }
}
