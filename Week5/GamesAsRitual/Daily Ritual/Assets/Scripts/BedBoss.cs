﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedBoss : MonoBehaviour
{

    Rigidbody2D rb;

    bool moveRight, moveLeft;

    Vector3 velocity;

    public GameObject waypointRight;
    public GameObject waypointLeft;

    public float speed;

    bool timeToMove;

    Vector3 shootDirection;

    public float timeUntilMove;

    public GameObject pillow;

    public GameObject zAttack;

    public float shootInterval;

    public Transform target;

    bool isShooting;

    bool canShootZ;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timeToMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveRight)
        {
            velocity.x = 1;
        }
        else if (moveLeft)
        {
            velocity.x = -1;
        }
        else
        {
            velocity.x = 0;
        }
        if (timeToMove)
        {
            StartCoroutine(WaitToMove());
        }

        shootDirection = (target.position - this.transform.position).normalized;

        if (!isShooting)
        {
            StartCoroutine(Shoot());
        }

        if(shootDirection.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (shootDirection.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + velocity  * speed * Time.deltaTime);
    }

    IEnumerator MoveRight()
    {
        while(this.transform.position.x < waypointRight.transform.position.x)
        {
            moveRight = true;
            yield return null;
        }
        moveRight = false;
        timeToMove = true;
    }

    IEnumerator MoveLeft()
    {
        while (this.transform.position.x > waypointLeft.transform.position.x)
        {
            moveLeft = true;
            yield return null;
        }
        moveLeft = false;
        timeToMove = true;
    }

    IEnumerator WaitToMove()
    {
        timeToMove = false;
        yield return new WaitForSeconds(timeUntilMove);
        if(transform.position.x < 0)
        {
            StartCoroutine(MoveRight());
        }
        if(transform.position.x > 0)
        {
            StartCoroutine(MoveLeft());
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;
        yield return new WaitForSeconds(shootInterval);

        int temp = Random.Range(0, 2);

        if (!canShootZ)
        {
            temp = 0;
        }
        
        //pillow attack
        if (temp == 0)
        {
            var newPillow = Instantiate(pillow, transform.position, Quaternion.identity);
            if (shootDirection.x > 0)
            {
                newPillow.GetComponent<PillowProjectile>().direction.x = 1;
            }
            if (shootDirection.x <= 0)
            {
                newPillow.GetComponent<PillowProjectile>().direction.x = -1;
            }
        }
        //z attack
        if(temp == 1)
        {
            var newZAttack = Instantiate(zAttack, transform.position, Quaternion.identity);
            newZAttack.GetComponent<ZProjectile>().direction = shootDirection;
        }

        isShooting = false;
    }

    public void GetHit()
    {

    }

}
