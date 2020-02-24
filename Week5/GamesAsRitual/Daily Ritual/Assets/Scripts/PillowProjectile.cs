using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillowProjectile : MonoBehaviour
{

    public float speed;

    [HideInInspector]
    public Vector3 direction;

    Rigidbody2D rb;

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
        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Floor")
        {
            Destroy(this.gameObject);
        }
    }

}
