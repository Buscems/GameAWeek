using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour
{
    [HideInInspector]
    public float damage;

    [HideInInspector]
    public Vector2 direction;

    Rigidbody2D rb;

    public float speed;

    [HideInInspector]
    public float chargeTime;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        MainPlayer.canUseChargeAttack = true;
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MainPlayer.canUseChargeAttack = true;
        this.gameObject.SetActive(false);
    }

}
