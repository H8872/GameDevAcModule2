using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerFab, asteroid;
    GameObject player;
    PlayerControl pControl;
    [SerializeField] Light sun1, sun2;
    Color playerThrusterColor, sun1color, sun2color;
    Light playerThruster;
    ParticleSystem playerThrusterParticles, bgParticles;
    public float Score {get{return score;} set{score = value;}}
    public float AsteroidCount {get{return asteroidCount;} set{asteroidCount = value;}}
    [SerializeField] float spawnAmount = 8, asteroidCount = 0, score = 0, lives = 3, level = 0;


    // Start is called before the first frame update
    void Start()
    {
        bgParticles = GameObject.FindWithTag("Background").GetComponent<ParticleSystem>();
        Debug.Log($"Game Start! You have {lives} lives");
        player = GameObject.FindWithTag("Player");
        if(player == null)
        {
            SpawnPlayer();
        }
        else
        {
            pControl = player.GetComponent<PlayerControl>();
            playerThruster = player.transform.GetChild(0).GetChild(1).GetComponent<Light>();
            playerThrusterParticles = player.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>();
        }
        pControl.ResetInvuln(4);
        SpawnAsteroids(spawnAmount);
    }

    public void ChangeLights()
    {
        sun1color = Random.ColorHSV(0f,1f,0f,1f,0f,1f,0.5f,0.5f);
        sun1.color = sun1color;
        var bgPCOverL = bgParticles.colorOverLifetime;
        if(sun1.color.grayscale < 0.5f)
        {
            sun2color = Random.ColorHSV(0f,1f,0f,1f,1f,1f,0.5f,0.5f);
            bgPCOverL.color = sun2color;
        }
        else
        {
            sun2color = Random.ColorHSV(0f,1f,0f,1f,0f,1f,0.5f,0.5f);
            bgPCOverL.color = sun1color;
        }
        sun2.color = sun2color;
        playerThrusterColor = Random.ColorHSV(0f,1f,0f,1f,0.8f,1f);
        playerThruster.color = playerThrusterColor;
        var pTPmain = playerThrusterParticles.main;
        pTPmain.startColor = playerThrusterColor;
    }

    public void SpawnAsteroids(float amount)
    {
        Debug.Log($"Spawning {amount} asteroids");
        for (int i = 0; i < amount; i++)
        {
            GameObject roid = Instantiate(asteroid, new Vector3(Random.Range(-15,15),Random.Range(-9,9),0f),Quaternion.Euler(0,0,0));
            roid.transform.GetChild(0).rotation = Quaternion.Euler(Random.Range(0,360),Random.Range(0,360),Random.Range(0,360));
            asteroidCount++;
        }
        Debug.Log("Done");
    }

    void SpawnPlayer()
    {
        Debug.Log("Spawning Player");
        if(player == null)
        {
            player = Instantiate(playerFab);
            playerThruster = player.transform.GetChild(0).GetChild(1).GetComponent<Light>();
            playerThrusterParticles = player.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>();
            Debug.Log("Done");
        }
        else
            Debug.Log("Player already exists");
    }

    // Update is called once per frame
    void Update()
    {
        if(asteroidCount == 0 && spawnAmount != 0)
        {
            level++;
            Debug.Log($"You reached level {level}! Current score: {score}");
            SpawnAsteroids(spawnAmount+level);
            ChangeLights();
            pControl.ResetInvuln(3);
        }
        if(player == null && lives > 0)
        {
            lives--;
            Debug.Log($"{lives} lives left");
            SpawnPlayer();
        }
        if(lives <= 0)
        {
            Debug.Log($"Game over! Level: {level} Score: {score}");
            Time.timeScale = 0;
        }

        //debug button :)
        if(Input.GetButtonDown("Jump"))
        {
            SpawnPlayer();
            ChangeLights();
            //SpawnAsteroids(1);
        }
    }
}
