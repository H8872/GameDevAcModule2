using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerFab, asteroid;
    GameObject player;
    [SerializeField] Light sun1, sun2;
    Light thruster;
    public float Score {get{return score;} set{score = value;}}
    public float AsteroidCount {get{return asteroidCount;} set{asteroidCount = value;}}
    [SerializeField] float spawnAmount = 8, asteroidCount = 0, score = 0;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        thruster = player.transform.GetChild(0).GetChild(1).GetComponent<Light>();

        SpawnAsteroids(spawnAmount);
    }

    public void ChangeLights()
    {
        sun1.color = Random.ColorHSV();
        sun2.color = Random.ColorHSV();
        thruster.color = Random.ColorHSV(0f,1f,0f,1f,0.8f,1f);
    }

    public void SpawnAsteroids(float amount)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject roid = Instantiate(asteroid, new Vector3(Random.Range(-15,15),Random.Range(-9,9),0f),Quaternion.Euler(0,0,0));
            roid.transform.GetChild(0).rotation = Quaternion.Euler(Random.Range(0,360),Random.Range(0,360),Random.Range(0,360));
            asteroidCount++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(asteroidCount == 0)
        {
            SpawnAsteroids(spawnAmount);
        }
    }
}
