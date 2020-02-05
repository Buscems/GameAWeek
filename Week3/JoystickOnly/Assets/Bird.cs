using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{

    public float joystickThreshold;

    public float rotateSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(transform.rotation.z) < 90)
        {
            if (Input.GetAxis("Vertical") > joystickThreshold)
            {
                transform.Rotate(new Vector3(0, 0, rotateSpeed * Input.GetAxis("Vertical") * Time.deltaTime));
            }

            if (Input.GetAxis("Vertical") > joystickThreshold)
            {
                transform.Rotate(new Vector3(0, 0, rotateSpeed * Input.GetAxis("Vertical") * Time.deltaTime));
            }

        }
        
    }
}
