using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float x, y, z;
    Rigidbody2D rb;

    [SerializeField] Color thrustCol;
    
    [SerializeField] Light sun, sun2, thruster;
    int colorSeq = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.tag == "Player")
        {
            transform.Rotate(0,0,-Input.GetAxisRaw("Horizontal"));
            //rb.AddTorque(-Input.GetAxis("Horizontal"));
            if(Input.GetAxisRaw("Vertical")>0f)
            {
                rb.AddForce(transform.up*Input.GetAxisRaw("Vertical"));
                transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,(Time.time % 0.2f)*100f+80f);
                thruster.enabled = true;
            }
            else
            {
                transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(0,0);
                thruster.enabled = false;
            }
            if(Input.GetButtonDown("Jump"))
            {
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
                
                sun.color = Random.ColorHSV();
                sun2.color = Random.ColorHSV();
                thrustCol = Random.ColorHSV(0.5f,1f,0.5f,1f);
                thruster.color = thrustCol;
            }
        }   
        else
        {
            transform.Rotate(x,y,z);
        }
    }
}
