using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;
using UnityEngine.UI;

public class MainPlayer : MonoBehaviour
{

    Animator anim;
    TrailRenderer tr;
    SpriteRenderer sr;

    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    [HideInInspector]
    public Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    Rigidbody2D rb;

    [Header("Health")]
    public Image[] healthImage;
    public int maxHealth;
    [HideInInspector]
    public int health;
    bool hasBeenHit;
    public float hitCooldown;

    [Header("Movement")]
    private Vector2 velocity;
    private Vector2 direction;
    public float speed;

    [Header("Attacking")]
    public GameObject pointer;
    public float pointerDistance;
    public GameObject bullet;
    public float shotInterval;
    bool isShooting;
    public float currentDamage;

    [Header("Dash")]
    public float dashMult;
    public float dashTime;
    bool isDashing;
    bool dashWait;
    float dashSpeed;
    public float dashCooldown;

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
        tr = GetComponent<TrailRenderer>();
        sr = GetComponent<SpriteRenderer>();

        health = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene")
        {
            if(health <= 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Lose");
            }
        }
        tr.startColor = sr.color;

        //setting player booleans
        anim.SetBool("dash", isDashing);
        anim.SetBool("beenHit", hasBeenHit);

        Movement();
        if (Time.timeScale == 1)
        {
            Aim();
        }
        try
        {
            HealthManager();
        }
        catch { }
        if (myPlayer.GetButton("Attack") && !isShooting && !isDashing)
        {
            StartCoroutine(Shoot());
        }

        if (myPlayer.GetButtonDown("Dash") && !isDashing && !dashWait)
        {
            StartCoroutine(Dash());
        }

    }

    private void FixedUpdate()
    {
        FixedMovement();
    }

    void Movement()
    {
        if (!isDashing)
        {
            velocity = new Vector2(myPlayer.GetAxisRaw("MoveHorizontal"), myPlayer.GetAxisRaw("MoveVertical"));
        }

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

    void Aim()
    {
        direction = new Vector2(myPlayer.GetAxisRaw("DirectionHorizontal"), myPlayer.GetAxisRaw("DirectionVertical"));

        if (Mathf.Abs(direction.x) >= .3 || Mathf.Abs(direction.y) >= .3)
        {
            pointer.transform.up = direction.normalized;
            pointer.transform.localPosition = direction.normalized * pointerDistance;
        }
        else if(Mathf.Abs(velocity.x) >= .3 || Mathf.Abs(velocity.y) >= .3)
        {
            pointer.transform.up = velocity.normalized * Time.deltaTime;
            pointer.transform.localPosition = velocity.normalized * pointerDistance;
        }
    }

    void HealthManager()
    {
        for(int i = 0; i < maxHealth; i++)
        {
            healthImage[i].enabled = false;
            if(health > i)
            {
                healthImage[i].enabled = true;
            }

        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        var temp = Instantiate(bullet, transform.position, pointer.transform.rotation);
        temp.GetComponent<PlayerBullet>().damage = currentDamage;
        yield return new WaitForSeconds(shotInterval);
        isShooting = false;
    }

    IEnumerator Dash()
    {
        isDashing = true;
        this.gameObject.layer = 8;
        dashSpeed = speed * dashMult;
        yield return new WaitForSeconds(dashTime);
        StartCoroutine(DashCooldown());
        isDashing = false;
        this.gameObject.layer = 0;
    }
    IEnumerator DashCooldown()
    {
        dashWait = true;
        yield return new WaitForSeconds(dashCooldown);
        dashWait = false;
    }

    public void GetHit()
    {
        if (!hasBeenHit)
        {
            health--;
            StartCoroutine(HitCooldown());
        }
    }

    IEnumerator HitCooldown()
    {
        hasBeenHit = true;
        yield return new WaitForSeconds(hitCooldown);
        hasBeenHit = false;
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
