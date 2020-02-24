using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZProjectile : MonoBehaviour
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
}
