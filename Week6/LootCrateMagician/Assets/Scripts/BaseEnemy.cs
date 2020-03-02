using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{

    public float health;

    bool isShaking;
    float shakeTime;

    float equationTime;

    float origScale;

    Color origColor;

    // Start is called before the first frame update
    void Start()
    {
        origScale = transform.localScale.x;
        origColor = this.GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        equationTime += Time.deltaTime * 15;
        float equationAdd = Mathf.Exp(-equationTime * 2) * Mathf.Cos(2 * Mathf.PI * equationTime) * 1;

        transform.localScale = new Vector2(origScale + equationAdd, origScale + equationAdd);

        this.GetComponent<SpriteRenderer>().color = new Color(origColor.r + equationAdd, origColor.g + equationAdd, origColor.b + equationAdd);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "MeleeAttack")
        {
            health -= collision.GetComponent<MeleeAttack>().damage;
            equationTime = 0;
        }

        if(collision.gameObject.tag == "ProjectileAttack")
        {
            health -= collision.GetComponent<ProjectileAttack>().damage;
            equationTime = 0;
        }

    }

    void StartShake()
    {
        if (!isShaking)
        {
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake()
    {
        isShaking = true;
        yield return new WaitForSeconds(shakeTime);
        isShaking = false;
    }

}
