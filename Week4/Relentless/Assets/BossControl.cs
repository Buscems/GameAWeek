using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;
public class BossControl : MonoBehaviour
{

    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    [HideInInspector]
    public Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    Rigidbody2D rb;

    Animator anim;

    bool attacking;

    [Header("Movement")]
    public float baseSpeed;
    public float superSpeed;
    float currentSpeed;
    Vector2 velocity;
    bool lockMovement;

    [Header("Health and Phase")]
    public int maxHealth;
    int health;

    [Header("Swipe Attack")]
    public GameObject sword;
    public int swordCooldown;

    [Header("Stomp Attack")]
    public GameObject stomp;
    public int stompCooldown;

    [Header("Homing Attack")]
    public GameObject homingMissile;
    public float homingInterval;
    bool isShootingHoming;
    Transform target;
    Vector2 shootDirection;

    enum BossPhase { normalBoss, poweredUpBoss}
    BossPhase currentPhase;

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
        anim = GetComponent<Animator>();
        TurnOffAttacks();

        health = maxHealth;

        target = GameObject.FindGameObjectWithTag("Player").transform;

    }


    void TurnOffAttacks()
    {
        sword.SetActive(false);
        stomp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

        shootDirection = (target.position - transform.position).normalized;

        if (!lockMovement)
        {
            velocity = new Vector2(myPlayer.GetAxisRaw("MoveHorizontal"), myPlayer.GetAxisRaw("MoveVertical"));
        }

        if (Mathf.Abs(velocity.x) > 0 || Mathf.Abs(velocity.y) > 0)
        {
            transform.up = velocity;
        }
        //second "form" of the boss
        if (!isShootingHoming)
        {
            StartCoroutine(HomingMissile());
        }
        if (health/maxHealth > .5f)
        {
            currentSpeed = baseSpeed;
            if (!isShootingHoming)
            {
                StartCoroutine(HomingMissile());
            }
        }
        else
        {
            currentSpeed = superSpeed;
        }

        if (myPlayer.GetButtonDown("LargeSwipe") && !attacking)
        {
            LargeSwipe();
        }

        if (myPlayer.GetButtonDown("Stomp") && !attacking)
        {
            Stomp();
        }
    }

    private void FixedUpdate()
    {
        if (!lockMovement)
        {
            rb.MovePosition(rb.position + velocity * currentSpeed * Time.deltaTime);
        }
    }

    public void LargeSwipe()
    {
        attacking = true;

        lockMovement = true;

        sword.SetActive(true);

        anim.SetTrigger("LargeSwipe");
    }

    public void Stomp()
    {
        attacking = true;
        lockMovement = true;

        stomp.SetActive(true);

        anim.SetTrigger("Stomp");
    }

    IEnumerator HomingMissile()
    {
        isShootingHoming = true;
        var temp = Instantiate(homingMissile, transform.position, Quaternion.identity);
        temp.GetComponent<BossHomingBullet>().transform.up = shootDirection;
        temp.GetComponent<BossHomingBullet>().target = target;
        yield return new WaitForSeconds(homingInterval);
        isShootingHoming = false;
    }

    public void AttackOver()
    {

        attacking = false;

        lockMovement = false;

        sword.SetActive(false);
        stomp.SetActive(false);

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
