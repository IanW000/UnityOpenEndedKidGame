using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafGenerator : MonoBehaviour
{
    public GameObject[] leaf;
    private float nextActionTime = 0.0f;
    private float randHor, randAngle;
    public float period = 3f;
    // Update is called once per frame
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            GameObject newLeaf = (GameObject)Instantiate(leaf[Random.Range(0, 4)], new Vector3(Random.Range(0, 1200), 600, 0), Quaternion.identity);
            newLeaf.transform.SetParent(gameObject.transform);
        }
    }
}
