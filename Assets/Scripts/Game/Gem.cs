using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    private Vector3 mousePositionOffset;
    private AudioSource audioSource;
    private bool PlayOnce, RotationOnce;
    private float horizontalVelocity;
    private Vector3 startPoint, endPoint, pointDifference;
    private Vector3 oriPoint;

    public bool newGenerated, dragging;
    public float dropSpeed;

    [SerializeField] private AudioClip pickSFX;
    [SerializeField] private AudioClip dropSFX;
    [SerializeField] private Texture2D normal;
    [SerializeField] private Texture2D clickable;

    private void Awake()
    {
        dropSpeed = 2;
        oriPoint = transform.position;
        newGenerated = true;
        dragging = false;
        audioSource = GetComponent<AudioSource>();
        Cursor.SetCursor(normal, Vector2.zero, CursorMode.ForceSoftware);
    }

    //get mouse position
    private Vector3 GetMouseWorldPosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    //hover
    private void OnMouseOver()
    {
        Cursor.SetCursor(clickable, Vector2.zero, CursorMode.ForceSoftware);
    }
    //pick
    private void OnMouseDown()
    {
        startPoint = Input.mousePosition;
        newGenerated = false;
        dropSpeed = 0;
        dragging = true;
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
        audioSource.PlayOneShot(pickSFX);
        //stop the animation when clicking the fruit
        StopCoroutine(squashAndStretch(dropSpeed));
        StopCoroutine(DropPositionTransform(dropSpeed));
    }
    //drag
    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }
    //drop
    private void OnMouseUp()
    {
        endPoint = Input.mousePosition;
        dragging = false;
    }
    //exit
    private void OnMouseExit()
    {
        Cursor.SetCursor(normal, Vector2.zero, CursorMode.ForceSoftware);
    }

    //use code to simulate the fruit falling down in a accelerated value instead of using Unity’s physics system
    //btw the 2D Collider attached to Fruit objects are only for interaction with mouse event
    private void Update()
    {
        //use startPoint (the user click) minus endPoint (the user release) to get distance how long user drag the objects, then convert that value into horizontal velocity
        pointDifference = startPoint - endPoint;
        horizontalVelocity = pointDifference.x;
        horizontalVelocity /= -5000;
        if (horizontalVelocity > 1)
            horizontalVelocity = 1;
        else if (horizontalVelocity < -1)
            horizontalVelocity = -1;
        else if (horizontalVelocity >= -0.005 && horizontalVelocity <= 0.005)
            horizontalVelocity = 0;

        if (dragging)
        {
            PlayOnce = false;
            RotationOnce = false;
        }
        if (!newGenerated & !dragging)
        {
            if (transform.position.y >= -4.3)
            {
                if (!RotationOnce)
                {
                    //fruit get one-time rotation based on horizontal velocity 
                    transform.rotation = Quaternion.Euler(0, 0, horizontalVelocity * 1000);
                    RotationOnce = true;
                }

                //fall rate fomula
                dropSpeed += Time.deltaTime * 9.8f;
                //the fruit get horizontal velocity when droping down
                transform.position = new Vector3(transform.position.x + horizontalVelocity * Time.deltaTime * 100, transform.position.y - dropSpeed * Time.deltaTime, transform.position.z);
            }
            else
            {
                //play fall animation & SFX once when the fruit fall on the ground, if the fruit is repicked after falling on the ground, then animation & SFX will be reactived
                if (!PlayOnce)
                {
                    StartCoroutine(hitGroundSFX(dropSpeed));
                    StartCoroutine(squashAndStretch(dropSpeed));
                    StartCoroutine(DropPositionTransform(dropSpeed));
                    PlayOnce = true;
                }
            }
        }
    }

    //since the fruit hit ground 2 times (for dropping animation), the SFX play two times as well
    //SFX volume are affected by drop speed, faster drop speed for higher volume
    IEnumerator hitGroundSFX(float dropSpeed)
    {
        if (dropSpeed <= 4)
            dropSpeed = 4.0001f;
        float magnitude = 0.1f * (dropSpeed - 4);
        if (magnitude > 1f)
            magnitude = 1f;
        else if (magnitude <= 0.2f)
            magnitude = 0.2f;
        audioSource.PlayOneShot(dropSFX, magnitude);
        yield return new WaitForSeconds(1);
        audioSource.PlayOneShot(dropSFX, magnitude / 2);
    }

    //make drop animation using script instead of Unity Animator Controller in order to involve falling speed value into animation
    //this method is for fruit squash and stretch animation
    IEnumerator squashAndStretch(float dropSpeed)
    {
        float duration;
        //get squash & stretch magnitude depending on drop speed, bigger squash if it’s falling fast, a much smaller squash if you drop it from just above the ground  
        if (dropSpeed <= 4)
            //to avoid divide by 0 error ↓
            dropSpeed = 4.0001f;
        float magnitude = 0.04f * (dropSpeed - 4);
        if (magnitude > .8f)
            magnitude = .8f;
        else if (magnitude <= 0f)
            magnitude = 0f;

        //when squash width down, stretch the height; when squash height down, stretch the width
        float[,] magnitudeHeightTransform = { { 1f, 1 - magnitude }, { 1 - magnitude, 1f + magnitude }, { 1f + magnitude, 1f } };
        float[,] magnitudeWidthTransform = { { 1f, 1 + magnitude }, { 1 + magnitude, 1f - magnitude }, { 1f - magnitude, 1f } };

        //index here means animation key frame, the fruit ① touch the ground - squash ② bounce off - stretch ③ drop to ground again - return to normal
        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
                //10/60 here means 10 frame in 60fps
                duration = 10 / 60f;
            else if (i == 1)
                duration = 25 / 60f;
            else
                duration = 10 / 60f;
            //use Mathf.Lerp to achive animating falling fruits
            float timePercent = 0;
            float timeFactor = 1 / duration;
            while (timePercent < 1)
            {
                timePercent += Time.deltaTime * timeFactor;
                transform.localScale = new Vector3(Mathf.Lerp(magnitudeWidthTransform[i, 0], magnitudeWidthTransform[i, 1], timePercent), Mathf.Lerp(magnitudeHeightTransform[i, 0], magnitudeHeightTransform[i, 1], timePercent), 1f);
                yield return null;
            }
        }
    }

    //this method is for fruits position transform animation
    IEnumerator DropPositionTransform(float dropSpeed)
    {
        float duration;
        //get bouncing magnitude depending on drop speed
        if (dropSpeed <= 4)
            dropSpeed = 4.0001f;
        float magnitude = 0.2f * (dropSpeed - 4);
        if (magnitude > 3f)
            magnitude = 3f;
        else if (magnitude <= 0f)
            magnitude = 0f;
        //-4.3f here means the floor height value
        float[,] magnitudeYTransform = { { -4.3f, -4.3f - magnitude / 10 }, { -4.3f - magnitude / 10, -4.3f + magnitude }, { -4.3f + magnitude, -4.3f } };

        for (int i = 0; i < 3; i++)
        {
            if (i == 0)
                duration = 10 / 60f;
            else
                duration = 25 / 60f;

            float timePercent = 0;
            float timeFactor = 1 / duration;
            while (timePercent < 1)
            {
                timePercent += Time.deltaTime * timeFactor;
                transform.transform.position = new Vector3(transform.position.x, Mathf.Lerp(magnitudeYTransform[i, 0], magnitudeYTransform[i, 1], timePercent), 0);
                yield return null;
            }
        }
    }
    
}

