using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainBoss : MonoBehaviour
{

    [Header("Health")]
    public float maxHealth;
    public float health;
    public Image healthBar;
    public float phaseHealthAmount;

    [Header("BossTypes")]

    public Transform target;

    public int maxForms;

    public bool shoot;
    bool isShooting;
    bool isShootingHoming;
    public float shootInterval;
    public float homingInterval;
    Vector2 shootDirection;
    public GameObject bullet;
    public GameObject homingBullet;
    public float currentDamage;

    public bool laser;
    bool isShootingLaser;
    int laserAmount;
    public GameObject laserAttack;
    public float laserInterval;

    public bool homingBullets;

    public bool autoHeal;
    public float autoHealAmount;

    int currentPhase;

    public Image powerUpSign;
    public TextMeshProUGUI powerUpText;
    float flashTimer;
    public float flashTimerMax;
    float opacity;

    [Header("Movement")]
    public float speed;
    public Transform[] waypoints;
    public float timeBetweenNewSpots;
    Vector3 moveDirection;
    Rigidbody2D rb;
    bool moving;

    [Header("Timer")]
    public TextMeshProUGUI bossTimer;
    float timerAmount;

    [Header("Screen Shake")]
    public Camera playerCamera;
    public Vector2 rangeOfShake;
    public float shakeDuration;
    bool screenShake;
    float equationTime;
    float origScale;

    public GameObject googlyEye;
    public GameObject squintyEye;
    float eyeTimer;
    public float eyeTimerMax;

    Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        timerAmount = 10;
        health = maxHealth;
        StartCoroutine(ChangePhase());
        currentPhase = 1;
        origScale = transform.localScale.x;
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene" || UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Lose")
        {
            StartCoroutine(StartScene());
        }
        try
        {
            googlyEye.SetActive(true);
            squintyEye.SetActive(false);
            powerUpSign.color = new Vector4(powerUpSign.color.r, powerUpSign.color.g, powerUpSign.color.b, 0);
            powerUpSign.enabled = false;
            powerUpText.color = new Vector4(powerUpText.color.r, powerUpText.color.g, powerUpText.color.b, 0);
            powerUpText.enabled = false;
        }
        catch { }
    }

    // Update is called once per frame
    void Update()
    {
        if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "GameScene")
        {
            if(health <= 0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Win");
            }
        }
        try
        {
            bossTimer.text = timerAmount.ToString();

            healthBar.fillAmount = health / maxHealth;
            if (health < 0)
            {
                health = 0;
            }
            if (health > 100)
            {
                health = 100;
            }

            if (autoHeal)
            {
                AutoHeal();
            }

            shootDirection = (target.position - transform.position).normalized;
            if (shoot)
            {
                if (!isShooting)
                {
                    StartCoroutine(Shoot());
                }
            }

            if (homingBullets)
            {
                if (!isShootingHoming)
                {
                    StartCoroutine(HomingShoot());
                }
            }

            if (laser)
            {
                if (!isShootingLaser)
                {
                    StartCoroutine(Laser());
                }
            }
        }
        catch { }
        if (screenShake)
        {
            ScreenShake();
        }

        equationTime += Time.deltaTime * 15;
        float equationAdd = Mathf.Exp(-equationTime * 2) * Mathf.Cos(2 * Mathf.PI * equationTime) * 1;

        transform.localScale = new Vector2(origScale + equationAdd, origScale + equationAdd);

        if (eyeTimer > 0)
        {
            googlyEye.SetActive(false);
            squintyEye.SetActive(true);
            eyeTimer -= Time.deltaTime;
        }
        else
        {
            googlyEye.SetActive(true);
            squintyEye.SetActive(false);
        }
        try
        {
            if (flashTimer > 0)
            {
                flashTimer -= Time.deltaTime;
                opacity += Time.deltaTime * 2;
                powerUpSign.color = new Vector4(powerUpSign.color.r, powerUpSign.color.g, powerUpSign.color.b, Mathf.Sin(opacity));
                powerUpText.color = new Vector4(powerUpText.color.r, powerUpText.color.g, powerUpText.color.b, Mathf.Sin(opacity));
            }
            else
            {
                powerUpSign.color = new Vector4(powerUpSign.color.r, powerUpSign.color.g, powerUpSign.color.b, 0);
                powerUpText.color = new Vector4(powerUpText.color.r, powerUpText.color.g, powerUpText.color.b, 0);
                powerUpSign.enabled = false;
                powerUpText.enabled = false;
            }
        }
        catch { }

    }

    private void FixedUpdate()
    {
        Movement();
    }

    IEnumerator StartScene()
    {
        yield return new WaitForSeconds(.1f);
        shoot = true;
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        var temp = Instantiate(bullet, transform.position, Quaternion.identity);
        temp.GetComponent<BossBulllet>().damage = currentDamage;
        temp.GetComponent<BossBulllet>().transform.up = shootDirection;
        yield return new WaitForSeconds(shootInterval);
        isShooting = false;
    }

    IEnumerator HomingShoot()
    {
        isShootingHoming = true;
        var temp = Instantiate(homingBullet, transform.position, Quaternion.identity);
        temp.GetComponent<BossHomingBullet>().damage = currentDamage / 1.5f;
        temp.GetComponent<BossHomingBullet>().transform.up = shootDirection;
        temp.GetComponent<BossHomingBullet>().target = target;
        yield return new WaitForSeconds(homingInterval);
        isShootingHoming = false;
    }

    IEnumerator Laser()
    {
        isShootingLaser = true;
        var temp = Instantiate(laserAttack, transform.position, Quaternion.identity);
        //temp.transform.SetParent(this.transform);
        temp.GetComponent<Laser>().target = target;
        temp.GetComponent<Laser>().transform.up = shootDirection;
        yield return new WaitForSeconds(laserInterval);
        isShootingLaser = false;
    }

    IEnumerator ChangePhase()
    {
        while (timerAmount > 0)
        {
            yield return new WaitForSeconds(1);
            timerAmount--;
        }
        yield return new WaitForSeconds(1);
        //change phase

        timerAmount = 10;
        StartCoroutine(NewAbility());
        StartCoroutine(ChangePhase());
    }

    void AutoHeal()
    {
        if(health < maxHealth)
        {
            health += autoHealAmount * Time.deltaTime;
        }
    }

    void Movement()
    {
        try
        {
            rb.MovePosition(transform.position + moveDirection * speed * Time.deltaTime);
            if (!moving)
            {
                StartCoroutine(PickSpot());
            }
        }
        catch { }
    }

    IEnumerator PickSpot()
    {

        moving = true;
        int temp = Random.Range(0, waypoints.Length);
        while ((waypoints[temp].position - transform.position).magnitude > .2)
        {
            moveDirection = (waypoints[temp].position - transform.position).normalized;
            yield return null;
        }
        moveDirection = new Vector3(0, 0, 0);
        yield return new WaitForSeconds(timeBetweenNewSpots);
        moving = false;
    }

    IEnumerator NewAbility()
    {

        int newForm = Random.Range(0, maxForms);
        switch (newForm)
        {
            case 0:
                if (!autoHeal)
                {
                    autoHeal = true;
                    powerUpText.text = "Auto Heal Enabled";
                }
                else
                {
                    autoHealAmount *= 1.2f;
                    powerUpText.text = "Auto Heal Increased";
                }
                break;
            case 1:
                shootInterval *= .8f;
                powerUpText.text = "Fire Rate Increased";
                break;
            case 2:
                if (!homingBullets)
                {
                    homingBullets = true;
                    powerUpText.text = "Homing Bullets Enabled";
                }
                else
                {
                    homingInterval *= .8f;
                    powerUpText.text = "Homing Bullet Fire Rate Increased";
                }
                break;
            case 3:
                if (!laser)
                {
                    laser = true;
                    powerUpText.text = "Laser Enabled";
                }
                else
                {
                    laserInterval *= .8f;
                    powerUpText.text = "Laser Fire Rate Increased";
                }
                break;
        }
        var temp = phaseHealthAmount * currentPhase;
        if (temp <= 75)
        {
            health += temp;
        }
        else
        {
            health += 75;
        }
        powerUpSign.enabled = true;
        powerUpText.enabled = true;
        flashTimer = flashTimerMax;
        currentPhase++;
        yield return new WaitForSeconds(.1f);
    }

    void ScreenShake()
    {
        playerCamera.transform.position = new Vector3(Random.Range(rangeOfShake.x, rangeOfShake.y), Random.Range(rangeOfShake.x, rangeOfShake.y), -10);
        equationTime = 0;
        
    }

    IEnumerator StartScreenShake()
    {
        screenShake = true;
        yield return new WaitForSeconds(shakeDuration);
        screenShake = false;
        playerCamera.transform.position = new Vector3(0, 0, -10);
        this.transform.localScale = new Vector3(3, 3, 3);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "playerBullet")
        {
            health -= collision.gameObject.GetComponent<PlayerBullet>().damage;
            StartCoroutine(StartScreenShake());
            Destroy(collision.gameObject);
            anim.SetTrigger("hit");
            eyeTimer = eyeTimerMax;
        }
    }

}
