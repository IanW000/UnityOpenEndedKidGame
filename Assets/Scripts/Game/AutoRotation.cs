using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    public int speed;
    // Sun automatically rotate
    void Update()
    {
        transform.Rotate(new Vector3(0,0,1) * speed * Time.deltaTime);
    }
}
