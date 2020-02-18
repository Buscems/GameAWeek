using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBulllet : MonoBehaviour
{

    public float speed;

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
        rb.MovePosition(transform.position + transform.up * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Boundary"))
        {
            Destroy(this.gameObject);
        }

        if (collision.GetComponent<MainPlayer>())
        {
            Destroy(this.gameObject);
        }

    }

}
