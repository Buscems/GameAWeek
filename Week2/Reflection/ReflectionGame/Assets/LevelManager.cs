﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public MainPlayer player1, player2;

    public int currentLevel;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player1.ready && player2.ready)
        {
            
            switch (currentLevel)
            {
                case 0:
                    SceneManager.LoadScene("Level1");
                    break;
                case 1:
                    SceneManager.LoadScene("Level2");
                    break;
                case 2:
                    SceneManager.LoadScene("Level3");
                    break;
            }
            
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if (Input.GetKeyDown(KeyCode.RightShift))
        {

            switch (currentLevel)
            {
                case 1:
                    SceneManager.LoadScene("Level2");
                    break;
                case 2:
                    SceneManager.LoadScene("Level3");
                    break;
            }

        }
    }
}
