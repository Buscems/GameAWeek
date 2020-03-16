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

    [Header("Movement")]
    public float speed;
    [SerializeField]
    Vector2 velocity;
    Rigidbody2D rb;

    [Header("Gravity Variables")]
    public float gravityUp;
    public float gravityDown;
    public float jumpVel;
    public float jumpTimerMax;
    bool isJumping;
    float jumpTimer;
    public float maxDownVel;
    public float onPlatformTimer;
    public float onPlatformTimerMax;
    public bool onTopOfPlatform;

    [Header("Easing Variables")]
    public float rippleSpeed;
    public float lengthOfRipple;
    public float rippleFrequency;
    public float amplitude;

    float equationTime;
    float origScale;
    public GameObject playerSprite;

    //[HideInInspector]
    public bool ready;

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
        origScale = playerSprite.transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {

        Movement();

        equationTime += Time.deltaTime * rippleSpeed;
        float equationAdd = Mathf.Exp(-equationTime * lengthOfRipple) * Mathf.Cos(rippleFrequency * Mathf.PI * equationTime) * amplitude;

        playerSprite.transform.localScale = new Vector2(origScale + (equationAdd / 1.8f), origScale - equationAdd);

        if (playerSprite.transform.localScale.y < 0)
        {
            playerSprite.transform.localScale = new Vector2(origScale + (equationAdd / 1.8f), 0.5f);
        }

    }

    private void FixedUpdate()
    {

        Gravity();

        rb.MovePosition(rb.position + velocity * speed * Time.deltaTime);
    }

    void Movement()
    {
        velocity.x = myPlayer.GetAxisRaw("MoveHorizontal");

        //jump logic
        if (onPlatformTimer > 0)
        {
            if (myPlayer.GetButtonDown("Jump"))
            {
                velocity.y = jumpVel;
                jumpTimer = jumpTimerMax;
                isJumping = true;
            }
        }
        if (myPlayer.GetButton("Jump") && isJumping)
        {
            velocity.y = jumpVel;
            jumpTimer -= Time.deltaTime;
        }

        if (myPlayer.GetButtonUp("Jump") || jumpTimer <= 0)
        {
            isJumping = false;
        }

        

        //set timer that will let the player jump slightly off the platform
        if (onTopOfPlatform)
        {
            onPlatformTimer = onPlatformTimerMax;
        }
        else
        {
            onPlatformTimer -= Time.deltaTime;
        }
    }

    void Gravity()
    {
        //gravity logic
        if (velocity.y > -maxDownVel)
        { //if we haven't reached maxDownVel
            if (velocity.y > 0)
            { //if player is moving up
                velocity.y -= gravityUp * Time.fixedDeltaTime;
            }
            else
            { //if player is moving down
                velocity.y -= gravityDown * Time.fixedDeltaTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        foreach (ContactPoint2D contact in collisionInfo.contacts)
        {
            //am I coming from the top/bottom?
            if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
            {
                velocity.y = 0; //stop vertical velocity
                if (contact.normal.y >= 0)
                { //am I hitting the top of the platform?

                    onTopOfPlatform = true;
                    equationTime = 0;
                    if (contact.collider.gameObject.tag == "Goal")
                    {
                        ready = true;
                    }
                }
                //am I hitting the bottom of a platform?
                if (contact.normal.y < 0)
                {
                    //hitHead = true;
                    velocity.y = 0;
                    //gotHitTimer = 0;
                    //maxKnockbackTime = 0;

                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collisionInfo)
    {
        foreach (ContactPoint2D contact in collisionInfo.contacts)
        {
            //am I coming from the top/bottom?
            if (Mathf.Abs(contact.normal.y) > Mathf.Abs(contact.normal.x))
            {
                velocity.y = 0; //stop vertical velocity
                if (contact.normal.y >= 0)
                { //am I hitting the top of the platform?

                    onTopOfPlatform = true;
                }
                //am I hitting the bottom of a platform?
                if (contact.normal.y < 0)
                {
                    //hitHead = true;
                    velocity.y = 0;
                    //gotHitTimer = 0;
                    //maxKnockbackTime = 0;

                }
            }
        }
        if (collisionInfo.gameObject.tag == "Goal")
        {
            ready = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Goal")
        {
            ready = false;
        }
        else
        {
            onTopOfPlatform = false;
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
