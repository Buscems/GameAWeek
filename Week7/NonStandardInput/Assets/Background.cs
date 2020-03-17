using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{

    public ParticleSystem psLeft;
    public ParticleSystem psRight;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        psLeft.startColor = this.GetComponent<SpriteRenderer>().color;
        psRight.startColor = this.GetComponent<SpriteRenderer>().color;
    }
}
