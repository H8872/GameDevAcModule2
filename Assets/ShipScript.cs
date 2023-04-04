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
        transform.position = new Vector3(0,0,transform.position.z-moveSpeed*Time.deltaTime);
        CreateBullet();
        //Destroy(gameObject, 2f);
    }

    void CreateBullet()
    {
        if(bulletTime <= 0f)
        {
            GameObject newBullet = Instantiate(bullet, shootPoint.position, bullet.transform.rotation);
            Destroy(newBullet, bulletLifetime);

            bulletTime = bulletInterval;
        }

        bulletTime -= Time.deltaTime;
    }
    
    private void OnDestroy() {
        Debug.Log("Im ded.");
    }
}
