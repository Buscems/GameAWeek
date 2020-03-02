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

    Rigidbody2D rb;

    [Header("Movement")]
    public float speed;
    private Vector2 velocity;
    private Vector2 direction;

    [Header("Dash")]
    public float dashMult;
    public float dashTime;
    bool isDashing;
    bool dashWait;
    float dashSpeed;
    public float dashCooldown;

    bool canAttack;

    [Header("Attack Spots")]
    public string basicAttack;
    public string mediumAttack;
    public string heavyAttack;

    public string[] basicAttacks;
    public string[] mediumAttacks;
    public string[] heavyAttacks;

    [Header("Melee Ability")]
    public GameObject[] meleeAttacks;
    bool firstAttack, secondAttack, finisherAttack;
    bool doSecondAttack, doFinisherAttack;
    public float timeToInputCombo;

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
        canAttack = true;
    }

    // Update is called once per frame
    void Update()
    {

        Movement();

        if (myPlayer.GetButtonDown("Dash") && !isDashing && !dashWait)
        {
            StartCoroutine(Dash());
        }

        if(myPlayer.GetButtonDown("BasicAttack") && canAttack)
        {
            if (basicAttack == "Melee")
            {
                if (!firstAttack && !secondAttack && !finisherAttack)
                {
                    StartCoroutine(FirstMelee());
                }
                else if (firstAttack && !doSecondAttack)
                {
                    doSecondAttack = true;
                }
                else if ((firstAttack || secondAttack) && !doFinisherAttack)
                {
                    doFinisherAttack = true;
                }
            }
        }

    }

    private void FixedUpdate()
    {
        FixedMovement();
    }

    void FixedMovement()
    {
        if (!isDashing)
        {
            rb.MovePosition(rb.position + velocity * speed * Time.deltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + velocity * dashSpeed * Time.deltaTime);
        }
    }

    void Movement()
    {
        if (!isDashing)
        {
            velocity = new Vector2(myPlayer.GetAxisRaw("MoveHorizontal"), myPlayer.GetAxisRaw("MoveVertical"));
        }

    }

    IEnumerator Dash()
    {
        isDashing = true;
        canAttack = false;
        this.gameObject.layer = 8;
        dashSpeed = speed * dashMult;
        yield return new WaitForSeconds(dashTime);
        StartCoroutine(DashCooldown());
        isDashing = false;
        canAttack = true;
        this.gameObject.layer = 0;
    }
    IEnumerator DashCooldown()
    {
        dashWait = true;
        yield return new WaitForSeconds(dashCooldown);
        dashWait = false;
    }

    IEnumerator FirstMelee()
    {
        firstAttack = true;
        Debug.Log("FirstAttack");
        yield return new WaitForSeconds(timeToInputCombo);
        firstAttack = false;
        if (doSecondAttack)
        {
            StartCoroutine(SecondMelee());
        }
    }

    IEnumerator SecondMelee()
    {
        doSecondAttack = false;
        secondAttack = true;
        Debug.Log("SecondAttack");
        yield return new WaitForSeconds(timeToInputCombo);
        secondAttack = false;
        if (doFinisherAttack)
        {
            StartCoroutine(FinisherMelee());
        }
    }

    IEnumerator FinisherMelee()
    {
        doFinisherAttack = false;
        finisherAttack = true;
        Debug.Log("FinisherAttack");
        yield return new WaitForSeconds(timeToInputCombo);
        finisherAttack = false;
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
