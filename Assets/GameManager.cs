using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    TextMeshProUGUI scoreText, levelText, livesText, bonusText, gamovText, scoboText;
    [SerializeField] GameObject playerFab, asteroid, ufo;
    GameObject player;
    Transform canvas;
    Camera cam;
    PlayerControl pControl;
    [SerializeField] Light sun1, sun2;
    Color playerThrusterColor, sun1color, sun2color;
    Light playerThruster;
    ParticleSystem playerThrusterParticles, bgParticles;
    AudioSource audioSource, bgAudioSource;
    [SerializeField] AudioClip levelChangeClip;
    bool isOver = false, scoreOver = false;
    public float Score {get{return score;} set{if(value <= 99999)score = value; else {score = 99999; scoreOver = true;}}}
    public float AsteroidCount {get{return asteroidCount;} set{asteroidCount = value;}}
    [SerializeField] float spawnAmount = 8, asteroidCount = 0, ufoCount = 0, score = 0, lives = 3, level = 1, ufoSpeed = 1000, levelTimer;
    float[] scores = new float[6];


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        audioSource = transform.GetComponent<AudioSource>();
        bgAudioSource = GameObject.FindWithTag("Background").GetComponent<AudioSource>();
        canvas = GameObject.FindWithTag("Canvas").transform;
        scoreText = canvas.GetChild(0).GetComponent<TextMeshProUGUI>();
        levelText = canvas.GetChild(1).GetComponent<TextMeshProUGUI>();
        livesText = canvas.GetChild(2).GetComponent<TextMeshProUGUI>();
        bonusText = canvas.GetChild(3).GetComponent<TextMeshProUGUI>();
        gamovText = canvas.GetChild(4).GetComponent<TextMeshProUGUI>();
        scoboText = canvas.GetChild(5).GetComponent<TextMeshProUGUI>();
        bonusText.enabled = false;
        gamovText.enabled = false;
        scoboText.enabled = false;
        scores[0] = 1;
        for(int i = 1; i < scores.Length-1; i++)
        {
            scores[i] = Random.Range(scores[i-1],i*5000+9999);
        }
        scores[scores.Length-1] = 99999;
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
        playerThrusterColor = playerThruster.color;
        pControl.ResetInvuln(4);
        UpdateUI();
        SpawnAsteroids(spawnAmount);
        SpawnUfo();
    }

    public void ChangeLights()
    {
        sun1color = Random.ColorHSV(0f,1f,0f,1f,0.6f,1f,0.35f,0.35f);
        sun1.color = sun1color;
        var bgPCOverL = bgParticles.colorOverLifetime;
        if(sun1.color.grayscale < 0.6f)
        {
            sun2color = Random.ColorHSV(0f,1f,0f,1f,1f,1f,0.35f,0.35f);
            bgPCOverL.color = sun2color;
        }
        else
        {
            sun2color = Random.ColorHSV(0f,1f,0f,1f,0f,1f,0.35f,0.35f);
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

    void SpawnUfo()
    {
        GameObject newfo = Instantiate(ufo,cam.ViewportToWorldPoint(new Vector3(1.1f,Random.Range(0.2f,0.8f),0)),ufo.transform.rotation);
        newfo.transform.position = new Vector3(newfo.transform.position.x, newfo.transform.position.y, 0);
        if(newfo.transform.position.x > 0)
            newfo.GetComponent<Rigidbody2D>().AddForce(-ufo.transform.right * ufoSpeed);
        else
            newfo.GetComponent<Rigidbody2D>().AddForce(ufo.transform.right * ufoSpeed);
        
        Destroy(newfo, 30f);
    }

    void SpawnPlayer()
    {
        Debug.Log("Spawning Player");
        if(player == null)
        {
            player = Instantiate(playerFab);
            playerThruster = player.transform.GetChild(0).GetChild(1).GetComponent<Light>();
            playerThruster.color = playerThrusterColor;
            playerThrusterParticles = player.transform.GetChild(0).GetChild(2).GetComponent<ParticleSystem>();
            pControl = player.GetComponent<PlayerControl>();
            
            pControl.ResetInvuln(3);
            Debug.Log("Done");
        }
        else
            Debug.Log("Player already exists");
    }

    public void UpdateUI()
    {
        scoreText.text = "Score: " + Score.ToString("00000");
        if(scoreOver) scoreText.text += "9";
        else scoreText.text += "0";

        levelText.text = "Level: " + (level).ToString("00");
        string livestmp = "";
        for(int i = 0; i < lives; i++)
        {
            livestmp += "V";
        }
        livesText.text = "Lives\n"+livestmp;
    }
    
    float ufoCd = 10f, ufoTimer = 10f;
    // Update is called once per frame
    void Update()
    {
        // Ufo spawning
        if(ufoCount>0)
        {
            if(ufoTimer<0)
            {
                SpawnUfo();
                ufoCount--;
                ufoTimer = ufoCd;
            }
            else ufoTimer -= Time.deltaTime;
        }

        // Level Up
        if(asteroidCount == 0 && spawnAmount != 0)
        {
            if(levelTimer>0)
            {
                score += Mathf.CeilToInt(levelTimer * 10f);
                bonusText.text = "SpeedBonus! +" + Mathf.CeilToInt(levelTimer * 10f)+"0";
                bonusText.enabled = true;
            }
            level++;
            levelTimer = level * 5f;


            for (int i = 0; i < ufoCount; i++)
            {
                SpawnUfo();
            }
            ufoTimer = ufoCd;
            ufoCount = Mathf.FloorToInt((level - 1)/3f);

            audioSource.clip = levelChangeClip;
            audioSource.Play();

            Debug.Log($"You reached level {level}! Current score: {score}");
            SpawnAsteroids(spawnAmount+level);
            ChangeLights();
            pControl.ResetInvuln(3);
        }
        if(levelTimer < (level-1) * 5f - 3f)
            bonusText.enabled = false;
        Debug.Log(levelTimer);

        // If player is gone
        if(player == null)
        {
            if(lives <= 0)
            {
                if(!isOver)
                {
                    GameOver();
                    isOver = true;
                }
            }
            else
            {
                lives--;
                Debug.Log($"{lives} lives left");
                SpawnPlayer();
            }
            UpdateUI();
        }

        //debug button :)
        if(Input.GetButtonDown("Jump"))
        {
            //SpawnPlayer();
            ChangeLights();
            //SpawnUfo();
            //SpawnAsteroids(1);
        }
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(levelTimer > 0)
            levelTimer -= Time.deltaTime;
    }

    string chars = "ABCDEFGHOIJKLMNOPQRSTUVWXYZ";
    private void GameOver()
    {
        Debug.Log($"Game over! Level: {level} Score: {score}0");
        scoreText.enabled = false;
        livesText.enabled = false;
        levelText.enabled = false;
        gamovText.enabled = true;
        scoboText.text = "";
        for (int i = 0; i < scores.Length; i++)
        {
            if(score <= scores[scores.Length-i-1])
            {
                //tmp is 3 random letters from 'chars' string
                string tmp = chars[Random.Range(0,chars.Length-1)].ToString()+chars[Random.Range(0,chars.Length-1)].ToString()+chars[Random.Range(0,chars.Length-1)].ToString();
                scoboText.text += tmp+" --- "+scores[scores.Length-i-1].ToString("00000")+"0\n";
            }
            else
            {
                scoboText.text += "YOU -x- "+score.ToString("00000");
                score = -1;
                if(scoreOver)
                 scoboText.text += "9\n";
                else
                 scoboText.text += "0\n";
            }
        }
        scoboText.enabled = true;
        bgAudioSource.Stop();
        Time.timeScale = 0;
    }
}
