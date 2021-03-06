﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;
using UnityEngine.UI;

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

    //[HideInInspector]
    public bool ready;

    public int health;

    public Image[] healthImage;

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

        health = 3;

    }

    // Update is called once per frame
    void Update()
    {

        if(health == 3)
        {
            for(int i = 0; i < health; i++)
            {
                //healthImage[i].enabled = true;
            }
        }
        if(health == 2)
        {
            healthImage[2].enabled = false;
        }
        if (health == 1)
        {
            healthImage[1].enabled = false;

        }
        if(health <= 0)
        {
            healthImage[0].enabled = false;
            UnityEngine.SceneManagement.SceneManager.LoadScene("LoseScene");
        }

        Movement();

        if(velocity.x < 0 && onTopOfPlatform)
        {
            this.GetComponent<SpriteRenderer>().flipX = false;
        }
        if (velocity.x > 0 && onTopOfPlatform)
        {
            this.GetComponent<SpriteRenderer>().flipX = true;
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

                    if(collisionInfo.gameObject.tag == "Pillow")
                    {
                        velocity.y = jumpVel * 5;
                        collisionInfo.gameObject.GetComponent<PillowProjectile>().direction = new Vector3(0, -1, 0);
                    }
                    if (collisionInfo.gameObject.tag == "Bed")
                    {
                        velocity.y = jumpVel * 5;
                        collisionInfo.gameObject.GetComponent<BedBoss>().GetHit();
                    }
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
            else
            {
                bool hasTakenDamage = false;
                if (!hasTakenDamage)
                {
                    health--;
                    hasTakenDamage = true;
                }
                if(collisionInfo.gameObject.tag == "Pillow")
                {
                    Destroy(collisionInfo.gameObject);
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
    }

    private void OnCollisionExit2D(Collision2D collisionInfo)
    {
        onTopOfPlatform = false;
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
