using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathMovement : MonoBehaviour
{

    public Transform[] path;
    public float percentsPerSecond; // %2 of the path moved per second
    public float currentPathPercent = 0.0f; //min 0, max 1

    public GameObject pathParent;

    // Start is called before the first frame update
    void Start()
    {

        for(int i = 0; i < path.Length; i++)
        {
            path[i] = pathParent.GetComponentsInChildren<Transform>()[i+1];
        }

        currentPathPercent += percentsPerSecond * Time.deltaTime;
        iTween.PutOnPath(gameObject, path, currentPathPercent);
    }

    // Update is called once per frame
    void Update()
    {
        currentPathPercent += percentsPerSecond * Time.deltaTime;
        iTween.PutOnPath(gameObject, path, currentPathPercent);
        if (currentPathPercent >= 1)
        {
            Destroy(this.gameObject);
        }
    }

    void OnDrawGizmos()
    {
        //Visual. Not used in movement
        iTween.DrawPath(path);
    }

}
