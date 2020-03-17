using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;
using TMPro;


public class TitleScreen : MonoBehaviour
{

    bool xboxIn, playstationIn;

    bool hasPluggedIn;

    public TextMeshProUGUI warning;
    public GameObject fade;

    bool mustUnplug;

    void Awake()
    {
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
    }

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            if (xboxIn || playstationIn)
            {
                mustUnplug = true;
            }
            else
            {
                warning.GetComponent<Animator>().SetTrigger("fade");
                fade.GetComponent<Animator>().SetTrigger("fade");
            }
        }
        catch { }
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Game")
            {
                if (mustUnplug && (!xboxIn && !playstationIn))
                {
                    warning.GetComponent<Animator>().SetTrigger("fade");
                    fade.GetComponent<Animator>().SetTrigger("fade");
                    mustUnplug = false;
                }

                if (!mustUnplug && (xboxIn || playstationIn))
                {
                    fade.GetComponent<Animator>().SetTrigger("fadeIn");
                }
            }
        }
        catch { }
    }

    public void GameScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
    }

    public void StartGame()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Game")
        {
            EnemySpawner.startGame = true;
        }
    }
    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);

        if (args.name == "XInput Gamepad 1")
        {
            xboxIn = true;
        }
        if (args.name == "Sony DualShock 4")
        {
            playstationIn = true;
        }

    }

    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);


        if (args.name == "XInput Gamepad 1")
        {
            xboxIn = false;
        }
        if (args.name == "Sony DualShock 4")
        {
            playstationIn = false;
        }

    }

}
