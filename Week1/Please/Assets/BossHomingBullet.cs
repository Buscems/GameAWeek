using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHomingBullet : MonoBehaviour
{

    public float speed;
    public float rotateSpeed;

    public float timeUntilDestroy;

    [HideInInspector]
    public float damage;

    [HideInInspector]
    public Transform target;

    Rigidbody2D rb;

    public GameObject explosionEffect;

    public float homingInterval;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(TimeUntilDestroy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.up).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;

        rb.velocity = transform.up * speed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Boundary"))
        {
            Destroy(this.gameObject);
        }

        if (collision.GetComponent<MainPlayer>())
        {
            collision.GetComponent<MainPlayer>().GetHit();
            Destroy(this.gameObject);
        }

    }

    IEnumerator TimeUntilDestroy()
    {
        yield return new WaitForSeconds(timeUntilDestroy);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);
    }

}
