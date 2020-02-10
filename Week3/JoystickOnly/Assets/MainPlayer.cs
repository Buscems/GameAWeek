using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class MainPlayer : MonoBehaviour
{
    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    [HideInInspector]
    public Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    public Vector2 joystickMinMax;

    [Header("Attack Sequence")]
    public float inputTime;

    public Vector2 inputDirection;

    public float quarterCircleForward;

    string currentAttack = "";

    private void Awake()
    {
        //Rewired Code
        myPlayer = ReInput.players.GetPlayer(playerNum - 1);
        ReInput.ControllerConnectedEvent += OnControllerConnected;
        CheckController(myPlayer);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        inputDirection = new Vector2(myPlayer.GetAxis("Horizontal"), myPlayer.GetAxis("Vertical"));



        Debug.Log(currentAttack);
        InputDetector();

        DecreaseTimers();

        if(currentAttack != "")
        {
            Attack();
        }

    }

    void Attack()
    {
        switch (currentAttack)
        {
            case "qcf":

                //do quarter circle forward thing
                currentAttack = "";

            break;
        }
    }

    string InputDetector()
    {
        

        if(inputDirection.y < -joystickMinMax.y && Mathf.Abs(inputDirection.x) < joystickMinMax.x)
        {
            quarterCircleForward = inputTime;
        }
        if (inputDirection.y < -.72f && inputDirection.x > .8f)
        {
            if (quarterCircleForward > 0)
            {
                quarterCircleForward = inputTime;
            }
        }
        if(Mathf.Abs(inputDirection.y) < joystickMinMax.x && inputDirection.x > joystickMinMax.y)
        {

            if(quarterCircleForward > 0)
            {
                currentAttack = "qcf";
                quarterCircleForward = -.1f;
            }

        }
        return currentAttack;

    }

    void DecreaseTimers()
    {
        if(quarterCircleForward >= 0)
        {
            quarterCircleForward -= Time.deltaTime;
        }
    }

    //[REWIRED METHODS]
    //these two methods are for ReWired, if any of you guys have any questions about it I can answer them, but you don't need to worry about this for working on the game - Buscemi
    void OnControllerConnected(ControllerStatusChangedEventArgs arg)
    {
        CheckController(myPlayer);
    }

    void CheckController(Player player)
    {
        foreach (Joystick joyStick in player.controllers.Joysticks)
        {
            var ds4 = joyStick.GetExtension<DualShock4Extension>();
            if (ds4 == null) continue;//skip this if not DualShock4
            switch (playerNum)
            {
                case 4:
                    ds4.SetLightColor(Color.yellow);
                    break;
                case 3:
                    ds4.SetLightColor(Color.green);
                    break;
                case 2:
                    ds4.SetLightColor(Color.blue);
                    break;
                case 1:
                    ds4.SetLightColor(Color.red);
                    break;
                default:
                    ds4.SetLightColor(Color.white);
                    Debug.LogError("Player Num is 0, please change to a number > 0");
                    break;
            }
        }
    }

}
