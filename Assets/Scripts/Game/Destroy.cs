using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour
{
    // Destroy cloud particle effects
    void Start()
    {
        Destroy(gameObject, 1f);
    }
}
