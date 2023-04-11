using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float moveSpeedMult, torgueMult, shootForce = 1, shootCd = 0.5f, maxSpeed = 15f, invulnTime = 4;
    float shootCdTimer, invulnTimer;
    Rigidbody2D rb;

    Color thrustCol;
    Camera cam;
    Vector3 viewportPos;
    Transform shootPoint;

    SkinnedMeshRenderer shipMesh;
    GameManager manager;
    
    //[SerializeField] Light sun, sun2;
    Light thrusterLight;
    ParticleSystem thrusterParticles;
    ParticleSystem.EmissionModule tEmission;
    [SerializeField] GameObject bullet;
    //int colorSeq = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        shipMesh = transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();
        shootPoint = transform.GetChild(0).GetChild(0);
        thrusterLight = transform.GetChild(0).GetChild(1).GetComponent<Light>();
        thrusterParticles = transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>();
        cam = Camera.main;
        viewportPos = cam.WorldToViewportPoint(transform.position);
        tEmission = thrusterParticles.emission;

        invulnTimer = invulnTime;
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
            
        }
        
        if(Input.GetButton("Fire1") && shootCdTimer <= 0f)
        {
            GameObject newBullet = Instantiate(bullet,shootPoint.position,shootPoint.rotation);
            newBullet.transform.GetComponent<Rigidbody2D>().velocity = rb.velocity;
            newBullet.transform.GetComponent<Rigidbody2D>().AddForce(shootPoint.up*shootForce);
            Destroy(newBullet,0.6f);
            shootCdTimer = shootCd;
        }
        shootCdTimer -= Time.deltaTime;
        
        if(invulnTimer > 0)
        {
            gameObject.GetComponent<Collider2D>().enabled = false;
            if(invulnTimer % 1 > 0.5f)
            {
                shipMesh.enabled = true;
            }
            else{
                shipMesh.enabled = false;
            }
            invulnTimer -= Time.deltaTime;
        }
        else
        {
            shipMesh.enabled = true;
            gameObject.GetComponent<Collider2D>().enabled = true;
        }
    }

    private void FixedUpdate() {
        
        /*if(Mathf.Abs(rb.angularVelocity)<320)
        {
            rb.AddTorque(-Input.GetAxis("Horizontal")*torgueMult*Time.fixedDeltaTime);
        }*/

        transform.Rotate(new Vector3(0,0,-Input.GetAxis("Horizontal")*torgueMult*Time.fixedDeltaTime));

        if(yaxis>0f)
            {
                if(rb.velocity.magnitude < maxSpeed)
                    rb.AddForce(transform.up*moveSpeedMult*yaxis*Time.fixedDeltaTime);
                shipMesh.SetBlendShapeWeight(0,(Time.time % 0.2f)*100f+80f);
                thrusterLight.intensity = 5.0f + (Time.time % 0.3f)*4;
                thrusterLight.enabled = true;
                tEmission.enabled = true;
            }
            else
            {
                shipMesh.SetBlendShapeWeight(0,0);
                thrusterLight.enabled = false;
                tEmission.enabled = false;
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
    public void ResetInvuln()
    {
        invulnTimer = invulnTime;
    }
}
