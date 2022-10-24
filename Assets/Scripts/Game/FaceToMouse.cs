using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceToMouse : MonoBehaviour
{
    private Vector3 originalPos;
    public Vector3 adjustedPosition;
    public float magnitude;
    private void Awake()
    {
        originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        FaceMouse(); 
    }
    public void FaceMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition = Camera.main.ScreenToViewportPoint(mousePosition);

        Vector3 direction = (mousePosition - originalPos).normalized;
        transform.position = adjustedPosition + direction * magnitude;
    }
}
