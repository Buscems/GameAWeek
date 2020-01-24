using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;

public class BasePlayer : MonoBehaviour
{

    public float joystickDeadzone;

    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    [HideInInspector]
    public Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    [Header("Movement")]
    Rigidbody2D rb;
    public float speed;
    Vector2 velocity;

    [Header("Attacking")]
    public GameObject bullet;
    public GameObject cursor;
    public Vector2 direction;
    public float cursorDistance;

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

        rb = GetComponent<Rigidbody2D>();    

    }

    // Update is called once per frame
    void Update()
    {

        velocity = new Vector2(myPlayer.GetAxisRaw("MoveHorizontal"), myPlayer.GetAxisRaw("MoveVertical"));
        direction = new Vector2(myPlayer.GetAxisRaw("DirectionHorizontal"), myPlayer.GetAxisRaw("DirectionVertical"));

    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(direction.x) > joystickDeadzone || Mathf.Abs(direction.y) > joystickDeadzone)
        {
            cursor.transform.up = direction;
            cursor.transform.localPosition = direction * cursorDistance;
        }
        rb.MovePosition(rb.position + velocity * speed * Time.deltaTime);
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
