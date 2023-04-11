using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerFab, asteroid;
    GameObject player;
    [SerializeField] Light sun1, sun2;
    Color playerThrusterColor;
    Light playerThruster;
    ParticleSystem playerThrusterParticles;
    public float Score {get{return score;} set{score = value;}}
    public float AsteroidCount {get{return asteroidCount;} set{asteroidCount = value;}}
    [SerializeField] float spawnAmount = 8, asteroidCount = 0, score = 0;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerThruster = player.transform.GetChild(0).GetChild(1).GetComponent<Light>();
        playerThrusterParticles = player.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>();

        SpawnAsteroids(spawnAmount);
    }

    public void ChangeLights()
    {
        sun1.color = Random.ColorHSV(0f,1f,0f,1f,0f,1f);
        if(sun1.color.grayscale < 0.7)
            sun2.color = Random.ColorHSV(0f,1f,0f,1f,1f,1f);
        else
            sun2.color = Random.ColorHSV(0f,1f,0f,1f,0f,1f);
        playerThrusterColor = Random.ColorHSV(0f,1f,0f,1f,0.8f,1f);
        playerThruster.color = playerThrusterColor;
        var pTPmain = playerThrusterParticles.main;
        pTPmain.startColor = playerThrusterColor;
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
        if(asteroidCount == 0 && spawnAmount != 0)
        {
            SpawnAsteroids(spawnAmount);
        }

        if(Input.GetButtonDown("Jump"))
        {
            ChangeLights();
            //SpawnAsteroids(1);
        }
    }
}
