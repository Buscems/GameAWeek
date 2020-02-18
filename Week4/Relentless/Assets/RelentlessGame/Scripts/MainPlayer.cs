using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;
using Rewired.ControllerExtensions;
using UnityEngine.UI;

public class MainPlayer : MonoBehaviour
{

    Animator anim;

    //the following is in order to use rewired
    [Tooltip("Reference for using rewired")]
    [HideInInspector]
    public Player myPlayer;
    [Header("Rewired")]
    [Tooltip("Number identifier for each player, must be above 0")]
    public int playerNum;

    Rigidbody2D rb;

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
    public int currentDamage;

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
            
        }

        Movement();
        if (Time.timeScale == 1)
        {
            Aim();
        }
        
        if (myPlayer.GetButton("Attack") && !isShooting)
        {
            StartCoroutine(Shoot());
        }

    }

    private void FixedUpdate()
    {
        FixedMovement();
    }

    void Movement()
    {
        velocity = new Vector2(myPlayer.GetAxisRaw("MoveHorizontal"), myPlayer.GetAxisRaw("MoveVertical"));
    }

    void FixedMovement()
    {
        rb.MovePosition(rb.position + velocity * speed * Time.deltaTime);
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

    IEnumerator Shoot()
    {
        isShooting = true;
        var temp = Instantiate(bullet, transform.position, pointer.transform.rotation);
        temp.GetComponent<PlayerBullet>().damage = currentDamage;
        yield return new WaitForSeconds(shotInterval);
        isShooting = false;
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
