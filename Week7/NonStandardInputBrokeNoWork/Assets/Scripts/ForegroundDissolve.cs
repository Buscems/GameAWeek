using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ForegroundDissolve : MonoBehaviour
{

    Material dissolve;

    [Header("Dissolve Variables")]
    public float dissolveSpeed;
    public Vector2 dissolveMinMax;

    float fadeValue;

    bool fadingOut;

    Rigidbody2D rb;

    [Header("Moving")]
    public float speed;
    Vector2 direction;

    // Start is called before the first frame update
    void Start()
    {
        dissolve = GetComponent<SpriteRenderer>().material;

        fadingOut = true;

        direction = new Vector2(Random.Range(0, 1f), Random.Range(0, 1f));

        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        dissolve.SetFloat("_Fade", fadeValue);

        if (fadingOut)
        {
            fadeValue -= Time.deltaTime * dissolveSpeed;
            if(fadeValue <= dissolveMinMax.x)
            {
                fadingOut = false;
            }
        }
        else
        {
            fadeValue += Time.deltaTime * dissolveSpeed;
            if (fadeValue >= dissolveMinMax.y)
            {
                fadingOut = true;
            }
        }
}

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + direction * Time.deltaTime * speed);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        direction = Vector2.Reflect(direction, collision.contacts[0].normal);
    }

}
