using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{

    public GameObject[] items;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnItem()
    {
        int temp = Random.Range(0, items.Length);
        Instantiate(items[temp], new Vector3(Random.Range(-18, 18), Random.Range(-10, 5), 0), Quaternion.identity);
    }

}
