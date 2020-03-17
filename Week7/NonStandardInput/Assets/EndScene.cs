using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Rewired;
using Rewired.ControllerExtensions;
public class EndScene : MonoBehaviour
{

    bool xboxIn, playstationIn;

    public TextMeshProUGUI timeLasted;
    public TextMeshProUGUI highScore;

    void Awake()
    {
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
    }

    // Start is called before the first frame update
    void Start()
    {

        if(PlayerPrefs.GetFloat("currentTime") > PlayerPrefs.GetFloat("highScore"))
        {
            PlayerPrefs.SetFloat("highScore", PlayerPrefs.GetFloat("currentTime"));
            highScore.text = "High Score\n" + PlayerPrefs.GetString("score");
        }
        else
        {
            highScore.text = "High Score\n" + PlayerPrefs.GetString("highScoreString");
        }

        timeLasted.text = "Time Lasted\n" + PlayerPrefs.GetString("score");

    }

    // Update is called once per frame
    void Update()
    {

        if (xboxIn)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScreen");
        }
        if (playstationIn)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
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
