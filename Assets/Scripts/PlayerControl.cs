using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float moveSpeedMult, torgueMult, shootForce = 1;
    Rigidbody2D rb;

    Color thrustCol;
    Camera cam;
    Vector3 viewportPos;
    Transform shootPoint;

    SkinnedMeshRenderer shipMesh;
    GameManager manager;
    
    [SerializeField] Light sun, sun2;
    Light thruster;
    [SerializeField] GameObject bullet;
    //int colorSeq = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        shipMesh = transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();
        shootPoint = transform.GetChild(0).GetChild(0);
        thruster = transform.GetChild(0).GetChild(1).GetComponent<Light>();
        cam = Camera.main;
        viewportPos = cam.WorldToViewportPoint(transform.position);
    }

    float yaxis = 0;

    // Update is called once per frame
    void Update()
    {
        viewportPos = cam.WorldToViewportPoint(transform.position);
        
        //Screen wrap around
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

        yaxis = Input.GetAxisRaw("Vertical");
        
        //transform.Rotate(0,0,-Input.GetAxisRaw("Horizontal"));
        
        if(Input.GetButtonDown("Jump"))
        {
            manager.ChangeLights();
            manager.SpawnAsteroids(1);

            /*switch(colorSeq)
            {
                case 0:
                    sun.color = Color.blue;
                    sun2.color = Color.blue;
                    break;
                case 1:
                    sun.color = Color.red;
                    sun2.color = Color.red;
                    break;
                case 2:
                    sun.color = Color.yellow;
                    sun2.color = Color.yellow;
                    break;
                default:
                    sun.color = Color.green;
                    sun2.color = Color.green;
                    colorSeq = 0;
                    break;
            }
            colorSeq++;*/
            
            //sun.color = Random.ColorHSV();
            //sun2.color = Random.ColorHSV();
            //thrustCol = Random.ColorHSV(0f,1f,0f,1f,0.8f,1f);
            //thruster.color = thrustCol;
            
        }
        
        if(Input.GetButtonDown("Fire1"))
        {
            GameObject newBullet = Instantiate(bullet,shootPoint.position,shootPoint.rotation);
            newBullet.transform.GetComponent<Rigidbody2D>().AddForce(shootPoint.up*shootForce);
            Destroy(newBullet,2f);
        }

    }

    private void FixedUpdate() {
        
        if(Mathf.Abs(rb.angularVelocity)<320)
            rb.AddTorque(-Input.GetAxis("Horizontal")*torgueMult*Time.fixedDeltaTime);
        
        if(yaxis>0f)
            {
                if(true)
                    rb.AddForce(transform.up*moveSpeedMult*yaxis*Time.fixedDeltaTime);
                shipMesh.SetBlendShapeWeight(0,(Time.time % 0.2f)*100f+80f);
                thruster.intensity = 5.0f + (Time.time % 0.3f)*4;
                thruster.enabled = true;
            }
            else
            {
                shipMesh.SetBlendShapeWeight(0,0);
                thruster.enabled = false;
            }
    }

    int value = 0;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Collectable")
        {
            Destroy(other.gameObject);
            value++;
            Debug.Log("hitted " + value);
        }

    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
