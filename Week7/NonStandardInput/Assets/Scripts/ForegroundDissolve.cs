using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForegroundDissolve : MonoBehaviour
{

    Material dissolve;

    [Header("Dissolve Variables")]
    public float dissolveSpeed;
    Vector2 dissolveMinMax;

    float fadeValue;

    // Start is called before the first frame update
    void Start()
    {
        dissolve = GetComponent<SpriteRenderer>().material;
    }

    // Update is called once per frame
    void Update()
    {

        dissolve.SetFloat("_Fade", fadeValue);

    }
}
