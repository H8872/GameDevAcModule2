using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject playerFab, asteroid;
    GameObject player;
    [SerializeField]
    Light sun1, sun2;
    Light thruster;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        thruster = player.transform.GetChild(0).GetChild(1).GetComponent<Light>();
    }

    public void ChangeLights()
    {
        sun1.color = Random.ColorHSV();
        sun2.color = Random.ColorHSV();
        thruster.color = Random.ColorHSV(0f,1f,0f,1f,0.8f,1f);
    }

    public void SpawnAsteroids(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject roid = Instantiate(asteroid, new Vector3(Random.Range(-8,8),Random.Range(-4,4),0f),Quaternion.Euler(0,0,0));
            roid.transform.GetChild(0).rotation = Quaternion.Euler(Random.Range(0,360),Random.Range(0,360),Random.Range(0,360));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
