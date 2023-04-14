using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape) || Input.GetKey(KeyCode.Joystick1Button6))
        {
            Application.Quit();
        }
        else if(Input.GetButton("Fire1"))
        {
            SceneManager.LoadScene("Asteroids");
        }
    }
}
