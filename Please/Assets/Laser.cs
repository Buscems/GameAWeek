using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    Animator anim;

    public Transform target;

    bool hasHit;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(LaserBeam());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LaserBeam()
    {
        yield return new WaitForSeconds(1.5f);
        transform.up = (target.position - transform.position).normalized;
        yield return new WaitForSeconds(.5f);
        anim.SetTrigger("fire");
        yield return new WaitForSeconds(.5f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<MainPlayer>() && !hasHit)
        {
            collision.GetComponent<MainPlayer>().GetHit();
            hasHit = true;
        }
    }

}
