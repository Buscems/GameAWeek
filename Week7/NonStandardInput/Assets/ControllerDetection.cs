using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class ControllerDetection : MonoBehaviour
{

    public GameObject firstCircle, secondCircle;

    public GameObject xbox, playstation;

    public GameObject psLeft, psRight;

    bool xboxIn, playstationIn;

    void Awake()
    {
        // Subscribe to events
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        ReInput.ControllerDisconnectedEvent += OnControllerDisconnected;
    }

    // Start is called before the first frame update
    void Start()
    {
        xbox.SetActive(false);
        playstation.SetActive(false);

        psLeft.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        if (xboxIn)
        {
            xbox.SetActive(true);
            psLeft.SetActive(true);
        }
        else
        {
            xbox.SetActive(false);
            psLeft.SetActive(false);
        }

        /*
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            print(names[x].Length);
            if (names[x].Length == 19)
            {
                print("PS4 CONTROLLER IS CONNECTED");
                //PS4_Controller = 1;
                //Xbox_One_Controller = 0;
                playstationIn = true;
            }
            else
            {
                playstationIn = false;
            }
            if (names[x].Length == 33)
            {
                print("XBOX ONE CONTROLLER IS CONNECTED");
                //set a controller bool to true
                //PS4_Controller = 0;
                //Xbox_One_Controller = 1;
                xboxIn = true;
            }
            else
            {
                xboxIn = false;
            }
        }
        */
    }

    void OnControllerConnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was connected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);

        if(args.name == "XInput Gamepad 1")
        {
            xboxIn = true;
        }
        
        
    }

    void OnControllerDisconnected(ControllerStatusChangedEventArgs args)
    {
        Debug.Log("A controller was disconnected! Name = " + args.name + " Id = " + args.controllerId + " Type = " + args.controllerType);

        
        if (args.name == "XInput Gamepad 1")
        {
            xboxIn = false;
        }
        
    }

}
