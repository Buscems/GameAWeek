using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Health")]
    public int maxHealth;
    int currentHealth;

    public enum EnemyType { Basic, ToughBoi, ShootyBoi, FastBoi, RandomMovement }
    public EnemyType enemy;

    Transform target;
    Rigidbody2D rb;

    [Header("Movement")]
    public float speed;
    Vector2 direction;
    bool canChangeDirection;
    bool updateDirection;
    public float changeDirectionInterval;

    [Header("Shooting")]
    public GameObject bullet;
    public float shootInterval;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {

        target = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

    }

    // Update is called once per frame
    void Update()
    {
        
        if(currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }

        Movement();

        if(enemy == EnemyType.ShootyBoi)
        {
            if (!isShooting)
            {
                StartCoroutine(Shoot());
            }
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }

    void Movement()
    {
        if(enemy == EnemyType.Basic || enemy == EnemyType.ToughBoi || enemy == EnemyType.ShootyBoi || enemy == EnemyType.FastBoi)
        {
            direction = (target.position - this.transform.position).normalized;
        }

        if(enemy == EnemyType.RandomMovement)
        {
            if (canChangeDirection)
            {
                if (!updateDirection)
                {
                    StartCoroutine(ChangeDirection());
                }
            }
            else
            {
                direction = (target.position - this.transform.position).normalized;
            }
        }

    }

    IEnumerator ChangeDirection()
    {
        updateDirection = true;
        direction.x = Random.Range(-1, 1f);
        direction.y = Random.Range(-1, 1f);
        direction = direction.normalized;
        yield return new WaitForSeconds(changeDirectionInterval);
        updateDirection = false;
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        var temp = Instantiate(bullet, transform.position, Quaternion.identity);
        temp.GetComponent<BossBulllet>().transform.up = direction;
        yield return new WaitForSeconds(shootInterval);
        isShooting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (enemy == EnemyType.RandomMovement)
        {
            direction = Vector2.Reflect(direction, collision.GetContact(0).normal);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "RandomChecker" && enemy == EnemyType.RandomMovement)
        {
            this.gameObject.layer = 0;
            canChangeDirection = true;
        }
    }

    public void GetHit(int damage)
    {
        currentHealth -= damage;
    }

}
