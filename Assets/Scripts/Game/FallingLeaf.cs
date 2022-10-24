using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingLeaf : MonoBehaviour
{
    public int speed;
    private float nextActionTime = 0.0f;
    private float randHor, randAngle;
    public float period = 3f;
    // Simulate leaves falling down - move horizontally in random direction & turn in random angle
    void Update()
    {
        if (Time.time > nextActionTime)
        {
            nextActionTime += period;
            randHor = Random.Range(3, -3);
            randAngle = Random.Range(1, -1);
        }
        transform.position = transform.position + new Vector3(randHor * speed * Time.deltaTime, -1 * speed * Time.deltaTime, 0);
        transform.Rotate(new Vector3(0, 0, randAngle) * speed * Time.deltaTime);
    }
}
