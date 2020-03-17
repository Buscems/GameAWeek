using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;
using UnityEngine.UI;
using TMPro;


public class ControllerDetection : MonoBehaviour
{

    bool xboxIn, playstationIn;

    public Transform[] playerSpots;

    [Header("Screen Shake")]
    public Camera playerCamera;
    public Vector2 rangeOfShake;
    public float shakeDuration;
    bool screenShake;

    public GameObject[] hearts;
    public int health;

    public TextMeshProUGUI timeLasted;

    float timer;

    void Awake()
    {
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (xboxIn && playstationIn)
        {
            transform.position = playerSpots[2].position;
        }
        else if (xboxIn && !playstationIn)
        {
            transform.position = playerSpots[1].position;
        }
        else if (!xboxIn && playstationIn)
        {
            transform.position = playerSpots[3].position;
        }
        else
        {
            transform.position = playerSpots[0].position;
        }
    }

    // Update is called once per frame
    void Update()
    {

        timer += Time.deltaTime;

        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        timeLasted.text = "Time: " + niceTime;

        for(int i = health; i < hearts.Length; i++)
        {
            hearts[i].SetActive(false);
        }

        if (screenShake)
        {
            ScreenShake();
        }

        if (xboxIn && playstationIn)
        {
            transform.position = playerSpots[2].position;
        }
        else if(xboxIn && !playstationIn)
        {
            transform.position = playerSpots[1].position;
        }
        else if (!xboxIn && playstationIn)
        {
            transform.position = playerSpots[3].position;
        }
        else
        {
            transform.position = playerSpots[0].position;
        }
        
        if(health <= 0)
        {
            PlayerPrefs.SetFloat("currentTime", timer);
            PlayerPrefs.SetString("score", niceTime);
            if(PlayerPrefs.GetFloat("currentTime") > PlayerPrefs.GetFloat("highScore"))
            {
                PlayerPrefs.SetString("highScoreString", niceTime);
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("End");
        }
        
    }

    void ScreenShake()
    {
        playerCamera.transform.position = new Vector3(Random.Range(rangeOfShake.x, rangeOfShake.y), Random.Range(rangeOfShake.x, rangeOfShake.y), -10);
    }

    IEnumerator StartScreenShake()
    {
        screenShake = true;
        yield return new WaitForSeconds(shakeDuration);
        screenShake = false;
        playerCamera.transform.position = new Vector3(0, 0, -10);
    }

    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);

        if(args.name == "XInput Gamepad 1")
        {
            xboxIn = true;
        }
        if(args.name == "Sony DualShock 4")
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(StartScreenShake());
        health--;
    }

}
