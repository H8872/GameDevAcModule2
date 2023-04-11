using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScript : MonoBehaviour
{
    [SerializeField]
    GameObject bullet;
    [SerializeField]
    Transform shootPoint;
    [SerializeField]
    float moveSpeed, bulletInterval, bulletLifetime;

    float bulletTime;

    // Start is called before the first frame update
    void Start()
    {
        bulletTime = bulletInterval;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(0,0,transform.position.z-moveSpeed*Time.deltaTime);
        CreateBullet();
        transform.Rotate(0,moveSpeed*Time.deltaTime,0);
    }

    void CreateBullet()
    {
        if(bulletTime <= 0f)
        {
            GameObject newBullet = Instantiate(bullet, shootPoint.position, shootPoint.transform.rotation);
            
            newBullet.GetComponent<Rigidbody>().AddForce(newBullet.transform.forward * moveSpeed);
            Destroy(newBullet, bulletLifetime);

            bulletTime = bulletInterval;
        }

        bulletTime -= Time.deltaTime;
    }
}
