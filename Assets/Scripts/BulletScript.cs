using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    Camera cam;
    Vector3 viewportPos;

    void Awake() {
        cam = Camera.main;
        viewportPos = cam.WorldToViewportPoint(transform.position);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        viewportPos = cam.WorldToViewportPoint(transform.position);
        //transform.position += transform.forward * 4f * Time.deltaTime;
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
}
