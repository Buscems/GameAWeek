using System.Collections;
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

    Rigidbody2D rb;

    public GameObject pointer;
    public float pointerDistance;

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

    bool attacking;

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
    public float baseMeleeDamage;
    float meleeModifier;
    float currentMeleeDamage;

    [Header("Throw Ability")]
    public GameObject chargeBar;
    public Image filledBar;
    public GameObject throwAttack;
    public float baseThrowDamage;
    float throwModifier;
    float currentThrowDamage;
    bool chargeAttack;
    float throwTimer;

    public static bool canUseChargeAttack = true;

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
        meleeModifier = 1;

        for(int i = 0; i < meleeAttacks.Length; i++)
        {
            meleeAttacks[i].SetActive(false);
        }
        chargeBar.SetActive(false);

        throwAttack.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        Movement();

        Aim();

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

        /*
        if (myPlayer.GetButtonDown("ThrowAttack") && canAttack && canUseChargeAttack)
        {
            canUseChargeAttack = false;
            chargeAttack = true;
            chargeBar.SetActive(true);
            throwTimer = 0;
            throwAttack.SetActive(true);
        }
        if (myPlayer.GetButtonUp("ThrowAttack"))
        {
            chargeAttack = false;
            ReleaseThrowAttack();
        }
        */ 

        if (chargeAttack)
        {
            ThrowAttack();
        }

        currentMeleeDamage = baseMeleeDamage * meleeModifier;

        currentThrowDamage = baseThrowDamage * throwModifier;

    }

    private void FixedUpdate()
    {
        FixedMovement();
    }

    void FixedMovement()
    {
        if (!attacking)
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
    }

    void Movement()
    {
        if (!isDashing && !attacking)
        {
            velocity = new Vector2(myPlayer.GetAxisRaw("MoveHorizontal"), myPlayer.GetAxisRaw("MoveVertical"));
        }

    }

    void Aim()
    {
        direction = new Vector2(myPlayer.GetAxisRaw("DirectionHorizontal"), myPlayer.GetAxisRaw("DirectionVertical"));

        if (Mathf.Abs(direction.x) >= .3 || Mathf.Abs(direction.y) >= .3)
        {
            pointer.transform.up = direction.normalized;
            pointer.transform.localPosition = direction.normalized * pointerDistance;
        }
        else if (Mathf.Abs(velocity.x) >= .3 || Mathf.Abs(velocity.y) >= .3)
        {
            pointer.transform.up = velocity.normalized * Time.deltaTime;
            pointer.transform.localPosition = velocity.normalized * pointerDistance;
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
        attacking = true;
        Debug.Log("FirstAttack");
        meleeAttacks[0].SetActive(true);
        meleeAttacks[0].transform.position = pointer.transform.position;
        meleeAttacks[0].GetComponent<MeleeAttack>().damage = currentMeleeDamage;
        yield return new WaitForSeconds(timeToInputCombo);
        firstAttack = false;
        attacking = false;
        meleeAttacks[0].SetActive(false);
        if (doSecondAttack)
        {
            //StartCoroutine(SecondMelee());
        }
    }

    IEnumerator SecondMelee()
    {
        doSecondAttack = false;
        secondAttack = true;
        Debug.Log("SecondAttack");
        meleeAttacks[1].SetActive(true);
        meleeAttacks[1].transform.position = pointer.transform.position;
        meleeAttacks[1].GetComponent<MeleeAttack>().damage = currentMeleeDamage;
        yield return new WaitForSeconds(timeToInputCombo);
        secondAttack = false;
        meleeAttacks[1].SetActive(false);
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
        meleeAttacks[2].SetActive(true);
        meleeAttacks[2].transform.position = pointer.transform.position;
        meleeAttacks[2].GetComponent<MeleeAttack>().damage = currentMeleeDamage;
        yield return new WaitForSeconds(timeToInputCombo);
        finisherAttack = false;
        meleeAttacks[2].SetActive(false);
    }

    void ThrowAttack()
    {
        attacking = true;
        throwTimer += Time.deltaTime;
        filledBar.fillAmount = throwTimer;
        throwAttack.transform.position = pointer.transform.position;
    }

    void ReleaseThrowAttack()
    {
        attacking = false;
        var ta = throwAttack.GetComponent<ProjectileAttack>();
        ta.direction = direction;
        ta.damage = currentThrowDamage;
        if (throwTimer < 1)
        {
            ta.speed = ta.speed * throwTimer;
        }
        chargeBar.SetActive(false);
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
